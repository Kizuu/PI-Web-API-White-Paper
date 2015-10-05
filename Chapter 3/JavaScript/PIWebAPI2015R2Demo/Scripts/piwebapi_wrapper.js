var piwebapi = (function () {
    var base_service_url = 'NotDefined';
    //Specify the domain user name
    var username = 'username';

    //Specify the domain user password
    var password = 'password';

    //Generate 100 future random values (one value per day)
    function generateRandomFutureValues(n) {
        var values = [];
        for (i = 0; i < 100; i++) {
            var newValue = new Object();
            newValue.Timestamp = "* + " + i.toString() + "d";
            newValue.Value = Math.random();;
            newValue.Good = true;
            newValue.Questionable = false;
            values.push(newValue);
        }
        return values;
    }

    //Get PI Point Web Id
    function getPIPointWebId(piDataArchiveName, piPointName, successCallBack, errorCallBack) {
        baseUrlCheck();
        var url = base_service_url + 'points?path=\\\\' + piDataArchiveName + '\\' + piPointName;
        return processJsonContent('GET', null, url, successCallBack, errorCallBack);
    }

    //Get PI Data Archive Web Id
    function getPIDataArchiveWebId(piDataArchiveName, successCallBack, errorCallBack) {
        baseUrlCheck();
        var url = base_service_url + "dataservers?name=" + piDataArchiveName;
        return processJsonContent('GET', null, url, successCallBack, errorCallBack);
    }

    //Get AF Attribute Web Id
    function getAttributeWebId(attributePath, successCallBack, errorCallBack) {
        baseUrlCheck();
        var url = base_service_url + "attributes?path=" + attributePath;
        return processJsonContent('GET', null, url, successCallBack, errorCallBack);
    }


    //Returns the ajax function that will be responsbile for communicating with PI Web API. The request starts after the method $.when method is called.
    function processJsonContent(type, data, url, successCallBack, errorCallBack) {
        return $.ajax({
            type: type,
            headers: {
                "Content-Type": "application/json; charset=utf-8",
            },
            url: url,
            cache: false,
            data: data,
            async: true,
            username: username,
            password: password,
            
            crossDomain: true,
            xhrFields: {
                withCredentials: true
            },
            success: successCallBack,
            error: errorCallBack
        });
        //It seems that contentType option doesn't work pretty well. Using the headers option seems to work better.
        //crossDomain and xhrFields with  withCredentials: true needed to be used because the Anonymous authentication is disabled
        //and that my web application and PI Web API are running in different domains.
    }


    function createFuturePIPoint(piPointName, piDataArchiveName, updateDOM) {
        
        baseUrlCheck();
        var errorCallBack = function (error) {
            alert(error.responseJSON.Errors[0]);
        }

        var ajax = getPIDataArchiveWebId(piDataArchiveName, null, errorCallBack);
        $.when(ajax).then(function (dataServerJson, textStatus, jqXHR) {
            //alert(jqXHR.status); // Alerts 200
            var url = base_service_url + 'dataservers/' + dataServerJson.WebId + '/points';
            var data = new Object();
            data.Name = piPointName;
            data.PointClass = "classic";
            data.PointType = "Float32";
            data.Future = true;
            var jsonString = JSON.stringify(data);
            var ajax2 = processJsonContent('POST', jsonString, url, updateDOM, errorCallBack);
            $.when(ajax2).then(function (data, textStatus, jqXHR) {
                alert('Future PI Point was created successfully!');
            });

        });

    }

    function writeFutureValuesForStream(piPointName, piDataArchiveName, updateDOM) {
        baseUrlCheck();
        var errorCallBack = function (error) {
            alert(error.responseJSON.Errors[0]);
        }

        var ajax = getPIPointWebId(piDataArchiveName, piPointName, null, errorCallBack);
        $.when(ajax).then(function (piPointJson, textStatus, jqXHR) {
            //alert(jqXHR.status); // Alerts 200
            var url = base_service_url + 'streams/' + piPointJson.WebId + '/recorded';
            var futureValues = generateRandomFutureValues(100);
            var jsonString = JSON.stringify(futureValues);
            var ajax2 = processJsonContent('POST', jsonString, url, updateDOM, errorCallBack);
            $.when(ajax2).then(function (data, textStatus, jqXHR) {
                alert('Future values were sent successfully!');
            });

        });
    }


    function readFutureValues(piPointName, piDataArchiveName, startTime, endTime, updateDOM) {
        baseUrlCheck();

        var errorCallBack = function (error) {
            alert(error.responseJSON.Errors[0]);
        }

        var ajax = getPIPointWebId(piDataArchiveName, piPointName, null, errorCallBack);
        $.when(ajax).then(function (piPointJson, textStatus, jqXHR) {
            //alert(jqXHR.status); // Alerts 200
            var url = base_service_url + 'streams/' + piPointJson.WebId + '/recorded?startTime=' + startTime + '&endTime=' + endTime;
            var ajax2 = processJsonContent('GET', null, url, updateDOM, errorCallBack);
            $.when(ajax2).then(function (data, textStatus, jqXHR) {

            });
        });
    }







    function getEndOfTheStream(piPointName, piDataArchiveName, updateDOM) {
        baseUrlCheck();
        var errorCallBack = function (error) {
            alert(error.responseJSON.Errors[0]);
        }

        var ajax = getPIPointWebId(piDataArchiveName, piPointName, null, errorCallBack);
        $.when(ajax).then(function (piPointJson, textStatus, jqXHR) {
            //alert(jqXHR.status); // Alerts 200
            var url = base_service_url + 'streams/' + piPointJson.WebId + '/end';
            var ajax2 = processJsonContent('GET', null, url, updateDOM, errorCallBack);
            $.when(ajax2).then(function (data, textStatus, jqXHR) {

            });
        });
    }



    function makeAttributeHidden(attributePath, updateDOM) {
        baseUrlCheck();
        var errorCallBack = function (error) {
            alert(error.responseJSON.Errors[0]);
        }

        var ajax = getAttributeWebId(attributePath, null, errorCallBack);
        $.when(ajax).then(function (attributeJson, textStatus, jqXHR) {
            //alert(jqXHR.status); // Alerts 200
            var url = base_service_url + 'attributes/' + attributeJson.WebId;
            var data = new Object();
            data.isHidden = true;
            data.Description = "Test description 124";
            var jsonString = JSON.stringify(data);
            var ajax2 = processJsonContent('PATCH', jsonString, url, updateDOM, errorCallBack);

            $.when(ajax2).then(function (data, textStatus, jqXHR) {
                alert('Hidden and description properties were changed successfully!');
            });
        });
    }

    function writeFutureValuesForStreams(piPointNames, piDataArchiveName, updateDOM) {
        baseUrlCheck();
        var errorCallBack = function (error) {
            alert(error.responseJSON.Errors[0]);
        }

        var ajaxPt1 = getPIPointWebId(piDataArchiveName, piPointNames[0], null, errorCallBack);
        var ajaxPt2 = getPIPointWebId(piDataArchiveName, piPointNames[1], null, errorCallBack);
        var ajaxPt3 = getPIPointWebId(piDataArchiveName, piPointNames[2], null, errorCallBack);
        var data = [];
        $.when(ajaxPt1, ajaxPt2, ajaxPt3).done(function (r1, r2, r3) {
            var results = new Array(3);
            results[0] = r1[0];
            results[1] = r2[0];
            results[2] = r3[0];

            for (var i = 0; i < results.length; i++) {
                var obj = new Object();
                obj.WebId = results[i].WebId;
                obj.Items = generateRandomFutureValues(100);
                data.push(obj);
            }

            var jsonString = JSON.stringify(data);
            var url = base_service_url + 'streamsets/recorded';
            var ajax = processJsonContent('POST', jsonString, url, updateDOM, errorCallBack);

            $.when(ajax).done(function (a1) {
                alert('Future values were sent successfully to the 3 pi tags!');
            });
        });
    }


    function readValuesFromMultipleStreams(attributePaths, startTime, endTime, updateDOM) {
        baseUrlCheck();
        var errorCallBack = function (error) {
            alert(error.responseJSON.Errors[0]);
        }

        var ajaxPt1 = getAttributeWebId(attributePaths[0], null, errorCallBack);
        var ajaxPt2 = getAttributeWebId(attributePaths[1], null, errorCallBack);
        var ajaxPt3 = getAttributeWebId(attributePaths[2], null, errorCallBack);
        var data = [];
        $.when(ajaxPt1, ajaxPt2, ajaxPt3).done(function (r1, r2, r3) {
            var results = new Array(3);
            results[0] = r1[0];
            results[1] = r2[0];
            results[2] = r3[0];
            var webIdSection = '';
            for (var i = 0; i < results.length; i++) {
                webIdSection = webIdSection + '&webid=' + results[i].WebId;
            }
            webIdSection = webIdSection.substr(1);
            var url = base_service_url + 'streamsets/recorded?' + webIdSection + '&startTime=' + startTime + '&endTime=' + endTime;
            var ajax = processJsonContent('GET', null, url, updateDOM, errorCallBack);

            $.when(ajax).done(function (a1) {
                alert('Values received successfully from the 3 different attributes!');
            });
        });
    }

    function baseUrlCheck() {
        if (base_service_url == "NotDefined") {
            alert("Service base url was not defined");
        }
    }


    return {
        CreateFuturePIPoint: function (piPointName, piDataArchiveName, updateDOM) {
            createFuturePIPoint(piPointName, piDataArchiveName, updateDOM)
        },
        UpdateValuesForSingleStream: function (piPointName, piDataArchiveName, updateDOM) {
            writeFutureValuesForStream(piPointName, piDataArchiveName, updateDOM);
        },
        UpdateValuesForMultipleStreams: function (piPointNames, piDataArchiveName, updateDOM) {
            writeFutureValuesForStreams(piPointNames, piDataArchiveName, updateDOM);
        },
        GetValuesFromMultipleStreams: function (attributePaths, startTime, endTime, updateDOM) {
            readValuesFromMultipleStreams(attributePaths, startTime, endTime, updateDOM);
        },
        GetValuesFromSingleStream: function (piPointName, piDataArchiveName, startTime, endTime, updateDOM) {
            readFutureValues(piPointName, piDataArchiveName, startTime, endTime, updateDOM);
        },
        GetEndOfTheStream: function (piPointName, piDataArchiveName, updateDOM) {
            getEndOfTheStream(piPointName, piDataArchiveName, updateDOM);
        },
        MakeAttributeHidden: function (attributePath, updateDOM) {
            makeAttributeHidden(attributePath, updateDOM);
        },
        SetBaseServiceUrl: function (baseUrl) {
            base_service_url = baseUrl;
            if (base_service_url.slice(-1) != "/") {
                base_service_url = base_service_url + "/";
            }
        }
    }
}());
