using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrenciesNBP
{
    class CurrencyFile
    {
        private string currencyFileCode;
        private string prettyFormattedDate;

        public CurrencyFile(string fileCode)
        {
            this.currencyFileCode = fileCode;
            string codeNameNoExtension = currencyFileCode.Split('.')[0];
            string codeDate = currencyFileCode.Split('z')[1];
            this.prettyFormattedDate = "20" + codeDate.Substring(0, 2) + "-" + codeDate.Substring(2, 2) + "-" + codeDate.Substring(4, 2);
        }

        public string GetFileDate()
        {
            return this.prettyFormattedDate;
        }

        public string GetFileCode()
        {
            return this.currencyFileCode;
        }
        
        override
        public string ToString()
        {
            return this.prettyFormattedDate;
        }
    }
}
