using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIWebAPIWebApplication.Models
{
    public class MainModel
    {
        public IEnumerable<SelectListItem> CitiesNames = null;
        public IEnumerable<SelectListItem> AttributesNames = null;
        public MainModel(IEnumerable<SelectListItem> downloadedCitiesNames, IEnumerable<SelectListItem> downloadedAttributesNames)
        {
            CitiesNames = downloadedCitiesNames;
            AttributesNames = downloadedAttributesNames;
            Value = string.Empty;
        }
        public MainModel()
        {
            Value = string.Empty;
        }

        [Display(Name = "Select city:")]
        public string SelectedCity { get; set; }
        [Display(Name = "Select attribute:")]
        public string SelectedAttribute { get; set; }
        [Display(Name = "Value to send:")]
        public string Value { get; set; }
        [Display(Name = "Status:")]
        public string StatusMessage { get; set; }
    }
}