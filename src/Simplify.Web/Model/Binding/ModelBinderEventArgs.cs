﻿#nullable disable

using System;
using Simplify.Web.Modules;

namespace Simplify.Web.Model.Binding
{
	/// <summary>
	/// Provides model binder event arguments
	/// </summary>
	public class ModelBinderEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ModelBinderEventArgs{T}"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public ModelBinderEventArgs(IWebContext context) => Context = context;

		/// <summary>
		/// Gets the model.
		/// </summary>
		/// <value>
		/// The model.
		/// </value>
		public T Model { get; private set; }

		/// <summary>
		/// Gets the context.
		/// </summary>
		/// <value>
		/// The context.
		/// </value>
		public IWebContext Context { get; }

		/// <summary>
		/// Gets a value indicating whether model was bound.
		/// </summary>
		/// <value>
		/// <c>true</c> if current model was bound; otherwise, <c>false</c>.
		/// </value>
		public bool IsBound { get; private set; }

		/// <summary>
		/// Sets current model.
		/// </summary>
		/// <param name="model">The model.</param>
		public void SetModel(T model)
		{
			Model = model;
			IsBound = true;
		}
	}
}