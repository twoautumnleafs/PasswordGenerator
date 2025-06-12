using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PasswordGeneratorApp.Data;
using PasswordGeneratorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasswordOptionsModel = PasswordGeneratorApp.Models.PasswordOptions;

namespace PasswordGeneratorApp.Controllers
{
    [Authorize]
    public class PasswordController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PasswordController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new PasswordOptionsModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(PasswordOptionsModel options)
        {
            options.GeneratedPassword = GeneratePassword(options);

            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var generated = new GeneratedPassword
                {
                    PasswordText = options.GeneratedPassword,
                    UserId = user.Id
                };

                _context.GeneratedPasswords.Add(generated);
                await _context.SaveChangesAsync();
            }

            return View(options);
        }

        private string GeneratePassword(PasswordOptionsModel options)
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

        [HttpGet]
        public async Task<IActionResult> History()
        {
            var user = await _userManager.GetUserAsync(User);
            var passwords = _context.GeneratedPasswords
                .Where(p => p.UserId == user.Id)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();

            return View(passwords);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> AdminLogs()
        {
            var logs = await _context.GeneratedPasswords
                .Include(p => p.User)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            return View(logs);
        }
    }
}
