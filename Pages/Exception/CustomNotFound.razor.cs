using Microsoft.AspNetCore.Components;

namespace AutorizadorSnipper.ULF.Cliente.Pages.Exception
{
    public partial class CustomNotFound
	{
		[Inject]
		public NavigationManager navigationManager { get; set; }

		public void NavigateToHome()
		{
			navigationManager.NavigateTo("index");
		}
	}
}
