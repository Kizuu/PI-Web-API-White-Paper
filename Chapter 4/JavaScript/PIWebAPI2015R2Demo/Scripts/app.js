var serviceBaseUrl = 'https://marc-web-sql.marc.net/piwebapi/';
var afServerName = 'MARC-PI2014';
var piServerName = 'MARC-PI2014';

var attributePathForHiddenTest = '\\\\marc-pi2014\\AFSDK%20Test\\Pump1|TestAttribute';
var attributePaths = ['\\\\marc-pi2014\\AFSDK%20Test\\PITags\\Sinusoid|Cdt158_value', '\\\\marc-pi2014\\AFSDK%20Test\\PITags\\Sinusoid|Cdt160_value', '\\\\marc-pi2014\\AFSDK%20Test\\PITags\\Sinusoid|Sinusoid_value'];
var piPointNames = ['sinusoid-future', 'sinusoid-future2', 'sinusoid-future3'];

var showOkAlert = function (result) {
    alert('Success!');
}

var showArrayLengthAlert = function (result) {
    alert('Found ' + result.Items.length + ' values.');
}

var showEndOfThreStreamAlert = function (result) {
    alert('End of stream value: ' + result.Value);
}




piwebapi.SetBaseServiceUrl(serviceBaseUrl);

function example1() {
    //Support for creating PI Points with the Future attribute
    piwebapi.CreateFuturePIPoint(piPointNames[0], piServerName, null);
    piwebapi.CreateFuturePIPoint(piPointNames[1], piServerName, null);
    piwebapi.CreateFuturePIPoint(piPointNames[2], piServerName, null);
}

function example2() {
    //Support for writing Future Data
    piwebapi.UpdateValuesForSingleStream(piPointNames[0], piServerName, null);
}

function example3() {
    //Support for reading Future Data
    piwebapi.GetValuesFromSingleStream(piPointNames[0], piServerName, '*', '*+200d', showArrayLengthAlert);
}

function example4() {
    //Support for retrieving end-of-stream values from Streams and StreamSets
    piwebapi.GetEndOfTheStream(piPointNames[0], piServerName, showEndOfThreStreamAlert);
    piwebapi.GetEndOfTheStream('sinusoid', piServerName, showEndOfThreStreamAlert);
}

function example5() {
    //Support for hidden and excluded AF Attributes
    piwebapi.MakeAttributeHidden(attributePathForHiddenTest, null);
}

function example6() {
    //Bulk writes of time series data (single and multiple value)
    piwebapi.UpdateValuesForMultipleStreams(piPointNames, piServerName, null);
}

function example7() {
    //Ad-hoc bulk reads and writes of time series data
    piwebapi.GetValuesFromMultipleStreams(attributePaths, '*-20d', '*', null);
}

