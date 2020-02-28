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
using monitoreoApiNetCore.Interfaces;
using SimpleImpersonation;

namespace monitoreoApiNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HistorialPlazaController : ControllerBase, ILog
    {
        readonly IConfiguration configuration;
        public HistorialPlazaController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [Route("Irapuato")]
        [HttpGet]
        public ActionResult<IEnumerable<HistorialPlaza>> GetHistorialIrapuato()
        {
            var historialPlazas = new List<HistorialPlaza>();
            foreach (IConfigurationSection datosPlaza in configuration.GetSection("ConnectionStrings:Mex-Ira").GetChildren())
            {
                if(new Ping().Send(configuration.GetSection($"{datosPlaza.Path}").GetSection("Ip").Value, 1000).Status == IPStatus.Success){
                    var registrosPlaza = new HistorialPlaza();
                    FileInfo ultimaLista = GetFileInfo(configuration.GetSection($"{datosPlaza.Path}").GetSection("Ip").Value, true);

                    using (var listasContext = new HistorialDbContext(new DbContextOptionsBuilder<HistorialDbContext>().UseSqlServer(configuration.GetSection($"{datosPlaza.Path}").GetSection("ListasSQlConnection").Value).Options)){
                        var historial = listasContext.Historial.FirstOrDefault();
                        registrosPlaza.Caseta = datosPlaza.Key;
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

        [Route("Acapulco")]
        [HttpGet]
        public ActionResult<IEnumerable<HistorialPlaza>> GetHistorialAcapulco()
        {
            var historialPlazas = new List<HistorialPlaza>();
            foreach (IConfigurationSection datosPlaza in configuration.GetSection("ConnectionStrings:Mex-Aca").GetChildren())
            {
                if(new Ping().Send(configuration.GetSection($"{datosPlaza.Path}").GetSection("Ip").Value, 1000).Status == IPStatus.Success){
                    var registrosPlaza = new HistorialPlaza();
                    FileInfo ultimaLista = GetFileInfo(configuration.GetSection($"{datosPlaza.Path}").GetSection("Ip").Value, true);

                    using (var listasContext = new HistorialDbContext(new DbContextOptionsBuilder<HistorialDbContext>().UseSqlServer(configuration.GetSection($"{datosPlaza.Path}").GetSection("ListasSQlConnection").Value).Options)){
                        var historial = listasContext.Historial.FirstOrDefault();
                        registrosPlaza.Caseta = datosPlaza.Key;
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

        public FileInfo GetFileInfo(string ip, bool p)
        {   
            if(System.Diagnostics.Debugger.IsAttached)
                return RunAsUser(new UserCredentials("WORKGROUP", "admin", "admin"), $@"\\{ip}\geaint\PARAM\ACTUEL");
            else{
                //if p = true se usa credenciales Irapuato
                var credentials = (p) ? new UserCredentials("WORKGROUP", "GEAINT", "G3jRm5f1") : new UserCredentials("WORKGROUP", "admin", "admin");
                return RunAsUser(credentials, $@"\\{ip}\geaint\PARAM\ACTUEL");
            }
        }

        private FileInfo RunAsUser(UserCredentials credentials, string directory)
        {
            FileInfo fileInfo = null;
            Impersonation.RunAsUser(credentials, LogonType.Interactive, () => {
                if(Directory.GetFiles(directory, "LSTABINT*").Length != 0)
                    fileInfo = new FileInfo(Directory.GetFiles(directory, "LSTABINT*").FirstOrDefault());
            });
            return fileInfo;
        }
        
        public void WriteLog(Exception exception, string method, string logFile)
        {
            if(System.IO.File.Exists(logFile)){
                string[] lines = System.IO.File.ReadAllLines(logFile);
                lines[0] = $"{method}. {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} {Convert.ToInt32(exception.StackTrace.Substring(exception.StackTrace.LastIndexOf(" ") + 1))}. {exception.Message}.\n{lines[0]}";
                System.IO.File.Delete(logFile);
                System.IO.File.WriteAllLines(logFile, lines);
            }
            else System.IO.File.WriteAllText(logFile, $"{method}. {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}. {Convert.ToInt32(exception.StackTrace.Substring(exception.StackTrace.LastIndexOf(" ") + 1))}. {exception.Message}.");
        }
        public void WriteLog(string info, string logFile)
        {
            if(System.IO.File.Exists(logFile)){
                string[] lines = System.IO.File.ReadAllLines(logFile);
                lines[0] = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} {info}\n{lines[0]}";
                System.IO.File.Delete(logFile);
                System.IO.File.WriteAllLines(logFile, lines);
            }
            else System.IO.File.WriteAllText(logFile, $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}. {info}");
        }
    }


        
}