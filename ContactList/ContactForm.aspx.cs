using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace ContactList {
    public partial class ContactForm : Page {

        private int fieldCount = 4;
        private string connectionString = "Server=localhost\\SQLEXPRESS;Database=Randombase;Trusted_Connection=True";
        private string[] fields = { "@firstname", "@lastname", "@phonenumber", "@email" };
        private string[] fieldvalues;

        private string deleteQuery = "delete from Contacts where Firstname = @firstname and Lastname = @lastname";

        private string updateQuery = "update contacts set Phonenumber = @phonenumber, Email = @email " +
                    "where Firstname = @firstname and Lastname = @lastname";

        private string insertQuery = "insert into contacts (Firstname, Lastname, Phonenumber, Email) " +
                   "values (@firstname, @lastname, @phonenumber, @email)";


        protected void Page_Load(object sender, EventArgs e) {
            fieldvalues = new string[fieldCount];
            FillList();
        }



        /* Fill the fieldvalues array with current values from the textboxes */
        private void SetFieldValues() {
            fieldvalues[0] = firstName.Text;
            fieldvalues[1] = lastName.Text;
            fieldvalues[2] = phoneNumber.Text;
            fieldvalues[3] = email.Text;
        }



        /* Filling the repeat controller with all the data stored in the Contacts table */
        private void FillList() {
            using(SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();
                string queryString = "select * from Contacts";
                SqlCommand command = new SqlCommand(queryString, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataSet dataset = new DataSet();
                adapter.Fill(dataset);
                repeater.DataSource = dataset;
                repeater.DataBind();
            }
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
                ModifyTable(updateQuery);
            }
            else {
                ModifyTable(insertQuery);
            }
            FillList();
        }



        /* If record of a contact is found, delete it from the Contacts table, 
         * else notify user that no such record is stored */
        protected void deleteButton_Click(object sender, EventArgs e) {
            if(RecordExists()) {
                ModifyTable(deleteQuery);
                FillList();
            }
            else {
                Response.Write("<script>alert('Unable to delete contact. Record not found')</script>");
            }
        }



        /* Fill the contact form with data from the corresponding record in the contact table */
        private void FillContactForm(string[] fullName) {
            using(SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();
                string queryString = "select * from Contacts where Firstname = @firstname and Lastname = @lastname";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@firstname", fullName[0]);
                command.Parameters.AddWithValue("@lastname", fullName[1]);
                using(SqlDataReader reader = command.ExecuteReader()) {
                    while(reader.Read()) {
                        firstName.Text = reader["Firstname"].ToString();
                        lastName.Text = reader["Lastname"].ToString();
                        phoneNumber.Text = reader["Phonenumber"].ToString();
                        email.Text = reader["Email"].ToString();
                    }
                }
            }
        }



        /* Returns true or false, depending on the existence of a user record with the current data found
         * in the Textboxes */
        private bool RecordExists() {
            bool result = false;
            using(SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();
                string queryString = "select count(1) from Contacts where Firstname = @firstname and Lastname = @lastname";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.AddWithValue("@firstname", firstName.Text);
                command.Parameters.AddWithValue("@lastname", lastName.Text);
                int count = (int)command.ExecuteScalar();
                result = (count == 1) ? true : false;
            }
            return result;
        }



        /* Perform either an update, insert or delete operation on the contacts table, depending
         * on the queryString argument */
        private void ModifyTable(string queryString) {
            using(SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();
                SetFieldValues();
                SqlCommand command = new SqlCommand(queryString, connection);
                for(int i = 0; i < 4; i++) {
                    command.Parameters.AddWithValue(fields[i], fieldvalues[i]);
                }
                command.ExecuteNonQuery();
            }
        }
    }
}