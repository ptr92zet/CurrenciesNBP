using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using System.Diagnostics;

namespace CurrenciesNBP
{
    class CurrenciesHelper
    {
        private string generalPath = "http://www.nbp.pl/kursy/xml/";
        private string yearFilePath;

        private HttpClient client;
        public CurrenciesHelper(string year)
        {
            if (year.Equals(System.DateTime.Now.Year.ToString()))
            {
                yearFilePath = "dir.txt";
            }
            else
            {
                yearFilePath = "dir" + year + ".txt";
            }
        }

        public string GetCurrencyFileCodes()
        {
            string result = "TEST";
            try
            {
                Uri uri = new Uri(generalPath + yearFilePath);
                client = new HttpClient();
                result = client.GetAsync(uri).GetResults().Content.ToString();
            } catch (Exception e)
            {
                Debug.WriteLine(e.Message + "\n" + e.StackTrace + "\n");
            } finally
            {
                if (client != null)
                {
                    client.Dispose();
                }
            }
            return result;
        }


    }
}
