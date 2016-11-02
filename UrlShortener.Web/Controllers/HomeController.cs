using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using UrlShortener.Data;
using UrlShortenerHelper= UrlShortener.Web.Helpers.UrlHelper;
using UrlShortener.Web.Repository;

namespace UrlShortener.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly UnitOfWork _unitOfWork;
        public HomeController()
        {
            this._unitOfWork = new UnitOfWork();
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ShortenUrl(string url)
        {
            try
            {
                // Validate Url
                if (!UrlShortenerHelper.IsUrlValid(url))
                {
                    return Json(new { redirect = false, message = UrlShortenerHelper.InvalidUrlMessage }, JsonRequestBehavior.AllowGet);
                }
                if (!UrlShortenerHelper.IsUrlActive(url) || Request.Url == null)
                {
                    return Json(new { redirect = false, message = UrlShortenerHelper.InactiveUrlMessage },JsonRequestBehavior.AllowGet);
                }
                
                // Search url in db
                var urlMatch = _unitOfWork.UrlRepository.SearchFor(u => u.LongUrl.ToLower()==url.ToLower()).FirstOrDefault();

                // If a match is found , return existing Url
                if (urlMatch != null && Request.Url !=null)
                {
                    var matchingUrl = string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, urlMatch.ShortUrl);
                    return Json(new { redirect = true, redirectUrl = matchingUrl }, JsonRequestBehavior.AllowGet);
                }
                
                // If a match is not found , create a new entry
                var newUrl = new Url
                {
                    CreatedDate = DateTime.Now,
                    LongUrl = url,
                    ShortUrl = Helpers.UrlHelper.GenerateShortUrl(url),
                };
                await _unitOfWork.UrlRepository.InsertAsync(newUrl);

                var redirectUrl = string.Format("{0}://{1}/{2}", Request.Url.Scheme, Request.Url.Authority, newUrl.ShortUrl);
                return Json(new { redirect = true, redirectUrl }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // logging needs to be done.
                return RedirectToAction("Error", "Home");
            }
        }



        public ActionResult RedirectUrl(string shortUrl)
        {
            try
            {

                // Validate Url
                if (UrlShortenerHelper.IsUrlValid(shortUrl))
                {
                    return RedirectToAction("RedirectNotFound", "Home");
                }

                // Search url in db
                var urlMatch = _unitOfWork.UrlRepository.SearchFor(u => u.ShortUrl.ToLower() == shortUrl.ToLower()).FirstOrDefault();

                if (urlMatch != null)
                {
                    Response.StatusCode = (int)HttpStatusCode.Redirect;
                    return Redirect(urlMatch.LongUrl);
                }
                else
                {
                    return RedirectToAction("RedirectNotFound", "Home");
                }
            }
            catch (Exception ex)
            {
                // logging needs to be done.
                return RedirectToAction("Error", "Home");
            }


        }


        public ActionResult RedirectNotFound()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Notes()
        {
            return View();
        }
    }
}