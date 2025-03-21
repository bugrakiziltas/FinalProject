
using FinalProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserClaimsPrincipalFactory<IdentityUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly SignInManager<IdentityUser> _signInManager;
        //private readonly IMapper _mapper;

        public IdentityService(
            UserManager<IdentityUser> userManager,
            IUserClaimsPrincipalFactory<IdentityUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService,
            SignInManager<IdentityUser> signInManager
            //IMapper mapper
            )
        {
            _userManager = userManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _signInManager = signInManager;
            //_mapper = mapper;
        }

        //public async Task<ApplicationUser?> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal)
        //{
        //    var user = await _userManager.GetUserAsync(claimsPrincipal);

        //    return user;
        //}

        public async Task<string?> GetUserNameAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user?.UserName;
        }

        public async Task<IdentityUser?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user;
        }
        public async Task<IdentityUser?> GetUserByUserNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return user;
        }

        public async Task<IdentityUser?> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

        //public async Task<(Result Result, string UserId)> CreateUserAsync(CreateUserRequestDto createUserRequestDto)
        //{
        //    var userId = Guid.NewGuid();

        //    //var user = _mapper.Map<ApplicationUser>(createUserRequestDto);

        //    var user = new ApplicationUser()
        //    {
        //        Id = userId,
        //        UserName = createUserRequestDto.UserName,
        //        Email = createUserRequestDto.Email,
        //        FirstName = createUserRequestDto.FirstName,
        //        SurName = createUserRequestDto.SurName,
        //        PhoneNumber = createUserRequestDto.PhoneNumber,
        //        BirthDate = createUserRequestDto.BirthDate.ToUniversalTime(),
        //        //ImageId = createUserRequestDto.ImageId,
        //        CreatedOn = DateTimeOffset.UtcNow,
        //        CreatedByUserId = userId,
        //    };

        //    var result = await _userManager.CreateAsync(user, createUserRequestDto.Password);

        //    return (result.ToApplicationResult(), user.Id.ToString());
        //}

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);

            return user != null && await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<bool> SignInUser(IdentityUser user, string password)
        {
            var loginResult = await _signInManager.PasswordSignInAsync(user, password, true, false);

            return loginResult.Succeeded;
        }

        //public async Task<Result> DeleteUserAsync(string userId)
        //{
        //    var user = await _userManager.FindByIdAsync(userId);

        //    return user != null ? await DeleteUserAsync(user) : Result.Success();
        //}

        //public async Task<Result> DeleteUserAsync(ApplicationUser user)
        //{
        //    var result = await _userManager.DeleteAsync(user);

        //    return result.ToApplicationResult();
        //}
    }
}
