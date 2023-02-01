using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Session;
using NuGet.Protocol.Plugins;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BBHTest.Pages.Clients
{
    public class LoginModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String succesMessage = "";
        
        public void OnGet()
        {

        }

        public void OnPost(object sender, EventArgs e)
        {
            clientInfo.Client_email = Request.Form["Client_email"];
            clientInfo.Client_password = Request.Form["Client_password"];
            

            if ( clientInfo.Client_email.Length == 0 || clientInfo.Client_password.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            try
            {
                {
                    string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=TestDB;Integrated Security=True";
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        String sql = "SELECT * FROM Client_info WHERE Client_email=@Client_email" + " AND Client_password=@Client_password";
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@Client_email", clientInfo.Client_email);
                            command.Parameters.AddWithValue("@Client_password", clientInfo.Client_password);
                            //SqlDataAdapter adapter = new SqlDataAdapter(command);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    clientInfo.id = "" + reader.GetInt32(0);
                                    Response.Redirect("/Clients/Edit?id=" + clientInfo.id.ToString());
                                    return;
                                }
                                else
                                {
                                    errorMessage = "Wrong email or password";
                                    return;
                                }
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
    }
}

