using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Irma.Models
{
    public class Uredjaj
    {
        public int id { get; set; }
        public int deviceId { get; set; }
        public string listaLokacija { get; set; }
        public List<Senzor> senzori { get; set; }
    }
}

//Ienumerable