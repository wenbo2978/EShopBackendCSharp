public record ImageDto(
    int Id,
    string FileName,
    string FileType,
    string DownloadUrl,
    bool IsPrimary,
    int ProductId
);

public record CreateImageRequest(
    string FileName,
    string FileType,
    string DownloadUrl,
    bool IsPrimary,
    int ProductId
);

public record UpdateImageRequest(
    string FileName,
    string FileType,
    string DownloadUrl,
    bool IsPrimary
);

