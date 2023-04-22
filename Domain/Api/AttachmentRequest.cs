using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Returns.Domain.Api;

public class AttachmentRequest
{
    [Required] public IFormFile File { get; init; } = default!;

    [FileExtensions(
        ErrorMessage = "Only files with the following extensions are accepted: {1}.",
        Extensions = "pdf, jpeg, bmp, tiff, png, doc, docx, xls, xlsx"
    )]
    [ReadOnly(true)]
    public string FileName => File.FileName;
}