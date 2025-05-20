using System;
using System.Collections.Generic;

namespace LibrarySystem
{
    public class Member
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Password { get; }
        public string Phone { get; }
        public List<Movie> BorrowedMovies { get; }

        public Member(string firstName, string lastName, string password, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Phone = phone;
            BorrowedMovies = new List<Movie>();
        }

        public string FullName => $"{FirstName} {LastName}";

        public bool BorrowMovie(Movie movie)
        {
            if (BorrowedMovies.Count >= 5) return false;
            if (BorrowedMovies.Exists(m => m.Title == movie.Title)) return false;
            if (movie.AvailableCopies <= 0) return false;

            BorrowedMovies.Add(movie);
            movie.AvailableCopies--;
            return true;
        }

        public bool ReturnMovie(Movie movie)
        {
            var borrowedMovie = BorrowedMovies.Find(m => m.Title == movie.Title);
            if (borrowedMovie != null)
            {
                BorrowedMovies.Remove(borrowedMovie);
                movie.AvailableCopies++;
                return true;
            }

            return false;
        }

        public void ListBorrowedMovies()
        {
            if (BorrowedMovies.Count == 0)
            {
                Console.WriteLine("No movies currently borrowed.");
                return;
            }

            Console.WriteLine("Movies currently borrowed:");
            foreach (var movie in BorrowedMovies)
                Console.WriteLine($"- {movie.Title}");

        }
    }
}