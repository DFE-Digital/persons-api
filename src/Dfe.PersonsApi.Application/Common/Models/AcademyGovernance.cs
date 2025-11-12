namespace Dfe.PersonsApi.Application.Common.Models
{
    public class AcademyGovernance : Person
    {
        public string? UKPRN { get; set; }
        public int? URN { get; set; }
        public string? DateOfAppointment { get; set; }
        public string? DateTermOfOfficeEndsEnded { get; set; }
    }
}
