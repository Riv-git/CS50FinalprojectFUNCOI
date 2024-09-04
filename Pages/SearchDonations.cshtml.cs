using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Fundacionproj.Pages
{
    public class SearchDonationsModel : PageModel
    {
        public List<DonationInfo> Results { get; set; } = new List<DonationInfo>();
        public void OnGet()
        {
        }

        //Search donations tends to be one of the more "complex" type of seach as it requires multiple possible parameters for multiple options
        //For this to function properly each characteristic is first separated from the main prompt to the sql to then be added as part of the consult for the user
        //First it tries to find which values are empty or unnesecary and then it adds the one used for the search, it specifically assumess the user will want to see without any other criteria for its characteristic regardless of the value, it also assumes there wont be any form manipulation, which may break the program
        // For the exception of the potential values being empty GPT **4 was used
        public void OnPost(string name, string location, string[] urgencyLevel, string[] typeOfAssistance, string[] beneficiaryGroup, int? impact, string startDate, string endDate, string[] assistanceStatus, decimal? goal)
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=Donations;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Donations WHERE 1=1 ";

                
                if (!string.IsNullOrEmpty(name))
                    sql += "AND Name LIKE '%' + @name + '%' ";
                if (!string.IsNullOrEmpty(location))
                    sql += "AND Location = @location ";

                if (urgencyLevel != null && urgencyLevel.Any(u => !string.IsNullOrEmpty(u)))
                    sql += $"AND UrgencyLevel IN ({string.Join(",", urgencyLevel.Where(u => !string.IsNullOrEmpty(u)).Select((_, index) => $"@urgencyLevel{index}"))}) ";

                if (typeOfAssistance != null && typeOfAssistance.Any(t => !string.IsNullOrEmpty(t)))
                    sql += $"AND TypeOfAssistance IN ({string.Join(",", typeOfAssistance.Where(t => !string.IsNullOrEmpty(t)).Select((_, index) => $"@typeOfAssistance{index}"))}) ";

                
                if (beneficiaryGroup != null && beneficiaryGroup.Any(b => !string.IsNullOrEmpty(b)))
                    sql += $"AND BeneficiaryGroup IN ({string.Join(",", beneficiaryGroup.Where(b => !string.IsNullOrEmpty(b)).Select((_, index) => $"@beneficiaryGroup{index}"))}) ";

                
                if (impact.HasValue)
                    sql += "AND Impact = @impact ";

                
                if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                {
                    sql += "AND TimeNeededBefore BETWEEN @startDate AND @endDate ";
                }

                
                if (assistanceStatus != null && assistanceStatus.Any(a => !string.IsNullOrEmpty(a)))
                    sql += $"AND AssistanceStatus IN ({string.Join(",", assistanceStatus.Where(a => !string.IsNullOrEmpty(a)).Select((_, index) => $"@assistanceStatus{index}"))}) ";

                if (goal.HasValue)
                    sql += "AND Goal = @goal ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    
                    if (!string.IsNullOrEmpty(name))
                        command.Parameters.AddWithValue("@name", name);
                    if (!string.IsNullOrEmpty(location))
                        command.Parameters.AddWithValue("@location", location);
                    if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate))
                    {
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);
                    }
                    if (impact.HasValue)
                        command.Parameters.AddWithValue("@impact", impact.Value);
                    if (goal.HasValue)
                        command.Parameters.AddWithValue("@goal", goal.Value);

                    // Add array parameters
                    AddArrayParameters(command, urgencyLevel, "urgencyLevel");
                    AddArrayParameters(command, typeOfAssistance, "typeOfAssistance");
                    AddArrayParameters(command, beneficiaryGroup, "beneficiaryGroup");
                    AddArrayParameters(command, assistanceStatus, "assistanceStatus");

                    // Execute the query
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

        private void AddArrayParameters(SqlCommand command, string[] array, string paramName)
        {
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    if (!string.IsNullOrEmpty(array[i]))
                        command.Parameters.AddWithValue($"@{paramName}{i}", array[i]);
                }
            }
        }




    }
}
