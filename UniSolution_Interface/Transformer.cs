using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InCentrik.UniSolution
{
    internal class Transformer
    {
        public Transformer() {
        
        }
        public string Execute(JArray arr)
        {
            StringBuilder csv = new StringBuilder();
            foreach (JObject item in arr)
            {
                string tankNumberStr = item.Value<string>("tanque");
                string timestampStr = item.Value<string>("ultima Leitura");
                string inventoryStr = item.Value<string>("litros");
                string productStr = item.Value<string>("produto");
                string tempStr = item.Value<string>("temperatura");

                timestampStr = DateTime.Parse(timestampStr, CultureInfo.GetCultureInfo("pt-BR")).ToString();
                string tagNamePrefix = "4000.Tank " + tankNumberStr.Trim() + ".";
                float inventory = (float)(ConvertToUSCultureFloat(inventoryStr) / 0.0062898108);
                float temp = (float) (ConvertToUSCultureFloat(tempStr)*9/5+32);



                var newLine = string.Format("{0},{1},{2}", tagNamePrefix + "Product", timestampStr, productStr.Trim());
                csv.AppendLine(newLine);

                newLine = string.Format("{0},{1},{2}", tagNamePrefix + "Inventory", timestampStr, inventory);
                csv.AppendLine(newLine);

                newLine = string.Format("{0},{1},{2}", tagNamePrefix + "Temp", timestampStr, temp);
                csv.AppendLine(newLine);

            }

            return csv.ToString();
        }

        static float ConvertToUSCultureFloat(string numberString)
        {
            // Define Brazilian culture
            CultureInfo brazilCulture = new CultureInfo("pt-BR");

            // Parse the string to float using Brazilian culture
            float number;
            if (float.TryParse(numberString, NumberStyles.Any, brazilCulture, out number))
            {
                // Convert the parsed float to US culture
                CultureInfo usCulture = new CultureInfo("en-US");
                return float.Parse(number.ToString(usCulture), usCulture);
            }
            else
            {
                throw new ArgumentException("Invalid number format");
            }
        }
    }
}
