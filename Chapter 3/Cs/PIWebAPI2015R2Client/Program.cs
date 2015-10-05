using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PIWebAPI2015R2Client
{

    class Program
    {
        

        static void Main(string[] args)
        {
          
            // create an array of pi point names
            List<string> piPointNames = new List<string>() { "sinusoid", "sinusoidu", "cdt160" };

            //this variable will store the final response with the compressed values from all the PI Points from the array
            dynamic finalResponse = null;

            // create an array of tasks
            Task<string>[] tasks = new Task<string>[piPointNames.Count];

            for (int i = 0; i < piPointNames.Count; i++)
            {

                // create a new task
                tasks[i] = new Task<string>((piPointName) =>
                    {

                        //get the webId from the selected PI Point
                        string url = @"https://marc-web-sql.marc.net/piwebapi/points?path=\\marc-pi2014\" + piPointName;
                        dynamic response = MakeRequest(url);
                        string webId = response.WebId.Value.ToString();

                        //the webIds will be available by accessing the property Task.Result
                        return webId;
                    }, piPointNames[i]);
            }

            // set up a multitask continuation
            Task continuation = Task.Factory.ContinueWhenAll(tasks, antecedents =>
            {
                //in order to generate an url with all webids to get compressed values from all PI Points within the array, the antecents tasks need be accessed
                string webIdsSection = string.Empty;
                foreach (Task<string> t in antecedents)
                {
                    webIdsSection += "&webid=" + t.Result;
                }
                //then, we can generate the url with the start time and end time
                string url = @"https://marc-web-sql.marc.net/piwebapi/streamsets/recorded?" + webIdsSection.Substring(1) + "&startTime=*-200d&endTime=*";
                finalResponse = MakeRequest(url);
            });

            // start the antecedent tasks
            foreach (Task t in tasks)
            {
                t.Start();
            }

            continuation.Wait();

            Console.WriteLine(finalResponse);
            Console.WriteLine("Press enter to finish");
            Console.ReadLine();
        }

        internal static dynamic MakeRequest(string url)
        {
            WebRequest request = WebRequest.Create(url);

            //Setting up this property will add the Basic Authentication on the header of the HTTP request
            request.Credentials = new NetworkCredential("marc.adm", "xxxxx");
            request.ContentType = "application/json";
            WebResponse response = request.GetResponse();
            using (StreamReader sw = new StreamReader(response.GetResponseStream()))
            {
                using (JsonTextReader reader = new JsonTextReader(sw))
                {
                    return JObject.ReadFrom(reader);
                }
            }
        }
    }
}