namespace Drivin.Services;

using Drivin.Models;

public class ProcessEmailFilesService
{
    private int EmailsPerFile { get; set; }
    private List<string> SortedEmailsList { get; set; }
    private List<EmailsSubfile> SplitedEmailsList { get; }

    public ProcessEmailFilesService(int emailsPerFile = 5)
    {
        EmailsPerFile = emailsPerFile;
        SortedEmailsList = new();
        SplitedEmailsList = new();
    }

    public async Task<List<EmailsSubfile>> ProcessEmailsFile(IFormFile file)
    {
        await ReadEmailsFile(file);
        SplitEmailsList(SortedEmailsList);
        return SplitedEmailsList;
    }

    /// <summary>
    /// Turns a text file in a lista of strings
    /// Sorted and without blank lines
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    private async Task ReadEmailsFile(IFormFile file)
    {
        var result = new HashSet<string>();
        using (var reader = new StreamReader(file.OpenReadStream()))
        {
            while (reader.Peek() >= 0)
            {
                var line = await reader.ReadLineAsync();
                var trimmedLine = line?.Trim();
                var emailIsValid = UtilsService.IsValidEmail(trimmedLine);
                if (emailIsValid)
                    result.Add(trimmedLine!);
            }
        }

        var sortedList = result.Order();
        SortedEmailsList = sortedList.ToList();
    }

    /// <summary>
    /// Turns a List of strings in a List of Lists of strings, grouping by number (EmailsPerFile)
    /// </summary>
    /// <param name="inputList"></param>
    /// <returns></returns>
    private void SplitEmailsList(List<string> inputList)
    {
        var index = 1;

        for (int i = 0; i < inputList.Count; i += EmailsPerFile)
        {
            var subList = new List<string>();
            for (int j = 0; j < EmailsPerFile && (i + j) < inputList.Count; j++)
                subList.Add(inputList[i + j]);

            var emailsSubfile = new EmailsSubfile($"{index:00}.txt", subList);
            index++;

            SplitedEmailsList.Add(emailsSubfile);
        }
    }
}
