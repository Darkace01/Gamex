
namespace Gamex.Service.Contract
{
    public interface IExtendedUserService
    {
        ApplicationUser? GetUserByName(string username);
        UserProfileDTO? GetUserByNameForProfile(string username);
    }
}