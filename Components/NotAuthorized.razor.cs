

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace AutorizadorSnipper.ULF.Cliente.Components
{
    public partial class NotAuthorized
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private async Task GoBack()
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }
    }
}
