GetCitiesName<- function() {
base_url = 'https://osi-serv.osibr.com/piwebapi/';
url=paste(base_url, 'elements/E07dtyl5PO4EmFExFPTz6FbghNYw-7Q65Uav8IhHRRw_WQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxDSVRJRVM/elements',sep="");
result = MakeGetRequest(url);
l = length(result$Items);
citiesname <- 1:l
	for(i in 1:l){
     citiesname[i] =  result$Items[[i]]$Name;
    }
	x=citiesname;
}

GetAttributesName<- function() {
base_url = 'https://osi-serv.osibr.com/piwebapi/';
url =paste(base_url,'elementtemplates/T07dtyl5PO4EmFExFPTz6FbgSnYaLt6aAkqQMOBOaLJPKQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxFTEVNRU5UVEVNUExBVEVTW0NJVElFUyBURU1QTEFURV0/attributetemplates',sep="");
result = MakeGetRequest(url);
l = length(result$Items);
attributesname <- 1:l
	for(i in 1:l){
     attributesname[i] =  result$Items[[i]]$Name;
    }
	x=attributesname;
}

SendValue<- function(cityName, attributeName, value){
base_url = 'https://osi-serv.osibr.com/piwebapi/';
stringarray <- c(base_url, "attributes?path=\\\\MARC-PI2014\\AFPartnerCourseWeather\\Cities\\",cityName,"|",attributeName)
url =paste(stringarray,collapse="");
result = MakeGetRequest(url);
url_to_send = result$Links$Value;
stringarray <- c('{\'Value\': ',value,' }')
postData = paste(stringarray,collapse="");
MakePostRequest(url_to_send,postData);
}

MakeGetRequest <- function(url) {
w=getURL(url,ssl.verifypeer = FALSE);
x=fromJSON(w);
}

MakePostRequest <- function(url, postData) {
headers <- list('Content-Type' = 'application/json')
rs = postForm(url, .opts=list(postfields=postData, httpheader=headers,ssl.verifypeer = FALSE))
}


   
       



