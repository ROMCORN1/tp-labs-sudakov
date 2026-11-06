using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lab3
{
    internal static class Program
    {
        private static int Main()
        {
            try { Console.OutputEncoding = Encoding.UTF8; } catch { /* на старых консолях */ }

            Console.WriteLine("Лаб. 3, вариант 1: поиск дубликатов (имя + размер)");
            Console.WriteLine();

            string folder = AskFolder();
            bool recursive = AskYesNo("Искать в подпапках? (y/n, по умолчанию y): ", true);
            string pattern = AskPattern();

            try
            {
                var groups = new DuplicateFinder().Find(folder, recursive, pattern);
                Print(groups);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine("Нет доступа: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Непредвиденная ошибка: " + ex.GetType().Name + ": " + ex.Message);
            }

            return 0;
        }

        private static string AskFolder()
        {
            while (true)
            {
                Console.Write("Введите путь к папке (Enter — текущая папка): ");
                var input = Console.ReadLine();
                var folder = string.IsNullOrWhiteSpace(input)
                    ? Directory.GetCurrentDirectory()
                    : input.Trim().Trim('"');

                if (Directory.Exists(folder)) return Path.GetFullPath(folder);

                Console.WriteLine("  Папка не существует: " + folder);
            }
        }

        private static bool AskYesNo(string prompt, bool defaultYes)
        {
            Console.Write(prompt);
            var s = Console.ReadLine();
            if (s != null) s = s.Trim().ToLowerInvariant();
            if (string.IsNullOrEmpty(s)) return defaultYes;
            return s == "y" || s == "yes" || s == "д" || s == "да";
        }

        private static string AskPattern()
        {
            Console.Write("Маска файлов (Enter — все, например *.txt): ");
            var s = Console.ReadLine();
            if (s != null) s = s.Trim();
            return string.IsNullOrEmpty(s) ? "*" : s;
        }

        private static void Print(IReadOnlyList<DuplicateGroup> groups)
        {
            Console.WriteLine();
            if (groups.Count == 0)
            {
                Console.WriteLine("Дубликатов не найдено.");
                return;
            }

            Console.WriteLine("Групп дубликатов: " + groups.Count);
            Console.WriteLine();

            long totalWasted = 0;
            int i = 1;

            foreach (var g in groups)
            {
                long wasted = g.Size * (g.Paths.Count - 1);
                totalWasted += wasted;

                Console.WriteLine(
                    "[" + i++ + "] " + g.FileName + "  —  " + FormatSize(g.Size) +
                    "  ×" + g.Paths.Count + "  (лишних: " + FormatSize(wasted) + ")");
                foreach (var p in g.Paths)
                    Console.WriteLine("        " + p);
                Console.WriteLine();
            }

            Console.WriteLine("Итого потенциально лишнего места: " + FormatSize(totalWasted));
        }

        private static string FormatSize(long bytes)
        {
            string[] units = { "Б", "КБ", "МБ", "ГБ", "ТБ" };
            double v = bytes;
            int k = 0;
            while (v >= 1024 && k < units.Length - 1) { v /= 1024; k++; }
            return k == 0 ? bytes + " " + units[k] : v.ToString("F2") + " " + units[k];
        }
    }
}