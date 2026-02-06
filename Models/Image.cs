public class Image
{
    public int Id { set; get; }
    public required string FileName { set; get; }
    public required string FileType {set; get; }
    public required string DownloadUrl {set; get; }
    public bool IsPrimary { get; set; }
    public int ProductId { set; get; }
}