using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSIsoft;

namespace InCentrik.UniSolution
{
    public class Loader
    {
        public OSIsoft.AF.PI.PIServer piServer = null;
        public Loader(string serverName) {
            if (serverName.Equals("TEST"))
            {

            } else
            {
                OSIsoft.AF.PI.PIServers piServers = new OSIsoft.AF.PI.PIServers();
                piServer = piServers[serverName];
            }
        }
        public void LoadCSVToPI(string csv)
        {
            OSIsoft.AF.Data.AFUpdateOption replace = 0;
            CultureInfo brazilCulture = new CultureInfo("pt-BR");

            string[] lines = csv.Split(new string[] { Environment.NewLine },StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (line.Length > 0)
                {
                    string[] fields = line.Split(',');
                    string tagname = fields[0];
                    DateTime timestamp = DateTime.Parse(fields[1]);
                    string value = fields[2];

                    OSIsoft.AF.Asset.AFValue AFValue = new OSIsoft.AF.Asset.AFValue(value, timestamp);

                    OSIsoft.AF.PI.PIPoint PIPoint = OSIsoft.AF.PI.PIPoint.FindPIPoint(piServer,tagname);

                    PIPoint.UpdateValue(AFValue, replace);

                }
            }
        }
    }
}
