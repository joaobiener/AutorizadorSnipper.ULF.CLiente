using AutorizadorSnipper.ULF.Cliente.HttpRepository;
using Microsoft.AspNetCore.Components;
using AutorizadorSnipper.ULF.Cliente.Extensions;

namespace AutorizadorSnipper.ULF.Cliente.Pages
{
    public partial class Logout
	{
		[Inject]
		public IAuthenticationService? AuthenticationService { get; set; }

		[Inject]
		public NavigationManager? NavigationManager { get; set; }
		[Inject]
		NavigationManager NavManager { get; set; }


		protected override async Task OnInitializedAsync()
		{
			await AuthenticationService.Logout();
			string _url = NavManager.ExtractQueryStringByKey<string>("url");
			await Task.Delay(500);
			if (!string.IsNullOrEmpty(_url))
			{
				NavigationManager.NavigateTo("login?url=" + _url);
			}
			else { 
				NavigationManager.NavigateTo("Login");
            }

        }
	}
}
