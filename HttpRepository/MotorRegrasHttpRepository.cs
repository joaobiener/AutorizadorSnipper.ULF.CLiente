using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;
using AutorizadorSnipper.ULF.Cliente.Features;
using Autorizador.ULF.Services.Shared.DataTransferObjects;
using Autorizador.ULF.Services.Shared.RequestFeatures;
using System.Net.Http.Json;

namespace AutorizadorSnipper.ULF.Cliente.HttpRepository;

public class MotorRegrasHttpRepository : IMotorRegrasHttpRepository
{
	private readonly HttpClient _client;
	private readonly NavigationManager _navManager;
	private readonly JsonSerializerOptions _options =
		new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

	public MotorRegrasHttpRepository(HttpClient client, NavigationManager navManager, IConfiguration configuration)
	{
		_client = client;
		_navManager = navManager;
	}

	//public async Task<PagingResponse<AutorizacaoPreAprovadaDto>> GetAllAutorizacaosync(AutorizacaoParameters parameters)
	//{
	//	var queryStringParam = new Dictionary<string, string>
	//	{
	//		["pageNumber"] = parameters.PageNumber.ToString(),
	//		["pageSize"] = parameters.PageSize.ToString(),
	//		["takeSize"] = parameters.TakeSize.ToString(),
	//		["Search"] = parameters.Search == null ? "" : parameters.Search,
	//		["Status"] = parameters.Status == null ? "" : parameters.Status,
	//		["Carteirinha"] = parameters.Carteirinha == null ? "" : parameters.Carteirinha,
	//		["StatusAutorizacao"] = parameters.StatusAutorizacao == null ? "" : parameters.StatusAutorizacao,
	//		["NumProtocolo"] = parameters.NumProtocolo == null ? "" : parameters.NumProtocolo,
	//		["IdAutorizacaoPreAprov"] = parameters.IdAutorizacaoPreAprov == null ? "" : parameters.IdAutorizacaoPreAprov.ToString(),
	//		["DtInicio"] = string.Empty,
	//		["DtFim"] = string.Empty,
	//		["orderBy"] = parameters.OrderBy == null ? "" : parameters.OrderBy
	//	};


	//	var response =
	//		await _client.GetAsync(QueryHelpers.AddQueryString($"AutorizacaoPreAprovada/", queryStringParam));

	//	try
	//	{
	//		response.EnsureSuccessStatusCode();
	//	}
	//	catch (Exception ex)
	//	{
	//		_navManager.NavigateTo("logout");
	//		return null;
	//	}
	//	var content = await response.Content.ReadAsStringAsync();

	//	var pagingResponse = new PagingResponse<AutorizacaoPreAprovadaDto>
	//	{
	//		Items = System.Text.Json.JsonSerializer.Deserialize<List<AutorizacaoPreAprovadaDto>>(content, _options),
	//		MetaData = System.Text.Json.JsonSerializer.Deserialize<MetaData>(
	//			response.Headers.GetValues("X-Pagination").First(), _options)
	//	};

	//	return pagingResponse;
	//}
	//public async Task<PagingResponse<AutorizacaoEmAutorizacao>> GetAutoEmAuto(string codPrestador, string codBenef, string codProcedimentos)
	//{
	//	var queryStringParam = new Dictionary<string, string>
	//	{
	//		["codPrestador"] = codPrestador,
	//		["codBenef"] = codBenef,
	//		["codProcedimentos"] = codProcedimentos
	//	};

	//	var response =
	//			await _client.GetAsync(QueryHelpers.AddQueryString("AutorizacaoPreAprovada/AutoByPrestBenefProcedimentos", queryStringParam));
		
	//	var content = await response.Content.ReadAsStringAsync();

	//	var pagingResponse = new PagingResponse<AutorizacaoEmAutorizacao>
	//	{
	//		Items = System.Text.Json.JsonSerializer.Deserialize<List<AutorizacaoEmAutorizacao>>(content, _options),
	//		MetaData = System.Text.Json.JsonSerializer.Deserialize<MetaData>(
	//			response.Headers.GetValues("X-Pagination").First(), _options)
	//	};

	//	return pagingResponse;
	//}
	//public async Task<Dictionary<string, int>> GetCountGroupStatus()
	//{
	//	var countGroupByStatus = await _client.GetFromJsonAsync<Dictionary<string, int>>($"AutorizacaoPreAprovada/getcountstatus");
	//	return countGroupByStatus;
	//}

	//public async Task<AutorizacaoPreAprovadaDto> CreateAutorizacao(AutorizacaoPreAprovadaForCreationDto autorizacao)
	//{
	//	var responseAutorizacao = await _client.PostAsJsonAsync("AutorizacaoPreAprovada/CreateAutorizacao", autorizacao);

	//	var createdAutorizacao = responseAutorizacao.Content.ReadFromJsonAsync<AutorizacaoPreAprovadaDto>().Result;

	//	return createdAutorizacao;


	//}
	public async Task DeleteAutorizacao(int id)
	=> await _client.DeleteAsync(Path.Combine($"AutorizacaoPreAprovada/", id.ToString()));


    public async Task<PagingResponse<MotorRegrasAutorizadorDto>> GetAllRegras(RegrasParameters parameters)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageNumber"] = parameters.PageNumber.ToString(),
            ["pageSize"] = parameters.PageSize.ToString(),
            ["takeSize"] = parameters.TakeSize.ToString(),
            ["Search"] = parameters.Search == null ? "" : parameters.Search,
            ["IdRegra"] = parameters.IdRegra == null ? "" : parameters.IdRegra,
            ["Ativo"] = parameters.Ativo == null ? "" : parameters.Ativo,
            ["CodPrestadorSolicitante"] = parameters.CodPrestadorSolicitante == null ? "" : parameters.CodPrestadorSolicitante,
            ["CodPrestadorExecutante"] = parameters.CodPrestadorExecutante == null ? "" : parameters.CodPrestadorExecutante,
            ["CodTipoGuia"] = parameters.CodTipoGuia== null ? "" : parameters.CodTipoGuia,
            ["DtInicio"] = string.Empty,
            ["DtFim"] = string.Empty,
            ["orderBy"] = parameters.OrderBy == null ? "" : parameters.OrderBy
        };

        var response =
            await _client.GetAsync(QueryHelpers.AddQueryString($"MotorRegras/", queryStringParam));

        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            _navManager.NavigateTo("logout");
            return null;
        }
        var content = await response.Content.ReadAsStringAsync();

        var pagingResponse = new PagingResponse<MotorRegrasAutorizadorDto>
        {
            Items = System.Text.Json.JsonSerializer.Deserialize<List<MotorRegrasAutorizadorDto>>(content, _options),
            MetaData = System.Text.Json.JsonSerializer.Deserialize<MetaData>(
                response.Headers.GetValues("X-Pagination").First(), _options)
        };

        return pagingResponse;
    }

    public async Task<MotorRegrasAutorizadorDto> CreateRegra(MotorRegrasAutorizadorForCreationDto regra)
    {
        var responseRegra = await _client.PostAsJsonAsync("MotorRegras/CreateRegra", regra);

        var createdAutorizacao = responseRegra.Content.ReadFromJsonAsync<MotorRegrasAutorizadorDto>().Result;

        return createdAutorizacao;
    }

    public Task UpdateRegra(int id, MotorRegrasAutorizadorForManipulationDto autorizacaoDtoEntity)
    {
        throw new NotImplementedException();
    }
} 

