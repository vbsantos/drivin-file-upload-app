namespace Drivin.Services;

using System.Text.RegularExpressions;

using Drivin.Models;

public class EmailFilesService
{
    public static async Task<List<EmailsSubfile>> ProcessEmailsFile(IFormFile file)
    {
        var emailsList = await ReadEmailsFile(file);
        var emailsSublists = SplitEmailsList(emailsList);
        return emailsSublists;
    }

    /// <summary>
    /// Turns a text file in a lista of strings
    /// Sorted and without blank lines
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private static async Task<List<string>> ReadEmailsFile(IFormFile file)
    {
        var result = new HashSet<string>();
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();
                var trimmedLine = line?.Trim();
                if (IsValidEmail(trimmedLine))
                    result.Add(trimmedLine!);
            }
        }

        var sortedList = result.Order();
        return sortedList.ToList();
    }

    /// <summary>
    /// Turns a List of strings in Lists of 5 strings max
    /// </summary>
    /// <param name="inputList"></param>
    /// <returns></returns>
    private static List<EmailsSubfile> SplitEmailsList(List<string> inputList)
    {
        var result = new List<EmailsSubfile>();
        var index = 1;

        for (int i = 0; i < inputList.Count; i += 5)
        {
            var subList = new List<string>();
            for (int j = 0; j < 5 && (i + j) < inputList.Count; j++)
            {
                subList.Add(inputList[i + j]);
            }
            var emailsSubfile = new EmailsSubfile($"{index:00}.txt", subList);
            index++;
            result.Add(emailsSubfile);
        }

        return result;
    }

    /// <summary>
    /// Validate emails
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    private static bool IsValidEmail(string? email)
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
