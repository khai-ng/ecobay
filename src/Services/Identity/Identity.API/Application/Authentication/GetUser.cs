using Core.Autofac;
using Core.Pagination;
using Core.AppResults;
using Identity.API.Application.Common.Abstractions;
using Identity.Domain.Entities.UserAggregate;
using MediatR;

namespace Identity.Application.Services
{
    public class GetUser : IRequestHandler<GetUserRequest, AppResult<PagingResponse<User>>>, ITransient
    {
        private readonly IUserRepository _userRepository;
        public GetUser(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<AppResult<PagingResponse<User>>> Handle(
            GetUserRequest request,
            CancellationToken ct)
        {
            var rs = await _userRepository.GetPagedAsync(request).ConfigureAwait(false);
            return AppResult.Success(rs);
		}
    }
}