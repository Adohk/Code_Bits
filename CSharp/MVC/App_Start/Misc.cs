//BundleConfig
public class BundleConfig {
	public static void RegisterBundles(BundleCollection bundles) {
	  bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				  "~/Scripts/jquery-{version}.js",
				  "~/Scripts/jquery.unobtrusive-ajax.js"));
				  
	  bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/bootstrap.css",
				"~/Content/site.css",
				"~/Content/animate.css"));
	}
}

//FilterConfig
public class FilterConfig {
	public static void RegisterGlobalFilters(GlobalFilterCollection filters) {
		//Make Attribute Global.
		//filters.Add(new CustomNameAttribute());
		filters.Add(new HandleErrorAttribute());
	}
}

//RouteConfig
public class RouteConfig {
	public static void RegisterRoutes(RouteCollection routes) {
		routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
		routes.MapMvcAttributeRoutes();
		routes.MapRoute(
			name: "Default",
			url: "{controller}/{action}/{id}",
			defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
		);
	}
}

//Custom Attributes
public class CustomNameAttribute : ActionFilterAttribute {
	public override void OnActionExecuting(ActionExecutingContext filterContext) {
		HttpContext ctx = HttpContext.Current;
		// check  sessions here
		if (HttpContext.Current.Session["UserID"] == null) {
			filterContext.Result = new RedirectResult("~/");
			return;
		}
		base.OnActionExecuting(filterContext);
	}
}

//Global.asax
namespace MyApp {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {		
            //GlobalFilters.Filters.Add(new CustomNameAttribute());
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}

