namespace Domain.Models
{
    public static class Constants
    {
#if DEBUG
        public const string DefaultConnection = "Server=DESKTOP-MULONQN;Database=Start01Bd;Trusted_Connection=True;MultipleActiveResultSets=true";
#else
        public const string DefaultConnection =
            "Server=DESKTOP-0HPQBAC;Database=Start01Bd;Trusted_Connection=True;MultipleActiveResultSets=true";
#endif
    }
}
