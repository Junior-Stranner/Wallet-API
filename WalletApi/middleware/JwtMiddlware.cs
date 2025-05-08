using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenService _tokenService;

    public JwtMiddleware(RequestDelegate next, TokenService tokenService)
    {
        _next = next;
        _tokenService = tokenService;
    }

    public async Task Invoke(HttpContext context, UserService userService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            try
            {
                var principal = _tokenService.ValidateToken(token);
                var email = principal.FindFirstValue(ClaimTypes.NameIdentifier);

                if (email != null)
                {
                    var user = userService.GetUserByEmailAsync(email); // Método a ser implementado
                    if (user != null)
                    {
                        var identity = new ClaimsIdentity(principal.Claims, "jwt");
                        context.User = new ClaimsPrincipal(identity);
                    }
                }
            }
            catch
            {
                // Token inválido - não autentica
            }
        }

        await _next(context);
    }
}
