using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Irma;
using Irma.Models;
using System.Xml.Linq;
using System.Net.Http;
using System.Xml;
using Irma.Services;
using Microsoft.Extensions.Configuration;
using Irma.Interface;
using Irma.Context;

namespace Irma.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class XMLDocsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IXMLService _service;
        static readonly HttpClient client = new HttpClient();
        private readonly IConfiguration _config;

        public XMLDocsController(DatabaseContext context, IConfiguration configuration, IXMLService ixmlService)
        {
            _context = context;
            _config = configuration;
            _service = ixmlService;

        }

        // GET: api/XMLDocs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<XMLDoc>>> GetXMLDocs()
        {
            var obj = await _context.XMLDocs.ToListAsync();
            return obj;
        }

        [HttpGet("proba")]
        public void GetDocument()
        {
            string link = _config.GetSection("DataSource").Value;

            XmlDocument document = new XmlDocument();

            document.Load(link);
            //Console.WriteLine(document.InnerXml);

            var xmlString = XElement.Parse(document.InnerXml);

            //vrijeme mjerenja za sve
            var timeStamp = document.DocumentElement.FirstChild.LastChild.InnerText;

            //id uredjaja-- ne znam jel bi trebalo preko for petlje ili ovako posebno a vjerovatno posebno 
            //i uz ovo ide ime lokacije u zavisnosti od id uredjaja
            var devices = document.GetElementsByTagName("Device");
            var senzor = document.GetElementsByTagName("Sensors");

            List<Uredjaj> uredjaji = new List<Uredjaj>();

            foreach(XmlNode device in devices)
            {
                if (!_service.ProvjeraUredjaja(device))
                {
                    _service.DodajUredjaj(device);
                }
            }

            //naziv senzora i tip (na osnovu naziva možes napisati tip bez da ga izvlacis iz xml-a
            //prikom svakog ocitanja xml-a u liste se spašavaju vrijednosti

            foreach(Uredjaj uredjaj in uredjaji)
            {        
                switch (uredjaj.listaLokacija)
                {
                    case "SERVER SALA 1":
                        List<Mjerenje> mjerenja_sala1 = new List<Mjerenje>();

                        for (int j = 0; j < senzor[0].ChildNodes.Count; j++)
                        {
                            Mjerenje mjerenje = _service.DodajMjerenje(senzor[0], j);
                            mjerenja_sala1.Add(mjerenje);

                            if (!_service.ProvjeraSenzora(senzor[0], j))
                            {
                                _service.DodajSenzor(senzor[0], j);
                            }
                            var _senzor = _service.PronadjiSenzor(int.Parse(senzor[0].ChildNodes[j].Attributes[0].InnerText));

                            _context.Senzori.FirstOrDefault(x => x.senzorId == _senzor.senzorId).mjerenja = mjerenja_sala1;

                            _context.SaveChanges();
                            
                        }
                        mjerenja_sala1 = new List<Mjerenje>();
                        break;
                    case "SERVER SALA 2":
                        List<Mjerenje> mjerenja_sala2 = new List<Mjerenje>();

                        for (int j = 0; j < senzor[1].ChildNodes.Count; j++)
                        {
                            Mjerenje mjerenje = _service.DodajMjerenje(senzor[1], j);
                            mjerenja_sala2.Add(mjerenje);

                            if (!_service.ProvjeraSenzora(senzor[1], j))
                            {
                                _service.DodajSenzor(senzor[1], j);
                            }
                            
                            var _senzor = _service.PronadjiSenzor(int.Parse(senzor[1].ChildNodes[j].Attributes[0].InnerText));
                            
                            _context.Senzori.FirstOrDefault(x => x.senzorId == _senzor.senzorId).mjerenja = mjerenja_sala2;

                            _context.SaveChanges();
                        }
                        mjerenja_sala2 = new List<Mjerenje>();
                        break;
                    case "KOTLOVNICA":
                        List<Mjerenje> mjerenja_kotlovnica = new List<Mjerenje>();
                        for (int j = 0; j < senzor[2].ChildNodes.Count; j++)
                        {
                            Mjerenje mjerenje = _service.DodajMjerenje(senzor[1], j);
                            mjerenja_kotlovnica.Add(mjerenje);

                            if (!_service.ProvjeraSenzora(senzor[2], j))
                            {
                                _service.DodajSenzor(senzor[2], j);
                            }

                            var _senzor = _service.PronadjiSenzor(int.Parse(senzor[2].ChildNodes[j].Attributes[0].InnerText));

                            _context.Senzori.FirstOrDefault(x => x.senzorId == _senzor.senzorId).mjerenja = mjerenja_kotlovnica;

                            _context.SaveChanges();
                        }
                        mjerenja_kotlovnica = new List<Mjerenje>();
                        break;
                    default:
                        continue;
                }
            }
        }

        // GET: api/XMLDocs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<XMLDoc>> GetXMLDoc(long id)
        {
            var xMLDoc = await _context.XMLDocs.FindAsync(id);

            if (xMLDoc == null)
            {
                return NotFound();
            }

            return xMLDoc;
        }

        // PUT: api/XMLDocs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutXMLDoc(long id, XMLDoc xMLDoc)
        {
            if (id != xMLDoc.Id)
            {
                return BadRequest();
            }

            _context.Entry(xMLDoc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!XMLDocExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/XMLDocs
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<XMLDoc>> PostXMLDoc(XMLDoc xMLDoc)
        {
            _context.XMLDocs.Add(xMLDoc);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetXMLDoc", new { id = xMLDoc.Id }, xMLDoc);
        }

        // DELETE: api/XMLDocs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<XMLDoc>> DeleteXMLDoc(long id)
        {
            var xMLDoc = await _context.XMLDocs.FindAsync(id);
            if (xMLDoc == null)
            {
                return NotFound();
            }

            _context.XMLDocs.Remove(xMLDoc);
            await _context.SaveChangesAsync();

            return xMLDoc;
        }

        private bool XMLDocExists(long id)
        {
            return _context.XMLDocs.Any(e => e.Id == id);
        }
    }
}
