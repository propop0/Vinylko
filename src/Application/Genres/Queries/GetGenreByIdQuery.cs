using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces.Queries;
using Domain.Genres;
using MediatR;

namespace Application.Genres.Queries
{
    public record GetGenreByIdQuery(Guid Id) : IRequest<Genre?>;

    public class GetGenreByIdQueryHandler : IRequestHandler<GetGenreByIdQuery, Genre?>
    {
        private readonly IGenreQueries _queries;

        public GetGenreByIdQueryHandler(IGenreQueries queries)
        {
            _queries = queries;
        }

        public Task<Genre?> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            return _queries.GetByIdAsync(request.Id, cancellationToken);
        }
    }
}


