using Microsoft.AspNetCore.Mvc;
using PasswordGeneratorApp.Models;
using System.Text;

namespace PasswordGeneratorApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View(new PasswordOptions());
    }

    [HttpPost]
    public IActionResult Index(PasswordOptions options)
    {
        options.GeneratedPassword = GeneratePassword(options);
        return View(options);
    }

    private string GeneratePassword(PasswordOptions options)
    {
        var random = new Random();
        var chars = new StringBuilder();

        if (options.IncludeLowercase) chars.Append("abcdefghijklmnopqrstuvwxyz");
        if (options.IncludeUppercase) chars.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
        if (options.IncludeNumbers) chars.Append("0123456789");
        if (options.IncludeSymbols) chars.Append("!@#$%^&*()-_=+[]{}|;:,.<>?");

        if (chars.Length == 0) return "";

        var result = new StringBuilder();
        for (int i = 0; i < options.Length; i++)
        {
            var idx = random.Next(chars.Length);
            result.Append(chars[idx]);
        }

        return result.ToString();
    }
}