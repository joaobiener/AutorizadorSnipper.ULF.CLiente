using AutorizadorSnipper.ULF.Cliente.Features;
using Autorizador.ULF.Services.Shared.DataTransferObjects;
using Autorizador.ULF.Services.Shared.RequestFeatures;


namespace AutorizadorSnipper.ULF.Cliente.HttpRepository
{
    public interface IMotorRegrasHttpRepository
    {
        //codProcedimentos string com multiplos codProcedimentos separados por virgula
		Task<PagingResponse<MotorRegrasAutorizadorDto>> GetAllRegras(RegrasParameters parameters);
		Task<MotorRegrasAutorizadorDto> CreateRegra(MotorRegrasAutorizadorForCreationDto regra);
		Task UpdateRegra(int id,MotorRegrasAutorizadorForManipulationDto autorizacaoDtoEntity);
        Task DeleteAutorizacao(int id);
	}
}
