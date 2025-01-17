﻿namespace Simplify.Web.Meta
{
	/// <summary>
	/// Provides controller route information
	/// </summary>
	public class ControllerRouteInfo
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ControllerRouteInfo" /> class.
		/// </summary>
		/// <param name="getRoute">The HTTP GET request route.</param>
		/// <param name="postRoute">The HTTP POST request route.</param>
		/// <param name="putRoute">The HTTP PUT request route.</param>
		/// <param name="patchRoute">The HTTP PATCH route.</param>
		/// <param name="deleteRoute">The HTTP DELETE route.</param>
		public ControllerRouteInfo(string? getRoute = null, string? postRoute = null, string? putRoute = null, string? patchRoute = null, string? deleteRoute = null)
		{
			GetRoute = getRoute;
			PostRoute = postRoute;
			PutRoute = putRoute;
			PatchRoute = patchRoute;
			DeleteRoute = deleteRoute;
		}

		/// <summary>
		/// Gets or sets the HTTP GET route.
		/// </summary>
		/// <value>
		/// The HTTP GET route.
		/// </value>
		public string? GetRoute { get; set; }

		/// <summary>
		/// Gets or sets the HTTP POST route.
		/// </summary>
		/// <value>
		/// The HTTP POST route.
		/// </value>
		public string? PostRoute { get; set; }

		/// <summary>
		/// Gets or sets the HTTP PUT route.
		/// </summary>
		/// <value>
		/// The HTTP PUT route.
		/// </value>
		public string? PutRoute { get; set; }

		/// <summary>
		/// Gets or sets the HTTP PATCH route.
		/// </summary>
		/// <value>
		/// The HTTP PATCH route.
		/// </value>
		public string? PatchRoute { get; set; }

		/// <summary>
		/// Gets or sets the HTTP DELETE route.
		/// </summary>
		/// <value>
		/// The HTTP DELETE route.
		/// </value>
		public string? DeleteRoute { get; set; }
	}
}