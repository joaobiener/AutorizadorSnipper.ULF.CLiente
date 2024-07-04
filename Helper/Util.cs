
using AutorizadorSnipper.ULF.Cliente.Model;
using MudBlazor;



namespace AutorizadorSnipper.ULF.Cliente.Helper;
public class Util : IUtil
{

    public Util()
    {
    }

    public async Task<List<Aplicacao>> GetAplicacoes()
    {
        List<Aplicacao> AplicacaoList = new List<Aplicacao>();

		
		
		#region aplicacoes
		AplicacaoList.Add(new Aplicacao
        {
            Title = "Dashboard",
            Summary = "Autorizador Snipper- Dashboard",
            HRef = "dashboard",
            Icon = @Icons.Material.Filled.Dashboard,
            IconColor = Color.Success,
            ImageAddress = "assets/MaquinaVendas.png",
            Roles = new List<Role>
                {
                    new Role { Title = RoleConst.Administrator },
                    new Role { Title = RoleConst.PrestadorAuditor },
					new Role { Title = RoleConst.RelacaoPrestador },
					new Role { Title = RoleConst.PrestadorAdmin }
                },

        });
		
		#endregion
		       

		return AplicacaoList;
    }

	

	public string NullToString(object Value)
	{

		// Value.ToString() allows for Value being DBNull, but will also convert int, double, etc.
		return Value == null ? "" : Value.ToString();

		// If this is not what you want then this form may suit you better, handles 'Null' and DBNull otherwise tries a straight cast
		// which will throw if Value isn't actually a string object.
		//return Value == null || Value == DBNull.Value ? "" : (string)Value;

	}

    public string OrderBy(string nomeColuna, SortDirection ordem)
    {

        if (ordem == SortDirection.Descending)
        {
            return nomeColuna + " desc ";
        }

        return nomeColuna;

        // If this is not what you want then this form may suit you better, handles 'Null' and DBNull otherwise tries a straight cast
        // which will throw if Value isn't actually a string object.
        //return Value == null || Value == DBNull.Value ? "" : (string)Value;
    }


	public bool  IsNumeric(string val)
	{
		var isNumeric = int.TryParse(val, out _);
        return isNumeric;
	}

}

