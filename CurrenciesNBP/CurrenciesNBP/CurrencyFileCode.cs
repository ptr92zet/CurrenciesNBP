using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrenciesNBP
{
    class CurrencyFileCode
    {
        private string currencyFileCode;
        private string prettyFormattedDate;

        public CurrencyFileCode(string code)
        {
            this.currencyFileCode = code;
            string codeNameNoExtension = currencyFileCode.Split('.')[0];
            string codeDate = currencyFileCode.Split('z')[1];
            this.prettyFormattedDate = "20" + codeDate.Substring(0, 2) + "-" + codeDate.Substring(2, 2) + "-" + codeDate.Substring(4, 2);
        }

        public string GetDate()
        {
            return this.prettyFormattedDate;
        }

        public string GetCode()
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
