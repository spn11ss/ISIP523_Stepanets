using System;
using System.Collections.Generic;
using System.Text;

namespace ISIP523_Stepanets
{
    // Класс для хранения статистики по тексту
    public class TextStatistics
    {
        public string Text { get; set; }
        public int WordCount { get; set; }
        public string ShortestWord { get; set; }
        public string LongestWord { get; set; }
        public int SentenceCount { get; set; }
        public int VowelCount { get; set; }
        public int ConsonantCount { get; set; }
        public Dictionary<char, int> LetterFrequency { get; set; }
        public DateTime AnalysisTime { get; set; }

        public TextStatistics()
        {
            LetterFrequency = new Dictionary<char, int>();
        }

        public void DisplayStatistics()
        {
            Console.WriteLine("\n=== СТАТИСТИКА ТЕКСТА ===");
            Console.WriteLine($"Время анализа: {AnalysisTime}");
            Console.WriteLine($"Количество слов: {WordCount}");
            Console.WriteLine($"Самое короткое слово: '{ShortestWord}'");
            Console.WriteLine($"Самое длинное слово: '{LongestWord}'");
            Console.WriteLine($"Количество предложений: {SentenceCount}");
            Console.WriteLine($"Гласных букв: {VowelCount}");
            Console.WriteLine($"Согласных букв: {ConsonantCount}");

            Console.WriteLine("\nЧастота букв:");
            foreach (var entry in LetterFrequency)
            {
                Console.WriteLine($"{entry.Key}: {entry.Value}");
            }
            Console.WriteLine(new string('=', 40));
        }
    }
}
