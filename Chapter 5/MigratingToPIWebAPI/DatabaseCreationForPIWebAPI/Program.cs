using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreationForPIWebAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            string piSystemName = "MARC-PI2014";
            string piServerName = "MARC-PI2014";
            string afDatabaseName = "PIWebAPIWhitePaperSampleDb2";
            string afElementTemplateName = "CityTemplate";
            string assetDatabasesWebId = PIWebAPIWrapper.GetAssetDatabaseWebId(piSystemName, afDatabaseName);
            int statusCode = -1;
            if (assetDatabasesWebId==null)
            {
               statusCode = PIWebAPIWrapper.CreateAssetDatabase(piSystemName, afDatabaseName);
            }
            assetDatabasesWebId = PIWebAPIWrapper.GetAssetDatabaseWebId(piSystemName, afDatabaseName);
            
            statusCode = PIWebAPIWrapper.CreateElementTemplate(afElementTemplateName, assetDatabasesWebId);
            statusCode = PIWebAPIWrapper.CreateElement("Cities", assetDatabasesWebId, true);


            string elementTemplateWebId = PIWebAPIWrapper.GetElementTemplateWebId(piSystemName, afDatabaseName, afElementTemplateName);
            string citiesElementWebId = PIWebAPIWrapper.GetElementWebId(@"\\" + piSystemName + @"\" + afDatabaseName + @"\Cities");


            statusCode = PIWebAPIWrapper.CreateAttributeTemplate(elementTemplateWebId, "Cloud Cover", "percent", piServerName, "Int32");
            statusCode = PIWebAPIWrapper.CreateAttributeTemplate(elementTemplateWebId, "Humidity", "percent", piServerName, "Double");
            statusCode = PIWebAPIWrapper.CreateAttributeTemplate(elementTemplateWebId, "Pressure", "milibar", piServerName, "Int32");
            statusCode = PIWebAPIWrapper.CreateAttributeTemplate(elementTemplateWebId, "Temperature", "degree Celsius", piServerName, "Int32");
            statusCode = PIWebAPIWrapper.CreateAttributeTemplate(elementTemplateWebId, "Visibility", "kilometer", piServerName, "Int32");
            statusCode = PIWebAPIWrapper.CreateAttributeTemplate(elementTemplateWebId, "Wind Speed", "kilometer per hour", piServerName, "Int32");

            statusCode = PIWebAPIWrapper.CreateElement("Chicago", citiesElementWebId, false, afElementTemplateName);
            statusCode = PIWebAPIWrapper.CreateElement("Los Angeles", citiesElementWebId, false, afElementTemplateName);
            statusCode = PIWebAPIWrapper.CreateElement("New York", citiesElementWebId, false, afElementTemplateName);
            statusCode = PIWebAPIWrapper.CreateElement("San Francisco", citiesElementWebId, false, afElementTemplateName);
            statusCode = PIWebAPIWrapper.CreateElement("Washington", citiesElementWebId, false, afElementTemplateName);
        }
    }
}

