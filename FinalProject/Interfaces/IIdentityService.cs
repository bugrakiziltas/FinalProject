
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Interfaces
{
    public interface IIdentityService
    {
        Task<string?> GetUserNameAsync(string userId);

        //Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal);

        Task<IdentityUser?> GetUserByIdAsync(string userId);

        Task<IdentityUser?> GetUserByUserNameAsync(string userName);

        Task<IdentityUser?> GetUserByEmailAsync(string email);

        Task<bool> IsInRoleAsync(string userId, string role);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<bool> SignInUser(IdentityUser user, string password);

        //Task<(Result Result, string UserId)> CreateUserAsync(CreateUserRequestDto createUserRequestDto);

        //Task<Result> DeleteUserAsync(string userId);
    }
}
