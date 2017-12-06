using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CurrenciesNBP {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {

        private string selectedYear;
        private ObservableCollection<string> years = new ObservableCollection<string>();
        private HttpClient client;

        public MainPage() {
            this.InitializeComponent();
            this.preConfigure();
        }

        private void preConfigure() {
            this.selectedYear = System.DateTime.Now.Year.ToString();

            for (int i = 2002; i <= System.DateTime.Now.Year; i++) {
                years.Add(i.ToString());
            }
            comboBox.DataContext = years;
            comboBox.SelectedItem = selectedYear;

            client = new HttpClient();
        }
        private async void button_Click(object sender, RoutedEventArgs e) {
            ObservableCollection<CurrencyFile> currencyFiles = new ObservableCollection<CurrencyFile>();
            listView.ItemsSource = currencyFiles;

            string generalPath = "http://www.nbp.pl/kursy/xml/";
            string yearFilePath;
            string selectedYear = (string) comboBox.SelectedItem;

            if (selectedYear.Equals(System.DateTime.Now.Year.ToString())) {
                yearFilePath = "dir.txt";
            } else {
                yearFilePath = "dir" + selectedYear + ".txt";
            }

            Uri uriObj = new Uri(generalPath + yearFilePath);
            string result = await client.GetStringAsync(uriObj);

            foreach (string currencyFileCode in result.Split('\n')) {
                if (!currencyFileCode.StartsWith("a"))
                    continue;
                else {
                    CurrencyFile code = new CurrencyFile(currencyFileCode);
                    currencyFiles.Add(code);
                }
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            selectedYear = (string) comboBox.SelectedItem;
        }

        private async void listView_ItemClick(object sender, ItemClickEventArgs e) {
            ObservableCollection<Currency> currencyRatings = new ObservableCollection<Currency>();
            CurrencyFile selectedDate = (CurrencyFile) e.ClickedItem;
            listView.SelectedItem = selectedDate;
            string date = selectedDate.GetFileDate();
            Uri uri = new Uri("http://api.nbp.pl/api/exchangerates/tables/a/" + date + "?format=xml");
            string response = await client.GetStringAsync(uri);

            XDocument document = XDocument.Load(new StringReader(response));
            var query =
                from rate in document.Root.Descendants("Rate")
                let code = rate.Element("Code")
                let currency = rate.Element("Currency")
                let mid = rate.Element("Mid")
                select new Currency() { Code = code.Value, Name = currency.Value, Mid = mid.Value, Date = date };
            currencyRatings = new ObservableCollection<Currency>(query);
            listViewCurrency.ItemsSource = currencyRatings;
        }
    }
}
