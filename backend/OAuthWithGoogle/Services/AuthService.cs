
using OAuthWithGoogle.Models;
using OAuthWithGoogle.Repository;
using System.Linq;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace OAuthWithGoogle.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(Payload payload);
    }

    public class AuthService : IAuthService
    {
        private readonly IBaseRepository _repository;

        public AuthService(IBaseRepository repository)
        {
            _repository = repository;
        }
        public async Task<User> Authenticate(Payload payload)
        {
            var user = _repository.GetAllQuery<User>().Where(u => u.Email == payload.Email).FirstOrDefault();
            if (user == null)
            {
                user = new User()
                {
                    Name = payload.Name,
                    Email = payload.Email,
                    OauthSubject = payload.Subject,
                    OauthIssuer = payload.Issuer
                };
                user = _repository.Insert<User>(user);
            }
            return user;
        }
    }
}
