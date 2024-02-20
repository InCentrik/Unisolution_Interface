using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using InCentrik.UniSolution;

namespace InCentrik.UniSolution
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
                //System.Diagnostics.Debugger.Launch();
                Client client = new Client("https://portal.unisolution.com.br","ERGON","Ergon@2024");
                Transformer transformer = new Transformer();
                Loader loader = new Loader("ERG-PIDATA");
                dynamic webResponse = client.GetData();
                string parsedResponse = transformer.Execute(webResponse);
                loader.LoadCSVToPI(parsedResponse);
                //Console.Read();
        }
    }
}
