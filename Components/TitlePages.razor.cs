using Microsoft.AspNetCore.Components;
using MudBlazor;


namespace AutorizadorSnipper.ULF.Cliente.Components
{
    public partial class TitlePages
	{
		[Parameter]
		 public string TituloPagina { get; set; }
		[Parameter]
		public Color CorTitulo { get; set; }
		[Parameter]
		public String IconString { get; set; }
    
		private MudTheme Theme = new MudTheme();
		

	}
}
