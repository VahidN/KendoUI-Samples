using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Mvc4TestViewModel.Models
{
    public class OrderDetailViewModel
    {
        [HiddenInput(DisplayValue = false)]
        public int OrderDetailId { get; set; }

        [Required(ErrorMessage = "لطفا فیلد منبع را وارد کنید")]
        public string Origin { get; set; }

        [DisplayName("Net Wt")]
        [Range(0, 20)]
        public decimal NetWeight { get; set; }

        [DisplayName("Value Date")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime ValueDate { get; set; }

        [StringLength(15)]
        [Required]
        public string Destination { get; set; }

        //[Moon.Web.Validation.RemoteValidator("DoesUserExist", "Home",
            //HttpMethod = "POST", ErrorMessage = "نام کاربری قبلا ثبت شده است.")]

        [Remote("DoesUserExist", "Home", HttpMethod = "POST",
            ErrorMessage = "این نام از قبل در سیستم وجود دارد.")]
        public string Username { get; set; }
    }
}