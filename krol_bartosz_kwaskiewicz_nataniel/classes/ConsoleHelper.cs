using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace krol_bartosz_kwaskiewicz_nataniel.classes
{
    public static class ConsoleHelper
    {
        public static void Red(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Cyan;
        }

        public static void CenterText(string text, int totalWidth = 78)
        {
            int spaces = (totalWidth - text.Length) / 2;
            Console.WriteLine(new string(' ', spaces) + text);
        }

        public static void PrintHeader(string title)
        {
            Console.Clear();
            string border = new string('_', 78);
            Console.WriteLine("|" + border + "|");
            CenterText(title);
            Console.WriteLine("|" + new string('_', 78) + "|\n");
        }

        public static void PrintMenuHeader(string title)
        {
            Console.WriteLine("\n╔" + new string('═', 78) + "╗");
            CenterText(title);
            Console.WriteLine("╚" + new string('═', 78) + "╝");
        }

        public static void PrintOption(int number, string text)
        {
            Console.WriteLine($"║ {number}. {text.PadRight(73)} ║");
        }

        public static void PrintMenuOptions(params string[] options)
        {
            Console.WriteLine("╔" + new string('═', 78) + "╗");
            for (int i = 0; i < options.Length; i++)
            {
                PrintOption(i + 1, options[i]);
            }
            Console.WriteLine("╚" + new string('═', 78) + "╝");
        }

        public static string ReadMaskedPassword()
        {
            var password = new System.Text.StringBuilder();
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);
                if (!char.IsControl(key.KeyChar))
                {
                    password.Append(key.KeyChar);
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password.Remove(password.Length - 1, 1);
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            Console.WriteLine();
            return password.ToString();
        }

        public static void InitializeConsole()
        {
            Console.Title = "Social Media Manager";
            Console.SetWindowSize(Math.Min(100, Console.LargestWindowWidth), Math.Min(40, Console.LargestWindowHeight));
            Console.ForegroundColor = ConsoleColor.Cyan;
        }
    }
}
