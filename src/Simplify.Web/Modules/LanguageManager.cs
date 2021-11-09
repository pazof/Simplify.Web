using System;
using System.Globalization;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Simplify.Web.Settings;

namespace Simplify.Web.Modules
{
	/// <summary>
	/// Current language controller and information container
	/// </summary>
	public class LanguageManager : ILanguageManager
	{
		/// <summary>
		/// Language field name in user cookies
		/// </summary>
		public const string CookieLanguageFieldName = "language";

		private readonly IResponseCookies _responseCookies;

		/// <summary>
		/// Initializes a new instance of the <see cref="LanguageManager" /> class.
		/// </summary>
		/// <param name="settings">The settings.</param>
		/// <param name="context">The OWIN context.</param>
		public LanguageManager(ISimplifyWebSettings settings, HttpContext context)
		{
			_responseCookies = context.Response.Cookies;

			if (TrySetLanguageFromCookie(context))
				return;

			Console.WriteLine($"Default lang: '{settings.DefaultLanguage}'");

			foreach (var c in CultureInfo.GetCultures(CultureTypes.AllCultures))
				Console.WriteLine($"Culture: '{c}'");

			if (!settings.AcceptBrowserLanguage || (settings.AcceptBrowserLanguage && !TrySetLanguageFromRequestHeader(context)))
				SetCurrentLanguage(settings.DefaultLanguage);
		}

		/// <summary>
		/// Site current language, for example: "en", "ru", "de" etc.
		/// </summary>
		public string Language { get; private set; } = null!;

		/// <summary>
		/// Set site cookie language value
		/// </summary>
		/// <param name="language">Language code</param>
		public void SetCookieLanguage(string? language)
		{
			if (string.IsNullOrEmpty(language))
				throw new ArgumentNullException(nameof(language));

			_responseCookies.Append(CookieLanguageFieldName, language, new CookieOptions
			{
				Expires = DateTime.Now.AddYears(5),
				SameSite = SameSiteMode.None,
				Secure = true
			});
		}

		/// <summary>
		/// Set language only for current request
		/// </summary>
		/// <param name="language">Language code</param>
		public bool SetCurrentLanguage(string language)
		{
			Console.WriteLine($"Set lang: '{language}'");

			try
			{
				Console.WriteLine($"TwoLetterISOLanguageName lang: '{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}'");
				Console.WriteLine($"Thread.CurrentThread.CurrentCulture.DisplayName lang: '{Thread.CurrentThread.CurrentCulture.DisplayName}'");

				Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
				Thread.CurrentThread.CurrentCulture = new CultureInfo(language);

				Console.WriteLine($"TwoLetterISOLanguageName lang: '{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}'");
				Console.WriteLine($"Thread.CurrentThread.CurrentCulture.DisplayName lang: '{Thread.CurrentThread.CurrentCulture.DisplayName}'");
				Language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine($"Exception: '{e.Message}'");
				return false;
			}
		}

		private bool TrySetLanguageFromCookie(HttpContext context)
		{
			var cookieLanguage = context.Request.Cookies[CookieLanguageFieldName];

			Console.WriteLine($"Cookie lang: '{cookieLanguage}'");

			return !string.IsNullOrEmpty(cookieLanguage) && SetCurrentLanguage(cookieLanguage);
		}

		private bool TrySetLanguageFromRequestHeader(HttpContext context)
		{
			var languages = context.Request.Headers["Accept-Language"];

			if (languages.Count == 0)
				return false;

			var languageString = languages[0];

			Console.WriteLine($"Accept lang: '{languageString}'");

			var items = languageString.Split(';');

			return SetCurrentLanguage(items[0]);
		}
	}
}