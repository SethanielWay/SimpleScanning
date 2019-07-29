using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Simple_Scanning
{
    class Program
    {
        static string help = "(N) Next Incomplete Task | (A) Add Task To List | (D) Mark Task As Done | (M) Move Task To End Of List\n(Arrows Keys) Navigate | (Esc) Save .txt & Exit Program \n";
        static int p = 0;
        static int i = 0;
        static int firstIncomplete = 0;

        static void Main(string[] args)
        {
            List<Tuple<string, int>> taskList = Program.ReadTextMakeList();
            RunListEditor(taskList);
        }
        
        static List<Tuple<string, int>> RunListEditor(List<Tuple<string, int>> list)
        {
            Program.TopList(list);
            ConsoleKeyInfo cki;
            Console.TreatControlCAsInput = true;
            do
            {
                cki = Console.ReadKey(true);
                switch (cki.Key)
                {
                    case ConsoleKey.N:
                        Program.FindNext(list);
                        break;
                    case ConsoleKey.M:
                        Program.MoveToEnd(list);
                        list = Program.ReadTextMakeList();
                        PrintList(list);
                        break;
                    case ConsoleKey.D:
                        list[Program.i + Program.p] = Program.MarkAsDone(list[Program.i + Program.p]);
                        list = CleanList(list);
                        PrintList(list);
                        Program.i = 0;
                        break;
                    case ConsoleKey.A:
                        AddToList(list);
                        list = Program.ReadTextMakeList();
                        break;
                    case ConsoleKey.LeftArrow:
                        if (Program.p == 0)
                        {
                            break;
                        }
                        else
                        {
                            Program.Left(list);
                            break;
                        }
                    case ConsoleKey.RightArrow:
                        if (list.Count / (Program.p + 20) < 1)
                        {
                            break;
                        }
                        else
                        {
                            Program.Right(list);
                            break;
                        }
                    case ConsoleKey.DownArrow:
                        if (list.Count > 20)
                        {
                            if (Console.CursorTop < 22)
                            {
                                try
                                {
                                    list[Program.i + Program.p + 1] = list[Program.i + Program.p + 1];
                                    (string T11, int T12) = parseTuple(list[Program.i + Program.p]);
                                    if (T12 > 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.DarkGray;
                                        Console.Write($"\r{T11}\r");
                                    }
                                    else
                                    {
                                        Console.Write($"\r{T11}\r");
                                    }
                                    Program.Down(list);
                                    break;
                                }
                                catch
                                {
                                    break;
                                }
                            }
                            else
                            {
                                    if (list.Count / (Program.p + 20) < 1)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        Program.Right(list);
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            if (Console.CursorTop < list.Count + 2)
                            {
                                Console.ResetColor();
                                (string T15, int T16) = parseTuple(list[Program.i + Program.p]);
                                if (T16 > 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkGray;
                                    Console.Write($"\r{T15}\r");
                                }
                                else
                                {
                                    Console.Write($"\r{T15}\r");
                                }
                                Program.Down(list);
                                break;
                            }
                            else // right arrow method
                            {
                                if (list.Count / (Program.p + 20) < 1)
                                {
                                    break;
                                }
                                else
                                {
                                    Program.Right(list);
                                    break;
                                }
                            }
                        }
                    case ConsoleKey.UpArrow:
                        if (Console.CursorTop > 3)
                        {
                            Console.ResetColor();
                            (string T19, int T20) = parseTuple(list[Program.i + Program.p]);
                            if (T20 > 0)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                Console.Write($"\r{T19}\r");
                            }
                            else
                            {
                                Console.Write($"\r{T19}\r");
                            }
                            Program.Up(list);
                            break;
                        }
                        else
                        {
                            if (Program.p == 0)
                            {
                                break;
                            }
                            else
                            {
                                Program.Left(list);
                                break;
                            }
                        }
                    case ConsoleKey.Enter:

                        break;
                }
            }
            while (cki.Key != ConsoleKey.Escape);
            UpdateTxt(list);
            Console.Clear();
            return list;
        }
         static List<Tuple<string, int>> ReadTextMakeList()
        {
            List<Tuple<string, int>> taskList = new List<Tuple<string, int>>();
            string line;

            try
            {
                StreamReader sr = new StreamReader("C:\\Users\\Admin\\Desktop\\MSSA\\Project Files\\Week 3\\Simple Scanning\\tasks.txt");
                line = sr.ReadLine();
                while(line != null)
                {
                    string cleanLine = line.Replace("(", "").Replace(")", "");
                    var values = cleanLine.Split(',');

                    var T1 = values[0];
                    var T2 = int.Parse(values[1]);

                    taskList.Add(Tuple.Create(T1, T2));
                    line = sr.ReadLine();
                }

                sr.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.SetCursorPosition(0,3);
            }

            return taskList;
        }

        static void AddToList(List<Tuple<string, int>> list)
        {
            try
            {
                Console.SetCursorPosition(0, 24);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Enter New Task.");
                Console.ResetColor();
                string task = Console.ReadLine();
                using (StreamWriter sw = new StreamWriter("C:\\Users\\Admin\\Desktop\\MSSA\\Project Files\\Week 3\\Simple Scanning\\tasks.txt"))
                {
                    foreach (var line in list)
                    {
                        sw.WriteLine($"{line}");

                    }
                    sw.Write("(" + task + ", 0)");
                    sw.Close();
                }
                Program.p = 0;
                Program.i = 0;
                PrintList(list);
                Console.SetCursorPosition(0, 24);
                Console.WriteLine($"{task} added to list.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.SetCursorPosition(0, 3);
            }
        }

        static void PrintList(List<Tuple<string, int>> list)
        {
            if (list.Count < 21)
            {
                Console.Clear();
                Console.WriteLine(Program.help);
                foreach (Tuple<string, int> task in list)
                {
                    (string stringTask, int intTask) = Program.parseTuple(task);
                    if (intTask > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine(stringTask);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(stringTask);
                    }
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine(Program.help);
                if(list.Count - Program.p >= 20)
                {
                    for (int j = 0; j < 20; j++)
                    
                    {
                        (string stringTask, int intTask) = Program.parseTuple(list[Program.p + j]);
                        if (intTask > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(stringTask);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine(stringTask);
                        }
                    }
                }
                else
                {
                    for(int j = 0; j < list.Count - Program.p; j++)


                    {
                        (string stringTask, int intTask) = Program.parseTuple(list[Program.p + j]);
                        if (intTask > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine(stringTask);
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine(stringTask);
                        }
                    }
                }             
            }
            Console.SetCursorPosition(0, 3);
            //Program.i = 0;
            //(string T21, int T22) = Program.parseTuple(list[Program.p + Program.i]);
            //Console.ForegroundColor = ConsoleColor.Black;
            //Console.BackgroundColor = ConsoleColor.White;
            //Console.Write($"\r{T21}\r");
            //Console.ResetColor();

        }

        static List<Tuple<string, int>> UpdateList(List<Tuple<string, int>> list)
        {
            list = CleanList(list);
            list = Program.ReadTextMakeList();
            return list;
        }

        static void UpdateTxt(List<Tuple<string, int>> list)
        {
            list = CleanList(list);
            using (StreamWriter sw = new StreamWriter("C:\\Users\\Admin\\Desktop\\MSSA\\Project Files\\Week 3\\Simple Scanning\\tasks.txt"))
            {
                foreach (var line in list)
                {
                    sw.WriteLine($"{line}");

                }
                sw.Close();
            }
        }

        static (string, int) parseTuple(Tuple<string, int> tuple)
        {
            (string T1, int T2) = tuple;
            return (T1, T2);
        }

        static Tuple<string, int> MarkAsDone(Tuple<string, int> tuple)
        {
            (string T1, int T2) = parseTuple(tuple);
            T2 = T2 + 1;
            tuple = Tuple.Create(T1, T2);
            return tuple;
        }

        static void MoveToEnd(List<Tuple<string, int>> list)
        {
            var tuple = list[Program.i + Program.p] = MarkAsDone(list[Program.i + Program.p]);
            list = CleanList(list);
            (string T1, int T2) = parseTuple(tuple);
            using (StreamWriter sw = new StreamWriter("C:\\Users\\Admin\\Desktop\\MSSA\\Project Files\\Week 3\\Simple Scanning\\tasks.txt"))
            {
                foreach (var line in list)
                {
                    sw.WriteLine($"{line}");

                }
                sw.Write("(" +T1 + ", 0)");
                sw.Close();
            }
            Program.p = 0;
            Program.i = 0;
            PrintList(list);
            Console.SetCursorPosition(0, 3);

        }

        static List<Tuple<string, int>> CleanList(List<Tuple<string, int>> list)
        {
            List<Tuple<string, int>> newList = new List<Tuple<string, int>>();
            for(int k = 0; k < list.Count; k++)
            {
                (string T1, int T2) = parseTuple(list[k]);
                if(T2 == 1)
                {
                    Program.firstIncomplete = k;
                }
                else
                {
                    Program.firstIncomplete = k;
                    break;
                }

            }
            for(int m = Program.firstIncomplete; m < list.Count; m++)
            {
                newList.Add(list[m]);
            }
            return newList;

        }

        static void FindNext(List<Tuple<string, int>> list)
        {
            try
            {
                list[Program.i + Program.p + 1] = list[Program.i + Program.p + 1];
                Console.ResetColor();
                (string T1, int T2) = parseTuple(list[Program.i + Program.p]);
                Console.Write($"\r{T1}\r");
                for (int j = Program.i + Program.p; j < list.Count; j++)
                {
                    (string T3, int T4) = parseTuple(list[j + 1]);
                    if (T4 == 0)
                    {
                        Program.i = j + 1;
                        if (Program.i < 20)
                        {
                            Console.SetCursorPosition(0, Program.i + 3);
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write($"\r{T3}\r");
                            Console.ResetColor();
                            break;
                        }
                        else
                        {
                            Program.i = (j + 1) % 20;
                            Program.p = (j + 1) - Program.i;
                            Program.PrintList(list);
                            Console.SetCursorPosition(0, Program.i + 3);
                            Console.ForegroundColor = ConsoleColor.Black;
                            Console.BackgroundColor = ConsoleColor.White;
                            Console.Write($"\r{T3}\r");
                            Console.ResetColor();
                            break;
                        }
                    }
                }
                 }
            catch
            {
                Console.SetCursorPosition(0, 24);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("End Of List.");
                Console.ResetColor();
                Console.SetCursorPosition(0, Program.i + 3);
            }
        }
        static void Left(List<Tuple<string, int>> list)
        {
            Program.p = Program.p - 20;
            Program.i = 19;
            Console.Clear();
            PrintList(list);
            Console.SetCursorPosition(0, 22);
            (string T1, int T2) = Program.parseTuple(list[Program.p + Program.i]);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write($"\r{T1}\r");
            Console.ResetColor();
        }

        static void Right(List<Tuple<string, int>> list)
        {
            Program.p = Program.p + 20;
            Program.i = 0;
            Console.Clear();
            PrintList(list);
            (string T13, int T14) = Program.parseTuple(list[Program.p + Program.i]);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write($"\r{T13}\r");
            Console.ResetColor();
        }

        static void Down(List<Tuple<string, int>> list)
        {
            ++Program.i;
            Console.CursorTop = Console.CursorTop + 1;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            (string T1, int T2) = parseTuple(list[Program.i + Program.p]);
            Console.Write($"\r{T1}\r");
            Console.ResetColor();
        }

        static void Up(List<Tuple<string, int>> list)
        {
            --Program.i;
            Console.CursorTop = Console.CursorTop - 1;
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            (string T1, int T2) = parseTuple(list[Program.i + Program.p]);
            Console.Write($"\r{T1}\r");
            Console.ResetColor();
        }

        static void TopList(List<Tuple<string, int>> list)
        {
            Console.Clear();
            Console.WriteLine(Program.help);
            PrintList(list);
            Console.SetCursorPosition(0, 3);
            (string T1, int T2) = Program.parseTuple(list[Program.p + Program.i]);
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.White;
            Console.Write($"\r{T1}\r");
            Console.ResetColor();
        }
    }   
}
