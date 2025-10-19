using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.Artists;
using MediatR;

namespace Application.Artists.Queries
{
    public record GetArtistByIdQuery(Guid Id) : IRequest<Artist?>;

    public class GetArtistByIdQueryHandler : IRequestHandler<GetArtistByIdQuery, Artist?>
    {
        private readonly IArtistQueries _queries;

        public GetArtistByIdQueryHandler(IArtistQueries queries)
        {
            _queries = queries;
        }

        public Task<Artist?> Handle(GetArtistByIdQuery request, CancellationToken cancellationToken)
        {
            return _queries.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}


