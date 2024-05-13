using Microsoft.AspNetCore.Identity;
using StudentRecruitment.BLL.DTOs;
using StudentRecruitment.Domain.Entities;
using System.Security.Cryptography;
using System.Text;

namespace StudentRecruitment.BLL.Utilities
{
    public class CredentialsGenerator
    {
        private readonly UserManager<Student> _userManager;

        public CredentialsGenerator(UserManager<Student> userManager)
        {
            _userManager = userManager;
        }

        public string GeneratePassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var byteBuffer = new byte[10];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteBuffer);
            }

            char[] chars = new char[10];
            for (int i = 0; i < byteBuffer.Length; i++)
            {
                chars[i] = validChars[byteBuffer[i] % validChars.Length];
            }

            return new string(chars);
        }

        public async Task<string> GenerateUsername(StudentImportDto studentDto)
        {
            string fullName = $"{studentDto.Surname}{studentDto.Name[0]}";

            string baseUsername = ContainsCyrillic(fullName) ? TransliterateCyrillicToLatin(fullName) : fullName;
            baseUsername = baseUsername.ToLower();

            string username = baseUsername;
            int suffix = 0;

            while (await _userManager.FindByNameAsync(username) != null)
            {
                suffix++;
                username = $"{baseUsername}{suffix}";
            }

            return username;
        }

        private bool ContainsCyrillic(string input)
        {
            return input.Any(c => c >= 'А' && c <= 'я'); 
        }

        private string TransliterateCyrillicToLatin(string input)
        {
            Dictionary<char, string> charMap = new Dictionary<char, string>
            {
                {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "h"}, {'ґ', "g"},
                {'д', "d"}, {'е', "e"}, {'є', "ie"}, {'ж', "zh"}, {'з', "z"},
                {'и', "y"}, {'і', "i"}, {'ї', "i"}, {'й', "i"}, {'к', "k"},
                {'л', "l"}, {'м', "m"}, {'н', "n"}, {'о', "o"}, {'п', "p"},
                {'р', "r"}, {'с', "s"}, {'т', "t"}, {'у', "u"}, {'ф', "f"},
                {'х', "kh"}, {'ц', "ts"}, {'ч', "ch"}, {'ш', "sh"}, {'щ', "shch"},
                {'ю', "iu"}, {'я', "ia"}, {'ь', ""}
            };
            var result = new StringBuilder();
            foreach (var c in input.ToLower())
            {
                if (charMap.ContainsKey(c))
                {
                    result.Append(charMap[c]);
                }
                else
                {
                    result.Append(c);
                }
            }
            return result.ToString();
        }
    }
}
