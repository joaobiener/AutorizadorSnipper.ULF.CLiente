﻿using Microsoft.AspNetCore.Components;

namespace AutorizadorSnipper.ULF.Cliente.Pages
{
	public partial class ServerError
	{
		[Inject]
		public NavigationManager NavigationManager { get; set; }

		public void NavigateToHome()
		{
			NavigationManager.NavigateTo("login");
		}
	}
}
