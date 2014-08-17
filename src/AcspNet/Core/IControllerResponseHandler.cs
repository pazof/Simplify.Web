﻿using Simplify.DI;

namespace AcspNet.Core
{
	/// <summary>
	/// Represent controller response handler
	/// </summary>
	public interface IControllerResponseHandler
	{
		/// <summary>
		/// Processes the specified response.
		/// </summary>
		/// <param name="response">The response.</param>
		/// <param name="containerProvider">The DI container provider.</param>
		ControllerResponseResult Process(ControllerResponse response, IDIContainerProvider containerProvider);
	}
}