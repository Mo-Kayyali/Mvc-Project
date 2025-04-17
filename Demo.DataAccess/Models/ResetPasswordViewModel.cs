using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.DataAccess.Models
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage ="Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password Is Required")]
        [Compare(nameof(Password),ErrorMessage ="Password Not Matching")]
        public string ConfirmPassword { get; set; }
    }
}
