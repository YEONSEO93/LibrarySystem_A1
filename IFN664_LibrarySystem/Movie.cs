using System;
namespace LibrarySystem;
    public class Movie
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public string Classification { get; set; }
        public int Duration { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public int BorrowCount { get; set; }

        public Movie(string title, string genre, string classification, int duration, int copies)
        {
            Title = title;
            Genre = genre;
            Classification = classification;
            Duration = duration;
            TotalCopies = copies;
            AvailableCopies = copies;
            BorrowCount = 0;
        }

        public void AddCopies(int number)
        {
            if (number > 0)
            {
                TotalCopies += number;
                AvailableCopies += number;
            }
        }

        public bool RemoveCopies(int number)
        {
            if (number > 0 && number <= AvailableCopies)
            {
                TotalCopies -= number;
                AvailableCopies -= number;
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Title} | Genre: {Genre} | Rating: {Classification} | Duration: {Duration} min | Available: {AvailableCopies}/{TotalCopies} | Borrowed: {BorrowCount} times";
        }
    }