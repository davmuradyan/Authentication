namespace Authentication.Infrastructure;

public class Policies
{
    public record Company
    {
        public const string Create = "Company.Create";
        public const string Read = "Company.Read";
        public const string Update = "Company.Update";
        public const string Delete = "Company.Delete";
    }
}