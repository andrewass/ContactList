using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
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


        /* If record of a contact is found, delete it from the Contacts table, 
         * else notify user that no such record is stored */
        protected void deleteButton_Click(object sender, EventArgs e) {
            string confirmedDelete = Request.Form["confirmedDelete"];
            bool foundRecord = RecordExists();
            if(foundRecord && confirmedDelete.Equals("yes")){
                ModifyTable(deleteQuery);
                FillList();
            }
            if(!foundRecord) { 
                Response.Write("<script>alert('Unable to delete contact. Record not found')</script>");
            }
        }


        /* Prior to any database modification, verify that the e-mail 
        * and phone number is in a valid format. If valid, save the 
        * user data to the database. If the user already exists,
        * update the record, else insert as a new user record */
        protected void saveButton_Click(object sender, EventArgs e) {
            if(!IsValidEmail(email.Text) || !IsValidPhoneNumber(phoneNumber.Text)) {
                Response.Write("<script>alert('Invalid input data!')</script>");
                return;
            }
            if(RecordExists()) {
                ModifyTable(updateQuery);
            }
            else {
                ModifyTable(insertQuery);
            }
            FillList();
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


        /* Verify if a string representing an e-mail address is in a valid form. 
         * Source : Microsoft Developer Network documentation */
        private bool IsValidEmail(string email) {
            return Regex.IsMatch(email,
                    @"^(?("")(""[^""]+?""@)|(([0-9a-zA-Z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-zA-Z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,6}))$");
        }


        /* Verify if a string representing a phone number is in a valid form. 
         * Allowing a prefix of '+' for country codes. */
        private bool IsValidPhoneNumber(string phoneNumber) {
            int startIndex;
            if(phoneNumber[0] == '+') {
                startIndex = 1;
            }
            else {
                startIndex = 0;
            }
            //Setting minimum limit of 3 digits
            if(phoneNumber.Length-startIndex < 3) {
                return false;
            }
            //Verify that each remaining character represents a digit
            for(int i=startIndex; i<phoneNumber.Length; i++) {
                if(!Char.IsDigit(phoneNumber[i])) {
                    return false;
                }
            }
            return true;
        }
    }
}