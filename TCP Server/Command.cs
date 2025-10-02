internal class Command
{
    public const string Get = "GET";
    public const string Post = "POST";
    public const string Put = "PUT";
    public const string Delete = "DELETE";
    public string? Method { get; set; }
    public Car? Car { get; set; }
}