using System.Text.RegularExpressions;

namespace Drivin;

public class UtilsService
{
    /// <summary>
    /// Validate emails
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        // The following regular expression pattern is a common pattern for email validation.
        // It checks for most valid email formats but is not 100% comprehensive.
        const string pattern = @"^[\w.-]+@([\w-]+\.)+[\w-]{2,4}$";
        var isValid = Regex.IsMatch(email, pattern);
        return isValid;
    }
}
