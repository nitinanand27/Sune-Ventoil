using DotSee.Models;
using DotSee.UmbracoExtensions;
using System;
using System.Text;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;

namespace DotSee.Controllers
{
    /// <summary>
    /// Summary description for ContactController
    /// </summary>
    public class ContactSurfaceController : SurfaceController
    {
        public ActionResult Index()
        {
            return PartialView("ContactView", new ContactViewModel());
        }

        [HttpPost]
        [NotChildAction]
        public ActionResult Submit(ContactViewModel form)
        {

            if (!ModelState.IsValid)
            {
                return Json(new { Success = false });
            }

            SectionContact currSection = (SectionContact)Umbraco.TypedContent(form.CurrentSectionId);

            string mailTo = currSection.AdministratorsEmail;
            string mailFrom = currSection.NotificationMailFrom;
            string mailFromAlias = currSection.NotificationMailSenderAlias;
            string message = string.Concat(currSection.NotificationEmailBody, "<br/><br/>", umbraco.library.StripHtml(form.Message));

            //Use predefined subject. If predefined subject is not defined, use current subject from form (if any)
            string subject = (string.IsNullOrEmpty(currSection.NotificationEmailSubject)) ? form.Subject : currSection.NotificationEmailSubject;

            //If all else fails, use standard subject from dictionary.
            subject = (string.IsNullOrEmpty(subject)) ? Umbraco.GetDictionaryValue("ContactForm.Subject") : subject;

            StringBuilder sb = new StringBuilder(string.Empty);

            if (!string.IsNullOrEmpty(form.Name))
            {
                sb.Append("Name:");
                sb.Append("<br/>");
                sb.Append(form.Name);
                sb.Append("<br/><br/>");
            }

            if (!string.IsNullOrEmpty(form.Subject))
            {
                sb.Append("Subject:");
                sb.Append("<br/>");
                sb.Append(form.Subject);
                sb.Append("<br/><br/>");
            }

            sb.Append("Email: ");
            sb.Append("<br/>");
            sb.Append(form.Email);
            sb.Append("<br/><br/>");

            sb.Append("Message: ");
            sb.Append("<br/>");
            sb.Append(umbraco.library.ReplaceLineBreaks(message));
            sb.Append("<br/><br/>");

            if (!string.IsNullOrEmpty(currSection.Dropdown1Values))
            {
                sb.Append(currSection.Dropdown1Values.Split(new string[] { "\n", "\n\r" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                sb.Append("<br/>");
                sb.Append(form.DropDown1);
                sb.Append("<br/><br/>");
            }

            if (!string.IsNullOrEmpty(currSection.Dropdown2Values))
            {
                sb.Append(currSection.Dropdown2Values.Split(new string[] { "\n", "\n\r" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                sb.Append("<br/>");
                sb.Append(form.DropDown2);
                sb.Append("<br/><br/>");
            }

            if (ModelState.IsValid)
            {
                Utils.SendEmail(mailFrom, mailFromAlias, mailTo, subject, sb.ToString());
            }

            return Json(new { Success = true });
        }
    }
}