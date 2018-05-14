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
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.PCmembers = new HashSet<PCmember>();
            this.Subreviewers = new HashSet<Subreviewer>();
            this.Authors = new HashSet<Author>();
        }
    
        public int id_user { get; set; }
        public string surname { get; set; }
        public string first_name { get; set; }
        public string email { get; set; }
        public string institution { get; set; }
        public string password { get; set; }
        public System.DateTime verified_account { get; set; }
        public System.DateTime date_verification_send { get; set; }
        public System.DateTime date_active { get; set; }
        public string verification_key { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PCmember> PCmembers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Subreviewer> Subreviewers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Author> Authors { get; set; }

        public String confirmPassword { get; set; }

        public String text { get; set; }
    }
}