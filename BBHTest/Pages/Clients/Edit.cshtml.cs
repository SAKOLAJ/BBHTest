using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
namespace BBHTest.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String succesMessage = "";
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=TestDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Client_info WHERE id= @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.Client_name = reader.GetString(1);
                                clientInfo.Client_surname = reader.GetString(2);
                                clientInfo.Client_phone_num = reader.GetString(3);
                                clientInfo.Client_email = reader.GetString(4);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
        }

        public void OnPost()
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.Client_name = Request.Form["Client_name"];
            clientInfo.Client_surname = Request.Form["Client_surname"];
            clientInfo.Client_phone_num = Request.Form["Client_phone_num"];
            clientInfo.Client_email = Request.Form["Client_email"];
            clientInfo.Client_password = Request.Form["Client_password"];

            if (clientInfo.id.Length == 0 || clientInfo.Client_name.Length == 0 || clientInfo.Client_surname.Length == 0 ||
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
                    {
                        String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=TestDB;Integrated Security=True";
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            String sql = "UPDATE Client_info"
                                + " SET Client_name=@Client_name, Client_surname=@Client_surname, Client_phone_num=@Client_phone_num, Client_email=@Client_email, Client_password=@Client_password "
                                + "WHERE id=@id";

                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@Client_name", clientInfo.Client_name);
                                command.Parameters.AddWithValue("@Client_surname", clientInfo.Client_surname);
                                command.Parameters.AddWithValue("@Client_phone_num", clientInfo.Client_phone_num);
                                command.Parameters.AddWithValue("@Client_email", clientInfo.Client_email);
                                command.Parameters.AddWithValue("@password", clientInfo.Client_password);
                                command.Parameters.AddWithValue("@id", clientInfo.id);


                                command.ExecuteNonQuery();
                                succesMessage = "all done!";
                                return;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    errorMessage = ex.Message;
                    return;
                }

                Response.Redirect("/Index");
            }
        }
    }
}
