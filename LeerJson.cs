using Newtonsoft.Json;
using Org.BouncyCastle.Math;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using static Lab3JSON.ClasesJSON;
using static System.Net.WebRequestMethods;

namespace Lab3JSON
{
    internal class LeerJson
    {
        static void Main(string[] args)
        {
            var url = "https://randomuser.me/api/?results=10";
            WebClient wc = new WebClient();
            string descargaJSon= wc.DownloadString(url);
            
            Root usuarios = JsonConvert.DeserializeObject<Root>(descargaJSon);
            
            foreach (Usuario usuario in usuarios.results)
            {
               Console.WriteLine("Usuario  name first: " + usuario.name.first);
               Console.WriteLine("Usuario  name last: " + usuario.name.last);
               Console.WriteLine("Login username: "+ usuario.login.username);
               Console.WriteLine("Login password: " + usuario.login.password);
               Console.WriteLine("-----------------------------");
            }

        }

    }
}
