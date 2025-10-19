using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.Genres;
using MediatR;

namespace Application.Genres.Queries
{
    public record GetGenresQuery : IRequest<IReadOnlyList<Genre>>;

    public class GetGenresQueryHandler : IRequestHandler<GetGenresQuery, IReadOnlyList<Genre>>
    {
        private readonly IGenreQueries _queries;

        public GetGenresQueryHandler(IGenreQueries queries)
        {
            _queries = queries;
        }

        public Task<IReadOnlyList<Genre>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            return _queries.GetAllAsync(cancellationToken);
        }
    }
}


