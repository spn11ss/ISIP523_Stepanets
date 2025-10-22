using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryManagement
{
    // Перечисление жанров книг
    public enum Genre
    {
        Fantasy,        // Фэнтези
        ScienceFiction, // Научная фантастика
        Mystery,        // Детектив
        Romance,        // Роман
        Historical,     // Исторический
        Biography,      // Биография
        Thriller        // Триллер
    }

    // Класс для представления книги
    public class Book
    {
        public int Id { get; set; }          // Уникальный идентификатор
        public string Title { get; set; }    // Название
        public string Author { get; set; }   // Автор
        public Genre Genre { get; set; }     // Жанр
        public int Year { get; set; }        // Год издания
        public decimal Price { get; set; }   // Цена

        // Конструктор
        public Book(int id, string title, string author, Genre genre, int year, decimal price)
        {
            Id = id;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }
        // Метод для вывода информации о книге
        public void DisplayInfo()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"Название: {Title}");
            Console.WriteLine($"Автор: {Author}");
            Console.WriteLine($"Жанр: {Genre}");
            Console.WriteLine($"Год издания: {Year}");
            Console.WriteLine($"Цена: {Price:C}");
            Console.WriteLine(new string('-', 40));
        }
    }
    class Program
    {
        private static List<Book> books = new List<Book>(); // Список всех книг
        private static int nextId = 1; // Счетчик для генерации ID

        static void Main(string[] args)
        {
            // Инициализация тестовыми данными
            InitializeTestData();

            bool exit = false;
            while (!exit)
            {
                DisplayMenu();
                string choice = Console.ReadLine();

                // Обработка выбора пользователя
                switch (choice)
                {
                    case "1": AddBook(); break;
                    case "2": RemoveBook(); break;
                    case "3": SearchBooks(); break;
                    case "4": SortBooks(); break;
                    case "5": FindExtremePriceBooks(); break;
                    case "6": GroupByAuthors(); break;
                    case "7": DisplayAllBooks(); break;
                    case "0": exit = true; Console.WriteLine("До свидания!"); break;
                    default: Console.WriteLine("Неверный выбор! Попробуйте снова."); break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
        }

        // Отображение главного меню
        static void DisplayMenu()
        {
            Console.WriteLine("=== СИСТЕМА УПРАВЛЕНИЯ БИБЛИОТЕКОЙ ===");
            Console.WriteLine("1. Добавить книгу");
            Console.WriteLine("2. Удалить книгу по ID");
            Console.WriteLine("3. Найти книги");
            Console.WriteLine("4. Отсортировать книги");
            Console.WriteLine("5. Самая дорогая и дешевая книга");
            Console.WriteLine("6. Группировка по авторам");
            Console.WriteLine("7. Показать все книги");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите действие: ");
        }
        // некоторые данные
        static void InitializeTestData()
        {
            books.Add(new Book(nextId++, "Властелин Колец", "Дж. Р. Р. Толкин", Genre.Fantasy, 1954, 1200));
            books.Add(new Book(nextId++, "1984", "Джордж Оруэлл", Genre.ScienceFiction, 1949, 800));
            books.Add(new Book(nextId++, "Убийство в Восточном экспрессе", "Агата Кристи", Genre.Mystery, 1934, 650));
            books.Add(new Book(nextId++, "Гордость и предубеждение", "Джейн Остин", Genre.Romance, 1813, 550));
            books.Add(new Book(nextId++, "Мастер и Маргарита", "Михаил Булгаков", Genre.Fantasy, 1967, 900));

            Console.WriteLine("Тестовые данные добавлены успешно!\n");
        }
        // Добавление новой книги с валидацией
        static void AddBook()
        {
            Console.WriteLine("\n=== ДОБАВЛЕНИЕ НОВОЙ КНИГИ ===");

            // Ввод названия с проверкой на пустоту
            string title;
            do
            {
                Console.Write("Введите название книги: ");
                title = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Название не может быть пустым!");
                }
            } while (string.IsNullOrWhiteSpace(title));

            // Ввод автора с проверкой на пустоту
            string author;
            do
            {
                Console.Write("Введите автора книги: ");
                author = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(author))
                {
                    Console.WriteLine("Автор не может быть пустым!");
                }
            } while (string.IsNullOrWhiteSpace(author));

            // Выбор жанра из списка
            Console.WriteLine("Выберите жанр:");
            var genres = Enum.GetValues(typeof(Genre));
            for (int i = 0; i < genres.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {genres.GetValue(i)}");
            }

            Genre genre;
            while (true)
            {
                Console.Write("Введите номер жанра: ");
                if (int.TryParse(Console.ReadLine(), out int genreIndex) && genreIndex >= 1 && genreIndex <= genres.Length)
                {
                    genre = (Genre)(genreIndex - 1);
                    break;
                }
                Console.WriteLine("Неверный номер жанра!");
            }

            // Ввод года с валидацией
            int year;
            while (true)
            {
                Console.Write("Введите год издания: ");
                if (int.TryParse(Console.ReadLine(), out year) && year > 0 && year <= DateTime.Now.Year)
                {
                    break;
                }
                Console.WriteLine($"Год должен быть положительным числом не больше {DateTime.Now.Year}!");
            }

            // Ввод цены с валидацией
            decimal price;
            while (true)
            {
                Console.Write("Введите цену книги: ");
                if (decimal.TryParse(Console.ReadLine(), out price) && price > 0)
                {
                    break;
                }
                Console.WriteLine("Цена должна быть положительным числом!");
            }

            // Создание и добавление новой книги
            Book newBook = new Book(nextId++, title, author, genre, year, price);
            books.Add(newBook);
            Console.WriteLine($"Книга '{title}' успешно добавлена с ID {newBook.Id}");
        }
        // Удаление книги по ID
        static void RemoveBook()
        {
            Console.WriteLine("\n=== УДАЛЕНИЕ КНИГИ ===");
            Console.Write("Введите ID книги для удаления: ");

            if (int.TryParse(Console.ReadLine(), out int id))
            {
                // LINQ поиск книги по ID
                Book bookToRemove = books.FirstOrDefault(b => b.Id == id);

                if (bookToRemove != null)
                {
                    books.Remove(bookToRemove);
                    Console.WriteLine($"Книга '{bookToRemove.Title}' успешно удалена.");
                }
                else
                {
                    Console.WriteLine($"Книга с ID {id} не найдена.");
                }
            }
            else
            {
                Console.WriteLine("Неверный формат ID!");
            }
        }

    }