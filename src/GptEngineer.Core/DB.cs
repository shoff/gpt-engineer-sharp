﻿using System.Text;

namespace GptEngineer;

public class DB
{
    private readonly string path;

    public DB(string path)
    {
        this.path = Path.GetFullPath(path);

        Directory.CreateDirectory(this.path);
    }

    public string this[string key]
    {
        get
        {
            string fullPath = Path.Combine(this.path, key);

            if (!File.Exists(fullPath))
            {
                throw new KeyNotFoundException(key);
            }

            return File.ReadAllText(fullPath, Encoding.UTF8);
        }

        set
        {
            string fullPath = Path.Combine(this.path, key);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath) ?? throw new InvalidOperationException());

            if (value != null)
            {
                File.WriteAllText(fullPath, value, Encoding.UTF8);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Value must be a string");
                Console.ResetColor();
                //  throw new ArgumentException("Value must be a string");
            }
        }
    }
}