using System.Web.Http;
using PanoramicData.OData.ProductService.App_Start;

namespace PanoramicData.OData.ProductService
{
    public class WebApiApplication : System.Web.HttpApplication
    {
		protected void Application_Start() => GlobalConfiguration.Configure(WebApiConfig.Register);
	}
}
