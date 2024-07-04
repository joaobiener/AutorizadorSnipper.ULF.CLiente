
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using AutorizadorSnipper.ULF.Cliente.AuthProviders;
using Microsoft.AspNetCore.Components;
using AutorizadorSnipper.ULF.Cliente.Configuration;
using Autorizador.ULF.Services.Shared.DataTransferObjects.Authentication;
using Shared.DataTransferObjects;



namespace AutorizadorSnipper.ULF.Cliente.HttpRepository
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly NavigationManager _navManager;
        private readonly AuthAPIClient _client;
        private readonly HttpClient _clientePrestadorEventual;
        private readonly JsonSerializerOptions _options =
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        private readonly AuthenticationStateProvider _authStateProvider;

        private readonly ILocalStorageService _localStorage;

        public AuthenticationService(AuthAPIClient client,
                                     HttpClient clientPrestadorEventual,
                                      AuthenticationStateProvider authStateProvider,
                                      ILocalStorageService localStorage,
                                      NavigationManager navManager)
        {
            _client = client;
            _clientePrestadorEventual = clientPrestadorEventual;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
            _navManager = navManager;
        }


        public async Task<AuthResponseDto> Login(UserForAuthenticationDto userForAuthentication)
        {

            try
            {
                var responseTest = await _client.Client.GetAsync("account");
				responseTest.EnsureSuccessStatusCode();
			}
            catch (Exception e)
            {

                return new AuthResponseDto
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = e.Message
                };
            }



            // _client.Timeout = new TimeSpan(0,0, 10);
            var response = await _client.Client.PostAsJsonAsync("account/loginLDAP",
                userForAuthentication);
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<AuthResponseDto>(content, _options);

            // _client.Timeout = new TimeSpan(0, 10, 0);


            if (!response.IsSuccessStatusCode)
                return result;

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            ((AuthStateProvider)_authStateProvider).NotifyUserAuthentication(
                result.Token);

            _clientePrestadorEventual.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "bearer", result.Token);

            _client.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "bearer", result.Token);

            return new AuthResponseDto { IsAuthSuccessful = true };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");

            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();

            _client.Client.DefaultRequestHeaders.Authorization = null;
            _clientePrestadorEventual.DefaultRequestHeaders.Authorization = null;
        }


        public async Task<ResponseDto> RegisterUser(UserForRegistrationDto userForRegistrationDto)
        {
            var response = await _client.Client.PostAsJsonAsync("account/register",
                userForRegistrationDto);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                var result = JsonSerializer.Deserialize<ResponseDto>(content, _options);

                return result;
            }

            return new ResponseDto { IsSuccessfulRegistration = true };
        }


        public async Task<string> RefreshToken()
        {
            var token = await _localStorage.GetItemAsync<string>("authToken");
            var refreshToken = await _localStorage.GetItemAsync<string>("refreshToken");

            var response = await _client.Client.PostAsJsonAsync("token/refresh",
                new TokenDto(token, refreshToken));

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<AuthResponseDto>(content, _options);

            await _localStorage.SetItemAsync("authToken", result.Token);
            await _localStorage.SetItemAsync("refreshToken", result.RefreshToken);

            _client.Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("bearer", result.Token);
            _clientePrestadorEventual.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                ("bearer", result.Token);

            return result.Token;
        }
    }

}
