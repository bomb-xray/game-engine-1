using System;
using System.IO;
using System.Collections.Generic;

namespace Engine.Core
{
    /// <summary>
    /// Layered Virtual File System (VFS).
    /// Resolves files by searching patches (e.g. patch_v1.1.pack, updates/) first,
    /// falling back to base game assets. Allows seamless episodic updates without overwriting original files.
    /// </summary>
    public sealed class VFS
    {
        private readonly List<string> _searchPaths = new List<string>();

        public void RegisterPath(string fullPath)
        {
            if (Directory.Exists(fullPath) && !_searchPaths.Contains(fullPath))
            {
                // Invert insert so latest patch registered takes priority over base path
                _searchPaths.Insert(0, fullPath);
            }
        }

        public bool FileExists(string relativePath)
        {
            foreach (var path in _searchPaths)
            {
                string candidate = Path.Combine(path, relativePath);
                if (File.Exists(candidate)) return true;
            }
            return false;
        }

        public string ResolvePath(string relativePath)
        {
            foreach (var path in _searchPaths)
            {
                string candidate = Path.Combine(path, relativePath);
                if (File.Exists(candidate)) return candidate;
            }
            throw new FileNotFoundException($"Virtual File System could not find asset '{relativePath}'");
        }

        public byte[] ReadAllBytes(string relativePath)
        {
            string actualPath = ResolvePath(relativePath);
            return File.ReadAllBytes(actualPath);
        }

        public string ReadAllText(string relativePath)
        {
            string actualPath = ResolvePath(relativePath);
            return File.ReadAllText(actualPath);
        }
    }
}
