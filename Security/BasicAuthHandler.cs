using BlogPersonal.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace BlogPersonal.Security
{
    public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;

        public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IUserService userService
            ) : base(options, logger, encoder)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if(!Request.Headers.ContainsKey("Authorization"))
            {
                Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Admin\""); //esto hace q se muestre el popup
                return AuthenticateResult.Fail("No hay header");
            }

            bool result = false;
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                var username = credentials[0];
                var password = credentials[1];
                result = _userService.IsUser(username, password);
            }
            catch
            {
                return AuthenticateResult.Fail("Ocurrio algo");
            }

            if(!result)
            {
                Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Admin\"");
                return AuthenticateResult.Fail("Usuario o contrasenia invalida");
            }

            var claims = new Claim[] {
                new Claim(ClaimTypes.NameIdentifier, "Id"),
                new Claim(ClaimTypes.Name, "FullName")
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}
