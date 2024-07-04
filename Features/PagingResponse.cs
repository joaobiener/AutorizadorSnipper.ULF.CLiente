using Autorizador.ULF.Services.Shared.RequestFeatures;

namespace AutorizadorSnipper.ULF.Cliente.Features
{
    public class PagingResponse<T> where T : class
	{
		public List<T>? Items { get; set; }
		public MetaData? MetaData { get; set; }
	}
}
