using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmailProvider.Middleware;

public class PolicyMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers.Authorization;
        var tokenHandler = new JwtSecurityTokenHandler();

        if (tokenHandler.CanReadToken(token))
        {
            var jwtToken = tokenHandler.ReadJwtToken(token);

            var policiesFromToken = jwtToken.Claims
                .Where(c => c.Type == "Policy")
                .Select(c => c.Value)
                .ToList();

            foreach (var policy in policiesFromToken)
            {
                context.User.AddIdentity(new ClaimsIdentity([new Claim("Policy", policy)]));
            }
        }

        await next(context);
    }
}