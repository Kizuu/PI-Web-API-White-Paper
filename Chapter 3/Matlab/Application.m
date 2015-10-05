clear all;
close all;
clc;
addpath('C:\Program Files\MATLAB\jsonlab');
citiesName =PIWebAPIWrapper.GetCitiesName();
attributesName = PIWebAPIWrapper.GetAttributesName();
value=50;

statusCode = PIWebAPIWrapper.SendValue(citiesName{1}, attributesName{2}, value);
if (statusCode==202)
    display('Success!');
else
    display('Error!');
end