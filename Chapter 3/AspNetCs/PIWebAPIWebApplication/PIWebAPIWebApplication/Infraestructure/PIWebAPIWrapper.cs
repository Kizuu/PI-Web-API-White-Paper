using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PIWebAPIWebApplication.Infraestructure
{
    public class PIWebAPIWrapper
    {
        //Store the base url of PI Web API to be used on the other methods.
        private static string base_url = "https://marc-web-sql.marc.net/piwebapi/";

        //This method will get all city names stored on PI AF.
        public static List<SelectListItem> GetCitiesName()
        {
            //SelectListItem is an ASP.NET MVC object used for the drop down list.
            List<SelectListItem> items = new List<SelectListItem>();

            //This url is shows the child elements from the Cities element. We could have searched it by path
            //but we would have to make an extra REST call.
            string url = base_url + "elements/E07dtyl5PO4EmFExFPTz6FbghNYw-7Q65Uav8IhHRRw_WQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxDSVRJRVM/elements";
            dynamic result = MakeGetRequest(url);
            foreach (var item in result.Items)
            {
                items.Add(new SelectListItem { Text = item.Name.Value, Value = item.Name.Value });
            }
            return items;
        }

        //This method will get all the attribute templates from the city template stored on PI AF.
        public static List<SelectListItem> GetAttributesName()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string url = base_url + "elementtemplates/T07dtyl5PO4EmFExFPTz6FbgSnYaLt6aAkqQMOBOaLJPKQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxFTEVNRU5UVEVNUExBVEVTW0NJVElFUyBURU1QTEFURV0/attributetemplates";
            dynamic result = MakeGetRequest(url);
            foreach (var item in result.Items)
            {
                items.Add(new SelectListItem { Text = item.Name.Value, Value = item.Name.Value });
            }
            return items;
        }

        //This method will update a value from a PI Point given a city and an attribute.
        public static int SendValue(string cityName, string attributeName, string value)
        {
            //We need to get the stream url of the attribute to be updated by making a GET request as shown below.
            var url = base_url + "attributes?path=\\\\MARC-PI2014\\AFPartnerCourseWeather\\Cities\\" + cityName + "|" + attributeName;
            dynamic result = MakeGetRequest(url);
            //This will return the correct stream url to update the value.
            string url_to_send = result["Links"]["Value"];

            //Post Http request requires a requst body message. Please refer for the PI Web API online help to know how it should be structured.
            string postData = "{'Value': " + value + " }";
            int statusCode = -1;
            MakePostRequest(url_to_send, postData, out statusCode);
            return statusCode;
        }


        //This function is responsable for the HTTP GET request
        internal static dynamic MakeGetRequest(string url)
        {
            WebRequest request = WebRequest.Create(url);

            //If you are not using Anonymous for PI Web API authentication, then you need to provide your credentials.

            //Kerberos Authentication Only
            //request.UseDefaultCredentials = true;

            //Basic and Kerberos Authentication
            request.Credentials = new NetworkCredential("username", "password");

            WebResponse response = request.GetResponse();
            using (StreamReader sw = new StreamReader(response.GetResponseStream()))
            {
                //After storing the JSON response on the StreamReader sw object, we need to convert to a dynamic object.
                using (JsonTextReader reader = new JsonTextReader(sw))
                {
                    return JObject.ReadFrom(reader);
                }
            }
        }

        //This function is responsable for the HTTP POST request
        internal static void MakePostRequest(string url, string postData, out int statusCode)
        {
            WebRequest request = WebRequest.Create(url);

            //Kerberos Authentication Only
            //request.UseDefaultCredentials = true;

            //Basic and Kerberos Authentication
            request.Credentials = new NetworkCredential("username", "password");

            ((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";

            //The method is "GET" by default.
            request.Method = "POST";

            //If the content-type is not specified as application/json, your post request won't work.
            request.ContentType = "application/json";

            //The content length should be present on the request header.
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            
            //Get the response to make sure the value was sent.
            WebResponse response = request.GetResponse();

            //The StatusCode property is available only if the WebRequest object is converted to HttpWebRequest
            //The StatusCode will confirm that the operation was executed successfully.
            statusCode = Convert.ToInt32(((System.Net.HttpWebResponse)(response)).StatusCode);
        }
    }
}