using Fundacionproj.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Fundacionproj.Pages.Donations
{ 

    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        public CreateModel(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public DonationInfo donationInfo = new DonationInfo();
        public String errorMessage = "";
        public String sucessMessage = "";
        

        public ApplicationUser? appUser;

        // The create function takes into account the user being logged in to create a donation
        // A donation has the following values endate, cost, impact (amount of people it help), who does it help, type of help, urgency level, name location and iframelink
        // The donation uses the iframe obtained from paypal to simulate and show hwo to make a payment for an spefic donation
        // In terms of functions the create.html obtains the data from the form to generate a new donation by adding trhough an sql syntax to the sql-server database
        // In the same way the create.cshtml uses scaffolding authorize in the backend to try an guarantee only logged users can create a donation
        // or so I believe from what I understand from scaffolding
        // In a similar way both for create.cs and registes.cs a user/user element I read through some tutorials on ytube on how to create and delete elements and users specific elements (like donations) through around 3-5 videos so that I could utilize that understanding to create the code

        
        public void OnGet()
        {
            var task = userManager.GetUserAsync(User);
            task.Wait();
            appUser = task.Result;
        }

        public void OnPost()
        {
            var task = userManager.GetUserAsync(User);
            task.Wait();
            appUser = task.Result;
            donationInfo.Name = Request.Form["Name"];
            donationInfo.IframeLink = Request.Form["IframeLink"];
            donationInfo.Location = Request.Form["Location"];
            donationInfo.UrgencyLevel = Request.Form["UrgencyLevel"];
            donationInfo.TypeOfAssistance = Request.Form["TypeOfAssistance"];
            donationInfo.BeneficiaryGroup = Request.Form["BeneficiaryGroup"];
            try
            {
                donationInfo.Impact = int.Parse(Request.Form["Impact"]).ToString();
            }
            catch
            {
                errorMessage = "Invalid input for Impact. Please enter a valid number.";
                return;
            }
            if (DateTime.TryParse(Request.Form["BeforeToDonate"], out DateTime parsedDate))
            {
                donationInfo.TimeNeededBefore = parsedDate;
            }
            else
            {
                errorMessage = "Whoops couldn't turn it into datetime";
                return;
            }

            donationInfo.AssistanceStatus = Request.Form["AssistanceStatus"];
            try
            {
                donationInfo.Goal = int.Parse(Request.Form["Goal"]).ToString();
            }
            catch
            {
                errorMessage = "Invalid input for Goal. Please enter a valid amount.";
                return;
            }

            // Check if any required field is empty
            if (string.IsNullOrWhiteSpace(donationInfo.Name) ||
                string.IsNullOrWhiteSpace(donationInfo.IframeLink) ||
                string.IsNullOrWhiteSpace(donationInfo.Location) ||
                string.IsNullOrWhiteSpace(donationInfo.UrgencyLevel) ||
                string.IsNullOrWhiteSpace(donationInfo.TypeOfAssistance) ||
                string.IsNullOrWhiteSpace(donationInfo.BeneficiaryGroup) ||
                donationInfo.TimeNeededBefore == default(DateTime) ||
                string.IsNullOrWhiteSpace(donationInfo.AssistanceStatus))
            {
                errorMessage = "All fields are required.";
                return;
            }
            

            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Donations;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO Donations" +
                                 "(Name, IframeLink, Location, UrgencyLevel, TypeOfAssistance, BeneficiaryGroup, Impact, TimeNeededBefore, AssistanceStatus, Goal) VALUES " +
                                 "(@name, @IframeLink, @location, @urgencyLevel, @typeOfAssistance, @beneficiaryGroup, @impact, @timeNeededBefore, @assistanceStatus, @goal);";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@name", donationInfo.Name);
                        command.Parameters.AddWithValue("@IframeLink", donationInfo.IframeLink);
                        command.Parameters.AddWithValue("@location", donationInfo.Location);
                        command.Parameters.AddWithValue("@urgencyLevel", donationInfo.UrgencyLevel);
                        command.Parameters.AddWithValue("@typeOfAssistance", donationInfo.TypeOfAssistance);
                        command.Parameters.AddWithValue("@beneficiaryGroup", donationInfo.BeneficiaryGroup);
                        command.Parameters.AddWithValue("@impact", donationInfo.Impact);
                        command.Parameters.AddWithValue("@timeNeededBefore", donationInfo.TimeNeededBefore);
                        command.Parameters.AddWithValue("@assistanceStatus", donationInfo.AssistanceStatus);
                        command.Parameters.AddWithValue("@goal", donationInfo.Goal);
                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            // Setting Sucess Message
            donationInfo.Name = "";
            donationInfo.IframeLink = "";
            donationInfo.Location = "";
            donationInfo.UrgencyLevel = "";
            donationInfo.TypeOfAssistance = "";
            donationInfo.BeneficiaryGroup = "";
            donationInfo.Impact = "";
            donationInfo.TimeNeededBefore = DateTime.Today;
            donationInfo.AssistanceStatus = "";
            donationInfo.Goal = "";

            sucessMessage = "La nueva Donacion se ha registrado correctamente";

            Response.Redirect("/User");

    }

    }
}
