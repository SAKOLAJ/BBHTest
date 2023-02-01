using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace BBHTest.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>();
        public void OnGet()
        {
            try
            {
                string connectionString = "Data Source=.\\sqlexpress;Initial Catalog=TestDB;Integrated Security=True";
                
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Client_info";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ClientInfo clientInfo = new ClientInfo();
                                clientInfo.id = "" + reader.GetInt32(0);
                                clientInfo.Client_name = reader.GetString(1);
                                clientInfo.Client_surname =  reader.GetString(2);
                                clientInfo.Client_phone_num =  reader.GetString(3);
                                clientInfo.Client_email =  reader.GetString(4);
                                
                                
                                listClients.Add(clientInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exceprion: " + ex.ToString());  
            }
        }
    }
    public class ClientInfo
    {
        public string id;
        public string Client_name;
        public string Client_surname;
        public string Client_email;
        public string Client_phone_num;
        public string Client_password;
      
        
    }
}
