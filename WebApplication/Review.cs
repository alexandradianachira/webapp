//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public int id_review { get; set; }
        public int id_paper_assignment { get; set; }
        public int grade { get; set; }
        public int confidence { get; set; }
        public string comment { get; set; }
        public string comment_to_edit { get; set; }
        public string date_submitted { get; set; }
        public string from_subreviewer { get; set; }
    
        public virtual PaperAssignment PaperAssignment { get; set; }
    }
}