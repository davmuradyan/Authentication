namespace Authentication.Application.Auth;

internal class HelperMethods
{
    internal static bool IsEmail(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }

        if (!email.Contains("@"))
        {
            return false;
        }

        if (!email.Contains("."))
        {
            return false;
        }

        if (email.Contains(" "))
        {
            return false;
        }
        
        var domain = email.Split('@')[1];
        if (string.IsNullOrEmpty(domain) ||  domain.Length < 3)
        {
            return false;
        }

        var address = email.Split('@')[0];
        if (string.IsNullOrEmpty(address))
        {
            return false;
        }
        
        return true;
    }
}