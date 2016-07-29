﻿using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System.Net.Http.Formatting;


namespace File_System
{
	public class MvcApplication : System.Web.HttpApplication
	{
		public static void RegisterGlobalFilters (GlobalFilterCollection filters)
		{
			filters.Add (new HandleErrorAttribute ());
		}

		protected void Application_Start (Object sender, EventArgs e)
		{
			GlobalConfiguration.Configure (config => {
				config.Formatters.Clear();
				config.Formatters.Add(new JsonMediaTypeFormatter());

				config.MapHttpAttributeRoutes();

				config.Routes.MapHttpRoute(
					name: "DefaultApi",
					routeTemplate: "api/{controller}",
					defaults: new  { }
				);
			});
			RouteTable.Routes.MapRoute(
				name: "Home",
				url: "",
				defaults: new { controller = "Home", action = "Index"}
			);
		}
	}
}
