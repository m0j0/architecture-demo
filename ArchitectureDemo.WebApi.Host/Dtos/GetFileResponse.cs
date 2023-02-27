namespace ArchitectureDemo.WebApi.Host.Dtos;

public class GetFileResponse
{
    public enum Tag
    {
        UserNotFound,
        FileNotFound
    }

    public Tag ResponseTag { get; set; }
}
