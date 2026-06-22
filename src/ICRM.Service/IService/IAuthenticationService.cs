namespace ICRM.Service;

public interface IAuthenticationService
{
    Task<bool> LoginAsync(ApplicationUserLoginInputModel model);
    Task<bool> RegisterAsync(ApplicationUserRegisterInputModel model);
    Task<bool> ForgotPasswordAsync(ApplicationUserRegisterInputModel model);
    Task<bool> ResetPasswordAsync(ApplicationUserRegisterInputModel model);
    Task<bool> ChangePasswordAsync(ApplicationUserRegisterInputModel model);
    Task<bool> RefreshTokenAsync(ApplicationUserRegisterInputModel model);
}
