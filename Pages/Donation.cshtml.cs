using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Fundacionproj.Pages
{
    public class DonationModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<DonationInfo> Results { get; set; } = new List<DonationInfo>();
        public List<DonationInfo> Resultsall { get; set; } = new List<DonationInfo>();
        public DonationModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        
        public void OnGet()
        {
            LoadDonationsCarousel();
            LoadDonations();
        }
        //Donation much like index.html uses a basic sql structure to obtain the donations currently and show them to the user, either as a carousel image or as css bottstrip carrds
        private void LoadDonationsCarousel()
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Donations;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT TOP 5 * FROM Donations WHERE TimeNeededBefore > GETDATE() ORDER BY Goal DESC, TimeNeededBefore ASC;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var donation = new DonationInfo
                            {
                                Name = reader["Name"] as string,
                                IframeLink = reader["IframeLink"] as string,
                                Location = reader["Location"] as string,
                                UrgencyLevel = reader["UrgencyLevel"] as string,
                                TypeOfAssistance = reader["TypeOfAssistance"] as string,
                                BeneficiaryGroup = reader["BeneficiaryGroup"] as string,
                                Impact = Convert.ToString(reader["Impact"]),
                                TimeNeededBefore = Convert.ToDateTime(reader["TimeNeededBefore"]),
                                AssistanceStatus = reader["AssistanceStatus"] as string,
                                Goal = Convert.ToString(reader["Goal"])
                            };
                            Results.Add(donation);
                        }
                    }
                }
            }
        }

        private void LoadDonations()
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Donations;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Donations WHERE TimeNeededBefore > GETDATE() ORDER BY TimeNeededBefore ASC, Impact ASC, Goal DESC;";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var donation = new DonationInfo
                            {
                                Name = reader["Name"] as string,
                                IframeLink = reader["IframeLink"] as string,
                                Location = reader["Location"] as string,
                                UrgencyLevel = reader["UrgencyLevel"] as string,
                                TypeOfAssistance = reader["TypeOfAssistance"] as string,
                                BeneficiaryGroup = reader["BeneficiaryGroup"] as string,
                                Impact = Convert.ToString(reader["Impact"]),
                                TimeNeededBefore = Convert.ToDateTime(reader["TimeNeededBefore"]),
                                AssistanceStatus = reader["AssistanceStatus"] as string,
                                Goal = Convert.ToString(reader["Goal"])
                            };
                            Resultsall.Add(donation);
                        }
                    }
                }
            }
        }
    }
}
