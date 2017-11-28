using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using System.Diagnostics;

namespace CurrenciesNBP
{
    class Currency
    {
        private string code { get; }
        private string currency { get; }
        private string mid { get; }

        public Currency(string readCode, string readCurrency, string readMid) {
            this.code = readCode;
            this.currency = readCurrency;
            this.mid = readMid;
        }

        override
        public string ToString() {
            return code.ToUpper() + " [" + currency + "] -> " + mid;
        }
    }
}
