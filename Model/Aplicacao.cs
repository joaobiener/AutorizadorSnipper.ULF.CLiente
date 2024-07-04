
namespace AutorizadorSnipper.ULF.Cliente.Model;

public class Aplicacao
{
    public string? Title { get; set; }
	public string? Summary { get; set; }
	public string? Icon { get; set; }
    public MudBlazor.Color IconColor { get; set; }
	public string? HRef { get; set; }
    public string? ImageAddress { get; set; }
    public List<Role>? Roles { get; set; }
}
