using System;
using System.Collections.Generic;
using System.Linq;

namespace LibrarySystem;

public class MovieCollection
    {
        private class Node
        {
            public string Key;
            public Movie Value;
            public Node Next;

            public Node(string key, Movie value)
            {
                Key = key;
                Value = value;
                Next = null;
            }
        }

        private const int SIZE = 1000;
        private Node[] table = new Node[SIZE];
        private int count = 0;

        private int Hash(string key)
        {
            int hash = 0;
            foreach (char c in key)
                hash = (31 * hash + c) % SIZE;
            return hash;
        }

        public void AddMovie(Movie movie)
        {
            int index = Hash(movie.Title);
            Node current = table[index];

            while (current != null)
            {
                if (current.Key == movie.Title)
                {
                    current.Value.AddCopies(movie.TotalCopies);
                    return;
                }
                current = current.Next;
            }

            Node newNode = new Node(movie.Title, movie);
            newNode.Next = table[index];
            table[index] = newNode;
            count++;
        }

        public Movie GetMovie(string title)
        {
            int index = Hash(title);
            Node current = table[index];
            while (current != null)
            {
                if (current.Key == title)
                    return current.Value;
                current = current.Next;
            }
            return null;
        }

        public void RemoveMovieCopies(string title, int number)
        {
            var movie = GetMovie(title);
            if (movie == null) return;

            if (movie.RemoveCopies(number) && movie.TotalCopies == 0)
                DeleteMovie(title);
        }

        private void DeleteMovie(string title)
        {
            int index = Hash(title);
            Node prev = null;
            Node current = table[index];

            while (current != null)
            {
                if (current.Key == title)
                {
                    if (prev == null) table[index] = current.Next;
                    else prev.Next = current.Next;
                    count--;
                    return;
                }
                prev = current;
                current = current.Next;
            }
        }

        public List<Movie> GetAllMovies()
        {
            var movies = new List<Movie>();
            foreach (var node in table)
            {
                Node current = node;
                while (current != null)
                {
                    movies.Add(current.Value);
                    current = current.Next;
                }
            }
            return movies.OrderBy(m => m.Title).ToList();
        }

        public List<Movie> GetTopThreeMovies()
        {
            return GetAllMovies()
                .OrderByDescending(m => m.BorrowCount)
                .Take(3).ToList();
        }
    }