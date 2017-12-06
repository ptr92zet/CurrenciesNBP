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
        private string Code { get; }
        private string Name { get; }
        private string Mid { get; }
        private string Date { get; }

        public Currency(string readCode, string readName, string readMid, string selectedDate) {
            this.Code = readCode;
            this.Name = readName;
            this.Mid = readMid;
            this.Date = selectedDate;
        }

        override
        public string ToString() {
            return Code.ToUpper() + " [" + Name + "] -> " + Mid + "\n" + Date;
        }
    }
}
