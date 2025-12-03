using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Repositories;
using Domain.ArtistGenres;
using MediatR;

namespace Application.ArtistGenres.Queries;

public record GetArtistGenresQuery : IRequest<IReadOnlyList<ArtistGenre>>
{
    public required Guid ArtistId { get; init; }
}

public class GetArtistGenresQueryHandler : IRequestHandler<GetArtistGenresQuery, IReadOnlyList<ArtistGenre>>
{
    private readonly IArtistGenreRepository _artistGenreRepository;

    public GetArtistGenresQueryHandler(IArtistGenreRepository artistGenreRepository)
    {
        _artistGenreRepository = artistGenreRepository;
    }

    public async Task<IReadOnlyList<ArtistGenre>> Handle(GetArtistGenresQuery request, CancellationToken cancellationToken)
    {
        return await _artistGenreRepository.GetByArtistIdAsync(request.ArtistId, cancellationToken);
    }
}

