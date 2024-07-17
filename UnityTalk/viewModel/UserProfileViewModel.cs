using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UnityTalk.vireModel
{
    public class UserProfileViewModel
    {
        public int Uid { get; set; }

        [Required(ErrorMessage = "Enter your Full Name")]
        [Display(Name = "Full Name")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Enter your Email address")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Please enter a valid Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Enter a UserName")]
        [Display(Name = "User Name")]
        [StringLength(maximumLength: 10, MinimumLength = 5, ErrorMessage = "User Name Length Must Be Max 10 & Min 5")]
        public string UserName { get; set; }

        public bool UserStatus { get; set; }
        public string UserImgPath { get; set; }

        public HttpPostedFileBase UserImg { get; set; }
    }
}

