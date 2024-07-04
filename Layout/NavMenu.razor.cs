using AutorizadorSnipper.ULF.Cliente.Helper;
using AutorizadorSnipper.ULF.Cliente.Model;
using Microsoft.AspNetCore.Components;



namespace AutorizadorSnipper.ULF.Cliente.Layout
{

    public partial class NavMenu
    {
        public bool Aguardando = false;
        public List<Aplicacao> AplicacaoList { get; set; } = new List<Aplicacao>();

        private bool collapseNavMenu = true;

        [Inject]
        public IUtil Util { get; set; }
        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private bool expandirSubMenu;

        private bool expandirSubMenu2;

        protected async override Task OnInitializedAsync()
        {
            Aguardando = true;
            AplicacaoList = await Util.GetAplicacoes();
            Aguardando = false;
        }

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
        private string GetDetalhes(string identName)
        {
            char[] chavetas = { '[', ']', '"' };
            string identSemChaves = identName.Replace("[", string.Empty).Replace("]", string.Empty).Replace("\"", string.Empty);


            string[] arrIdent = identSemChaves.Split(",");


            return arrIdent[1] + " " + arrIdent[2];
        }
    }
}
