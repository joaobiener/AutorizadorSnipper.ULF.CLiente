using Autorizador.ULF.Services.Shared.DataTransferObjects;
using Autorizador.ULF.Services.Shared.DataTransferObjects.Authentication;
using Shared.DataTransferObjects;

namespace AutorizadorSnipper.ULF.Cliente.HttpRepository;

    public interface IAuthenticationService
{
	Task<ResponseDto> RegisterUser(UserForRegistrationDto userForRegistrationDto);
	Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication);
	Task Logout();
	Task<string> RefreshToken();
}
