using System;
using LibrarySystem;

class Program
{
    static MovieCollection movieCollection = new MovieCollection();
    static MemberCollection memberCollection = new MemberCollection();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("\n--- Main Menu ---");
            Console.WriteLine("1. Staff Login");
            Console.WriteLine("2. Member Login");
            Console.WriteLine("3. Exit");
            Console.Write("Select option: ");
            string choice = Console.ReadLine();

            if (choice == "1") StaffLogin();
            else if (choice == "2") MemberLogin();
            else if (choice == "3") break;
            else Console.WriteLine("Invalid input. Try again.");
        }
    }

    static void StaffLogin()
    {
        Console.Write("\nEnter username: ");
        var user = Console.ReadLine();
        Console.Write("Enter password: ");
        var pass = Console.ReadLine();

        if (user == "staff" && pass == "today123")
            StaffMenu();
        else
            Console.WriteLine("Incorrect credentials.");
    }

    static void StaffMenu()
    {
        while (true)
        {
            Console.WriteLine("\n--- Staff Menu ---");
            Console.WriteLine("1. Add Movie");
            Console.WriteLine("2. Remove Movie Copies");
            Console.WriteLine("3. Register New Member");
            Console.WriteLine("4. Remove Member");
            Console.WriteLine("5. Find Member Phone");
            Console.WriteLine("6. Find Members Renting a Movie");
            Console.WriteLine("7. Back to Main Menu");
            Console.Write("Select option: ");
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
            Console.WriteLine("Login failed.");
    }

    static void MemberMenu(Member member)
    {
        while (true)
        {
            Console.WriteLine("\n--- Member Menu ---");
            Console.WriteLine("1. Display All Movies");
            Console.WriteLine("2. Display Movie Information");
            Console.WriteLine("3. Borrow Movie");
            Console.WriteLine("4. Return Movie");
            Console.WriteLine("5. List Borrowed Movies");
            Console.WriteLine("6. Display Top 3 Borrowed Movies");
            Console.WriteLine("7. Back to Main Menu");
            Console.Write("Select option: ");
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
        Console.Write("Genre: ");
        string genre = Console.ReadLine();
        Console.Write("Classification (G/PG/M15+/MA15+): ");
        string classification = Console.ReadLine();
        Console.Write("Duration (minutes): ");
        int duration = int.Parse(Console.ReadLine());
        Console.Write("Number of copies: ");
        int copies = int.Parse(Console.ReadLine());

        var movie = new Movie(title, genre, classification, duration, copies);
        movieCollection.AddMovie(movie);
    }

    static void RemoveMovieCopies()
    {
        Console.Write("Enter movie title to remove copies: ");
        string title = Console.ReadLine();
        Console.Write("Enter number of copies to remove: ");
        int copies = int.Parse(Console.ReadLine());

        movieCollection.RemoveMovieCopies(title, copies);
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

        var member = new Member(firstName, lastName, password, phone);
        memberCollection.AddMember(member);
    }

    static void RemoveMember()
    {
        Console.Write("First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();

        memberCollection.RemoveMember(firstName, lastName);
    }

    static void FindMemberPhone()
    {
        Console.Write("First Name: ");
        string firstName = Console.ReadLine();
        Console.Write("Last Name: ");
        string lastName = Console.ReadLine();

        var member = memberCollection.FindMember(firstName, lastName);
        if (member != null)
            Console.WriteLine($"Phone: {member.Phone}");
        else
            Console.WriteLine("Member not found.");
    }

    static void FindMembersRentingMovie()
    {
        Console.Write("Enter movie title: ");
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
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();
        var movie = movieCollection.GetMovie(title);

        if (movie != null)
            Console.WriteLine(movie);
        else
            Console.WriteLine("Movie not found.");
    }

    static void BorrowMovie(Member member)
    {
        Console.Write("Enter movie title to borrow: ");
        string title = Console.ReadLine();
        var movie = movieCollection.GetMovie(title);

        if (movie != null)
            member.BorrowMovie(movie);
        else
            Console.WriteLine("Movie not found.");
    }

    static void ReturnMovie(Member member)
    {
        Console.Write("Enter movie title to return: ");
        string title = Console.ReadLine();
        var movie = movieCollection.GetMovie(title);

        if (movie != null)
            member.ReturnMovie(movie);
        else
            Console.WriteLine("Movie not found.");
    }

    static void DisplayTopThreeMovies()
    {
        var topMovies = movieCollection.GetTopThreeMovies();
        Console.WriteLine("Top 3 Most Borrowed Movies:");
        foreach (var movie in topMovies)
            Console.WriteLine($"{movie.Title} - Borrowed {movie.BorrowCount} times");
    }
}
