using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace krol_bartosz_kwaskiewicz_nataniel.classes
{
    public delegate void PostAction(string post, User user);
    public class SocialMediaPostManager : IUserActions
    {
        private Dictionary<string, List<string>> posts;
        public event PostAction OnPostAdd;
        public DateTime dateTime = new DateTime();

        public SocialMediaPostManager()
        {
            posts = new Dictionary<string, List<string>>()
        {
            { "facebook", new List<string>() },
            { "instagram", new List<string>() },
            { "reddit", new List<string>() }
        };
        }

        public void Red(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public bool IsNotChosen()
        {
            if (OnPostAdd != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void AddFacebookPost(string post, User user)
        {
            if (user.Role != "admin" && user.Role != "user")
            {
                Red("Access denied. Only users and admins can post.");
                return;
            }

            if (!File.Exists("instagram.txt"))
            {
                File.Create("instagram.txt").Close();
            }

            Console.WriteLine("Facebook post added: " + post);
            File.AppendAllText("facebook.txt", user.Username + ";" + "[" + DateTime.Now + "]" + ";" + post + Environment.NewLine);
            Logger.LogPostOperation(user.Username, "Facebook", "added");
        }
        public void AddInstagramPost(string post, User user)
        {
            if (user.Role != "admin" && user.Role != "user")
            {
                Red("Access denied. Only users and admins can post.");
                return;
            }

            if (!File.Exists("instagram.txt"))
            {
                File.Create("instagram.txt").Close();
            }

            Console.WriteLine("Instagram post added: " + post);
            File.AppendAllText("instagram.txt", user.Username + ";" + "[" + DateTime.Now + "]" + ";" + post + Environment.NewLine);
            Logger.LogPostOperation(user.Username, "Instagram", "added");
        }
        public void AddRedditPost(string post, User user)
        {
            if (user.Role != "admin" && user.Role != "user")
            {
                Red("Access denied. Only users and admins can post.");
                return;
            }

            if (!File.Exists("reddit.txt"))
            {
                File.Create("reddit.txt").Close();
            }

            Console.WriteLine("Reddit post added: " + post);
            File.AppendAllText("reddit.txt", user.Username + ";" + "[" + DateTime.Now + "]" + ";" + post + Environment.NewLine);
            Logger.LogPostOperation(user.Username, "Reddit", "added");
        }

        public void InvokePosts(string post, User user)
        {
            OnPostAdd?.Invoke(post, user);
        }

        //admin i user; jeden wybrany swój
        public void DeletePostOneOwn(User user)
        {
            if (user.Role == "guest")
            {
                Logger.LogError($"Guest user '{user.Username}' attempted to delete posts");
                Red("\nYou need user or admin privilages in order to delete all of your own posts.");
                Thread.Sleep(2000);
            }
            else
            {
                Logger.LogAction(user.Username, "started deleting own post");
                ConsoleHelper.PrintHeader("Choose platform to delete a post from:");
                ConsoleHelper.PrintMenuOptions(
                               "Facebook",
                               "Instagram",
                               "Reddit",
                               "Go back to main menu"
                            );

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                {
                    Red("Invalid choice.");
                }

                string platform = "";
                string fileName = "";

                switch (choice)
                {
                    case 1:
                        platform = "Facebook";
                        fileName = "facebook.txt";
                        break;
                    case 2:
                        platform = "Instagram";
                        fileName = "instagram.txt";
                        break;
                    case 3:
                        platform = "Reddit";
                        fileName = "reddit.txt";
                        break;
                    case 4:
                        return;
                }

                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"No posts found for {platform}.");

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();

                    return;
                }

                Console.WriteLine($"\nYour posts on {platform}:");
                string[] lines = File.ReadAllLines(fileName);
                List<int> validPostIndices = new List<int>();
                int displayPostId = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    var parts = lines[i].Split(';');
                    if (parts.Length >= 3 && parts[0] == user.Username)
                    {
                        displayPostId++;
                        validPostIndices.Add(i);
                        Console.WriteLine($"{displayPostId}. {parts[1]}: {parts[2]}");
                    }
                }

                if (displayPostId == 0)
                {
                    Console.WriteLine("\nYou have no posts to delete on this platform.");

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();

                    return;
                }

                Console.WriteLine("\nEnter the ID of the post you want to delete:");
                int postId;
                while (!int.TryParse(Console.ReadLine(), out postId) || postId < 1 || postId > displayPostId)
                {
                    Red($"Please enter a valid ID (1-{displayPostId}):");
                }

                List<string> linesList = lines.ToList<string>();
                linesList.RemoveAt(postId - 1);

                File.Delete(fileName);
                File.WriteAllLines(fileName, linesList);

                Logger.LogPostOperation(user.Username, platform,$"deleted own post");

                Console.WriteLine($"\nYour ({user.Username}'s) post has been successfully deleted.");

            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();

        }

        //admin; jeden wybrany od każdego użytkownika 
        public void DeletePostOneAdmin(User user)
        {
            if (user.Role != "admin")
            {
                Logger.LogError($"User '{user.Username}' attempted admin delete without privileges");
                Red("\nYou need admin privilages in order to delete one of all posts.");
                Thread.Sleep(2000);
            }
            else
            {
                Logger.LogAction(user.Username, "started admin post deletion");

                ConsoleHelper.PrintHeader("Choose platform to delete a post from:");
                ConsoleHelper.PrintMenuOptions(
                               "Facebook",
                               "Instagram",
                               "Reddit",
                               "Go back to main menu"
                            );

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                {
                    Red("Invalid choice.");
                }

                string platform = "";
                string fileName = "";

                switch (choice)
                {
                    case 1:
                        platform = "Facebook";
                        fileName = "facebook.txt";
                        break;
                    case 2:
                        platform = "Instagram";
                        fileName = "instagram.txt";
                        break;
                    case 3:
                        platform = "Reddit";
                        fileName = "reddit.txt";
                        break;
                    case 4:
                        return;
                }

                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"No posts found for {platform}.");

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();

                    return;
                }

                Console.WriteLine($"\nPosts on {platform}:");
                string[] lines = File.ReadAllLines(fileName);
                List<int> validPostIndices = new List<int>();
                int displayPostId = 0;

                for (int i = 0; i < lines.Length; i++)
                {
                    var parts = lines[i].Split(';');
                    if (parts.Length >= 3)
                    {
                        displayPostId++;
                        validPostIndices.Add(i);
                        Console.WriteLine($"{displayPostId}. {parts[1]}: {parts[2]}");
                    }
                }

                if (displayPostId == 0)
                {
                    Console.WriteLine("\nYou have no posts to delete on this platform.");

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();

                    return;
                }

                // Get post ID to edit
                Console.WriteLine("\nEnter the ID of the post you want to delete:");
                int postId;
                while (!int.TryParse(Console.ReadLine(), out postId) || postId < 1 || postId > displayPostId)
                {
                    Red($"Please enter a valid ID (1-{displayPostId}):");
                }

                List<string> linesList = lines.ToList<string>();
                linesList.RemoveAt(postId - 1);

                File.Delete(fileName);
                File.WriteAllLines(fileName, linesList);

                Logger.LogPostOperation(user.Username, platform,$"deleted post as admin");

                Console.WriteLine($"\nPost number {postId} post has been successfully deleted.");

            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        //tylko admin; wszystkie wszystkie z danej platformy
        public void DeletePostAllAdmin(User user)
        {
            if (user.Role != "admin")
            {
                Logger.LogError($"User '{user.Username}' attempted mass delete without admin rights");
                Red("\nYou need admin privilages in order to delete all posts.");
                Thread.Sleep(2000);
            }
            else
            {
                Logger.LogAction(user.Username, "started mass admin deletion");

                ConsoleHelper.PrintHeader("Choose platform to delete posts from:");
                ConsoleHelper.PrintMenuOptions(
                               "Facebook",
                               "Instagram",
                               "Reddit",
                               "Go back to main menu"
                            );


                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                {
                    Red("Invalid choice.");
                }

                string platform = "";
                string fileName = "";

                switch (choice)
                {
                    case 1:
                        platform = "Facebook";
                        fileName = "facebook.txt";
                        break;
                    case 2:
                        platform = "Instagram";
                        fileName = "instagram.txt";
                        break;
                    case 3:
                        platform = "Reddit";
                        fileName = "reddit.txt";
                        break;
                    case 4:
                        return;
                }

                if (!File.Exists(fileName))
                {
                    Logger.LogError($"No posts file found for {platform} when deleting");
                    Console.WriteLine($"No posts found for {platform}.");

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();

                    return;
                }

                File.Delete(fileName);
                File.Create(fileName).Close();

                var fileInfo = new FileInfo(fileName);
                if (File.Exists(fileName) && fileInfo.Length == 0)
                {
                    Logger.LogPostOperation(user.Username, platform,$"deleted ALL posts)");

                    Console.WriteLine($"\nAll posts from {platform} have been successfully deleted.");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        //admin i user; wszystkie swoje
        public void DeletePostAllOwn(User user)
        {
            if (user.Role == "guest")
            {
                Logger.LogError($"Guest user '{user.Username}' attempted to delete own posts");
                Red("\nYou need user or admin privilages in order to delete all of your own posts.");
                Thread.Sleep(2000);
            }
            else
            {
                Logger.LogAction(user.Username, "started deleting all own posts");

                ConsoleHelper.PrintHeader("Choose platform to delete posts from:");
                ConsoleHelper.PrintMenuOptions(
                               "Facebook",
                               "Instagram",
                               "Reddit",
                               "Go back to main menu"
                            );

                int choice;
                while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
                {
                    Red("Invalid choice.");
                }

                string platform = "";
                string fileName = "";

                switch (choice)
                {
                    case 1:
                        platform = "Facebook";
                        fileName = "facebook.txt";
                        break;
                    case 2:
                        platform = "Instagram";
                        fileName = "instagram.txt";
                        break;
                    case 3:
                        platform = "Reddit";
                        fileName = "reddit.txt";
                        break;
                    case 4:
                        return;
                }

                if (!File.Exists(fileName))
                {
                    Console.WriteLine($"No posts found for {platform}.");

                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();

                    return;
                }

                string[] lines = File.ReadAllLines(fileName);
                List<string> linesList = lines.ToList<string>();

                bool containsPostsFromUser = false;

                foreach (string line in linesList)
                {
                    if (line.Contains(user.Username))
                    {
                        containsPostsFromUser = true;
                    }
                }

                if (containsPostsFromUser)
                {
                    for (int i = 0; i < linesList.Count; i++)
                    {
                        if (linesList[i].Contains(user.Username))
                        {
                            linesList.RemoveAt(i);
                            i--;
                        }
                    }

                    File.Delete(fileName);
                    File.WriteAllLines(fileName, linesList);

                    Logger.LogPostOperation(user.Username, platform, $"deleted ALL own posts)");
                    Console.WriteLine($"\nYour ({user.Username}'s) posts from {platform} have been successfully deleted.");
                }
                else
                {
                    Logger.LogError($"No posts file found for {platform} when deleting");
                    Console.WriteLine($"\nThere currently are no posts of yours ({user.Username}'s) on {platform}");
                }
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public void EditPost(User user)
        {
            if (user.Role == "guest")
            {
                Logger.LogError($"Guest user '{user.Username}' attempted to edit post");
                Red("Access denied. Guests cannot edit posts.");
                return;
            }

            Logger.LogAction(user.Username, "started post editing");

            int platformChoice;
            while (!int.TryParse(Console.ReadLine(), out platformChoice) || platformChoice < 1 || platformChoice > 4)
            {
                Red("Invalid choice.");
            }

            string platform = "";
            string fileName = "";
            switch (platformChoice)
            {
                case 1:
                    platform = "Facebook";
                    fileName = "facebook.txt";
                    break;
                case 2:
                    platform = "Instagram";
                    fileName = "instagram.txt";
                    break;
                case 3:
                    platform = "Reddit";
                    fileName = "reddit.txt";
                    break;
                case 4:
                    Logger.LogAction(user.Username, "cancelled post editing");
                    return;
            }

            if (!File.Exists(fileName))
            {
                Logger.LogError($"No posts file found for {platform} when editing");
                Console.WriteLine($"No posts found for {platform}.");
                return;
            }


            Console.WriteLine($"\nPosts to edit on {platform}:\n");
            string[] lines = File.ReadAllLines(fileName);
            List<int> validPostIndices = new List<int>();
            int displayPostId = 0;

            for (int i = 0; i < lines.Length; i++)
            {
                var parts = lines[i].Split(';');
                if (parts.Length >= 3 && parts[0] == user.Username)
                {
                    displayPostId++;
                    validPostIndices.Add(i);
                    Console.WriteLine($"{displayPostId}. {parts[1]}: {parts[2]}");
                }
            }

            if (displayPostId == 0)
            {
                Console.WriteLine("\nYou have no posts to edit on this platform.");

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();

                return;
            }

            Console.WriteLine("\nEnter the ID of the post you want to edit:");
            int postId;
            while (!int.TryParse(Console.ReadLine(), out postId) || postId < 1 || postId > displayPostId)
            {
                Red($"Please enter a valid ID (1-{displayPostId}):");
            }

            Console.WriteLine("\nEnter the new content for the post:");
            string? newContent = Console.ReadLine();

            int actualIndex = validPostIndices[postId - 1];
            var postParts = lines[actualIndex].Split(';');
            lines[actualIndex] = $"{postParts[0]};{postParts[1]};{newContent}";

            File.WriteAllLines(fileName, lines);

            Logger.LogPostOperation(user.Username, platform,$"edited a post to: '{newContent}'");

            Console.WriteLine("\nPost edited successfully!");
            Console.WriteLine($"Updated post: {postParts[1]}: {newContent}");
        }

        public void LoadPostsFromFile()
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
            {
                Red("Invalid choice.");
            }

            string platform = "";
            string fileName = "";

            switch (choice)
            {
                case 1:
                    platform = "facebook";
                    fileName = "facebook.txt";
                    break;
                case 2:
                    platform = "instagram";
                    fileName = "instagram.txt";
                    break;
                case 3:
                    platform = "reddit";
                    fileName = "reddit.txt";
                    break;
                case 4:
                    return;
            }

            if (!File.Exists(fileName))
            {
                Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"║ No posts found for {platform.PadRight(57)} ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            string[] lines = File.ReadAllLines(fileName);

            string title = $"{platform.ToUpper()} POSTS";
            int padding = (78 - title.Length) / 2;

            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║{new string(' ', padding)}{title}{new string(' ', 78 - title.Length - padding)}║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");

            if (lines.Length == 0)
            {
                Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ No posts available".PadRight(79) + "║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            foreach (string line in lines)
            {
                var parts = line.Split(';');
                if (parts.Length >= 3)
                {
                    string username = parts[0];
                    string timestamp = parts[1];
                    string content = parts[2];

                    Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════╗");
                    Console.WriteLine($"║ User: {username}".PadRight(79) + "║");
                    Console.WriteLine($"║ Date: {timestamp}".PadRight(79) + "║");
                    Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════╣");

                    int maxLength = 74;
                    for (int i = 0; i < content.Length; i += maxLength)
                    {
                        int length = Math.Min(maxLength, content.Length - i);
                        Console.WriteLine($"║ {content.Substring(i, length)}".PadRight(79) + "║");
                    }

                    Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                }
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public void DisplayUserPosts(User user)
        {
            int choice;
            while (!int.TryParse(Console.ReadLine(), out choice) || choice < 1 || choice > 4)
            {
                Red("Invalid choice.");
            }

            string platform = "";
            string fileName = "";

            switch (choice)
            {
                case 1:
                    platform = "Facebook";
                    fileName = "facebook.txt";
                    break;
                case 2:
                    platform = "Instagram";
                    fileName = "instagram.txt";
                    break;
                case 3:
                    platform = "Reddit";
                    fileName = "reddit.txt";
                    break;
                case 4:
                    return;
            }

            if (!File.Exists(fileName))
            {
                Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine($"║ No posts found for {platform.PadRight(57)} ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                return;
            }

            string[] lines = File.ReadAllLines(fileName);
            int postCount = 0;

            string title = $"YOUR {platform.ToUpper()} POSTS";
            int padding = (78 - title.Length) / 2;

            Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine($"║{new string(' ', padding)}{title}{new string(' ', 78 - title.Length - padding)}║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");


            foreach (string line in lines)
            {
                var parts = line.Split(';');
                if (parts.Length >= 3 && parts[0] == user.Username)
                {
                    postCount++;
                    string timestamp = parts[1];
                    string content = parts[2];

                    Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════╗");
                    Console.WriteLine($"║ Post #{postCount}".PadRight(79) + "║");
                    Console.WriteLine($"║ Date: {timestamp}".PadRight(79) + "║");
                    Console.WriteLine("╠══════════════════════════════════════════════════════════════════════════════╣");

                    int maxLength = 74;
                    for (int i = 0; i < content.Length; i += maxLength)
                    {
                        int length = Math.Min(maxLength, content.Length - i);
                        Console.WriteLine($"║ {content.Substring(i, length)}".PadRight(79) + "║");
                    }

                    Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
                }
            }

            if (postCount == 0)
            {
                Console.WriteLine("\n╔══════════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ You haven't posted anything on this platform yet".PadRight(79) + "║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝");
            }

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
