using MudBlazor;
using AutorizadorSnipper.ULF.Cliente.Model;




namespace AutorizadorSnipper.ULF.Cliente.Helper
{
    public interface IUtil
    {
        Task<List<Aplicacao>> GetAplicacoes();
        string NullToString(object Value);
        string OrderBy(string nomeColuna, SortDirection ordem);
        bool IsNumeric(string  val);
       
	}
}
