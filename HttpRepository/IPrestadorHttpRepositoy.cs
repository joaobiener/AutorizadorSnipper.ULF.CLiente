﻿using Autorizador.ULF.Services.Shared.DataTransferObjects;
using Autorizador.ULF.Services.Shared.RequestFeatures;
using AutorizadorSnipper.ULF.Cliente.Features;


namespace AutorizadorSnipper.ULF.Cliente.HttpRepository
{
    public interface IPrestadorHttpRepositoy
	{
		 Task<PagingResponse<PrestadorDto>> GetPrestadores(PrestadorParameters parameters);
         Task<PagingResponse<MotorRegrasTipoPrestadorDto>> GetTiposPrestadores(SearchParameters parameters);
    }
}
