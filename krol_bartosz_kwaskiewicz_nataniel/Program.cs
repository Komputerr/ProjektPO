using krol_bartosz_kwaskiewicz_nataniel.classes;
using System.Security.Cryptography;
using System.Text;

namespace krol_bartosz_kwaskiewicz_nataniel
{
    public class Program
    {
        static void Main(string[] args)
        {
            ConsoleHelper.InitializeConsole();
            Logger.Log("Application started");

            var socialMedia = new SocialMediaPostManager();
            var systemManager = new SystemManager();
            systemManager.CreateUsersFile();

            ConsoleHelper.PrintHeader("WELCOME TO SOCIAL MEDIA MANAGER");
            ConsoleHelper.CenterText("Choose whether you want to log in or sign up");
            Console.WriteLine();

            ConsoleHelper.PrintMenuOptions(
                "Log in (Using an existing account)",
                "Sign up (Create a new user)",
                "Log in as a guest",
                "Exit program"
            );

            int choiceEnter;
            while (!int.TryParse(Console.ReadLine(), out choiceEnter) || choiceEnter < 1 || choiceEnter > 4)
            {
                ConsoleHelper.Red("Invalid choice, please enter 1, 2, 3 or 4");
            }

            systemManager.LoadUsers();
            string? loginUsername = "guest";
            string? loginPassword = SystemManager.HashPassword("guest");
            string? inputPassword;
            User? user = null;

            switch (choiceEnter)
            {
                case 1:
                    ConsoleHelper.PrintHeader("LOGIN");
                    Console.WriteLine();
                    ConsoleHelper.CenterText("Enter username:");

                    loginUsername = Console.ReadLine();
                    while (string.IsNullOrEmpty(loginUsername) && systemManager.users.ContainsKey(loginUsername))
                    {
                        ConsoleHelper.Red("Username can not be empty! Please try again: ");
                        loginUsername = Console.ReadLine();
                    }
                    Logger.Log($"Login attempt for user: {loginUsername}");
                    ConsoleHelper.CenterText("Enter password:");
                    inputPassword = ConsoleHelper.ReadMaskedPassword();
                    while (string.IsNullOrEmpty(loginPassword))
                    {
                        ConsoleHelper.Red("Password can not be empty! Please try again: ");
                        inputPassword = ConsoleHelper.ReadMaskedPassword();
                    }
                    loginPassword = SystemManager.HashPassword(inputPassword);
                    break;
                case 2:
                    ConsoleHelper.PrintHeader("REGISTER");
                    Console.WriteLine();
                    ConsoleHelper.CenterText("Enter username:");

                    loginUsername = Console.ReadLine();
                    while (string.IsNullOrEmpty(loginUsername))
                    {
                        ConsoleHelper.Red("Username can not be empty! Please try again: ");
                        loginUsername = Console.ReadLine();
                    }
                    Logger.Log($"Registration attempt for user: {loginUsername}");
                    systemManager.LoadUsers();
                    while(systemManager.users.ContainsKey(loginUsername) || string.IsNullOrEmpty(loginUsername))
                    {
                        ConsoleHelper.Red($"Username '{loginUsername}' already exists. Please try again with a different username.");
                        ConsoleHelper.CenterText("Enter username:");
                        loginUsername = Console.ReadLine();
                    }

                    ConsoleHelper.CenterText("Enter password:");
                    inputPassword = ConsoleHelper.ReadMaskedPassword();
                    while (string.IsNullOrEmpty(inputPassword))
                    {
                        ConsoleHelper.Red("Password can not be empty! Please try again: ");
                        inputPassword = ConsoleHelper.ReadMaskedPassword();
                    }
                    loginPassword = SystemManager.HashPassword(inputPassword);
                    systemManager.RegisterUser(loginUsername, loginPassword, "user");
                    ConsoleHelper.PrintHeader("REGISTRATION COMPLETE");
                    ConsoleHelper.CenterText($"User '{loginUsername}' successfully registered!");
                    Logger.LogRegistration(loginUsername);
                    Thread.Sleep(1500);
                    break;
                case 3:
                    ConsoleHelper.PrintHeader("GUEST ACCESS");
                    Logger.Log("Guest access started");
                    ConsoleHelper.CenterText("You are logged in as a guest with limited functionality.");
                    ConsoleHelper.CenterText("You can only view posts.");
                    Thread.Sleep(2000);
                    user = new User("guest", SystemManager.HashPassword("guest"), "guest");

                    while (true)
                    {
                        ConsoleHelper.PrintHeader("GUEST MENU");
                        ConsoleHelper.PrintMenuOptions(
                            "View All Posts",
                            "Exit Program"
                        );

                        int guestChoice;
                        while (!int.TryParse(Console.ReadLine(), out guestChoice) || guestChoice < 1 || guestChoice > 2)
                        {
                            ConsoleHelper.Red("Invalid choice, please enter 1 or 2");
                        }


                        if (guestChoice == 1)
                        {
                            ConsoleHelper.PrintHeader("VIEW ALL POSTS");
                            ConsoleHelper.PrintMenuOptions(
                               "Facebook",
                               "Instagram",
                               "Reddit",
                               "Go back to main menu"
                            );
                            socialMedia.LoadPostsFromFile();
                        }
                        else
                        {
                            ConsoleHelper.PrintHeader("GOODBYE");
                            ConsoleHelper.CenterText("Thank you for visiting as a guest!");
                            Console.ForegroundColor = ConsoleColor.Green;
                            ConsoleHelper.CenterText("The application will now close...");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                            Thread.Sleep(2000);
                            return;
                        }
                    }
                case 4:
                    ConsoleHelper.PrintHeader("GOODBYE");
                    Logger.Log("Application closed by user");
                    ConsoleHelper.CenterText("Thank you for using our social media manager!");
                    Console.ForegroundColor = ConsoleColor.Green;
                    ConsoleHelper.CenterText("The application will now close...");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                    break;

            }

            systemManager.LoadUsers();
            user = systemManager.Login(loginUsername, loginPassword);

            int postMethodCounterFacebook = 0;
            int postMethodCounterInstagram = 0;
            int postMethodCounterReddit = 0;

            if (user == null)
            {
                ConsoleHelper.PrintHeader("LOGIN FAILED");
                Console.ForegroundColor = ConsoleColor.Red;
                ConsoleHelper.CenterText("Invalid login attempt. The application will now close.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Thread.Sleep(2000);
                return;
            }
            if (user != null || user.Role == "admin" || user.Role == "user")
            {
                bool isRunning = true;

                while (isRunning)
                {
                    ConsoleHelper.PrintHeader($"SOCIAL MEDIA MANAGER - {user.Username.ToUpper()}");
                    ConsoleHelper.PrintMenuHeader("MAIN MENU");
                    ConsoleHelper.PrintMenuOptions(
                        "Toggle Post Method",
                        "Create New Post",
                        "View My Posts",
                        "View All Posts",
                        "Edit Post",
                        "Delete Post",
                        "View Application Logs",
                        "Exit Program"
                    );

                    int choice;
                    while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 8)
                    {
                        ConsoleHelper.Red("Invalid choice, please enter a number between 1 and 8");
                    }

                    switch (choice)
                    {
                        case 1:
                            ConsoleHelper.PrintHeader("TOGGLE POST METHOD");
                            ConsoleHelper.PrintMenuOptions(
                                "Add a post method",
                                "Remove a post method",
                                "Go back to main menu"
                            );

                            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 3)
                            {
                                ConsoleHelper.Red("Invalid choice, please enter 1, 2 or 3");
                            }
                            switch (choice)
                            {
                                case 1:
                                    ConsoleHelper.PrintHeader("ADD A POST METHOD");
                                    ConsoleHelper.PrintMenuOptions(
                                        "Facebook",
                                        "Instagram",
                                        "Reddit",
                                        "Go back to main menu"
                                    );

                                    while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                                    {
                                        ConsoleHelper.Red("Invalid choice, please enter 1, 2, 3 or 4");
                                    }

                                    switch (choice)
                                    {
                                        case 1:
                                            if (postMethodCounterFacebook == 0)
                                            {
                                                socialMedia.OnPostAdd += socialMedia.AddFacebookPost;
                                                postMethodCounterFacebook++;
                                                Console.WriteLine("\nPosting method set to Facebook!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nYou can toggle a posting method for Facebook only once");
                                            }
                                            Thread.Sleep(2000);
                                            break;
                                        case 2:
                                            if (postMethodCounterInstagram == 0)
                                            {
                                                socialMedia.OnPostAdd += socialMedia.AddInstagramPost;
                                                postMethodCounterInstagram++;
                                                Console.WriteLine("\nPosting method set to Instagram!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nYou can toggle a posting method for Instagram only once");
                                            }
                                            Thread.Sleep(2000);
                                            break;
                                        case 3:
                                            if (postMethodCounterReddit == 0)
                                            {
                                                socialMedia.OnPostAdd += socialMedia.AddRedditPost;
                                                postMethodCounterReddit++;
                                                Console.WriteLine("\nPosting method set to Reddit!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nYou can toggle a posting method for Reddit only once");
                                            }
                                            Thread.Sleep(2000);
                                            break;
                                        case 4:
                                            break;
                                    }
                                    break;
                                case 2:
                                    ConsoleHelper.PrintHeader("REMOVE A POST METHOD");
                                    ConsoleHelper.PrintMenuOptions(
                                        "Facebook",
                                        "Instagram",
                                        "Reddit",
                                        "Go back to main menu"
                                    );

                                    while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                                    {
                                        ConsoleHelper.Red("Invalid choice, please enter 1, 2, 3 or 4");
                                    }

                                    switch (choice)
                                    {
                                        case 1:
                                            if (postMethodCounterFacebook >= 1)
                                            {
                                                socialMedia.OnPostAdd -= socialMedia.AddFacebookPost;
                                                postMethodCounterFacebook--;
                                                Console.WriteLine("\nPosting method for Facebook removed!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nThere's currently no active posting method for Facebook");
                                            }
                                            Thread.Sleep(2000);
                                            break;
                                        case 2:
                                            if (postMethodCounterInstagram >= 1)
                                            {
                                                socialMedia.OnPostAdd -= socialMedia.AddInstagramPost;
                                                postMethodCounterInstagram--;
                                                Console.WriteLine("\nPosting method for Instagram removed!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nThere's currently no active posting method for Instagram");
                                            }
                                            Thread.Sleep(2000);
                                            break;
                                        case 3:
                                            if (postMethodCounterReddit >= 1)
                                            {
                                                socialMedia.OnPostAdd -= socialMedia.AddRedditPost;
                                                postMethodCounterReddit--;
                                                Console.WriteLine("\nPosting method for Reddit removed!");
                                            }
                                            else
                                            {
                                                Console.WriteLine("\nThere's currently no active posting method for Reddit");
                                            }
                                            Thread.Sleep(2000);
                                            break;
                                        case 4:
                                            break;
                                    }
                                    break;
                                case 3:
                                    break;
                            }
                            break;

                        case 2:
                            if (socialMedia.IsNotChosen())
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                ConsoleHelper.CenterText("No posting method configured!");
                                ConsoleHelper.CenterText("Please configure a posting method first (Option 1 in main menu).");
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                Thread.Sleep(2000);
                                break;
                            }

                            ConsoleHelper.PrintHeader("CREATE NEW POST");
                            ConsoleHelper.CenterText("Enter your post content:");
                            string content = Console.ReadLine();
                            socialMedia.InvokePosts(content, user);
                            Console.WriteLine("\nPost created successfully!");
                            Thread.Sleep(1000);
                            break;

                        case 3:
                            ConsoleHelper.PrintHeader("VIEW YOUR POSTS");
                            ConsoleHelper.PrintMenuOptions(
                               "Facebook",
                               "Instagram",
                               "Reddit",
                               "Go back to main menu"
                            );
                            socialMedia.DisplayUserPosts(user);
                            break;

                        case 4:
                            ConsoleHelper.PrintHeader("VIEW ALL POSTS");
                            ConsoleHelper.PrintMenuOptions(
                               "Facebook",
                               "Instagram",
                               "Reddit",
                               "Go back to main menu"
                            );
                            socialMedia.LoadPostsFromFile();
                            break;

                        case 5:
                            ConsoleHelper.PrintHeader("EDIT POST");
                            ConsoleHelper.PrintMenuOptions(
                               "Facebook",
                               "Instagram",
                               "Reddit",
                               "Go back to main menu"
                            );
                            socialMedia.EditPost(user);
                            break;

                        case 6:
                            ConsoleHelper.PrintHeader("DELETE POST");
                            ConsoleHelper.PrintMenuOptions(
                            "Delete all of your posts",
                            "Delete one of your posts",
                            "Delete all posts (admin)",
                            "Delete one of all posts (admin)",
                            "Go back to main menu");


                            int choiceDelete;
                            while (!int.TryParse(Console.ReadLine(), out choiceDelete) || choiceDelete < 1 || choiceDelete > 5)
                            {
                                ConsoleHelper.Red("Invalid choice, please enter a number between 1 and 5");
                            }

                            switch (choiceDelete)
                            {
                                case 1:
                                    socialMedia.DeletePostAllOwn(user);
                                    break;
                                case 2:
                                    socialMedia.DeletePostOneOwn(user);
                                    break;
                                case 3:
                                    socialMedia.DeletePostAllAdmin(user);
                                    break;
                                case 4:
                                    socialMedia.DeletePostOneAdmin(user);
                                    break;
                                case 5:
                                    break;
                            }
                            break;
                        case 7:
                            ConsoleHelper.PrintHeader("APPLICATION LOGS");
                            if (user.Role == "admin")
                            {
                                Logger.DisplayLogsInConsole();
                            }
                            else
                            {
                                ConsoleHelper.Red("Access denied. Only administrators can view application logs.");
                                Thread.Sleep(2000);
                            }
                            break;
                        case 8:
                            ConsoleHelper.PrintHeader("GOODBYE");
                            ConsoleHelper.CenterText("Thank you for using our social media manager!");
                            Console.ForegroundColor = ConsoleColor.Green;
                            ConsoleHelper.CenterText("The application will now close...");
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Thread.Sleep(2000);
                            isRunning = false;
                            break;
                    }
                }
            }
            else
            {
                ConsoleHelper.PrintHeader("AN ERROR OCCURRED");
                Console.ForegroundColor = ConsoleColor.Red;
                ConsoleHelper.CenterText("An error occurred. The application will now close.");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Thread.Sleep(2000);
            }
        }
    }
}
