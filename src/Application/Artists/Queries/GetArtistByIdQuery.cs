using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.Artists;
using MediatR;
using Optional;

namespace Application.Artists.Queries
{
    public record GetArtistByIdQuery(Guid Id) : IRequest<Option<Artist>>;

    public class GetArtistByIdQueryHandler : IRequestHandler<GetArtistByIdQuery, Option<Artist>>
    {
        private readonly IArtistQueries _queries;

        public GetArtistByIdQueryHandler(IArtistQueries queries)
        {
            _queries = queries;
        }

        public Task<Option<Artist>> Handle(GetArtistByIdQuery request, CancellationToken cancellationToken)
        {
            return _queries.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}


