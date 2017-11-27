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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CurrenciesNBP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string selectedYear;
        public MainPage()
        {
            this.InitializeComponent();
            this.selectedYear = System.DateTime.Now.Year.ToString();

            for (int i = 2002; i <= System.DateTime.Now.Year; i++)
            {
                comboBox.Items.Add(i.ToString());
            }
            comboBox.SelectedItem = selectedYear;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CurrenciesHelper helper = new CurrenciesHelper(selectedYear);
            string result = helper.GetCurrencyFileCodes();
            string text = "";
            foreach (string currencyFileCode in result.Split('\n'))
            {
                if (!currencyFileCode.StartsWith("a"))
                    continue;
                else
                {
                    CurrencyFileCode code = new CurrencyFileCode(currencyFileCode);
                    listView.Items.Add(code);
                }
                text += currencyFileCode;
            }
            textBlock.Text = result;

        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedYear = (string) comboBox.SelectedItem;
        }

        private void listView_ItemClick(object sender, ItemClickEventArgs e)
        {

            
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //TODO: Investigate why there is Exception while using HTTPClient second time
            CurrencyFileCode selectedDate = (CurrencyFileCode)listView.SelectedItem;
            string date = selectedDate.GetDate();
            Uri uri = new Uri("http://api.nbp.pl/api/exchangerates/tables/a/" + date);
            HttpClient client = new HttpClient();

            try
            {
                string response = client.GetAsync(uri).GetResults().Content.ToString();
                Debug.WriteLine(response);
                textBlock.Text = response;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "\n" + ex.StackTrace);
            }
            finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }

        }
    }
}
