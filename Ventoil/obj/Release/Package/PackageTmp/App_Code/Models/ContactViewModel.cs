using System.ComponentModel.DataAnnotations;

namespace DotSee.Models
{
    /// <summary>
    /// Summary description for ReservationViewModel
    /// </summary>
    public class ContactViewModel
    {
       
        public string Name { get; set; }
        public int CurrentSectionId { get; set; }
        public string Subject { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$")]
        public string Email { get; set; }
        public string DropDown1 { get; set; }
        public string DropDown2 { get; set; }
    }
}