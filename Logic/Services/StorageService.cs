using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Returns.Domain.Dto;
using Returns.Domain.Dto.Storage;
using Returns.Domain.Services;

namespace Returns.Logic.Services;

public class StorageService : IStorageService
{
    private readonly ILogger _logger;
    private readonly StorageOptions _options;

    // ReSharper disable once SuggestBaseTypeForParameterInConstructor
    public StorageService(ILogger<StorageService> logger, IOptions<StorageOptions> options)
    {
        _logger = logger;
        _options = options.Value;
    }

    public async Task<ValueResponse<Guid>> Create(Stream stream)
    {
        try
        {
            var id = Guid.NewGuid();

            await using var file = File.OpenWrite(string.Format(_options.Path, id));

            await stream.CopyToAsync(file);

            return new ValueResponse<Guid>
            {
                Success = true,
                Value = id
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save the file.");

            return new ValueResponse<Guid>
            {
                Message = "Failed to save the file."
            };
        }
    }

    public Response Delete(params Guid[] ids)
    {
        return Delete(ids.AsEnumerable());
    }

    public Response Delete(IEnumerable<Guid> ids)
    {
        var messages = new List<string>();
        var success = true;

        foreach (var id in ids)
        {
            try
            {
                File.Delete(string.Format(_options.Path, id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Failed to delete file {id}.", id);

                messages.Add($"Failed to delete file {id}.");

                success = false;
            }
        }

        return new Response
        {
            Message = messages.Any() ? "One or more files failed to be deleted." : default(string),
            Messages = messages,
            Success = success
        };
    }

    public ValueResponse<Stream> Get(Guid id)
    {
        try
        {
            var file = File.OpenRead(string.Format(_options.Path, id));

            return new ValueResponse<Stream>
            {
                Success = true,
                Value = file
            };
        }
        catch (FileNotFoundException fnfe)
        {
            _logger.LogError(fnfe, "File {id} was not found.", id);

            return new ValueResponse<Stream>
            {
                Message = "File not found."
            };
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to get file {id}.", id);

            return new ValueResponse<Stream>
            {
                Message = "Failed to get file."
            };
        }
    }
}
