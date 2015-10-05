using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCreationForPIWebAPI
{
    public class PIWebAPIWrapper
    {
        private static string base_url = "https://marc-web-sql.marc.net/piwebapi/";

        public static string GetAssetDatabaseWebId(string piSystemName, string afDatabaseName)
        {
            dynamic jsonObj = jsonObj = MakeGetRequest(base_url + @"assetdatabases?path=\\" + piSystemName + @"\" + afDatabaseName);
            string json = jsonObj.ToString();
            int intResult = 0;
            bool result = Int32.TryParse(json, out intResult);
            if (result == false)
            {
                return jsonObj.WebId.Value;
            }
            else
            {
                return null;
            }
        }

        public static string GetPISystemWebId(string piSystemName)
        {
            dynamic jsonObj = jsonObj = MakeGetRequest(base_url + @"assetservers?path=\\" + piSystemName);
            return jsonObj.WebId.Value;
        }

        public static int CreateAssetDatabase(string piSystemName, string afDatabaseName)
        {
            string piSystemWebId = GetPISystemWebId(piSystemName);
            int statusCode = -1;
            string url = base_url + "assetservers/" + piSystemWebId + "/assetdatabases";
            string postData = "{\"Name\": \"" + afDatabaseName + "\", \"Description\": \"Database of the PI Web API white paper\"}";
            MakePostRequest(url, postData, out statusCode);
            return statusCode;
        }

        public static string GetElementTemplateWebId(string piSystemName, string afDatabaseName, string afElementTemplateName)
        {
            string elementTemplatePath = @"\\" + piSystemName + @"\" + afDatabaseName + @"\ElementTemplates[" + afElementTemplateName + "]";
            dynamic jsonObj = MakeGetRequest(base_url + @"elementtemplates?path=" + elementTemplatePath);
            return jsonObj.WebId.Value;
        }

        public static string GetElementWebId(string elementPath)
        {
            dynamic jsonObj = MakeGetRequest(base_url + @"elements?path=" + elementPath);
            return jsonObj.WebId.Value;
        }

        public static int CreateElement(string afElementName, string webId, bool onRoot, string templateName = null)
        {
            int statusCode = -1;
            string url = string.Empty;
            string postData = "{\"Name\": \"" + afElementName + "\"}";
            if (templateName != null)
            {
                postData = "{\"Name\": \"" + afElementName + "\", \"TemplateName\": \"" + templateName + "\"}";
            }
            if (onRoot == true)
            {
                url = base_url + "assetdatabases/" + webId + "/elements";
            }
            else
            {
                url = base_url + "elements/" + webId + "/elements";
            }
            MakePostRequest(url, postData, out statusCode);
            return statusCode;
        }




        public static int CreateElementTemplate(string afElementTemplateName, string assetdatabasesWebId)
        {
            int statusCode = -1;
            string postData = "{\"Name\": \"" + afElementTemplateName + "\"}";
            string url = base_url + "assetdatabases/" + assetdatabasesWebId + "/elementtemplates";
            MakePostRequest(url, postData, out statusCode);
            return statusCode;
        }


        public static int CreateAttributeTemplate(string elementTemplateWebId, string afAttributeTemplateName, string uomString, string piServerName, string attributeType)
        {
            int statusCode = -1;
            string postData = "{\"Name\": \"" + afAttributeTemplateName + "\",\"Type\": \"" + attributeType + "\",\"DefaultUnitsName\": \"" + uomString + "\",\"DataReferencePlugIn\": \"PI Point\",  \"ConfigString\": \"\\\\\\\\" + piServerName + "\\\\%Element%_%Attribute%\"}";
            string url = base_url + "elementtemplates/" + elementTemplateWebId + "/attributetemplates";
            MakePostRequest(url, postData, out statusCode);
            return statusCode;
        }


        internal static dynamic MakeGetRequest(string url)
        {
            WebRequest request = WebRequest.Create(url);
            //Basic Authentication
            request.Credentials = new NetworkCredential("username", "password");

            //Kerberos
            //request.Credentials = CredentialCache.DefaultCredentials;
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException ex)
            {
                int statusCode = (int)((HttpWebResponse)ex.Response).StatusCode;
                return (dynamic)statusCode;
            }

            using (StreamReader sw = new StreamReader(response.GetResponseStream()))
            {
                using (JsonTextReader reader = new JsonTextReader(sw))
                {
                    return JObject.ReadFrom(reader);
                }
            }
        }


        internal static void MakePostRequest(string url, string postData, out int statusCode)
        {
            WebRequest request = WebRequest.Create(url);
            ((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            request.Method = "POST";

            //Basic Authentication
            request.Credentials = new NetworkCredential("username", "password");

            //Kerberos
            //request.Credentials = CredentialCache.DefaultCredentials;

            request.ContentType = "application/json";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            statusCode = Convert.ToInt32(((System.Net.HttpWebResponse)(response)).StatusCode);
        }
    }
}




