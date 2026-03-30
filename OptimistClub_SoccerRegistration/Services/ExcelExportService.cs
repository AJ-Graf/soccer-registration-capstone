using ClosedXML.Excel;
using OptimistClub_SoccerRegistration.Data.Models;

namespace OptimistClub_SoccerRegistration.Services
{
    public class ExcelExportService
    {
        private readonly IRegistrationService _registrationService;

        public ExcelExportService(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        public async Task<byte[]> ExportRegistrationsAsync()
        {
            var players = await _registrationService.GetAllPlayersAsync();
            var parents = await _registrationService.GetAllParentsAsync();
            var volunteers = await _registrationService.GetAllVolunteersAsync();
            var registrations = await _registrationService.GetAllRegistrationsAsync();

            using var workbook = new XLWorkbook();

            var playersSheet = workbook.Worksheets.Add("Players");
            playersSheet.Cell(1, 1).Value = "Player ID";
            playersSheet.Cell(1, 2).Value = "First Name";
            playersSheet.Cell(1, 3).Value = "Last Name";
            playersSheet.Cell(1, 4).Value = "Date of Birth";
            playersSheet.Cell(1, 5).Value = "Gender";
            playersSheet.Cell(1, 6).Value = "Town";
            playersSheet.Cell(1, 7).Value = "Shirt Size";
            playersSheet.Cell(1, 8).Value = "Parent";
            playersSheet.Cell(1, 9).Value = "Payment Status";
            playersSheet.Cell(1, 10).Value = "Registered";

            for (int i = 0; i < players.Count; i++)
            {
                var p = players[i];
                var reg = registrations.FirstOrDefault(r => r.PlayerId == p.PlayerId);
                playersSheet.Cell(i + 2, 1).Value = p.PlayerId;
                playersSheet.Cell(i + 2, 2).Value = p.FirstName;
                playersSheet.Cell(i + 2, 3).Value = p.LastName;
                playersSheet.Cell(i + 2, 4).Value = p.DateOfBirth.ToString("yyyy-MM-dd");
                playersSheet.Cell(i + 2, 5).Value = p.Gender;
                playersSheet.Cell(i + 2, 6).Value = p.Town;
                playersSheet.Cell(i + 2, 7).Value = p.ShirtSize;
                playersSheet.Cell(i + 2, 8).Value = p.Parent != null
                    ? $"{p.Parent.FirstName} {p.Parent.LastName}" : "";
                playersSheet.Cell(i + 2, 9).Value = reg?.PaymentStatus ?? "";
                playersSheet.Cell(i + 2, 10).Value = reg?.RegisteredAt.ToString("yyyy-MM-dd") ?? "";
            }
            playersSheet.Row(1).Style.Font.Bold = true;
            playersSheet.Columns().AdjustToContents();

            var parentsSheet = workbook.Worksheets.Add("Parents");
            parentsSheet.Cell(1, 1).Value = "Parent ID";
            parentsSheet.Cell(1, 2).Value = "First Name";
            parentsSheet.Cell(1, 3).Value = "Last Name";
            parentsSheet.Cell(1, 4).Value = "Phone";
            parentsSheet.Cell(1, 5).Value = "Email";
            parentsSheet.Cell(1, 6).Value = "Waiver Accepted";
            parentsSheet.Cell(1, 7).Value = "Date Added";

            for (int i = 0; i < parents.Count; i++)
            {
                var pr = parents[i];
                parentsSheet.Cell(i + 2, 1).Value = pr.ParentId;
                parentsSheet.Cell(i + 2, 2).Value = pr.FirstName;
                parentsSheet.Cell(i + 2, 3).Value = pr.LastName;
                parentsSheet.Cell(i + 2, 4).Value = pr.PhoneNumber;
                parentsSheet.Cell(i + 2, 5).Value = pr.Email;
                parentsSheet.Cell(i + 2, 6).Value = pr.WaiverAccepted ? "Yes" : "No";
                parentsSheet.Cell(i + 2, 7).Value = pr.DateAdded.ToString("yyyy-MM-dd");
            }
            parentsSheet.Row(1).Style.Font.Bold = true;
            parentsSheet.Columns().AdjustToContents();

            var volunteersSheet = workbook.Worksheets.Add("Volunteers");
            volunteersSheet.Cell(1, 1).Value = "Volunteer ID";
            volunteersSheet.Cell(1, 2).Value = "First Name";
            volunteersSheet.Cell(1, 3).Value = "Last Name";
            volunteersSheet.Cell(1, 4).Value = "Role";
            volunteersSheet.Cell(1, 5).Value = "Shirt Size";
            volunteersSheet.Cell(1, 6).Value = "Criminal Check";
            volunteersSheet.Cell(1, 7).Value = "Date Added";

            for (int i = 0; i < volunteers.Count; i++)
            {
                var v = volunteers[i];
                volunteersSheet.Cell(i + 2, 1).Value = v.VolunteerId;
                volunteersSheet.Cell(i + 2, 2).Value = v.FirstName;
                volunteersSheet.Cell(i + 2, 3).Value = v.LastName;
                volunteersSheet.Cell(i + 2, 4).Value = v.Role;
                volunteersSheet.Cell(i + 2, 5).Value = v.ShirtSize;
                volunteersSheet.Cell(i + 2, 6).Value = v.CriminalCheckCompleted ? "Yes" : "No";
                volunteersSheet.Cell(i + 2, 7).Value = v.DateAdded.ToString("yyyy-MM-dd");
            }
            volunteersSheet.Row(1).Style.Font.Bold = true;
            volunteersSheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}