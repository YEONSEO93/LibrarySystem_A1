
using System;
using LibrarySystem;

class Program
{
    static MovieCollection movieCollection = new MovieCollection(1000); // max size: 1000
    static MemberCollection memberCollection = new MemberCollection();

    static void Main(string[] args)
    {
        LoadSampleData();

        while (true)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("\n1. Staff Login");
            Console.WriteLine("2. Member Login");
            Console.WriteLine("3. Exit");
            Console.Write("\nSelect option: ");
            string choice = Console.ReadLine();

            if (choice == "1") StaffLogin();
            else if (choice == "2") MemberLogin();
            else if (choice == "3") break;
            else Console.WriteLine("Invalid input. Try again.");
        }
    }

    static void LoadSampleData()
    {
        movieCollection.AddMovie(new Movie("Inception", "sci-fi", "M15+", 148, 5));
        movieCollection.AddMovie(new Movie("Harry Potter", "fantasy", "PG", 152, 4));
        movieCollection.AddMovie(new Movie("Frozen", "animated", "G", 102, 3));
        movieCollection.AddMovie(new Movie("The Godfather", "drama", "MA15+", 175, 2));
        movieCollection.AddMovie(new Movie("Avengers", "action", "M15+", 181, 3));

        memberCollection.AddMember(new Member("Yeonseo", "Ko", "1234", "0412345678"));
        memberCollection.AddMember(new Member("Gyuri", "Park", "5678", "0412349876"));
        memberCollection.AddMember(new Member("Emily", "Smith", "0000", "0499999999"));
        memberCollection.AddMember(new Member("John", "Doe", "1111", "0488888888"));
        memberCollection.AddMember(new Member("Emma", "Johnson", "2222", "0477777777"));
    }

    static void StaffLogin()
    {
        Console.Write("\nStaff login selected");

        Console.Write("\nEnter username: ");
        var user = Console.ReadLine();
        Console.Write("Enter password: ");
        var pass = Console.ReadLine();

        if (user == "staff" && pass == "today123")
            StaffMenu();
        else
            Console.WriteLine("\nIncorrect credentials.");
    }

    static void StaffMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Staff Menu ---");
            Console.WriteLine("\n1. Add Movie");
            Console.WriteLine("2. Remove Movie Copies");
            Console.WriteLine("3. Register New Member");
            Console.WriteLine("4. Remove Member");
            Console.WriteLine("5. Find Member Phone");
            Console.WriteLine("6. Find Members Renting a Movie");
            Console.WriteLine("7. Back to Main Menu");
            Console.Write("\nSelect option: ");
            string choice = Console.ReadLine();

            if (choice == "1") AddMovie();
            else if (choice == "2") RemoveMovieCopies();
            else if (choice == "3") RegisterMember();
            else if (choice == "4") RemoveMember();
            else if (choice == "5") FindMemberPhone();
            else if (choice == "6") FindMembersRentingMovie();
            else if (choice == "7") break;
            else Console.WriteLine("Invalid input. Try again.");
        }
    }

    static void MemberLogin()
    {
        Console.Write("\nStaff login selected");

        Console.Write("\nEnter first name: ");
        string f = Console.ReadLine();
        Console.Write("Enter last name: ");
        string l = Console.ReadLine();
        Console.Write("Enter password: ");
        string p = Console.ReadLine();

        var member = memberCollection.FindMember(f, l);
        if (member != null && member.Password == p)
            MemberMenu(member);
        else
            Console.WriteLine("\nLogin failed.");
    }

    static void MemberMenu(Member member)
    {
        while (true)
        {
            Console.WriteLine("\n--- Member Menu ---");
            Console.WriteLine("\n1. Display All Movies");
            Console.WriteLine("2. Display Movie Information");
            Console.WriteLine("3. Borrow Movie");
            Console.WriteLine("4. Return Movie");
            Console.WriteLine("5. List Borrowed Movies");
            Console.WriteLine("6. Display Top 3 Borrowed Movies");
            Console.WriteLine("7. Back to Main Menu");
            Console.Write("\nSelect option: ");
            string choice = Console.ReadLine();

            if (choice == "1") DisplayAllMovies();
            else if (choice == "2") DisplayMovieInfo();
            else if (choice == "3") BorrowMovie(member);
            else if (choice == "4") ReturnMovie(member);
            else if (choice == "5") member.ListBorrowedMovies();
            else if (choice == "6") DisplayTopThreeMovies();
            else if (choice == "7") break;
            else Console.WriteLine("Invalid input. Try again.");
        }
    }

    static void AddMovie()
    {
        Console.Write("Movie Title: ");
        string title = Console.ReadLine();

        var existing = movieCollection.GetMovie(title);

        if (existing != null)
        {
            // existing movie → get input about num of copy
            Console.Write($"\"{title}\" already exists. Enter number of additional copies: ");
            int additionalCopies = int.Parse(Console.ReadLine());

            var movie = new Movie(title, existing.Genre, existing.Classification, existing.Duration, additionalCopies);
            bool added = movieCollection.AddMovie(movie);

            if (added)
            {
                var updated = movieCollection.GetMovie(title);
                Console.WriteLine("Additional copies added.");
                Console.WriteLine($"{updated.Title} now has {updated.TotalCopies} total copies ({updated.AvailableCopies} available).");
            }
            else
            {
                Console.WriteLine("Failed to add additional copies.");
            }
        }
        else
        {
            // add a new movie → get all info of the movie
            Console.Write("Genre: ");
            string genre = Console.ReadLine();
            Console.Write("Classification (G/PG/M15+/MA15+): ");
            string classification = Console.ReadLine();
            Console.Write("Duration (minutes): ");
            int duration = int.Parse(Console.ReadLine());
            Console.Write("Number of copies: ");
            int copies = int.Parse(Console.ReadLine());

            var movie = new Movie(title, genre, classification, duration, copies);
            bool added = movieCollection.AddMovie(movie);

            if (added)
            {
                Console.WriteLine("Movie added successfully.");
                Console.WriteLine(movie);
            }
            else
            {
                Console.WriteLine("Failed to add movie. Hash table may be full.");
            }
        }
    }

    static void RemoveMovieCopies()
    {
        Console.Write("Enter movie title to remove copies: ");
        string title = Console.ReadLine();
        Console.Write("Enter number of copies to remove: ");
        int copies = int.Parse(Console.ReadLine());

        var movie = movieCollection.GetMovie(title);
        if (movie == null)
        {
            Console.WriteLine("\nMovie not found.");
            return;
        }

        bool removed = movieCollection.RemoveMovieCopies(title, copies);
        if (removed)
        {
            Console.WriteLine($"{copies} copies of \"{title}\" have been removed.");

            if (movie.TotalCopies == 0)
            {
                Console.WriteLine($"All copies removed. \"{title}\" has been deleted from the system.");
            }
            else
            {
                Console.WriteLine($"\"{title}\" now has {movie.TotalCopies} total copies ({movie.AvailableCopies} available).");
            }
        }
        else
        {
            Console.WriteLine("Cannot remove more copies than are available.");
        }
    }

    static void RegisterMember()
    {
        Console.Write("First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();
        Console.Write("Phone: ");
        string phone = Console.ReadLine();
        Console.Write("Set 4-digit password: ");
        string password = Console.ReadLine();

        if (password.Length != 4 || !int.TryParse(password, out _))
        {
            Console.WriteLine("Password must be a four-digit number.");
            return;
        }

        var member = new Member(firstName, lastName, password, phone);
        bool added = memberCollection.AddMember(member);
        if (added)
            Console.WriteLine("\nMember registered successfully.");
        else
            Console.WriteLine("\nRegistration failed. Member already exists.");
    }

    static void RemoveMember()
    {
        Console.Write("First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();

        var member = memberCollection.FindMember(firstName, lastName);
        if (member == null)
        {
            Console.WriteLine("Member not found.");
            return;
        }

        if (member.BorrowedCount > 0)
        {
            Console.WriteLine("This member is currently holding movies. All movies must be returned before removal.");
            return;
        }

        bool removed = memberCollection.RemoveMember(firstName, lastName);
        if (removed)
            Console.WriteLine("Member has been successfully removed.");
        else
            Console.WriteLine("Failed to remove member.");
    }

    static void FindMemberPhone()
    {
        Console.Write("First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();

        var member = memberCollection.FindMember(firstName, lastName);
        if (member != null)
            Console.WriteLine($"\nPhone: {member.Phone}");
        else
            Console.WriteLine("Member not found.");
    }

    static void FindMembersRentingMovie()
    {
        Console.Write("\nEnter movie title: ");
        string title = Console.ReadLine();
        Member[] members = memberCollection.FindMembersRentingMovie(title);

        if (members.Length > 0)
        {
            Console.WriteLine("Members renting this movie:");
            for (int i = 0; i < members.Length; i++)
            {
                Console.WriteLine($"- {members[i].FullName}");
            }
        }
        else
        {
            Console.WriteLine("No members are currently renting this movie.");
        }
    }

    static void DisplayAllMovies()
    {
        var movies = movieCollection.GetAllMovies();
        foreach (var movie in movies)
            Console.WriteLine(movie);
    }

    static void DisplayMovieInfo()
    {
        Console.Write("\nEnter movie title: ");
        string title = Console.ReadLine();
        var movie = movieCollection.GetMovie(title);

        if (movie != null)
            Console.WriteLine(movie);
        else
            Console.WriteLine("\nMovie not found.");
    }

    static void BorrowMovie(Member member)
    {
        Console.Write("\nEnter movie title to borrow: ");
        string title = Console.ReadLine();
        var movie = movieCollection.GetMovie(title);

        if (movie != null)
            member.BorrowMovie(movie);
        else
            Console.WriteLine("\nMovie not found.");
    }

    static void ReturnMovie(Member member)
    {
        Console.Write("\nEnter movie title to return: ");
        string title = Console.ReadLine();
        var movie = movieCollection.GetMovie(title);

        if (movie != null)
            member.ReturnMovie(movie);
        else
            Console.WriteLine("\nMovie not found.");
    }

    static void DisplayTopThreeMovies()
    {
        var topMovies = movieCollection.GetTopThreeMovies();
        Console.WriteLine("Top 3 Most Borrowed Movies:");
        foreach (var movie in topMovies)
            Console.WriteLine($"{movie.Title} - Borrowed {movie.BorrowCount} times");
    }
}
