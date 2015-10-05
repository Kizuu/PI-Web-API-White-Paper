classdef PIWebAPIWrapper < handle 
   methods(Static)   
      function citiesName = GetCitiesName()
        base_url = 'https://osi-serv.osibr.com/piwebapi/';
		url =strcat(base_url,'elements/E07dtyl5PO4EmFExFPTz6FbghNYw-7Q65Uav8IhHRRw_WQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxDSVRJRVM/elements');
        result = PIWebAPIWrapper.MakeGetRequest(url);
        [~,y] = size(result.Items);
		citiesName = cell(1,y);
            for i=1:y
                citiesName{i} = result.Items{i}.Name; 
            end
      end
       
     function attributesName = GetAttributesName()
        base_url = 'https://osi-serv.osibr.com/piwebapi/';
		url =strcat(base_url,'elementtemplates/T07dtyl5PO4EmFExFPTz6FbgSnYaLt6aAkqQMOBOaLJPKQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxFTEVNRU5UVEVNUExBVEVTW0NJVElFUyBURU1QTEFURV0/attributetemplates');
        result = PIWebAPIWrapper.MakeGetRequest(url);
        [~,y] = size(result.Items);
		attributesName = cell(1,y);
            for i=1:y
                attributesName{i} = result.Items{i}.Name; 
            end
     end   
    
     function statusCode = SendValue(cityName, attributeName, value)
 		base_url = 'https://osi-serv.osibr.com/piwebapi/';
 		url =strcat(base_url,'attributes?path=\\MARC-PI2014\AFPartnerCourseWeather\Cities\',cityName,'|',attributeName);
 		result = PIWebAPIWrapper.MakeGetRequest(url);
 		url_to_send = result.Links.Value;
 		postData = strcat('{''Value'': ',value,' }');
        NET.addAssembly('System.Net');
        import System.Net.*
        import System.Text.*
        webrequest = WebRequest.Create(url_to_send);
        webrequest.Method = 'POST';
        webrequest.ContentType = 'application/json';
        byteArray = Encoding.UTF8.GetBytes(postData);          
        webrequest.ContentLength = byteArray.Length;
        dataStream = webrequest.GetRequestStream();
        dataStream.Write(byteArray, 0, byteArray.Length);
        dataStream.Close();
        webresponse = webrequest.GetResponse();
        if (webresponse.StatusDescription=='Accepted')
           statusCode=202;        
        else           
           statusCode=404; 
        end  
     end
       
       function json_response = MakeGetRequest(url)
            strResponse = urlread(url);
            json_response = loadjson(strResponse);           
       end   
   end
end
        
 