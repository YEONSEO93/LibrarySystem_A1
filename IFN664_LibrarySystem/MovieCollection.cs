using System;

namespace LibrarySystem
{
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
            int sum = 0;
            foreach (char c in key)
                sum += c;
            return sum % SIZE;
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

        public Movie[] GetAllMovies()
        {
            Movie[] result = new Movie[count];
            int idx = 0;
            foreach (Node node in table)
            {
                Node current = node;
                while (current != null)
                {
                    result[idx++] = current.Value;
                    current = current.Next;
                }
            }

            for (int i = 0; i < idx - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < idx; j++)
                {
                    if (string.Compare(result[j].Title, result[minIndex].Title) < 0)
                        minIndex = j;
                }
                var temp = result[i];
                result[i] = result[minIndex];
                result[minIndex] = temp;
            }
            return result;
        }

        public Movie[] GetTopThreeMovies()
        {
            Movie[] all = GetAllMovies();
            int len = all.Length;

            for (int i = 0; i < len - 1; i++)
            {
                int maxIdx = i;
                for (int j = i + 1; j < len; j++)
                {
                    if (all[j].BorrowCount > all[maxIdx].BorrowCount)
                        maxIdx = j;
                }
                var temp = all[i];
                all[i] = all[maxIdx];
                all[maxIdx] = temp;
            }

            int top = len >= 3 ? 3 : len;
            Movie[] result = new Movie[top];
            for (int i = 0; i < top; i++)
                result[i] = all[i];
            return result;
        }
    }
}
