using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkelonTask.Models
{
    internal class Product
    {

        public int? Code { get; set; }
        public string? Name { get; set; }
        public string? UnitOfMeasurement { get; set; }
        public double? Price { get; set; }

    }
}
