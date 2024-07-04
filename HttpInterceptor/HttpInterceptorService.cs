using Autorizador.ULF.Cliente.HttpRepository;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using Toolbelt.Blazor;
using MudBlazor;
using AutorizadorSnipper.ULF.Cliente.Extensions;
using AutorizadorSnipper.ULF.Cliente.HttpRepository;

namespace AutorizadorSnipper.ULF.Cliente.HttpInterceptor;

public class HttpInterceptorService
{
	private readonly HttpClientInterceptor _interceptor;
	private readonly NavigationManager _navManager;

	private readonly RefreshTokenService _refreshTokenService;

	[Inject]
	public ISnackbar _snackbar { get; set; }
	string _url;
	public HttpInterceptorService(HttpClientInterceptor interceptor,
		NavigationManager navManager, 
		RefreshTokenService refreshTokenService,
		ISnackbar snackbar)
	{
		_interceptor = interceptor;
		_navManager = navManager;
		_refreshTokenService = refreshTokenService;
		_snackbar = snackbar;
	}

	
	public void RegisterEvent() => _interceptor.AfterSend += HandleResponse;
	
	public void RegisterBeforeSendEvent() =>
		_interceptor.BeforeSendAsync += InterceptBeforeSendAsync;

	public void DisposeEvent()
	{
		_interceptor.AfterSend -= HandleResponse;
		
		_interceptor.BeforeSendAsync -= InterceptBeforeSendAsync;
	}

	private async Task InterceptBeforeSendAsync(object sender,
		HttpClientInterceptorEventArgs e)
	{

		var absolutePath = e.Request.RequestUri.AbsolutePath;

		if (!absolutePath.Contains("token") && !absolutePath.Contains("account"))
		{
			var token = "";

                try { 
				token = await _refreshTokenService.TryRefreshToken();
                }
			catch (Exception ex)  
			{
                    
                    _navManager.NavigateTo("logout");

                }

                if (!string.IsNullOrEmpty(token))
			{

				e.Request.Headers.Authorization =
					new AuthenticationHeaderValue("bearer", token);
			}
		}
	}

	private  void HandleResponse(object? sender, HttpClientInterceptorEventArgs e)
	{
		_url = _navManager.ExtractQueryStringByKey<string>("url");

		if (e.Response is null)
		{
			_navManager.NavigateTo("ManutError");
			_snackbar.Add("Servidor não está disponível");
		}

		var message = "";

		if (!e.Response.IsSuccessStatusCode)
		{
			switch (e.Response.StatusCode)
			{
				case HttpStatusCode.NotFound:
					_navManager.NavigateTo("404");
					message = "Resource not found.";
					break;
				case HttpStatusCode.BadRequest:
					message = "Invalid request. Please try again.";
					//StateHasChanged();
					break;
				case HttpStatusCode.Unauthorized:
					message = "Acesso não Autorizado!";
					_snackbar.Add(message, Severity.Warning);
					_navManager.NavigateTo("logout");

					break;
				default:
					_navManager.NavigateTo("error");
					message = "Something went wrong. Please contact the administrator.";
					break;
			}

			throw new HttpResponseException(message);
		}
	}
}
