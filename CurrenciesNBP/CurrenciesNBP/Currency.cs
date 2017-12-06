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
        public string Code { get; set; }
        public string Name { get; set; }
        public string Mid { get; set; }
        public string Date { get; set; }

        //public Currency(string readCode, string readName, string readMid, string selectedDate) {
        //    Code = readCode.ToUpper();
        //    Name = "[" + readName + "]";
        //    Mid = readMid;
        //    Date = selectedDate;
        //}

        override
        public string ToString() {
            return Code + " " + Name + " -> " + Mid + "\n" + Date;
        }
    }
}
