using System;
using Microsoft.AspNetCore.Mvc.Razor;

namespace Application.Models.Web.Mvc.Razor.Extensions
{
	public static class HttpContextExtension
	{
		#region Methods

		public static bool IsActive(this RazorPageBase razorPage, string action)
		{
			var routeValueDictionary = razorPage?.ViewContext.RouteData.Values;

			// ReSharper disable InvertIf
			if(routeValueDictionary != null)
			{
				if(routeValueDictionary.TryGetValue("action", out var value))
				{
					if(value is string stringValue)
						return string.Equals(action, stringValue, StringComparison.OrdinalIgnoreCase);
				}
			}
			// ReSharper restore InvertIf

			return false;
		}

		#endregion
	}
}