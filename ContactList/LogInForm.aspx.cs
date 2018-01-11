using System;
using System.Data.SqlClient;
using System.Web.UI;

namespace ContactList {

    public partial class LogInForm : Page {

        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=Randombase;Trusted_Connection=True";
        private SqlConnection connection;

        protected void Page_Load(object sender, EventArgs e) {
            connection = new SqlConnection(connectionString);
        }


        /* Redirect to the contact list page if the username and password combination
         * exists in the database */
        protected void submitLogIn_Click(object sender, EventArgs e) {
            if(CorrectLogIn(username.Text, password.Text)) {
                Response.Redirect("ContactForm.aspx");
            }
            else {}
        }


        /* Returns true if a pair of username and password exists in the database,
         * else false is returned */
        private bool CorrectLogIn(string username, string password) {
            bool result = false;
            connection.Open();
            SqlCommand command = new SqlCommand("select count(1) from Users where Username = @username and password = @password", connection);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", password);
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read()) {
                if((int)reader[0] == 1) {
                    result = true;
                }
            }
            reader.Close();
            connection.Close();
            return result;
        }

    }
}