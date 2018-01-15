using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace ContactList {

    public partial class LogInForm : Page {

        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=Randombase;Trusted_Connection=True";

        protected void Page_Load(object sender, EventArgs e) {}


        /* Redirect to the contact list page if the username and password combination
         * exists in the database */
        protected void submitLogIn_Click(object sender, EventArgs e) {
            if(CorrectLogIn(username.Text, password.Text)) {
                Response.Redirect("ContactForm.aspx");
            }
        }


        /* Returns true if a pair of username and password exists in the database,
         * else false is returned */
        private bool CorrectLogIn(string username, string password) {
            bool result = false;
            using(SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();
                string queryString = "select count(1) from Users where Username = @username and password = @password";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);
                int count = (int)command.ExecuteScalar();
                result = (count == 1) ? true : false;
            }
            return result;
        }
    }
}