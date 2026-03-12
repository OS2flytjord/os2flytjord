using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Moq;

namespace Niras.Jordflytning.IntegrationTests.TestsSetup
{
	public static class MockHelpers
	{
		public static HttpContextBase FakeHttpContext(MockHttpSession session)
		{
			var browser = new Mock<HttpBrowserCapabilitiesBase>(MockBehavior.Strict);
			var context = new Mock<HttpContextBase>(MockBehavior.Strict);
			var request = new Mock<HttpRequestBase>(MockBehavior.Strict);
			var response = new Mock<HttpResponseBase>(MockBehavior.Strict);
			var server = new Mock<HttpServerUtilityBase>(MockBehavior.Strict);
			var cookies = new HttpCookieCollection();
			var items = new ListDictionary();

			browser.Setup(b => b.IsMobileDevice).Returns(false);

			request.Setup(r => r.Cookies).Returns(cookies);
			request.Setup(r => r.ValidateInput());
			request.Setup(r => r.UserAgent).Returns("Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.64 Safari/537.11");
			response.Setup(r => r.Cookies).Returns(cookies);

			request.Setup(r => r.Browser).Returns(browser.Object);
			context.Setup(ctx => ctx.Items).Returns(items);

			context.SetupGet(ctx => ctx.Request).Returns(request.Object);
			context.SetupGet(ctx => ctx.Response).Returns(response.Object);
			context.SetupGet(ctx => ctx.Session).Returns(session);
			context.SetupGet(ctx => ctx.Server).Returns(server.Object);

			return context.Object;
		}

		public static void SetFakeControllerContext(this Controller controller, RouteData route, Dictionary<string, object> session)
		{
			var httpContext = FakeHttpContext(new MockHttpSession(session));

			var context = new ControllerContext(new RequestContext(httpContext, route), controller);

			controller.ControllerContext = context;
		}
	}

	public class MockHttpSession : HttpSessionStateBase
	{
		Dictionary<String, object> _sessionState = new Dictionary<string, object>();

		public MockHttpSession(Dictionary<string, object> session)
		{
			_sessionState = session;
		}

		public override object this[string name]
		{
			get
			{
				return _sessionState.ContainsKey(name) ? _sessionState[name] : null;
			}
			set
			{
				_sessionState[name] = value;
			}
		}
	}
}
