using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using monitoreoApiNetCore.Contexts;
using monitoreoApiNetCore.Entities;

namespace monitoreoApiNetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistorialPlazaController : ControllerBase
    {
        readonly IConfiguration configuration;
        public HistorialPlazaController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [HttpGet]
        public ActionResult<IEnumerable<HistorialPlaza>> GetHistorialIrapuato()
        {
            var historialPlazas = new List<HistorialPlaza>();
            foreach (IConfigurationSection datosPlaza in configuration.GetSection("ConnectionStrings:Mex-Ira").GetChildren())
            {
                if(new Ping().Send(configuration.GetSection($"{datosPlaza.Path}").GetSection("Ip").Value, 1000).Status == IPStatus.Success){
                    var registrosPlaza = new HistorialPlaza();
                    FileInfo ultimaLista = GetFileInfo(configuration.GetSection($"{datosPlaza.Path}").GetSection("Ip").Value);

                    using (var listasContext = new HistorialDbContext(new DbContextOptionsBuilder<HistorialDbContext>().UseSqlServer(configuration.GetSection($"{datosPlaza.Path}").GetSection("ListasSQlConnection").Value).Options)){
                        var historial = listasContext.Historial.FirstOrDefault();
                        registrosPlaza.Caseta = datosPlaza.Value;
                        registrosPlaza.ListaSql = historial.Nombre;
                        registrosPlaza.Extension = ultimaLista.Extension;
                        registrosPlaza.ListaServidor = ultimaLista.Name;
                        registrosPlaza.PesoLista = ultimaLista.Length.ToString();
                    }
                    using(var webServiceContext = new pn_importacion_wsIndraContext(new DbContextOptionsBuilder<pn_importacion_wsIndraContext>().UseSqlServer(configuration.GetSection($"{datosPlaza.Path}").GetSection("WebServiceConnection").Value).Options)){
                        var historial = webServiceContext.pn_importacion_wsIndra.FirstOrDefault();
                        registrosPlaza.WebService = historial.FechaExt;
                    }
                    historialPlazas.Add(registrosPlaza);
                }
                else
                {
                    historialPlazas.Add(new HistorialPlaza{
                        Caseta = configuration.GetSection($"{datosPlaza.Path}").GetSection("WebServiceConnection").Value,
                        ListaSql = null,
                        Extension = null,
                        ListaServidor = null,
                        PesoLista = null,
                        WebService = null
                    });    
                }
            }
            return historialPlazas;
        }

        public FileInfo GetFileInfo(string ip)
        {   
            FileInfo file;
            if(Directory.GetFiles($@"\\{ip}\geaint\PARAM\ACTUEL", "LSTABINT*").Length != 0){
                
                foreach (var item in Directory.GetFiles($@"\\{ip}\geaint\PARAM\ACTUEL", "LSTABINT*"))
                {
                    file = new FileInfo(item);
                }
                return new FileInfo(Directory.GetFiles($@"\\{ip}\geaint\PARAM\ACTUEL", "LSTABINT*").FirstOrDefault());
            }
                //return new FileInfo(Directory.GetFiles($@"\\{ip}\geaint\PARAM\ACTUEL", "LSTABINT*").FirstOrDefault());
            else
                return null;
        }
    }
}