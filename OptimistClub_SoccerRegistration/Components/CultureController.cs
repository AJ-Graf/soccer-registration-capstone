using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace OptimistClub_SoccerRegistration.Components
{
    [Route("Culture")]
    public class CultureController : Controller
    {
        [HttpGet("Set")]
        public IActionResult Set(string culture, string redirectUri)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(
                        new RequestCulture(culture)
                    ),
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddYears(1)
                    }
                );
            }

            return string.IsNullOrWhiteSpace(redirectUri)
                ? Redirect("/")
                :LocalRedirect(redirectUri);
            }
    }
}
