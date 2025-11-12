namespace Dfe.PersonsApi.Application.Common.Models
{
    public class TrustGovernance : Person
    {
        public string? UKPRN { get; set; }
        public string? TRN { get; set; }
        public string? DateOfAppointment { get; set; }
        public string? DateTermOfOfficeEndsEnded { get; set; }
    }
}
