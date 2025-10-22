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
        static void AnalyzeNewText()
        {
            Console.WriteLine("\n=== АНАЛИЗ НОВОГО ТЕКСТА ===");
            Console.WriteLine("Введите текст (минимум 100 символов):");

            string text;
            do
            {
                text = Console.ReadLine();
                if (text.Length < 100)
                {
                    Console.WriteLine($"Текст слишком короткий! Введено {text.Length} символов. Нужно минимум 100.");
                    Console.Write("Пожалуйста, введите текст еще раз: ");
                }
            } while (text.Length < 100);

            TextStatistics stats = AnalyzeText(text);
            allStatistics.Add(stats);
            stats.DisplayStatistics();
        }

        static TextStatistics AnalyzeText(string text)
        {
            TextStatistics stats = new TextStatistics
            {
                Text = text,
                AnalysisTime = DateTime.Now
            };

            // Подсчет слов и поиск самого короткого/длинного слова
            string[] words = SplitTextIntoWords(text);
            stats.WordCount = words.Length;

            if (words.Length > 0)
            {
                stats.ShortestWord = words[0];
                stats.LongestWord = words[0];

                for (int i = 0; i < words.Length; i++)
                {
                    string word = words[i];

                    // Поиск самого короткого слова
                    if (word.Length < stats.ShortestWord.Length)
                    {
                        stats.ShortestWord = word;
                    }

                    // Поиск самого длинного слова
                    if (word.Length > stats.LongestWord.Length)
                    {
                        stats.LongestWord = word;
                    }
                }
            }
            // Подсчет предложений
            stats.SentenceCount = CountSentences(text);

            // Подсчет гласных, согласных и частоты букв
            CountLetters(text, stats);

            return stats;
        }

        // Разделение текста на слова
        static string[] SplitTextIntoWords(string text)
        {
            List<string> words = new List<string>();
            StringBuilder currentWord = new StringBuilder();

            // Проходим по каждому символу в тексте
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                // Если символ - буква, добавляем его к текущему слову
                if (char.IsLetter(c))
                {
                    currentWord.Append(c);
                }
                // Если не буква и у нас есть накопленное слово - добавляем его в список
                else if (currentWord.Length > 0)
                {
                    words.Add(currentWord.ToString());
                    currentWord.Clear();
                }
            }

            // Добавляем последнее слово, если оно есть
            if (currentWord.Length > 0)
            {
                words.Add(currentWord.ToString());
            }

            return words.ToArray();
        }
    }
}