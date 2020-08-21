namespace Domain.Models
{
    public static class Constants
    {
#if DEBUG
        public const string DefaultConnection = "Server=DESKTOP-JPQ21PS;Database=Start01Bd;Trusted_Connection=True;MultipleActiveResultSets=true";
        //public const string DefaultConnection = "Server=24.234.59.102;Database=Start01Bd;Persist Security Info=True;User ID=eau199;Password=SafeBroker%100";
#else
        //.102
        public const string DefaultConnection =
            "Server=SP-VL;Database=Start01Bd;Persist Security Info=True;User ID=eau199;Password=SafeBroker%100";
#endif
    }
}
