using PIWebAPIWebApplication.Infraestructure;
using PIWebAPIWebApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PIWebAPIWebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<SelectListItem> citiesNames = PIWebAPIWrapper.GetCitiesName();
            List<SelectListItem> attributesNames = PIWebAPIWrapper.GetAttributesName();
            MainModel mainModel = new MainModel(citiesNames, attributesNames);
            return View(mainModel);
        }

        [HttpPost]
        public ActionResult Index(MainModel mainModel)
        {

            mainModel.CitiesNames = PIWebAPIWrapper.GetCitiesName();
            mainModel.AttributesNames = PIWebAPIWrapper.GetAttributesName();

            int statusCode = PIWebAPIWrapper.SendValue(mainModel.SelectedCity, mainModel.SelectedAttribute, mainModel.Value);
            if (statusCode == 202)
            {
                mainModel.StatusMessage = "Success!";
            }
            else
            {
                mainModel.StatusMessage = "Error";
            }
            return View("Index", mainModel);
        }
    }
}