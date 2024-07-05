using Microsoft.AspNetCore.Components;
using System.Text.Json;
using AutorizadorSnipper.ULF.Cliente.Features;
using Autorizador.ULF.Services.Shared.DataTransferObjects;
using Autorizador.ULF.Services.Shared.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;

namespace AutorizadorSnipper.ULF.Cliente.HttpRepository
{
    public class PrestadorHttpRepositoy : IPrestadorHttpRepositoy
	{
		private readonly HttpClient _client;
		private readonly NavigationManager _navManager;
		private readonly JsonSerializerOptions _options =
			new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

		public PrestadorHttpRepositoy(
			HttpClient client, 
			NavigationManager navManager,
			IConfiguration configuration)
		{
			_client = client;
			_navManager = navManager;

		}

        public async Task<PagingResponse<PrestadorDto>> GetPrestadores(PrestadorParameters parameters)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageNumber"] = parameters.PageNumber.ToString(),
                ["pageSize"] = parameters.PageSize.ToString(),
                ["takeSize"] = parameters.TakeSize.ToString(),
                ["codigocontrato"] = parameters.CodigoContrato == null ? string.Empty : parameters.CodigoContrato,
                ["nomeprestador"] = parameters.NomePrestador == null ? string.Empty : parameters.NomePrestador,
                ["especialidade"] = parameters.Especialidade == null ? string.Empty : parameters.Especialidade,
                ["tipoprestador"] = parameters.TipoPrestador == null ? string.Empty : parameters.TipoPrestador,
                ["orderBy"] = parameters.OrderBy == null ? "" : parameters.OrderBy
            };


            var response =
                    await _client.GetAsync(QueryHelpers.AddQueryString("Infomed/Prestador", queryStringParam));

            var content = await response.Content.ReadAsStringAsync();

            var pagingResponse = new PagingResponse<PrestadorDto>
            {
                Items = JsonSerializer.Deserialize<List<PrestadorDto>>(content, _options),
                MetaData = JsonSerializer.Deserialize<MetaData>(
                    response.Headers.GetValues("X-Pagination").First(), _options)
            };

            return pagingResponse;
        }


    }
}
