namespace GovUK.Dfe.PersonsApi.Client.Security
{
    public interface ITokenAcquisitionService
    {
        Task<string> GetTokenAsync();
    }
}
