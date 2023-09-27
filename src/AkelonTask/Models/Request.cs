using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkelonTask.Models
{
    internal class Request
    {

        public int RequestCode { get; set; }
        public int ProductCode { get; set; }
        public int ClientCode { get; set; }
        public int RequestId { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; } // d format.

    }
}
