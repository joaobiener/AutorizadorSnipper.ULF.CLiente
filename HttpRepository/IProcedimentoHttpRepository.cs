using AutorizadorSnipper.ULF.Cliente.Features;
using Autorizador.ULF.Services.Shared.RequestFeatures;
using Entities.Models.Infomed;


namespace AutorizadorSnipper.ULF.Cliente.HttpRepository
{
	public interface IProcedimentoHttpRepository
	{
		Task<PagingResponse<Procedimento>> GetAllProcedimentosAsync(
           PrestadorParameters parameters);
		//Task<IEnumerable<InfServicoDto>> GetAllServicosBySearchTerm(string searchTerm);
	}
}
