namespace Drivin.Models;

public class EmailsSubfile
{
    public string FileName { get; }
    public string Url { get; }
    public List<string> Emails { get; }

    public EmailsSubfile(string fileName, List<string> emails)
    {
        FileName = fileName;
        Emails = emails;
        Url = GenerateLink("/EmailsSubfile", fileName, emails);
    }

    private static string GenerateLink(string endpoint, string fileName, List<string> emails)
    {
        var emailsList = string.Join("", emails.Select(e => $"&emails={e}"));
        return $"{endpoint}?fileName={fileName}{emailsList}";
    }
}
