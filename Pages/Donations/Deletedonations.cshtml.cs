using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Data.SqlClient;
using Fundacionproj.Models;
using System.Linq.Expressions;

namespace Fundacionproj.Pages.Donations
{
    [Authorize]
    public class DeletedonationsModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;

        // This part of the webpage is used to delete an specific donation for the main user, for this the user must be already logged in
        // For this I used Authorize and the scaffold way of loggin in a user, which im still learning about
        // Anyway the user once logged in in the menu for user selects an specific donation and the link for that speciic donation is obtained through the post/get redirection so that the id is obtained
        // once this is Ddeletedonations.html the user can confirm in this page to delete the selected donation, at which point if the users select to accept th deletion a basci sql structure is used to delete the donation

        public DeletedonationsModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public DonationInfo donationInfo = new DonationInfo();
        public String errorMessage = "";
        public String sucessMessage = "";


        public ApplicationUser? appUser;

        public int DonationId { get; set; }
        public string ErrorMessage { get; private set; }

        public void OnGet(int id)
        {
            var task = userManager.GetUserAsync(User);
            task.Wait();
            appUser = task.Result;
            DonationId = id;
        }

        public IActionResult OnPost(int donationId)
        {
            var task = userManager.GetUserAsync(User);
            task.Wait();
            appUser = task.Result;

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Donations;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "DELETE FROM Donations WHERE DonationID = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", donationId);
                        int deletedCount = command.ExecuteNonQuery();
                        if (deletedCount == 0)
                        {
                            ErrorMessage = "No Donation found with this ID.";
                            return Page();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }

            return RedirectToPage("/User");
        }
    }
}
