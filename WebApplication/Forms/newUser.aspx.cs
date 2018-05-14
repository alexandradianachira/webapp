using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class newUser : System.Web.UI.Page
    {
       

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submitNewUser_onClick(object sender, EventArgs e)
        {
            string surname = Request.Form["surname"];
            string first_name=Request.Form["first_name"];
            string email = Request.Form["email"];
            string institution = Request.Form["institution"];
            //var db = new User();
            //var insertCommand = "INSERT INTO Movies (Title, Genre, Year) VALUES(@0, @1, @2)";
            //db.Execute(insertCommand, title, genre, year);


        }
    }
}