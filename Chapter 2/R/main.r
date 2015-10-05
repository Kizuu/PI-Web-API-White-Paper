MainScript<- function() {
source('c:\\piwebapi.r');
library(RCurl);
library(rjson);
citiesName = GetCitiesName();
attributesName = GetAttributesName();
SendValue(citiesName[1],attributesName[2],50);

}
   