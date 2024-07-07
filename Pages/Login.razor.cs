using AutorizadorSnipper.ULF.Cliente.HttpRepository;
using Shared.DataTransferObjects;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using Microsoft.AspNetCore.Components.Forms;
using AutorizadorSnipper.ULF.Cliente.HttpInterceptor;
using AutorizadorSnipper.ULF.Cliente.Extensions;
using AutorizadorSnipper.ULF.Cliente.Model;
using Autorizador.ULF.Services.Shared.DataTransferObjects;
using AutorizadorSnipper.ULF.Cliente.Helper;
using Autorizador.ULF.Services.Shared.DataTransferObjects.Authentication;

namespace AutorizadorSnipper.ULF.Cliente.Pages
{
    public partial class Login
	{
		bool success;
        public bool Aguardando = false;
        protected Dictionary<string, int> _statusContagem = new Dictionary<string, int>();

        public List<Aplicacao> AplicacaoList { get; set; } = new List<Aplicacao>();

        [CascadingParameter]
		private Task<AuthenticationState> authenticationStateTask { get; set; }
		[Inject]
		public IJSRuntime JSRuntime { get; set; }

		private IJSObjectReference _jsModule;

		private UserForAuthenticationDto _userForAuthentication = new UserForAuthenticationDto();
		private bool loading;
		[Inject]
		public IAuthenticationService? AuthenticationService { get; set; }
       
        [Inject]
		public NavigationManager? NavigationManager { get; set; }

		public bool ShowAuthError { get; set; }
		public string? Error { get; set; }
		[Inject]
		public ISnackbar Snackbar { get; set; }
		[Inject]
		public HttpInterceptorService? Interceptor { get; set; }
      
        [Inject]
		NavigationManager NavManager { get; set; }

        bool isShow;
        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
		string _url;
		protected async override Task OnInitializedAsync()
		{
			Interceptor.RegisterEvent();
			Interceptor.RegisterBeforeSendEvent();

			_url = NavManager.ExtractQueryStringByKey<string>("url");

        }


        void ButtonTestclick()
        {
            if(isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
			else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }
    

		private async Task FocusAndStyleElement()
		{
          //  await _jsModule.InvokeVoidAsync("focusInputComponent", "password");
            await _jsModule.InvokeVoidAsync("focusInputComponent", "username");
        }

        private void OnValidSubmit(EditContext editFormContext)
		{
			success = true;
			StateHasChanged();
		}
		

		public async Task ExecuteLogin()
		{
			ShowAuthError = false;
			loading = true;
			var result = await AuthenticationService.Login(_userForAuthentication);
			loading = false;
			if (!result.IsAuthSuccessful)
			{
				Error = result.ErrorMessage;
				if (Error != "Autenticação Inválida")
				{
					Snackbar.Add(Error, Severity.Warning);
					StateHasChanged();
				}
			}
			else
			{
				var user = (await authenticationStateTask).User;
				if (user.IsInRole("Administrator") || 
				   user.IsInRole("SnipperAdmin"))
				{
                    NavManager.NavigateTo("regras");
				}
				
			}
		}
		public void Dispose() => Interceptor.DisposeEvent();
	}
}
