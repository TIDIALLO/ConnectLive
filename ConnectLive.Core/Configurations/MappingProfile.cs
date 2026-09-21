using AutoMapper;
using ConnectLive.Portal.Shared.Request;
using ConnectLive.Portal.Shared.Response;
using ConntectLive.DAL;
using System.Security.Cryptography;
using System.Text;

namespace ConnectLive.Core.Api.Configurations;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SaveUserRequest, UserEntity>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserFirstName));

        CreateMap<UserEntity, SaveUserResponse>()
            .ForMember(dest => dest.Signature, opt => opt.MapFrom<TokenResolver>()); 
    }
}

public class TokenResolver : IValueResolver<UserEntity, SaveUserResponse, Token>
{
    public Token Resolve(UserEntity source, SaveUserResponse destination, Token destMember, ResolutionContext context)
    {
        StringBuilder sb = new StringBuilder();
        using (HashAlgorithm algorithm = SHA256.Create())
            foreach (var b in algorithm.ComputeHash(Encoding.UTF8.GetBytes(source.Email)))
            {
                sb.Append(b.ToString("x2"));
            };

        return new Token
        {
            Time = DateTime.Now,
            Secret = sb.ToString()
        };
    }
}
