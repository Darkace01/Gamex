namespace Gamex.Service.Contract
{
    public interface IExtendedUserService
    {
        ApplicationUser? GetUserByName(string username);
    }
}