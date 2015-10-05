
var base_url = "https://marc-web-sql.marc.net/piwebapi/";


//Function to get the cities names, which are child elements from the Cities element.
function GetCitiesName() {
    var url = base_url + "elements/E07dtyl5PO4EmFExFPTz6FbghNYw-7Q65Uav8IhHRRw_WQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxDSVRJRVM/elements";
    MakeAjaxRequest('GET', url, function (data) {
        for (var i = 0; i < data.Items.length; i++) {
            $('#city_name').append($('<option>', {
                value: data.Items[i].Name,
                text: data.Items[i].Name
            }));
        }
    });
}
//Function to get the attributes template names, which are attribute templates from the city element template.
function GetAttributesName() {
    var url = base_url + "elementtemplates/T07dtyl5PO4EmFExFPTz6FbgSnYaLt6aAkqQMOBOaLJPKQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxFTEVNRU5UVEVNUExBVEVTW0NJVElFUyBURU1QTEFURV0/attributetemplates";

    MakeAjaxRequest('GET', url, function (data) {
        for (var i = 0; i < data.Items.length; i++) {
            $('#attribute_name').append($('<option>', {
                value: data.Items[i].Name,
                text: data.Items[i].Name
            }), null);
        }
    });
}

//Function used to send a value to a PI Point/Attribute 
function SendValue() {
    //Get the value of the text box with jQuery
    var value = $("#value")[0].value;

    //Get the city name with jQuery
    var cityName = $("#city_name")[0].value;

    //Get the attribute name with jQuery
    var attributeName = $("#attribute_name")[0].value;

    //Generate a url whose response will contain the other url to update the value
    var url = base_url + "attributes?path=\\\\MARC-PI2014\\AFPartnerCourseWeather\\Cities\\" + cityName + "|" + attributeName;
    var sendValueFunction = function (data) {
        var valueUrl = data["Links"]["Value"];
        MakeAjaxRequest('POST', valueUrl, function () {
            alert("Value has being sent successfully");
        }, "{'Value': " + value + " }");
    };

    //In this approach we are making a GET request. If it is successful, it will make a POST request using the url from the GET response. The sendValueFunction variable is actually a function.
    MakeAjaxRequest('GET', url, sendValueFunction, null);
}

function SendValue() {
    var value = $("#value")[0].value;
    var cityName = $("#city_name")[0].value;
    var attributeName = $("#attribute_name")[0].value;
    var url = base_url + "attributes?path=\\\\MARC-PI2014\\AFPartnerCourseWeather\\Cities\\" + cityName + "|" + attributeName;
    var sendValueFunction = function (data) {
        var valueUrl = data["Links"]["Value"];
        MakeAjaxRequest('POST', valueUrl, function () {
            alert("Value has being sent successfully");
        }, "{'Value': " + value + " }");
    };
    //var sendValueFunction = function() {alert("OK!");}
    MakeAjaxRequest('GET', url, sendValueFunction, null);
}


function GetCitiesName() {
    var url = base_url + "elements/E07dtyl5PO4EmFExFPTz6FbghNYw-7Q65Uav8IhHRRw_WQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxDSVRJRVM/elements";
    MakeAjaxRequest('GET', url, function (data) {
        for (var i = 0; i < data.Items.length; i++) {
            $('#city_name').append($('<option>', {
                value: data.Items[i].Name,
                text: data.Items[i].Name
            }));
        }
    });
}

function GetAttributesName() {
    var url = base_url + "elementtemplates/T07dtyl5PO4EmFExFPTz6FbgSnYaLt6aAkqQMOBOaLJPKQTUFSQy1QSTIwMTRcQUZQQVJUTkVSQ09VUlNFV0VBVEhFUlxFTEVNRU5UVEVNUExBVEVTW0NJVElFUyBURU1QTEFURV0/attributetemplates";

    MakeAjaxRequest('GET', url, function (data) {
        for (var i = 0; i < data.Items.length; i++) {
            $('#attribute_name').append($('<option>', {
                value: data.Items[i].Name,
                text: data.Items[i].Name
            }), null);
        }
    });
}

//This function calls the ajax method to make calls against PI Web API. 
//It was possible to use the same function for GET and POST HTTP requests
//The difference is the type and the data. 
//For Get requests, type='GET' and data is null;
//For Post requests, type='POST'and data is the request body.
function MakeAjaxRequest(type, url, SuccessCallBack, data) {
    $.ajax({
        type: type,
        url: url,
        cache: false,
        async: true,
        data: data,
        contentType: "application/json",
        username: 'useername',
        password: 'password',
        crossDomain: true,
        xhrFields: {
            withCredentials: true
        },
        success: SuccessCallBack,
        error: (function (error, variable) {
            console.log(error);
            alert('There was an error with the request');
        })
    });
}


$(document).ready(function () {
    //Once the page is loaded, both methods below will be executed.
    GetCitiesName();
    GetAttributesName();

});