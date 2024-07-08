using Autorizador.ULF.Services.Shared.DataTransferObjects;
using Autorizador.ULF.Services.Shared.RequestFeatures;
using AutorizadorSnipper.ULF.Cliente.Extensions;
using AutorizadorSnipper.ULF.Cliente.Helper;
using AutorizadorSnipper.ULF.Cliente.HttpInterceptor;
using AutorizadorSnipper.ULF.Cliente.HttpRepository;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Extensions;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Security.Claims;
using System.Text;
using static MudBlazor.Icons;

namespace AutorizadorSnipper.ULF.Cliente.Pages.Principais
{
	public partial class RegrasAutorizacao
	{
        public bool Aguardando { get; set; }
        private string searcRegra = "";
        string state = "Message box ainda nao foi chamada";
       

        public DateRange _dateRange { get; set; }
        public TipoGuiaEnum enumTipoValue { get; set; } = TipoGuiaEnum.C;
        public enum TipoGuiaEnum
        {
            [Description("Faturamento Guia de Consulta")]
            C = 1,
            [Description("Faturamento SP/SADT")]
            F = 2,
            [Description("Faturamento Resumo Internação")]
            I = 3,
            [Description("Faturamento Honorario Individual")]
            H = 4,
            [Description("Faturamento Odonto")]
            O = 5,
            [Description("Solicitacao Anexo")]
            A = 6,
            [Description("Elegibilidade")]
            E = 7,
            [Description("Solicitação de Prorrogação")]
            P = 8,
            [Description("Solicitação SP/SADT")]
            S = 9,
            [Description("Solicitacação de Internação Comunicação Internação")]
            T = 10,
            [Description("Comunicação Internação")]
            M = 11
        }


        [Inject]
        public IUtil Util { get; set; }
        [Inject]
		AuthenticationStateProvider AuthenticationStateProvider { get; set; }
		
		[Inject]
		public ISnackbar Snackbar { get; set; }

		[Inject]
		public IDialogService DialogService { get; set; }
		[Inject]
		public IMotorRegrasHttpRepository? motorRegrasRepository { get; set; }
        [Inject]
        public IPrestadorHttpRepositoy? prestadorRepository { get; set; }
        [Inject]
		public HttpInterceptorService? Interceptor { get; set; }
		public List<MotorRegrasTipoPrestadorDto> tiposPrestadores { get; set; }

		public MotorRegrasAutorizadorDto regra = new MotorRegrasAutorizadorDto();
        public List<MotorRegrasAutorizadorDto> Regras { get; set; } = new List<MotorRegrasAutorizadorDto>();
		public bool bnOnSearchChangePage = false;
		MudMessageBox mboxApagar { get; set; }

		bool _expanded = false;

	
		private HashSet<MotorRegrasAutorizadorDto> selectedItems = new HashSet<MotorRegrasAutorizadorDto>();

		private readonly int[] _pageSizeOption = {10, 20 };
		private RegrasParameters regrasParameters = new RegrasParameters();

		public MetaData _MetaData { get; set; } = new MetaData();

		private int totalItems;
		private bool firstTime = true;
		private bool bordered = true;
		private MotorRegrasAutorizadorDto SelectedItem;

		private int selectedRowNumber = -1;
		private MudTable<MotorRegrasAutorizadorDto> mudTable;

        public bool IsAtivo
        {
            get => regra.Ativo == "S";
            set => regra.Ativo = value ? "S" : "N";
        }
		public bool IsBeneLocal
		{
			get => regra.BenefLocal == "S";
			set => regra.BenefLocal = value ? "S" : "N";
		}
		private string _statusSearch { get; set; } = "S";
        public bool IsActiveSearch
        {
            get => _statusSearch == "S";
            set => _statusSearch = value ? "S" : "N";
        }

        private string UserId { get; set; }
		private bool isAdminstrator { get; set; } = false;
		private void Expandir(bool expandir)
		{
			_expanded = expandir;
		}
	

		protected async override Task OnInitializedAsync()
		{
			ClaimsPrincipal claimAtual;
			Interceptor.RegisterEvent();
			Interceptor.RegisterBeforeSendEvent();
			var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
			var user = authState.User;

			if (user.Identity.IsAuthenticated)
			{
				IEnumerable<Claim> _claims = user.Claims;
				foreach (var item in _claims)
				{
					if (item.Value.ToLower() == "snipperadmin")
					{
						isAdminstrator = true;
					}
				}
				string jsonData = user.Identity.Name;

				// Deserialize the JSON array
				var data = JsonConvert.DeserializeObject<string[]>(jsonData);


				UserId = data[0];
				await GetTipoPrestador();
                //UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            }
			else
			{
				UserId = "Not authenticated";
			}

		}
        private bool StatusParaEdicao()
        {
			return (SelectedItem != null);
		}
        private bool StatusParaInclusao()
        {
			regra = new MotorRegrasAutorizadorDto()
			{
				StatusGuia = "EA",
				CodTipoGuia = TipoGuiaEnum.C.ToString()
			};
			return true;
        }

        private async Task GetTipoPrestador() {
           
            var pagingResponse = await prestadorRepository.GetTiposPrestadores(new SearchParameters());
            tiposPrestadores = pagingResponse.Items;
            StateHasChanged();
        }

        private async Task<TableData<MotorRegrasAutorizadorDto>> ServerReload(TableState state)
		{
			state.Page = bnOnSearchChangePage == false ? state.Page + 1 : 1;



			if (state.SortLabel != null)
				regrasParameters.OrderBy = Util.OrderBy(state.SortLabel, state.SortDirection);

			regrasParameters.Ativo = _statusSearch;
			regrasParameters.Search = searcRegra;
			regrasParameters.PageNumber = state.Page;
			regrasParameters.PageSize = state.PageSize;
			regrasParameters.TakeSize = 75000;
			await GetRegras();
			bnOnSearchChangePage = false;
			//	pagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
			return new TableData<MotorRegrasAutorizadorDto>() { TotalItems = _MetaData.TotalCount, Items = Regras };

		}
		private async Task GetRegras()
		{
			//_AutoPreAprovParameters.Status = IsActiveSearchAutorizacao;

			var pagingResponse = await motorRegrasRepository.GetAllRegras(regrasParameters);

			Regras = pagingResponse.Items;
			_MetaData = pagingResponse.MetaData;
			totalItems = Regras.Count();

			InvokeAsync(StateHasChanged);
		}

	



		private void OnSearch()
		{
			
            bnOnSearchChangePage = true;


			InvokeAsync(StateHasChanged);
			mudTable.ReloadServerData();
		}
		private void RowClickEvent(TableRowClickEventArgs<MotorRegrasAutorizadorDto> tableRowClickEventArgs)
		{
		}

		private string SelectedRowClassFunc(MotorRegrasAutorizadorDto regra, int rowNumber)
		{
			
			if (mudTable.SelectedItem != null && mudTable.SelectedItem.Equals(regra))
			{
				//mudTable.SelectedItems.Add
				SelectedItem = regra;
				selectedRowNumber = rowNumber;
				return "selected";
			}
			else
			{
				return string.Empty;
			}
		}


		public async void Save()
		{
			if (regra.IdRegra == 0) 
			{
				MotorRegrasAutorizadorForCreationDto regraToCreate = new MotorRegrasAutorizadorForCreationDto()
				{
					NomeRegra = regra.NomeRegra,
					DescricaoRegra = regra.DescricaoRegra,
					CodTipoGuia = regra.CodTipoGuia,
					Ativo = regra.Ativo,
					CodPrestadorExec=regra.CodPrestadorExec,
					CodPrestadorSolic=regra.CodPrestadorSolic,
					CodProcedimento = regra.CodProcedimento,
				    CodTipoPrestador = regra.CodTipoPrestador,
				    DtInicioVigencia = _dateRange.Start,
				    DtFimVigencia = _dateRange.End,
				    NomeAcao = regra.NomeAcao,
				    StatusGuia = regra.StatusGuia
				};
				var pagingResponse = await motorRegrasRepository.CreateRegra(regraToCreate);
				Snackbar.Add("Autorização criada com sucesso", Severity.Warning);
				mudTable.ReloadServerData();


			}
			else
			{

				MotorRegrasAutorizadorForManipulationDto regraToUpdate = new MotorRegrasAutorizadorForCreationDto()
				{
					
					NomeRegra = regra.NomeRegra,
					DescricaoRegra = regra.DescricaoRegra,
					CodTipoGuia = regra.CodTipoGuia,
					Ativo = regra.Ativo,
					CodPrestadorExec = regra.CodPrestadorExec,
					CodPrestadorSolic = regra.CodPrestadorSolic,
					CodProcedimento = regra.CodProcedimento,
					CodTipoPrestador = regra.CodTipoPrestador,
					DtInicioVigencia = _dateRange.Start,
					DtFimVigencia = _dateRange.End,
					NomeAcao = regra.NomeAcao,
					StatusGuia = regra.StatusGuia
				};
				await motorRegrasRepository.UpdateRegra(SelectedItem.IdRegra, regraToUpdate);
				Snackbar.Add("Autorização atualizada com sucesso", Severity.Warning);
				mudTable.ReloadServerData();

			}
			
			Expandir(false);
		}

		private async Task Delete()
		{
			bool? result = await mboxApagar.Show();

			state = result == null ? "Cancelado" : "Apagar";
			if (state == "Apagar")
			{
				await motorRegrasRepository.DeleteAutorizacao(SelectedItem.IdRegra);
				Snackbar.Add("Regra apagada com sucesso", Severity.Warning);
				mudTable.ReloadServerData();
				InvokeAsync(StateHasChanged);
			}

		}

		private void OpenFormCreate()
		{
			Expandir(true);
		}
		
		private void OpenFormEdit()
		{
			Edit(SelectedItem.IdRegra);
			Expandir(true);
		}
		private void Edit(int id)
		{
			regra = Regras.FirstOrDefault(c => c.IdRegra == id);
		}
		

		public void Dispose() => Interceptor.DisposeEvent();

	}
}
