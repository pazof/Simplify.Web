﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Simplify.DI;
using Simplify.Web.Model.Binding;
using Simplify.Web.Model.Validation;
using Simplify.Web.Modules;

namespace Simplify.Web.Model
{
	/// <summary>
	/// Provides model handling
	/// </summary>
	public class HttpModelHandler : IModelHandler
	{
		private readonly IWebContext _context;

		private object? _model;

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpModelHandler"/> class.
		/// </summary>
		/// <param name="context">The context.</param>
		public HttpModelHandler(IWebContext context) => _context = context;

		/// <summary>
		/// Gets the model binders types.
		/// </summary>
		/// <value>
		/// The model binders types.
		/// </value>
		// ReSharper disable once ConvertToAutoProperty
		public static IList<Type> ModelBindersTypes { get; } = new List<Type>
		{
			// Default model binders

			typeof (HttpQueryModelBinder),
			typeof (HttpFormModelBinder)
		};

		/// <summary>
		/// Gets the model validators types.
		/// </summary>
		/// <value>
		/// The model validators types.
		/// </value>
		// ReSharper disable once ConvertToAutoProperty
		public static IList<Type> ModelValidatorsTypes { get; } = new List<Type>
		{
			// Default model validators

			typeof (ValidationAttributesExecutor)
		};

		/// <summary>
		/// Gets a value indicating whether model has been processed (parsed/validated).
		/// </summary>
		public bool Processed { get; private set; }

		/// <summary>
		/// Adds the model binder into model binders list, this type should be registered in Simplify.DI container.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static void RegisterModelBinder<T>()
			where T : IModelBinder =>
			ModelBindersTypes.Add(typeof(T));

		/// <summary>
		/// Adds the model validator into model validators list, this type should be registered in Simplify.DI container.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		public static void RegisterModelValidator<T>()
			where T : IModelValidator =>
			ModelValidatorsTypes.Add(typeof(T));

		/// <summary>
		/// Parses model and validates it asynchronously
		/// </summary>
		/// <typeparam name="T">Model type</typeparam>
		/// <param name="resolver">The resolver.</param>
		/// <returns></returns>
		public async Task ProcessAsync<T>(IDIResolver resolver)
		{
			var args = new ModelBinderEventArgs<T>(_context);

			foreach (var binder in ModelBindersTypes.Select(binderType => (IModelBinder)resolver.Resolve(binderType)))
			{
				await binder.BindAsync(args);

				if (!args.IsBound)
					continue;

				Validate(args, resolver);

				_model = args.Model;
				Processed = true;

				return;
			}

			throw new ModelBindingException($"Unrecognized request content type for binding: {_context.Request.ContentType}");
		}

		/// <summary>
		/// Gets the model.
		/// </summary>
		/// <typeparam name="T">The model</typeparam>
		/// <returns></returns>
		/// <exception cref="InvalidOperationException">Error getting model, model should be processed via ProcessAsync&lt;T&gt; method first</exception>
		public T GetModel<T>()
		{
			if (_model == null)
				throw new InvalidOperationException("Error getting model, model should be processed via ProcessAsync<T> method first");

			return (T)_model;
		}

		private static void Validate<T>(ModelBinderEventArgs<T> args, IDIResolver resolver)
		{
			foreach (var validator in ModelValidatorsTypes.Select(x => (IModelValidator)resolver.Resolve(x)))
				validator.Validate(args.Model, resolver);
		}
	}
}