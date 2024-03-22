using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyShoeStudio.Data.Models
{
    public class PersonalInfo
    {
    
        
        public int Id { get; set; }

        
        public string FirstName { get; set; }=string.Empty;

        public string LastName { get; set; } = string.Empty;

       
        public string PhoneNumber { get; set; } = string.Empty;

       
        public string Address { get; set; }=string.Empty;

       
        public string ZipCode { get; set; } = string.Empty;

       
        public string City { get; set; } = string.Empty;

        public string PaymentInfo { get; set; }=string.Empty;
        public string UserId { get; set; }

        public User User { get; set; }=new User();

       
    }

}
