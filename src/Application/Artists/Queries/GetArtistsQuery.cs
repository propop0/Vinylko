using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.Artists;
using MediatR;

namespace Application.Artists.Queries
{
    public record GetArtistsQuery : IRequest<IReadOnlyList<Artist>>;

    public class GetArtistsQueryHandler : IRequestHandler<GetArtistsQuery, IReadOnlyList<Artist>>
    {
        private readonly IArtistQueries _queries;

        public GetArtistsQueryHandler(IArtistQueries queries)
        {
            _queries = queries;
        }

        public Task<IReadOnlyList<Artist>> Handle(GetArtistsQuery request, CancellationToken cancellationToken)
        {
            return _queries.GetAllAsync(cancellationToken);
        }
    }
}


