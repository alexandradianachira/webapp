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
    
    public partial class Subreview
    {
        public int id_subreview { get; set; }
        public int id_subreviewer { get; set; }
        public double grade { get; set; }
        public string confidence { get; set; }
        public string comment { get; set; }
        public string comment_to_edit { get; set; }
        public System.DateTime date_submitted { get; set; }
    
        public virtual Subreviewer Subreviewer { get; set; }
    }
}
