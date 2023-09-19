namespace Drivin.Models;

public class FileUploadViewModel
{
    public required IFormFile File { get; set; }
}

public class SubfileViewModel
{
    public Guid Id { get; set; }
}
