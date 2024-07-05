using Microsoft.AspNetCore.Components;
using System.Text.Json;
using AutorizadorSnipper.ULF.Cliente.Features;
using Autorizador.ULF.Services.Shared.RequestFeatures;
using Microsoft.AspNetCore.WebUtilities;
using Entities.Models.Infomed;

namespace AutorizadorSnipper.ULF.Cliente.HttpRepository;

	public class ProcedimentoHttpRepository : IProcedimentoHttpRepository
{
    private readonly HttpClient _client;
    private readonly NavigationManager _navManager;
    private readonly JsonSerializerOptions _options =
        new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public ProcedimentoHttpRepository(
        HttpClient client,
        NavigationManager navManager,
        IConfiguration configuration)
    {
        _client = client;
        _navManager = navManager;

    }

    public async Task<PagingResponse<Procedimento>> GetAllProcedimentosAsync(PrestadorParameters parameters)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = parameters.PageNumber.ToString(),
            ["pageSize"] = parameters.PageSize.ToString(),
            ["takeSize"] = parameters.TakeSize.ToString(),
            ["codigocontrato"] = parameters.CodigoContrato == null ? string.Empty : parameters.CodigoContrato,
            ["CodServico"] = parameters.CodServico == null ? string.Empty : parameters.CodServico,
            ["NomeServico"] = parameters.NomeServico == null ? string.Empty : parameters.NomeServico,
            ["orderBy"] = parameters.OrderBy == null ? "" : parameters.OrderBy
        };

        var response =
                await _client.GetAsync(QueryHelpers.AddQueryString("Infomed/Procedimento", queryStringParam));

        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<Procedimento>
        {
            Items = JsonSerializer.Deserialize<List<Procedimento>>(content, _options),
            MetaData = JsonSerializer.Deserialize<MetaData>(
                response.Headers.GetValues("X-Pagination").First(), _options)
        };

        return pagingResponse;
    }
}
