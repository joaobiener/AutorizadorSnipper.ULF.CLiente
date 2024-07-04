namespace AutorizadorSnipper.ULF.Cliente.Helper
{
    public static class RoleConst
    {
        
        public const string Administrator = "Administrator";
        public const string PrestadorAdmin = "PrestadorAdmin";
        public const string PrestadorAuditor = "PrestadorAuditor";
		public const string RelacaoPrestador = "RelacaoPrestador";

		public const string Central0800 = "Central0800";

		public static string RolesString(params string[] roles)
        {
            return string.Join(",", roles);
        }
    }
}
