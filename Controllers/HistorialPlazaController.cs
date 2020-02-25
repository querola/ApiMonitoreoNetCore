using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
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
        public ActionResult<HistorialPlaza> GetHistorialIrapuato()
        {
            try{
                foreach (IConfigurationSection datosPlaza in configuration.GetSection("ConnectionStrings:Mex-Ira").GetChildren())
                {
                    var ip = configuration.GetSection($"{datosPlaza.Path}").GetSection("Ip").Value;
                    var listas = configuration.GetSection($"{datosPlaza.Path}").GetSection("ListasSQlConnection").Value;
                    var service = configuration.GetSection($"{datosPlaza.Path}").GetSection("WebServiceConnection").Value;
                    if(new Ping().Send(ip, 1000).Status == IPStatus.Success){
                        var optionsBuilder = new DbContextOptionsBuilder<HistorialDbContext>();
                        optionsBuilder.UseSqlServer(listas);
                        using(var listasContext = new HistorialDbContext(optionsBuilder.Options)){
                            var historial = listasContext.Historial.FirstOrDefault();
                        }

                    }
                    else{
                    }
                }
                
            }catch(Exception ex){ return new string[] { "Haz una solicitud" };}
        }
    }
}