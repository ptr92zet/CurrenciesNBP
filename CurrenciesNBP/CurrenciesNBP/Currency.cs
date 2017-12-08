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
    }
}
