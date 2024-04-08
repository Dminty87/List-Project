using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;

namespace ToDoList
{
    internal class Program
    {

        public static FileInfo TryOpenList(DirectoryInfo dir, out XDocument doc)
        {
            List<FileInfo> dirFiles = new List<FileInfo>(dir.GetFiles("*.xml"));
            List<XDocument> documents = new List<XDocument>();
            doc = null;
            for (int i = 0; i < dirFiles.Count; i++)
            {
                try
                {
                    documents.Add(XDocument.Load(dirFiles[i].FullName));
                }
                catch 
                {
                    dirFiles.RemoveAt(i);
                    i--;
                }
            }
            while (doc == null)
            {
                Console.Clear();
                Console.WriteLine("Open a List or type \"Exit\" to go exit.\n0) New List");
                for (int i = 0; i < documents.Count; i++)
                {
                    Console.WriteLine($"{i + 1}) {documents[i].Root.Attribute("name").Value}");
                }
                Console.Write("\n>");
                string input = Console.ReadLine().ToLower().Trim();
                Console.WriteLine();
                if (input.Equals("0") || input.Equals("new list"))
                {
                    Console.Write("Enter a name for the new list:\n\n>");
                    input = Console.ReadLine().Trim();
                    Console.WriteLine();
                    string file = Path.ChangeExtension(dir.FullName + Path.DirectorySeparatorChar + input, ".xml");
                    if (File.Exists(file))
                    {
                        Console.Write("A list with the same name already exists, replace it? y/n\n\n>");
                        string inputReplace = Console.ReadLine().ToLower().Trim();
                        Console.WriteLine();
                        if ( ! (inputReplace.Equals("y") || inputReplace.Equals("yes")) )
                        {
                            continue;
                        }
                    }
                    doc = new XDocument(new XDeclaration("1.0", "utf-8", null), new XElement("List", new XAttribute("name", input)));
                    doc.Save(file);
                    return new FileInfo(file);
                }
                try
                {
                    doc = documents[int.Parse(input) - 1];
                    return dirFiles[int.Parse(input) - 1];
                }
                catch
                {
                    for (int i = 0; i < dirFiles.Count; i++)
                    {
                        if (input.Equals(documents[i].Root.Attribute("name").Value.ToLower()))
                        {
                            doc = documents[i];
                            return dirFiles[i];
                        }
                    }   
                }
                if (input.Equals("exit"))
                {
                    Environment.Exit(Environment.ExitCode);
                }
            }
            return null;
        }

        public static void DisplayList(XDocument listDoc)
        {
            Console.Clear();
            Console.WriteLine($"List: {listDoc.Root.Attribute("name").Value}");
            int item = 1;
            foreach (XElement e in listDoc.Root.Descendants("ListItem"))
            {
                Console.WriteLine(item + ") " + e.Value);
                item++;
            }
            if (item == 1)
            {
                Console.WriteLine("The list is empty.");
            }
            Console.WriteLine();
        }

        public enum MenuOptions
        {
            Add = 1,
            Remove = 2,
            Delete = 3,
            Open = 4,
            Exit = 5
        }

        public static MenuOptions DisplayMenu()
        {
            MenuOptions choice = 0;
            while ( ! Enum.IsDefined(typeof(MenuOptions), choice))
            {
                foreach (int i in Enum.GetValues(typeof(MenuOptions)))
                {
                    Console.WriteLine(i+") "+Enum.GetName(typeof(MenuOptions), i));
                }

                Console.WriteLine();
                string input = Console.ReadLine().ToLower().Trim();

                if ( ! Enum.TryParse(input, true, out choice))
                {
                    Console.WriteLine("Invalid");
                    continue;
                }
            }
            return choice;
        }

        static void Main(string[] args)
        {
            bool exit = false;

            DirectoryInfo dir = Directory.CreateDirectory(Directory.GetCurrentDirectory() + @"\ToDoListData");

            FileInfo listFile = null;
            XDocument listDoc = null;

            while (!exit)
            {

                while (listFile == null)
                {
                    listFile = TryOpenList(dir, out listDoc);
                }

                DisplayList(listDoc);

                MenuOptions choice = DisplayMenu();

                if (choice == MenuOptions.Add)
                {
                    Console.Write("What would you like to add?\n\n>");

                    listDoc.Root.Add(new XElement("ListItem", new XCData(Console.ReadLine().Trim())));

                    listDoc.Save(listFile.FullName);
                }

                if (choice == MenuOptions.Remove)
                {
                    DisplayList(listDoc);
                    Console.Write("Which item would you like to remove?\n\n>");
                    string toRemove = Console.ReadLine().ToLower().Trim();

                    List<XElement> elements = new List<XElement>(listDoc.Root.Elements("ListItem"));

                    try
                    {
                        elements[int.Parse(toRemove) - 1].Remove();
                    }
                    catch
                    {
                        foreach (XElement element in elements)
                        {
                            if (element.Value.ToLower().Trim().Equals(toRemove))
                            {
                                element.Remove();
                                break;
                            }
                        }
                    }
                    listDoc.Save(listFile.FullName);
                }

                if (choice == MenuOptions.Open)
                {
                    listFile = TryOpenList(dir, out listDoc);
                }

                if (choice == MenuOptions.Delete)
                {
                    string confirmation = $"Confirm {listFile.Name}";
                    Console.WriteLine($"To delete the file \"{listFile.Name}\" type \"{confirmation}\".");
                    if (Console.ReadLine().Equals(confirmation))
                    {
                        listFile.Delete();
                        listFile = null;
                    }
                }

                if (choice == MenuOptions.Exit)
                {
                    exit = true;
                }

            }
        }
    }
}
