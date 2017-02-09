using System.ComponentModel.DataAnnotations;

namespace DotSee.Models
{
    public class NewsletterSubscription
    {
        [Required(ErrorMessage = "Required!")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$")]
        public string EmailAddress { get; set; }
        public string PageId { get; set; }
     
    }
}


