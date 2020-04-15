using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Irma.Models
{
    public class Mjerenje
    {
        public int id { get; set; }
        public string vrijemeMjerenja { get; set; }
        public string minVrijednost { get; set; }
        public string maxVrijednost { get; set; }
        public string alarm { get; set; }
        public string vrijednostMjerenja { get; set; }
        public string validnostMjeranja { get; set; }
    }
}
