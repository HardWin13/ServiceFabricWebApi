using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CoreRCON;
using CoreRCON.Parsers.Standard;
using Microsoft.AspNetCore.Mvc;
using System.Fabric;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace Monitoring.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        //static string clientCertThumb = "D855F7DBF237E278563547E8FE2294DD3D532487";
        //static string serverCertThumb = "D855F7DBF237E278563547E8FE2294DD3D532487";
        //static string CommonName = "www.clustername.westus.azure.com";
        //static string connection = "sfopenhack.westeurope.cloudapp.azure.com:19000";
        //static string connection = "13.95.89.57:19000";



        //static X509Credentials GetCredentials(string clientCertThumb, string serverCertThumb, string name)
        //{
        //    X509Credentials xc = new X509Credentials();
        //    xc.StoreLocation = StoreLocation.CurrentUser;
        //    //xc.StoreLocation = StoreLocation.LocalMachine;
        //    xc.StoreName = "My";
        //    xc.FindType = X509FindType.FindByThumbprint;
        //    xc.FindValue = clientCertThumb;
        //    xc.RemoteCommonNames.Add(name);
        //    xc.RemoteCertThumbprints.Add(serverCertThumb);
        //    xc.ProtectionLevel = ProtectionLevel.EncryptAndSign;
        //    return xc;
        //}

        public class Rootobject
        {
            public Endpoints Endpoints { get; set; }
        }

        public class Endpoints
        {
            public string MinecraftEndpoint { get; set; }
            public string MinecraftRCONEndpoint { get; set; }
        }

    // GET api/values
    [HttpGet]
    public List<string> Get()
    {
            var results = new List<string>();
        try
        {
            var fc = new FabricClient();
            var result = fc.ServiceManager.ResolveServicePartitionAsync(new Uri("fabric:/minecraft/DefaultMinecraftInstance")).Result;

        
            foreach (var endpoint in result.Endpoints)
            {
                var endpointObject = JsonConvert.DeserializeObject< Rootobject>(endpoint.Address);
                var hostArray = endpointObject.Endpoints.MinecraftRCONEndpoint.Split(':');
                var rcon = new RCON(IPAddress.Parse(hostArray[0]), ushort.Parse(hostArray[1]), "cheesesteakjimmys");
                // Send "status"
                var status = rcon.SendCommandAsync("list").Result;
                    results.Add(status);
                
                }

            //try
            //{
            //    var ret = fc.ClusterManager.GetClusterManifestAsync().Result;
            //    Console.WriteLine(ret.ToString());
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Connect failed: {0}", e.Message);
            //}


            //FabricClient cl = new FabricClient("13.95.89.57:19000");
            //var temp = cl.ServiceManager.GetServiceDescriptionAsync(new Uri("fabric:/MinecraftApplicationCluster/Guest")).Result;
            //temp.ToString();
            //// Connect to a server
            //var rcon = new RCON(IPAddress.Parse("13.95.89.57"), 25575, "minecraft");
            //// Send "status"
            //var status = rcon.SendCommandAsync("list").Result;
            //return status;
        }
        catch (Exception e)
        {
            throw e;
        }
            return results;
    }

    // GET api/values/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody]string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody]string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
}
