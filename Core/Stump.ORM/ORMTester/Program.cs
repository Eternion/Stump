using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using MySql.Data.MySqlClient;
using Stump.ORM;
using Stump.ORM.SubSonic.DataProviders;
using Stump.ORM.SubSonic.Extensions;
using Database = Stump.ORM.Database;

namespace ORMTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new DatabaseAccessor(new DatabaseConfiguration()
            {
                DbName = "test",
                Host = "localhost",
                Password = "",
                User = "root",
                ProviderName = "MySql.Data.MySqlClient",
            });

            db.RegisterMappingAssembly(Assembly.GetExecutingAssembly());
            
            db.Initialize();
            db.OpenConnection();

            db.Database.Execute("DELETE FROM authors");
            db.Database.Execute("DELETE FROM books");

            var author = new Author()
            {
                Books = new List<Book>(),
                Name = "Bernard"
            };

            var author2 = new Author()
            {
                Books = new List<Book>(),
                Name = "Clement"
            };

            var book = new Book()
            {
                Author = author,
                Name = "Livre de Bernard 1",
                Type = BookType.Romance,
                PublicationDate = DateTime.Now,
            };

            var book2 = new Book()
            {
                Author = author,
                Name = "Livre de Bernard 2",
                Type = BookType.Polar,
                PublicationDate = DateTime.Now,
            };

            var book3 = new Book()
            {
                Author = author2,
                Name = "Livre de Clement 1",
                Type = BookType.Romance,
                PublicationDate = DateTime.Now,
            };

            author.Books.Add(book);
            author.Books.Add(book2);
            author2.Books.Add(book3);

            db.Database.Save(author);
            db.Database.Save(author2);
            db.Database.Save(book);
            db.Database.Save(book2);
            db.Database.Save(book3);

            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var books = db.Database.Fetch<Book, Author, Book>(new BookRelator().Map, BookRelator.FetchQuery);
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds / 100d + "ms");

            var authors = db.Database.Fetch<Author, Book, Author>(new AuthorRelator().Map, AuthorRelator.FetchQuery);

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var bookGet = db.Database.Query<Book, Author, Book>(new BookRelator().Map, "SELECT * FROM books LEFT JOIN authors ON books.AuthorId = authors.Id WHERE books.Name=@0 LIMIT 1", "").FirstOrDefault();
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds / 100d + "ms");

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM books LEFT JOIN authors ON books.AuthorId = authors.Id";
                var reader = cmd.ExecuteReader();
                while (reader.NextResult())
                    ;
                reader.Close();
            }
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds / 100d + "ms");

            var exists = db.Database.ExecuteScalar<bool>("SELECT EXISTS(SELECT 1 FROM books WHERE Name='Livre de Benard 1')");

            Console.Read();
        }
    }
}
