using SwaggerStoreWeb.Models;
using SwaggerTranslator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SwaggerStoreWeb.Areas.CodeGen.Controllers
{
    public class AndroidController : Controller
    {


        public ActionResult Hypercube(CodeModel model) {


            var swagger = SwaggerModel.From(model.Text);
            

            return Json(new {
            });
        }


    }

    
}