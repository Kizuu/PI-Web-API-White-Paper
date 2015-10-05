using OSIsoft.AF;
using OSIsoft.AF.Asset;
using OSIsoft.AF.UnitsOfMeasure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DatabaseCreationWithPIAFSDK
{
    class Program
    {
        private static PISystem myPISystem = null;
        private static string PIServerName = string.Empty;
        static void Main(string[] args)
        {
            myPISystem = new PISystems()["MARC-PI2014"];
            PIServerName = "MARC-PI2014";
            AFDatabase myDb = myPISystem.Databases["PIWebAPIWhitePaperSampleDb"];
            if (myDb == null)
            {
                myDb = myPISystem.Databases.Add("PIWebAPIWhitePaperSampleDb");
            }

            AFElementTemplate myCityTemplate = myDb.ElementTemplates["CityTemplate"];
            if (myCityTemplate == null)
            {
                myCityTemplate = myDb.ElementTemplates.Add("Cities");
            }


            UOM degreeC = myPISystem.UOMDatabase.UOMClasses["Temperature"].UOMs["degree Celsius"];
            UOM speedKmph = myPISystem.UOMDatabase.UOMClasses["Speed"].UOMs["kilometer per hour"];
            UOM millibar = myPISystem.UOMDatabase.UOMClasses["Pressure"].UOMs["milibar"];
            UOM percent = myPISystem.UOMDatabase.UOMClasses["Ratio"].UOMs["percent"];
            UOM kilometres = myPISystem.UOMDatabase.UOMClasses["Length"].UOMs["kilometer"];


            CreateAttributeTemplate("Cloud Cover", myCityTemplate, typeof(int), percent);
            CreateAttributeTemplate("Humidity", myCityTemplate, typeof(double), percent);
            CreateAttributeTemplate("Pressure", myCityTemplate, typeof(int), millibar);
            CreateAttributeTemplate("Temperature", myCityTemplate, typeof(int), degreeC);
            CreateAttributeTemplate("Visibility", myCityTemplate, typeof(int), kilometres);
            CreateAttributeTemplate("Wind Speed", myCityTemplate, typeof(int), speedKmph);

            AFElement cities = myDb.Elements.Add("Cities");
            cities.Elements.Add("Chicago", myCityTemplate);
            cities.Elements.Add("Los Angeles", myCityTemplate);
            cities.Elements.Add("New York", myCityTemplate);
            cities.Elements.Add("San Francisco", myCityTemplate);
            cities.Elements.Add("Washington", myCityTemplate);
            myDb.CheckIn();
        }

        public static void CreateAttributeTemplate(string name, AFElementTemplate elementTemplate, Type attributeType, UOM attributeUOM)
        {
            AFAttributeTemplate myAttributeTemplate = elementTemplate.AttributeTemplates[name];
            if (myAttributeTemplate == null)
            {
                myAttributeTemplate = elementTemplate.AttributeTemplates.Add(name);
            }
            myAttributeTemplate.DataReferencePlugIn = myPISystem.DataReferencePlugIns["PI Point"];
            myAttributeTemplate.Type = attributeType;
            myAttributeTemplate.DefaultUOM = attributeUOM;
            if (myAttributeTemplate.DefaultUOM == null)
            {
                myAttributeTemplate.ConfigString = @"\\" + PIServerName + @"\%Element%_%Attribute%;";
            }
            else
            {
                myAttributeTemplate.ConfigString = @"\\" + PIServerName + @"\%Element%_%Attribute%;UOM=" + myAttributeTemplate.DefaultUOM.Abbreviation;
            }

        }
    }
}
