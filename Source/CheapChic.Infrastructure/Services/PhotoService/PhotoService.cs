using CheapChic.Data;
using CheapChic.Data.Entities;
using CheapChic.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace CheapChic.Infrastructure.Services.PhotoService;

public class PhotoService : IPhotoService
{
    private readonly CheapChicContext _context;

    public PhotoService(CheapChicContext context)
    {
        _context = context;
    }

    public async Task<Guid> GetPhotoId(byte[] content, CancellationToken cancellationToken = default)
    {
        var hash = content.GetHashSha512();

        var photoId = await _context.Photos
            .AsNoTracking()
            .Where(x => x.Hash == hash)
            .Select(x => x.Id)
            .FirstOrDefaultAsync(cancellationToken);

        if (photoId == default)
        {
            var photo = new PhotoEntity
            {
                FileSize = content.LongLength,
                Content = content,
                Hash = hash
            };

            await _context.AddAsync(photo, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            photoId = photo.Id;
        }

        return photoId;
    }
}