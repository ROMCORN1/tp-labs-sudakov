using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lab3
{
    public sealed class DuplicateGroup
    {
        public string FileName { get; }
        public long Size { get; }
        public IReadOnlyList<string> Paths { get; }

        public DuplicateGroup(string fileName, long size, IReadOnlyList<string> paths)
        {
            FileName = fileName;
            Size = size;
            Paths = paths;
        }
    }

    public sealed class DuplicateFinder
    {
        public IReadOnlyList<DuplicateGroup> Find(
            string folder,
            bool recursive = true,
            string searchPattern = "*")
        {
            if (string.IsNullOrWhiteSpace(folder))
                throw new ArgumentException("Путь к папке не задан.", nameof(folder));
            if (!Directory.Exists(folder))
                throw new DirectoryNotFoundException("Папка не найдена: " + folder);

            bool caseInsensitive = Path.DirectorySeparatorChar == '\\';

            var buckets = new Dictionary<string, List<string>>();

            foreach (var path in EnumerateFilesSafe(folder, searchPattern, recursive))
            {
                try
                {
                    var info = new FileInfo(path);
                    if (!info.Exists) continue;

                    string name = caseInsensitive ? info.Name.ToLowerInvariant() : info.Name;
                    string key = name + "\u0000" + info.Length.ToString();

                    List<string> list;
                    if (!buckets.TryGetValue(key, out list))
                    {
                        list = new List<string>(1);
                        buckets[key] = list;
                    }
                    list.Add(info.FullName);
                }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
            }

            var result = new List<DuplicateGroup>();
            foreach (var kv in buckets)
            {
                if (kv.Value.Count <= 1) continue;

                var sorted = kv.Value
                    .OrderBy(p => p, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                result.Add(new DuplicateGroup(
                    Path.GetFileName(sorted[0]),
                    new FileInfo(sorted[0]).Length,
                    sorted));
            }

            result.Sort((a, b) =>
            {
                long wa = a.Size * (a.Paths.Count - 1);
                long wb = b.Size * (b.Paths.Count - 1);
                int c = wb.CompareTo(wa);
                if (c != 0) return c;
                return string.Compare(a.FileName, b.FileName, StringComparison.OrdinalIgnoreCase);
            });

            return result;
        }

        private static IEnumerable<string> EnumerateFilesSafe(string folder, string pattern, bool recursive)
        {
            IEnumerable<string> files;
            try
            {
                files = Directory.EnumerateFiles(folder, pattern, SearchOption.TopDirectoryOnly);
            }
            catch (UnauthorizedAccessException) { yield break; }
            catch (DirectoryNotFoundException) { yield break; }
            catch (IOException) { yield break; }

            foreach (var f in files) yield return f;

            if (!recursive) yield break;

            IEnumerable<string> dirs;
            try
            {
                dirs = Directory.EnumerateDirectories(folder);
            }
            catch (UnauthorizedAccessException) { yield break; }
            catch (DirectoryNotFoundException) { yield break; }
            catch (IOException) { yield break; }

            foreach (var d in dirs)
            {
                try
                {
                    var attrs = File.GetAttributes(d);
                    if ((attrs & FileAttributes.ReparsePoint) != 0) continue;
                }
                catch { continue; }

                foreach (var f in EnumerateFilesSafe(d, pattern, true))
                    yield return f;
            }
        }
    }
}