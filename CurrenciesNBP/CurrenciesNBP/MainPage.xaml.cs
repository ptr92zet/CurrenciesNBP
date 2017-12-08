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
using CurrenciesNBP.Common;
using Newtonsoft.Json;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CurrenciesNBP {
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        private string selectedYear;
        private CurrencyFile selectedCurrencyFile;
        private Currency selectedCurrencyRating;
        private ObservableCollection<string> years = new ObservableCollection<string>();
        private ObservableCollection<CurrencyFile> currencyFiles = new ObservableCollection<CurrencyFile>();
        private ObservableCollection<Currency> currencyRatings = new ObservableCollection<Currency>();
        private HttpClient client;
        Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;


        public ObservableDictionary DefaultViewModel {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper {
            get { return this.navigationHelper; }
        }

        public MainPage() {
            this.InitializeComponent();
            this.preConfigure();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
        }

        private void preConfigure() {
            this.selectedYear = System.DateTime.Now.Year.ToString();

            for (int i = 2002; i <= System.DateTime.Now.Year; i++) {
                years.Add(i.ToString());
            }
            comboBox.DataContext = years;
            comboBox.SelectedItem = selectedYear;

            
            roamingSettings.Values["years"] = JsonConvert.SerializeObject(years);
            roamingSettings.Values["selectedYear"] = JsonConvert.SerializeObject(selectedYear);
            client = new HttpClient();
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e) {
            // Restore values stored in session state.
            //if (e.PageState != null && e.PageState.ContainsKey("years")) {
            //    comboBox.DataContext = e.PageState["greetingOutputText"].ToString();
            //}

            //Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("years")) {
                years = (ObservableCollection<string>) JsonConvert.DeserializeObject(roamingSettings.Values["years"].ToString());
                comboBox.DataContext = years;
            }
            if (roamingSettings.Values.ContainsKey("selectedYear")) {
                selectedYear = (string) JsonConvert.DeserializeObject(roamingSettings.Values["selectedYear"].ToString());
                comboBox.SelectedItem = selectedYear;
            }
            if (roamingSettings.Values.ContainsKey("currencyFiles")) {
                currencyFiles = (ObservableCollection<CurrencyFile>) JsonConvert.DeserializeObject(roamingSettings.Values["currencyFiles"].ToString());
                listView.ItemsSource = currencyFiles;
            }
            if (roamingSettings.Values.ContainsKey("selectedCurrencyFile")) {
                selectedCurrencyFile = (CurrencyFile) JsonConvert.DeserializeObject(roamingSettings.Values["selectedCurrencyFile"].ToString());
                listView.SelectedItem = selectedCurrencyFile;
            }
            if (roamingSettings.Values.ContainsKey("currencyRatings")) {
                currencyRatings = (ObservableCollection<Currency>) JsonConvert.DeserializeObject(roamingSettings.Values["currencyRatings"].ToString());
                listView.SelectedItem = selectedCurrencyFile;
            }
            if (roamingSettings.Values.ContainsKey("selectedCurrencyRating")) {
                selectedCurrencyRating = (Currency) JsonConvert.DeserializeObject(roamingSettings.Values["selectedCurrencyRating"].ToString());
                listViewCurrency.SelectedItem = selectedCurrencyRating;
            }
        }

        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e) {
            //Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["years"] = JsonConvert.SerializeObject(years);
            roamingSettings.Values["selectedYear"] = JsonConvert.SerializeObject(selectedYear);
            roamingSettings.Values["currencyFiles"] = JsonConvert.SerializeObject(currencyFiles);
            roamingSettings.Values["currencyRatings"] = JsonConvert.SerializeObject(currencyRatings);
            roamingSettings.Values["selectedCurrencyFile"] = JsonConvert.SerializeObject(selectedCurrencyFile);
            roamingSettings.Values["selectedCurrencyRating"] = JsonConvert.SerializeObject(selectedCurrencyRating);

        }

        private async void button_Click(object sender, RoutedEventArgs e) {
            this.currencyFiles = new ObservableCollection<CurrencyFile>();
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
            //Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["currencyFiles"] = JsonConvert.SerializeObject(currencyFiles);
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            selectedYear = (string) comboBox.SelectedItem;
            //Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["selectedYear"] = selectedYear;
        }

        private async void listView_ItemClick(object sender, ItemClickEventArgs e) {
            selectedCurrencyFile = (CurrencyFile) e.ClickedItem;
            listView.SelectedItem = selectedCurrencyFile;
            string date = selectedCurrencyFile.GetFileDate();
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

            //Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["currencyRatings"] = JsonConvert.SerializeObject(currencyRatings);
            roamingSettings.Values["selectedCurrencyFile"] = JsonConvert.SerializeObject(selectedCurrencyFile);
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e) {
            Application.Current.Exit();
        }

        private void listViewCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            selectedCurrencyRating = (Currency) listViewCurrency.SelectedItem;
            //Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["selectedCurrencyRating"] = JsonConvert.SerializeObject(selectedCurrencyRating);
        }
    }
}
