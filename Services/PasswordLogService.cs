using PasswordGeneratorApp.Models;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace PasswordGeneratorApp.Services
{
    public class PasswordLogService
    {
        private readonly string _filePath = "Data/password_logs.json";

        public async Task<List<PasswordLogEntry>> GetAllAsync()
        {
            if (!File.Exists(_filePath)) return new List<PasswordLogEntry>();

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<PasswordLogEntry>>(json) ?? new List<PasswordLogEntry>();
        }

        public async Task AddAsync(PasswordLogEntry entry)
        {
            var entries = await GetAllAsync();
            entries.Add(entry);
            var json = JsonSerializer.Serialize(entries, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }
}