using Microsoft.AspNetCore.Components;
using AutorizadorSnipper.ULF.Cliente.Model;
using System.Globalization;

namespace AutorizadorSnipper.ULF.Cliente.Components;

public partial class NavApp
{
	[Parameter]
	public List<Aplicacao>? Aplicacoes { get; set; }
	[Parameter]
	public bool Aguardando { get; set; }

	public string ConvertListToString(List<Role> roles)
	{
		string retorno = "";
		if (roles.Count > 0)
		{
                retorno = string.Join(", ", roles.Select(role => role.Title));

		}

		return retorno;
	}
}
