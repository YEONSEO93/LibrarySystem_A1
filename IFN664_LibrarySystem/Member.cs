using System;

namespace LibrarySystem
{
    public class Member
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string Password { get; }
        public string Phone { get; }

        private const int MaxBorrow = 5;
        private Movie[] borrowedMovies = new Movie[MaxBorrow];
        private int borrowedCount = 0;

        public Member(string firstName, string lastName, string password, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
            Phone = phone;
        }

        public string FullName => $"{FirstName} {LastName}";

        public bool BorrowMovie(Movie movie)
        {
            if (borrowedCount >= MaxBorrow) return false;

            for (int i = 0; i < borrowedCount; i++)
            {
                if (borrowedMovies[i].Title == movie.Title)
                    return false;
            }

            if (movie.AvailableCopies <= 0) return false;

            borrowedMovies[borrowedCount++] = movie;
            movie.AvailableCopies--;
            movie.BorrowCount++;
            return true;
        }

        public bool ReturnMovie(Movie movie)
        {
            for (int i = 0; i < borrowedCount; i++)
            {
                if (borrowedMovies[i].Title == movie.Title)
                {
                    for (int j = i; j < borrowedCount - 1; j++)
                        borrowedMovies[j] = borrowedMovies[j + 1];

                    borrowedMovies[--borrowedCount] = null;
                    movie.AvailableCopies++;
                    return true;
                }
            }
            return false;
        }

        public void ListBorrowedMovies()
        {
            if (borrowedCount == 0)
            {
                Console.WriteLine("No movies currently borrowed.");
                return;
            }

            Console.WriteLine("Movies currently borrowed:");
            for (int i = 0; i < borrowedCount; i++)
                Console.WriteLine($"- {borrowedMovies[i].Title}");
        }

        public Movie[] GetBorrowedMovies()
        {
            Movie[] copy = new Movie[borrowedCount];
            for (int i = 0; i < borrowedCount; i++)
                copy[i] = borrowedMovies[i];
            return copy;
        }

        public int BorrowedCount => borrowedCount;
    }
}