using System;

namespace LibrarySystem
{
    public class MovieCollection
    {
        private int SIZE;
        private Movie[] table;
        private bool[] deleted;
        private int count = 0;

        // constructor 
        public MovieCollection(int expectedCount)
        {
            double loadFactor = 0.7;
            int suggestedSize = (int)(expectedCount / loadFactor);
            SIZE = FindNextPrime(suggestedSize);

            table = new Movie[SIZE];
            deleted = new bool[SIZE];
        }

        private int FindNextPrime(int start)
        {
            while (!IsPrime(start))
                start++;
            return start;
        }

        private bool IsPrime(int n)
        {
            if (n < 2) return false;
            if (n == 2 || n == 3) return true;
            if (n % 2 == 0) return false;
            for (int i = 3; i * i <= n; i += 2)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

        private int Hash(string key)
        {
            const int p = 31;
            int m = SIZE;
            long hash = 0;
            long pow = 1;
            foreach (char c in key)
            {
                hash = (hash + (c - 'a' + 1) * pow) % m;
                pow = (pow * p) % m;
            }
            return (int)hash;
        }

        private int FindIndex(string key)
        {
            int index = Hash(key);
            int i = 0;
            while (i < SIZE)
            {
                int probeIndex = (index + i * i) % SIZE;
                if (table[probeIndex] == null)
                    return -1;
                if (!deleted[probeIndex] && table[probeIndex].Title == key)
                    return probeIndex;
                i++;
            }
            return -1;
        }

        public bool AddMovie(Movie movie)
        {
            int index = Hash(movie.Title);
            int i = 0;

            while (i < SIZE)
            {
                int probeIndex = (index + i * i) % SIZE; // quadratic probing
                if (table[probeIndex] == null || deleted[probeIndex])
                {
                    table[probeIndex] = movie;
                    deleted[probeIndex] = false;
                    count++;
                    return true;
                }
                if (table[probeIndex].Title == movie.Title)
                {
                    table[probeIndex].AddCopies(movie.TotalCopies);
                    return true;
                }
                i++;
            }
            return false; 
        }

        public Movie GetMovie(string title)
        {
            int index = FindIndex(title);
            if (index == -1) return null;
            return table[index];
        }

        public bool RemoveMovieCopies(string title, int number)
        {
            int index = FindIndex(title);
            if (index == -1)
            {
                return false;
            }

            var movie = table[index];
            bool removed = movie.RemoveCopies(number);

            if (removed)
            {
                if (movie.TotalCopies == 0)
                {
                    table[index] = null;
                    deleted[index] = true;
                    count--;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Movie[] GetAllMovies()
        {
            Movie[] result = new Movie[count];
            int idx = 0;
            for (int i = 0; i < SIZE; i++)
            {
                if (table[i] != null && !deleted[i])
                {
                    result[idx++] = table[i];
                }
            }

            for (int i = 0; i < idx - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < idx; j++)
                {
                    if (string.Compare(result[j].Title, result[min].Title) < 0)
                    {
                        min = j;
                    }
                }
                var temp = result[i];
                result[i] = result[min];
                result[min] = temp;
            }

            return result;
        }

        // ===== HEAP SECTION =====

        private void Heapify(Movie[] array, int n, int i)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n && array[left].BorrowCount > array[largest].BorrowCount)
                largest = left;
            if (right < n && array[right].BorrowCount > array[largest].BorrowCount)
                largest = right;

            if (largest != i)
            {
                var temp = array[i];
                array[i] = array[largest];
                array[largest] = temp;
                Heapify(array, n, largest);
            }
        }

        private void BuildMaxHeap(Movie[] array, int n)
        {
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(array, n, i);
            }
        }

        public Movie[] GetTopThreeMovies()
        {
            Movie[] movies = new Movie[count];
            int idx = 0;
            for (int i = 0; i < SIZE; i++)
            {
                if (table[i] != null && !deleted[i])
                {
                    movies[idx++] = table[i];
                }
            }

            BuildMaxHeap(movies, idx);

            int top = idx >= 3 ? 3 : idx;
            Movie[] mostBorrowedMovies = new Movie[top];
            for (int i = 0; i < top; i++)
            {
                mostBorrowedMovies[i] = movies[0];
                movies[0] = movies[idx - 1];
                idx--;
                Heapify(movies, idx, 0);
            }

            return mostBorrowedMovies;
        }
    }
}
