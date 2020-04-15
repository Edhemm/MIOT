using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Irma.Models
{
    public class Senzor
    {
        public int id { get; set; }
        public int senzorId { get; set; }
        public string imeSenzora { get; set; }
        public string tipSenzora { get; set; }
        public List<Mjerenje> mjerenja { get; set; }
    }
}
