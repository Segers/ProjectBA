using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_MVVM.Model
{
    class LineUp
    {
        public string ID { get; set; }
        public DateTime Date { get; set; }
        public string From { get; set; }
        public string Until { get; set; }
        public Podium Podium { get; set; }
        public Band Band { get; set; }
    }
}
