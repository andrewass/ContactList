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
            FillList();
        }


        /* Create a new SqlCommand object based on a query string. 
         * Parameter values are taken from the textboxes on the contact list page*/
        private SqlCommand GetSqlCommand(string queryString) {
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@firstname", firstName.Text);
            command.Parameters.AddWithValue("@lastname", lastName.Text);
            command.Parameters.AddWithValue("@phonenumber", phoneNumber.Text);
            command.Parameters.AddWithValue("@email", email.Text);
            return command;
        }



        /* Filling the repeat controller with all the data stored in the Contacts table */
        private void FillList() {
            connection.Open();
            string queryString = "select * from Contacts";
            SqlCommand command = GetSqlCommand(queryString);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataSet dataset = new DataSet();
            adapter.Fill(dataset);
            repeater.DataSource = dataset;
            repeater.DataBind();
            connection.Close();
        }


        /* Search for a contact record based on first and last name.
         * If the contact record exists, the contact form is filled 
         * with data from the contact record */
        protected void searchButton_Click(object sender, EventArgs e) {
            FillContactForm(searchPhrase.Text.Split(null));
        }


        /* Save the user data to the database. If the user already exists,
         * update the record, else insert as a new user record */
        protected void saveButton_Click(object sender, EventArgs e) {
            if(RecordExists()) {
                UpdateRecord();
            }
            else {
                InsertRecord();
            }
            FillList();
        }


        /* If record of a contact is found, delete it from the Contacts table, 
         * else notify user that no such record is stored */
        protected void deleteButton_Click(object sender, EventArgs e) {
            if(RecordExists()) {
                DeleteRecord();
                FillList();
            }
            else {
                Response.Write("<script>alert('Unable to delete contact. Record not found')</script>");
            }
        }


        /* Fill the contact form with data from the corresponding record in the contact table */
        private void FillContactForm(string[] fullName) {
            connection.Open();
            string queryString = "select * from Contacts where Firstname = @firstname and Lastname = @lastname";
            SqlCommand command = new SqlCommand(queryString, connection);
            command.Parameters.AddWithValue("@firstname", fullName[0]);
            command.Parameters.AddWithValue("@lastname", fullName[1]);
            SqlDataReader reader = command.ExecuteReader();
            while(reader.Read()) {
                firstName.Text = (string)reader["Firstname"];
                lastName.Text = (string)reader["Lastname"];
                phoneNumber.Text = (string)reader["Phonenumber"];
                email.Text = (string)reader["Email"];
            }
            reader.Close();
            connection.Close();
        }


        /* Returns true or false, depending on the existence of a user record with the current data found
         * in the Textboxes */
        private bool RecordExists() {
            bool result = false;
            connection.Open();
            string queryString = "select count(1) from Contacts where Firstname = @firstname and Lastname = @lastname";
            SqlCommand command = GetSqlCommand(queryString);
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


        /* Insert a new record into the contacts table */
        private void InsertRecord() {
            connection.Open();
            string queryString = "insert into contacts (Firstname, Lastname, Phonenumber, Email) " +
                "values (@firstname, @lastname, @phonenumber, @email)";
            SqlCommand command = GetSqlCommand(queryString);
            command.ExecuteNonQuery();
            connection.Close();
        }


        /* Update phone number or mail adress for a record in the contacts table */
        private void UpdateRecord() {
            connection.Open();
            string queryString = "update contacts " +
                "set Phonenumber = @phonenumber, Email = @email " +
                "where Firstname = @firstname and Lastname = @lastname";
            SqlCommand command = GetSqlCommand(queryString);
            command.ExecuteNonQuery();
            connection.Close();
        }


        /* Delete a record from the contacts table */
        private void DeleteRecord() {
            connection.Open();
            string queryString = "delete from Contacts where Firstname = @firstname and Lastname = @lastname";
            SqlCommand command = GetSqlCommand(queryString);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}