namespace Domain.Models
{
    public static class Constants
    {
#if DEBUG
        public const string DefaultConnection = "Server=DESKTOP-JPQ21PS;Database=Start01Bd;Trusted_Connection=True;MultipleActiveResultSets=true";
#else
        public const string DefaultConnection =
            "Server=SP-VL;Database=Start01Bd;Persist Security Info=True;User ID=xxxx;Password=xxxxxx";
#endif
    }
}
