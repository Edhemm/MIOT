using Irma.Context;
using Irma.Interface;
using Irma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Irma.Services
{
    public class XMLService : IXMLService
    {
        private readonly DatabaseContext _context;

        public XMLService(DatabaseContext context)
        {
            _context = context;
        } 
        public Mjerenje DodajMjerenje(XmlNode senzor, int j)
        {
            Mjerenje mjerenje = new Mjerenje();

            mjerenje.vrijemeMjerenja = senzor.ChildNodes[j].ChildNodes[14].InnerText;
            mjerenje.minVrijednost = senzor.ChildNodes[j].ChildNodes[6].InnerText;
            mjerenje.maxVrijednost = senzor.ChildNodes[j].ChildNodes[7].InnerText;
            mjerenje.alarm = senzor.ChildNodes[j].ChildNodes[10].InnerText;
            mjerenje.vrijednostMjerenja = senzor.ChildNodes[j].ChildNodes[13].InnerText;
            mjerenje.validnostMjeranja = senzor.ChildNodes[j].ChildNodes[15].InnerText;

            return mjerenje;
        }
        public bool ProvjeraSenzora(XmlNode senzor, int j)
        {
            var _senzor = _context.Senzori.FirstOrDefault(x => x.senzorId == int.Parse(senzor.ChildNodes[j].Attributes[0].InnerText));
            if (_senzor == null)
                return false;
            else
                return true;
        }
        public void DodajSenzor(XmlNode senzor, int j)
        {
            Senzor _senzor = new Senzor();

            _senzor.imeSenzora = senzor.ChildNodes[j].FirstChild.InnerText;
            _senzor.tipSenzora = senzor.ChildNodes[j].ChildNodes[2].InnerText;
            _senzor.senzorId = int.Parse(senzor.ChildNodes[j].Attributes[0].InnerText);

            _context.Senzori.Add(_senzor);
            _context.SaveChanges();
        }

        public Senzor PronadjiSenzor(int id)
        {
           return _context.Senzori.FirstOrDefault(x => x.senzorId == id);
        }

        public bool ProvjeraUredjaja(XmlNode uredjaj)
        {
            Uredjaj _uredjaj = _context.Uredjaji.FirstOrDefault(x => x.deviceId == int.Parse(uredjaj.Attributes[0].InnerText));
            if (_uredjaj == null)
                return false;
            else
                return true;
        }
        public void DodajUredjaj(XmlNode uredjaj)
        {
            Uredjaj _uredjaj = new Uredjaj();

            _uredjaj.listaLokacija = uredjaj.ChildNodes[0].ChildNodes[0].InnerText;
            _uredjaj.deviceId = int.Parse(uredjaj.Attributes[0].InnerText);
            
            _context.Uredjaji.Add(_uredjaj);
            _context.SaveChanges();
        }

    }
}
