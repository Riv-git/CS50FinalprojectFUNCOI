using Fundacionproj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Fundacionproj.Pages
{
    [Authorize]
    public class UserModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        public ApplicationUser? appUser;

        public List<DonationInfo> listDonations = new List<DonationInfo>();

        public UserModel(UserManager<ApplicationUser> userManager) {
            this.userManager = userManager;
        }
        public void OnGet()
        {
            //Ok so basically this part is made so that the user may have the main panel for all the donations for this a pre formated sql consult is made to the database
            //After that it is saved in an array which later called in the html
            // User.html also takes into account the user being login based on scaffold, or what I understood from the scaffold to loggin the user...
            var task = userManager.GetUserAsync(User);
            task.Wait();
            appUser = task.Result;
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Donations;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Donations";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DonationInfo donationInfo = new DonationInfo();
                                donationInfo.DonationID = reader.GetInt32(0).ToString(); 
                                donationInfo.Name = reader.GetString(1); 
                                donationInfo.IframeLink = reader.GetString(2); 
                                donationInfo.Location = reader.GetString(3);
                                donationInfo.UrgencyLevel = reader.GetString(4);
                                donationInfo.TypeOfAssistance = reader.GetString(5);
                                donationInfo.BeneficiaryGroup = reader.GetString(6);
                                donationInfo.Impact = reader.GetInt32(7).ToString();
                                donationInfo.TimeNeededBefore = reader.GetDateTime(8);
                                donationInfo.AssistanceStatus = reader.GetString(9);
                                donationInfo.Goal = reader.GetInt32(10).ToString();

                                listDonations.Add(donationInfo);
                            }
                            Console.WriteLine($"Number of donations fetched: {listDonations.Count}");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("My exception in user.cshmtml" + ex.ToString());
            }

        }
        
    }
    public class DonationInfo
    {
        public String DonationID;
        public String Name;
        public String IframeLink;
        public String Location;
        public String UrgencyLevel;
        public String TypeOfAssistance;
        public String BeneficiaryGroup;
        public String Impact;
        public DateTime TimeNeededBefore;
        public String AssistanceStatus;
        public String Goal;


        public DonationInfo()
        {
            TimeNeededBefore = DateTime.Today;
        }
    }
}
