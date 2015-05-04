using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mvc4TestViewModel.Models
{
    public class OrderDetailViewModel
    {
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
    }
}