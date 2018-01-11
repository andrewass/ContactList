using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace ContactList {
    public partial class ContactForm : Page {

        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=Randombase;Trusted_Connection=True";
        private SqlConnection connection;


        protected void Page_Load(object sender, EventArgs e) {
            connection = new SqlConnection(connectionString);
            fillList();
        }


        /* Filling the repeat controller with all the data stored in the Contacts table */
        private void fillList() {
            SqlCommand command = new SqlCommand("select * from Contacts", connection);
            connection.Open();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);
            repeater.DataSource = dataset;
            repeater.DataBind();
            connection.Close();
        }


        /* Search for a contact record based on first and last name */
        protected void searchButton_Click(object sender, EventArgs e) {
            SqlCommand command = new SqlCommand("select * from Contacts where Firstname = @firstname and Lastname = @lastname", connection);
            connection.Open();


        }


        /* Save the user data to the database. If the user already exists,
         * update the record, else insert as a new user record */
        protected void saveButton_Click(object sender, EventArgs e) {

        }
    }
}