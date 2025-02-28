using Newtonsoft.Json;

namespace PizzaStar.Services;

public class ReCaptchaService
{
    private readonly string _secretKey;

    public ReCaptchaService(IConfiguration configuration)
    {
        _secretKey = configuration["ReCaptcha:SecretKey"];
    }

    public async Task<bool> ValidateCaptchaAsync(string captchaResponse)
    {
        if (string.IsNullOrEmpty(captchaResponse))
        {
            return false;
        }

        var client = new HttpClient();
        var response = await client.PostAsync(
            $"https://www.google.com/recaptcha/api/siteverify?secret={_secretKey}&response={captchaResponse}",
            null
        );

        var jsonResponse = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject(jsonResponse);

        return result.success == "true";
    }
}
