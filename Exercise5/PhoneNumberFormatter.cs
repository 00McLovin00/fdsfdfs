using System;
using System.Text.RegularExpressions;

namespace Exercise5
{
    public class PhoneNumberFormatter
    {
        // Регулярное выражение для поиска номеров в формате (012) 345-67-89
        private static readonly string PhonePattern = @"\((\d{3})\)\s*(\d{3})-(\d{2})-(\d{2})";

        // Регулярное выражение для поиска номеров в других форматах
        private static readonly string PhonePatternAlt1 = @"(\d{3})\s*(\d{3})\s*(\d{2})\s*(\d{2})";
        private static readonly string PhonePatternAlt2 = @"(\d{3})-(\d{3})-(\d{2})-(\d{2})";

        // Замена номера телефона на новый формат
        public string FormatPhoneNumbers(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            string result = text;

            // Замена формата (012) 345-67-89 -> +380 12 345 67 89
            result = Regex.Replace(result, PhonePattern, match =>
            {
                string code = match.Groups[1].Value;      // 012
                string part1 = match.Groups[2].Value;     // 345
                string part2 = match.Groups[3].Value;     // 67
                string part3 = match.Groups[4].Value;     // 89

                // Если код начинается с 0, заменяем на +380
                if (code.StartsWith("0"))
                {
                    return $"+380 {code.Substring(1)} {part1} {part2} {part3}";
                }
                else
                {
                    return $"+{code} {part1} {part2} {part3}";
                }
            });

            // Замена формата 0123456789 -> +380 12 345 67 89
            result = Regex.Replace(result, PhonePatternAlt1, match =>
            {
                string part1 = match.Groups[1].Value;     // 012
                string part2 = match.Groups[2].Value;     // 345
                string part3 = match.Groups[3].Value;     // 67
                string part4 = match.Groups[4].Value;     // 89

                if (part1.StartsWith("0"))
                {
                    return $"+380 {part1.Substring(1)} {part2} {part3} {part4}";
                }
                else
                {
                    return $"+{part1} {part2} {part3} {part4}";
                }
            });

            // Замена формата 012-345-67-89 -> +380 12 345 67 89
            result = Regex.Replace(result, PhonePatternAlt2, match =>
            {
                string part1 = match.Groups[1].Value;     // 012
                string part2 = match.Groups[2].Value;     // 345
                string part3 = match.Groups[3].Value;     // 67
                string part4 = match.Groups[4].Value;     // 89

                if (part1.StartsWith("0"))
                {
                    return $"+380 {part1.Substring(1)} {part2} {part3} {part4}";
                }
                else
                {
                    return $"+{part1} {part2} {part3} {part4}";
                }
            });

            return result;
        }

        // Проверка, есть ли номера телефонов в тексте
        public bool ContainsPhoneNumber(string text)
        {
            return Regex.IsMatch(text, PhonePattern) ||
                   Regex.IsMatch(text, PhonePatternAlt1) ||
                   Regex.IsMatch(text, PhonePatternAlt2);
        }

        // Извлечение всех номеров телефонов из текста
        public string[] ExtractPhoneNumbers(string text)
        {
            var matches = Regex.Matches(text, PhonePattern);
            var result = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                result[i] = matches[i].Value;
            }

            return result;
        }
    }
}