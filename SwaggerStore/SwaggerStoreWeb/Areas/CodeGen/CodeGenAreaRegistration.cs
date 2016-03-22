using System.Web.Mvc;

namespace SwaggerStoreWeb.Areas.CodeGen
{
    public class CodeGenAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CodeGen";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CodeGen_default",
                "CodeGen/{controller}/{action}/{id}",
                new {
                    action = "Index",
                    id = UrlParameter.Optional
                },
                new string[] {
                    "SwaggerStoreWeb.Areas.CodeGen.Controllers"
                }
            );
        }
    }
}