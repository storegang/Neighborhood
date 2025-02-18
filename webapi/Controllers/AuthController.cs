using Microsoft.AspNetCore.Mvc;
using webapi.Models.Authentication;
using System.Text.Json;

namespace webapi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IConfiguration config) : ControllerBase
{
    private readonly IConfiguration _config = config;

    [HttpPost]
    public async Task<ActionResult> GetToken([FromForm] LoginInfo loginInfo)
    {
        string authUri = "https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=" + _config["AUTH_API_KEY"];

        using HttpClient httpClient = new();

        FirebaseLoginInfo firebaseLoginInfo = new()
        {
            Email = loginInfo.Username,
            Password = loginInfo.Password,
        };

        var authResult = await httpClient.PostAsJsonAsync(authUri, firebaseLoginInfo,
            new JsonSerializerOptions() { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var googleToken = await authResult.Content.ReadFromJsonAsync<GoogleToken>();

        Token token = new()
        {
            token_type = "Bearer",
            access_token = googleToken.idToken,
            id_token = googleToken.idToken,
            expires_in = int.Parse(googleToken.expiresIn),
            refresh_token = googleToken.refreshToken,
        };

        return Ok(token);
    }
}
