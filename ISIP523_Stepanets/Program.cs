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

    }