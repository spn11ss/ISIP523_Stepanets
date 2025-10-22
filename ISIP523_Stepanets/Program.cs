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
}
}