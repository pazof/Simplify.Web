﻿using System;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using Simplify.Web.Modules;

namespace Simplify.Web.Tests.Modules
{
	[TestFixture]
	public class RedirectorTests
	{
		private Mock<IWebContext> _context = null!;
		private Redirector _redirector = null!;
		private Mock<IResponseCookies> _responseCookies = null!;

		[SetUp]
		public void Initialize()
		{
			_context = new Mock<IWebContext>();
			_redirector = new Redirector(_context.Object);
			_responseCookies = new Mock<IResponseCookies>();

			_context.Setup(x => x.Response.Redirect(It.IsAny<string>()));

			_context.SetupGet(x => x.Request.Scheme).Returns("http");
			_context.SetupGet(x => x.Request.Host).Returns(new HostString("localhost"));
			_context.SetupGet(x => x.Request.PathBase).Returns("/mywebsite");
			_context.SetupGet(x => x.Request.Path).Returns("/myaction?=foo");
			_context.SetupGet(x => x.SiteUrl).Returns("http://localhost/mywebsite/");

			_context.SetupGet(x => x.Response.Cookies).Returns(_responseCookies.Object);
			_context.SetupGet(x => x.Request.Cookies).Returns(Mock.Of<IRequestCookieCollection>());
		}

		[Test]
		public void Redirect_NullUrl_ArgumentNullExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>(() => _redirector.Redirect(null));
		}

		[Test]
		public void Redirect_NormalUrl_ResponseRedirectCalled()
		{
			// Act
			_redirector.Redirect("http://testwebsite.com");

			// Assert
			_context.Verify(x => x.Response.Redirect(It.Is<string>(c => c == "http://testwebsite.com")), Times.Once);
		}

		[Test]
		public void Redirect_ToRedirectUrlHaveRedirectUrl_RedirectCalledWithCorrectLinkPreviousNavigatedUrlSet()
		{
			// Assign

			var cookieCollection = new Mock<IRequestCookieCollection>();
			cookieCollection.SetupGet(x => x[It.Is<string>(s => s == Redirector.RedirectUrlCookieFieldName)]).Returns("foo");
			_context.SetupGet(x => x.Request.Cookies).Returns(cookieCollection.Object);

			_responseCookies.Setup(x => x.Append(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((key, value) =>
			{
				Assert.AreEqual(Redirector.PreviousNavigatedUrlCookieFieldName, key);
				Assert.AreEqual("http://localhost/mywebsite/myaction%3F=foo", value);
			});

			// Act
			_redirector.Redirect(RedirectionType.RedirectUrl);

			// Assert
			_context.Verify(x => x.Response.Redirect(It.Is<string>(c => c == "foo")), Times.Once);
		}

		[Test]
		public void Redirect_ToRedirectLinkNoUrl_RedirectCalledToSiteVirtualPath()
		{
			// Act
			_redirector.Redirect(RedirectionType.RedirectUrl);

			// Assert
			_context.Verify(x => x.Response.Redirect(It.Is<string>(c => c == "http://localhost/mywebsite/")), Times.Once);
		}

		[Test]
		public void Redirect_ToLoginReturnUrl_NoUrl_SiteUrl()
		{
			// Act
			_redirector.Redirect(RedirectionType.LoginReturnUrl);

			// Assert
			_context.Verify(x => x.Response.Redirect(It.Is<string>(c => c == "http://localhost/mywebsite/")), Times.Once);
		}

		[Test]
		public void Redirect_ToLoginReturnUrl_NotNullOrEmpty_LoginReturnUrl()
		{
			// Assign

			var cookieCollection = new Mock<IRequestCookieCollection>();
			cookieCollection.SetupGet(x => x[It.Is<string>(s => s == Redirector.LoginReturnUrlCookieFieldName)]).Returns("loginFoo");
			_context.SetupGet(x => x.Request.Cookies).Returns(cookieCollection.Object);

			// Act
			_redirector.Redirect(RedirectionType.LoginReturnUrl);

			// Assert
			_context.Verify(x => x.Response.Redirect(It.Is<string>(c => c == "loginFoo")), Times.Once);
		}

		[Test]
		public void Redirect_ToPreviousPageHavePreviousPageUrl_RedirectCalledWithCorrectUrl()
		{
			// Arrange

			var cookieCollection = new Mock<IRequestCookieCollection>();
			cookieCollection.SetupGet(x => x[It.Is<string>(s => s == Redirector.PreviousPageUrlCookieFieldName)]).Returns("foo");
			_context.SetupGet(x => x.Request.Cookies).Returns(cookieCollection.Object);

			// Act
			_redirector.Redirect(RedirectionType.PreviousPage);

			// Assert
			_context.Verify(x => x.Response.Redirect(It.Is<string>(c => c == "foo")), Times.Once);
		}

		[Test]
		public void Redirect_ToPreviousPageWithBookmarkHaveUrl_RedirectCalledWithCorrectBookmarkUrl()
		{
			// Assign

			var cookieCollection = new Mock<IRequestCookieCollection>();
			cookieCollection.SetupGet(x => x[It.Is<string>(s => s == Redirector.PreviousPageUrlCookieFieldName)]).Returns("foo");
			_context.SetupGet(x => x.Request.Cookies).Returns(cookieCollection.Object);

			// Act
			_redirector.Redirect(RedirectionType.PreviousPageWithBookmark, "bar");

			// Assert
			_context.Verify(x => x.Response.Redirect(It.Is<string>(c => c == "foo#bar")), Times.Once);
		}

		[Test]
		public void Redirect_ToCurrentPage_RedirectCalledToCurrentPage()
		{
			// Act
			_redirector.Redirect(RedirectionType.CurrentPage);

			// Assert
			_context.Verify(x => x.Response.Redirect(It.Is<string>(c => c == "http://localhost/mywebsite/myaction%3F=foo")), Times.Once);
		}

		[Test]
		public void Redirect_ToDefaultPage_RedirectCalledToDefaultPage()
		{
			// Act
			_redirector.Redirect(RedirectionType.DefaultPage);

			// Assert
			_context.Verify(x => x.Response.Redirect(It.Is<string>(c => c == "http://localhost/mywebsite/")), Times.Once);
		}

		[Test]
		public void SetRedirectUrlToCurrentPage_NormalUrl_Set()
		{
			// Assign
			_responseCookies.Setup(x => x.Append(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((key, value) =>
			{
				Assert.AreEqual(Redirector.RedirectUrlCookieFieldName, key);
				Assert.AreEqual("http://localhost/mywebsite/myaction%3F=foo", value);
			});

			// Act
			_redirector.SetRedirectUrlToCurrentPage();
		}

		[Test]
		public void SetLoginReturnUrlFromQuery_NormalUrl_Set()
		{
			// Assign

			_context.SetupGet(x => x.Request.Path).Returns("/foo2");

			_responseCookies.Setup(x => x.Append(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((key, value) =>
			{
				Assert.AreEqual(Redirector.LoginReturnUrlCookieFieldName, key);
				Assert.AreEqual("http://localhost/mywebsite/foo2", value);
			});

			// Act
			_redirector.SetLoginReturnUrlFromCurrentUri();
		}

		[Test]
		public void GetPreviousPageUrl_Return()
		{
			// Assign

			var cookieCollection = new Mock<IRequestCookieCollection>();
			cookieCollection.SetupGet(x => x[It.Is<string>(s => s == Redirector.PreviousPageUrlCookieFieldName)]).Returns("foo");
			_context.SetupGet(x => x.Request.Cookies).Returns(cookieCollection.Object);

			// Act & Assert
			Assert.AreEqual("foo", _redirector.PreviousPageUrl);
		}

		[Test]
		public void GetPreviousNavigatedUrl_Return()
		{
			// Assign
			var cookieCollection = new Mock<IRequestCookieCollection>();
			cookieCollection.SetupGet(x => x[It.Is<string>(s => s == Redirector.PreviousNavigatedUrlCookieFieldName)]).Returns("foo");
			_context.SetupGet(x => x.Request.Cookies).Returns(cookieCollection.Object);

			// Act & Assert
			Assert.AreEqual("foo", _redirector.PreviousNavigatedUrl);
		}

		[Test]
		public void SetPreviousPageUrl_Set()
		{
			// Assign
			_responseCookies.Setup(x => x.Append(It.IsAny<string>(), It.IsAny<string>())).Callback<string, string>((key, value) =>
			{
				Assert.AreEqual(Redirector.PreviousPageUrlCookieFieldName, key);
				Assert.AreEqual("foo", value);
			});

			// Act & Assert
			_redirector.PreviousPageUrl = "foo";
		}
	}
}