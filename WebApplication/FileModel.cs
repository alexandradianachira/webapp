using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication
{
    public class FileModel
    {
        [RegularExpression(@"([a-zA-Z0-9\s_\\.\-:])+(.pdf)$", ErrorMessage = "Only pdf files allowed.")]
        public HttpPostedFileBase pdf { get; set; }
    }
}