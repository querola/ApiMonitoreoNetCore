using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using monitoreoApiNetCore.Contexts;
using monitoreoApiNetCore.Entities;
using monitoreoApiNetCore.Interfaces;
using SimpleImpersonation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;

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
            try
            {
                var historialPlazas = new List<HistorialPlaza>();
                foreach (IConfigurationSection datosPlaza in configuration.GetSection("ConnectionStrings:Mex-Ira").GetChildren())
                {
                    var registrosPlaza = new HistorialPlaza();
                    registrosPlaza.Caseta = datosPlaza.Key;
                    //Datos LSTABINT en servidor 
                    if (new Ping().Send(configuration.GetSection($"{datosPlaza.Path}").GetSection("ServidorIp").Value, 1000).Status == IPStatus.Success)
                    {
                        FileInfo ultimaLista = GetFileInfo(configuration.GetSection($"{datosPlaza.Path}").GetSection("ServidorIp").Value, true);
                        registrosPlaza.ListaServidor = new ListaServidor(
                            ultimaLista.Name,
                            ultimaLista.CreationTime,
                            BytesToString(ultimaLista.Length),
                            (ultimaLista.CreationTime < DateTime.Now.AddMinutes(-45)) ? true : false
                        );
                    }
                    else
                        registrosPlaza.ListaServidor = null;
                    //Datos listas procesadas en máquina Listas/Web Service
                    if (new Ping().Send(configuration.GetSection($"{datosPlaza.Path}").GetSection("ListasIp").Value, 1000).Status == IPStatus.Success)
                    {
                        using (var listasContext = new HistorialDbContext(new DbContextOptionsBuilder<HistorialDbContext>().UseSqlServer(configuration.GetSection($"{datosPlaza.Path}").GetSection("ListasSQlConnection").Value).Options))
                        {
                            var historial = listasContext.Historial.OrderByDescending(x => x.FechaDeCreacion).FirstOrDefault();
                            registrosPlaza.Lista = new Lista(
                                historial.Nombre,
                                historial.FechaDeCreacion,
                                historial.Extension,
                                (historial.FechaDeCreacion < DateTime.Now.AddMinutes(-45)) ? true : false
                            );
                        }
                        using (var webServiceContext = new pn_importacion_wsIndraContext(new DbContextOptionsBuilder<pn_importacion_wsIndraContext>().UseSqlServer(configuration.GetSection($"{datosPlaza.Path}").GetSection("WebServiceConnection").Value).Options))
                        {
                            var historial = webServiceContext.pn_importacion_wsIndra.OrderByDescending(x => x.FechaExt).FirstOrDefault();
                            registrosPlaza.WebService = new WebService(
                                historial.FechaExt,
                                (historial.FechaExt < DateTime.Now.AddMinutes(-45)) ? true : false
                            );
                        }
                    }
                    else
                        registrosPlaza.Lista = null;
                    historialPlazas.Add(registrosPlaza);
                }
                return historialPlazas;
            }
            catch (Exception info)
            {
                var path = @"C:\LogApiMonitoreo";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string logFile = $@"{path}\logApiMonitoreo.txt";
                string error = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss}: Line: {Convert.ToInt32(info.StackTrace.Substring(info.StackTrace.LastIndexOf(" ") + 1))} {info.Message}";
                if (System.IO.File.Exists(logFile))
                {
                    using (StreamWriter sw = System.IO.File.AppendText(logFile))
                    {
                        sw.WriteLine(error);
                    }
                }
                else
                {
                    System.IO.File.WriteAllText(logFile, error);
                }
                return NotFound();
                //WriteLog(ex.InnerException, "Consulta Irapuato", @"C:\temporal\prueba.txt"); 
            }
        }

        [Route("Acapulco")]
        [HttpGet]
        public ActionResult<IEnumerable<HistorialPlaza>> GetHistorialAcapulco()
        {
            try
            {
                var historialPlazas = new List<HistorialPlaza>();
                foreach (IConfigurationSection datosPlaza in configuration.GetSection("ConnectionStrings:Mex-Aca").GetChildren())
                {
                    var registrosPlaza = new HistorialPlaza();
                    registrosPlaza.Caseta = datosPlaza.Key;
                    //Datos LSTABINT en servidor 
                    if (new Ping().Send(configuration.GetSection($"{datosPlaza.Path}").GetSection("ServidorIp").Value, 1000).Status == IPStatus.Success)
                    {
                        FileInfo ultimaLista = GetFileInfo(configuration.GetSection($"{datosPlaza.Path}").GetSection("ServidorIp").Value, false);
                        registrosPlaza.ListaServidor = new ListaServidor(
                            ultimaLista.Name,
                            ultimaLista.CreationTime,
                            BytesToString(ultimaLista.Length),
                            (ultimaLista.CreationTime < DateTime.Now.AddMinutes(-45)) ? true : false
                        );
                    }
                    else
                        registrosPlaza.ListaServidor = null;
                    //Datos listas procesadas en máquina Listas/Web Service
                    if (new Ping().Send(configuration.GetSection($"{datosPlaza.Path}").GetSection("ListasIp").Value, 1000).Status == IPStatus.Success)
                    {
                        using (var listasContext = new HistorialDbContext(new DbContextOptionsBuilder<HistorialDbContext>().UseSqlServer(configuration.GetSection($"{datosPlaza.Path}").GetSection("ListasSQlConnection").Value).Options))
                        {
                            var historial = listasContext.Historial.OrderByDescending(x => x.FechaDeCreacion).FirstOrDefault();
                            registrosPlaza.Lista = new Lista(
                                historial.Nombre,
                                historial.FechaDeCreacion,
                                historial.Extension,
                                (historial.FechaDeCreacion < DateTime.Now.AddMinutes(-45)) ? true : false
                            );
                        }
                        using (var webServiceContext = new pn_importacion_wsIndraContext(new DbContextOptionsBuilder<pn_importacion_wsIndraContext>().UseSqlServer(configuration.GetSection($"{datosPlaza.Path}").GetSection("WebServiceConnection").Value).Options))
                        {
                            var historial = webServiceContext.pn_importacion_wsIndra.OrderByDescending(x => x.FechaExt).FirstOrDefault();
                            registrosPlaza.WebService = new WebService(
                                historial.FechaExt,
                                (historial.FechaExt < DateTime.Now.AddMinutes(-45)) ? true : false
                            );
                        }
                    }
                    else
                        registrosPlaza.Lista = null;
                    historialPlazas.Add(registrosPlaza);
                }
                return historialPlazas;
            }
            catch (Exception info)
            {
                var path = @"C:\LogApiMonitoreo";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string logFile = $@"{path}\logApiMonitoreo.txt";
                string error = $"{DateTime.Now:dd/MM/yyyy hh:mm:ss}: Line: {Convert.ToInt32(info.StackTrace.Substring(info.StackTrace.LastIndexOf(" ") + 1))} {info.Message}";
                if (System.IO.File.Exists(logFile))
                {
                    using (StreamWriter sw = System.IO.File.AppendText(logFile))
                    {
                        sw.WriteLine(error);
                    }
                }
                else
                {
                    System.IO.File.WriteAllText(logFile, error);
                }
                return NotFound();
                //WriteLog(ex.InnerException, "Consulta Irapuato", @"C:\temporal\prueba.txt"); 
            }
        }
        public FileInfo GetFileInfo(string ip, bool p)
        {
            if (System.Diagnostics.Debugger.IsAttached)
                return RunAsUser(new UserCredentials("WORKGROUP", "GEAINT", "G3jRm5f1"), $@"\\{ip}\geaint\PARAM\ACTUEL");
            else
                return RunAsUser((p) ? new UserCredentials("WORKGROUP", "GEAINT", "G3jRm5f1") : new UserCredentials("WORKGROUP", "admin", "admin"), $@"\\{ip}\geaint\PARAM\ACTUEL");

        }
        private FileInfo RunAsUser(UserCredentials credentials, string directory)
        {
            FileInfo fileInfo = null;
            Impersonation.RunAsUser(credentials, LogonType.Interactive, () =>
            {
                if (Directory.GetFiles(directory, "LSTABINT*").Length != 0)
                    fileInfo = new FileInfo(Directory.GetFiles(directory, "LSTABINT*").FirstOrDefault());
            });
            return fileInfo;

        }
        public void WriteLog(Exception exception, string method, string logFile)
        {
            if (System.IO.File.Exists(logFile))
            {
                string[] lines = System.IO.File.ReadAllLines(logFile);
                lines[0] = $"{method}. {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} {Convert.ToInt32(exception.StackTrace.Substring(exception.StackTrace.LastIndexOf(" ") + 1))}. {exception.Message}.\n{lines[0]}";
                System.IO.File.Delete(logFile);
                System.IO.File.WriteAllLines(logFile, lines);
            }
            else System.IO.File.WriteAllText(logFile, $"{method}. {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}. {Convert.ToInt32(exception.StackTrace.Substring(exception.StackTrace.LastIndexOf(" ") + 1))}. {exception.Message}.");
        }
        public void WriteLog(string info, string logFile)
        {
            if (System.IO.File.Exists(logFile))
            {
                string[] lines = System.IO.File.ReadAllLines(logFile);
                lines[0] = $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} {info}\n{lines[0]}";
                System.IO.File.Delete(logFile);
                System.IO.File.WriteAllLines(logFile, lines);
            }
            else System.IO.File.WriteAllText(logFile, $"{DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")}. {info}");
        }

        static String BytesToString(long byteCount)
        {
            string[] suf = { "B", "KB", "MB", "GB", "TB", "PB", "EB" }; //Longs run out around EB
            if (byteCount == 0)
                return "0" + suf[0];
            long bytes = Math.Abs(byteCount);
            int place = Convert.ToInt32(Math.Floor(Math.Log(bytes, 1024)));
            double num = Math.Round(bytes / Math.Pow(1024, place), 1);
            return (Math.Sign(byteCount) * num).ToString() + suf[place];
        }
    }
}