namespace CheapChic.Infrastructure.Services.PhotoService;

public interface IPhotoService
{
    Task<Guid> GetPhotoId(byte[] content, CancellationToken cancellationToken = default);
}