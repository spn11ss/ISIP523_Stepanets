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
    class Program
    {
        private static List<TextStatistics> allStatistics = new List<TextStatistics>();

        // Множества гласных букв (русские и английские)
        private static readonly HashSet<char> vowels = new HashSet<char>
        {
            'а', 'е', 'ё', 'и', 'о', 'у', 'ы', 'э', 'ю', 'я',
            'a', 'e', 'i', 'o', 'u', 'y'
        };

        // Множества согласных букв (русские и английские)
        private static readonly HashSet<char> consonants = new HashSet<char>
        {
            'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м', 'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ',
            'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'z'
        };

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            bool exit = false;

            while (!exit)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Функция анализа текста будет реализована в следующем коммите");
                        break;
                    case "2":
                        ShowAllStatistics();
                        break;
                    case "3":
                        exit = true;
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор! Попробуйте снова.");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("=== АНАЛИЗАТОР ТЕКСТА ===");
            Console.WriteLine("1. Анализировать новый текст");
            Console.WriteLine("2. Показать статистику по всем текстам");
            Console.WriteLine("3. Выход");
            Console.Write("Выберите действие: ");
        }

    }
}
