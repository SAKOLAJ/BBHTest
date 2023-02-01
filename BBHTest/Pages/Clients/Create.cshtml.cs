using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace BBHTest.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string succesMessage = "";
        public void OnGet()
        {
        }

        public void OnPost()
        {

            clientInfo.Client_name = Request.Form["Client_name"];
            clientInfo.Client_surname = Request.Form["Client_surname"];
            clientInfo.Client_phone_num = Request.Form["Client_phone_num"];
            clientInfo.Client_email = Request.Form["Client_email"];
            clientInfo.Client_password = Request.Form["Client_password"];

            if (clientInfo.Client_name.Length == 0 || clientInfo.Client_surname.Length == 0 ||
                clientInfo.Client_phone_num.Length == 0 || clientInfo.Client_email.Length == 0 || clientInfo.Client_password.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }


            bool IsValidEmail(string email)
            {
                email = Request.Form["Client_email"];
                var trimmedEmail = email.Trim();

                if (trimmedEmail.EndsWith("."))
                {

                    return false;
                }
                try
                {
                    var addr = new System.Net.Mail.MailAddress(email);
                    return addr.Address == trimmedEmail;


                }
                catch
                {
                    errorMessage = "wrong email format";
                    return false;
                }
            }
            if (IsValidEmail(clientInfo.Client_email))
            {

                try
                {
                    string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=TestDB;Integrated Security=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string sql = "INSERT INTO Client_info" + "(Client_name, Client_surname, Client_phone_num, Client_email, Client_password) VALUES" +
                            "(@Client_name, @Client_surname, @Client_phone_num, @Client_email, @Client_password);";

                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Client_name", clientInfo.Client_name);
                            command.Parameters.AddWithValue("@Client_surname", clientInfo.Client_surname);
                            command.Parameters.AddWithValue("@Client_phone_num", clientInfo.Client_phone_num);
                            command.Parameters.AddWithValue("@Client_email", clientInfo.Client_email);
                            command.Parameters.AddWithValue("@Client_password", clientInfo.Client_password);

                            command.ExecuteNonQuery();
                        }
                    }

                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return;
                }

                clientInfo.Client_name = ""; clientInfo.Client_surname = "";
                clientInfo.Client_phone_num = ""; clientInfo.Client_email = ""; clientInfo.Client_password = "";
                succesMessage = "New Client Added Correctly";

                Response.Redirect("/Clients/Index");
            }
            else
            {
                errorMessage = "Wrong email format";
                return;
            }
        }
    }
}

