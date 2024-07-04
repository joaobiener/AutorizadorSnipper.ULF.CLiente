namespace AutorizadorSnipper.ULF.Cliente.Configuration { 
    public class AuthAPIClient
	{
        public AuthAPIClient(HttpClient client)
        {
            Client = client;
        }

        public HttpClient Client { get; }
    }
}
