using System;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Script.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UrlShortener.Web;
using UrlShortener.Web.Controllers;

namespace UrlShortener.Web.Tests
{
    [TestClass]
    public class UnitTest
    {
        public class Result
        {
            public bool Redirect;
            public string RedirectUrl;
        }

        [TestMethod]
        public void ShortenUrl()
        {
            //Arrange
            var request = new Mock<System.Web.HttpRequestBase>();
            request.SetupGet(x => x.Headers)
                   .Returns(
                    new System.Net.WebHeaderCollection
                     {
                         {"X-Requested-With", "XMLHttpRequest"}
                     });
            
            request.SetupGet(x => x.Url).Returns(new Uri("http://localhost:62243/"));

            var context = new Mock<System.Web.HttpContextBase>();
            context.SetupGet(x => x.Request).Returns(request.Object);

            var controller = new HomeController();
            controller.ControllerContext = new ControllerContext(context.Object, new RouteData(), controller);

            //Act
            var controllerRequest = controller.ShortenUrl("http://www.mydeal.com.au/electronics");
            
            //Assert
            var serializer = new JavaScriptSerializer();
            var result =  serializer.Deserialize<Result>(serializer.Serialize(((JsonResult) controllerRequest.Result).Data));
            Assert.IsTrue(result.Redirect==true);

        }
    }
}
