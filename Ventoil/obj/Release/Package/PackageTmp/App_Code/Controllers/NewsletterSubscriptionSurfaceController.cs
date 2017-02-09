using DotSee.Models;
using MailChimp.Net;
using MailChimp.Net.Interfaces;
using MailChimp.Net.Models;
using System.Web.Mvc;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.Controllers
{
    public class NewsletterSubscriptionSurfaceController : Umbraco.Web.Mvc.SurfaceController
    {
        [HttpPost]
        [NotChildAction]
        public ActionResult SendMail(NewsletterSubscription model)
        {
            UmbracoHelper h = ContentHelper.GetHelper();
            string email = model.EmailAddress;
            string apiKey =
                h.TypedContent(model.PageId)
                    .AncestorOrSelf(1)
                    .Descendant<ConfigGlobalSettings>()
                    .MailChimpApikey;
            string listId = h.TypedContent(model.PageId)
                    .AncestorOrSelf(1)
                    .Descendant<ConfigGlobalSettings>()
                    .MailChimpListID;

            if (!ModelState.IsValid)
            {
                return Json(new { Success = false });
            }

            IMailChimpManager manager = new MailChimpManager(apiKey); 
            //var listId = listI" 0d3a461bf3";
            var member = new MailChimp.Net.Models.Member { EmailAddress = email, Status = Status.Subscribed };
            manager.Members.AddOrUpdateAsync(listId, member);

            return Json(new { Success = true });
        }
    }
}