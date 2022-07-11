using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace nodelist
{

    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;
    }

    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            List<ListNode> arr = new List<ListNode>();
            ListNode temp = new ListNode();
            temp = Head;

            
            do
            {
                arr.Add(temp);
                temp = temp.Next;
            } while (temp != null);

            
            using (StreamWriter w = new StreamWriter(s))
                foreach (ListNode n in arr)
                    w.WriteLine(n.Data.ToString() + ":" + arr.IndexOf(n.Rand).ToString());
        }

        public void Deserialize(FileStream s)
        {
            List<ListNode> arr = new List<ListNode>();
            ListNode temp = new ListNode();
            Count = 0;
            Head = temp;
            string line;

            
            try
            {
                using (StreamReader sr = new StreamReader(s))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (!line.Equals(""))
                        {
                            Count++;
                            temp.Data = line;
                            ListNode next = new ListNode();
                            temp.Next = next;
                            arr.Add(temp);
                            next.Prev = temp;
                            temp = next;
                        }
                    }
                }

                
                Tail = temp.Prev;
                Tail.Next = null;

                
                foreach (ListNode n in arr)
                {
                    n.Rand = arr[Convert.ToInt32(n.Data.Split(':')[1])];
                    n.Data = n.Data.Split(':')[0];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Не удалось обработать файл");
                Console.WriteLine(e.Message);
                Console.WriteLine("Enter --> exit.");
                Console.Read();
                Environment.Exit(0);
            }
        }
    }



    class Program
    {

        static Random rand = new Random();

        
        static ListNode addNode(ListNode prev)
        {
            ListNode result = new ListNode();
            result.Prev = prev;
            result.Next = null;
            result.Data = rand.Next(0, 100).ToString();
            prev.Next = result;
            return result;
        }

        
        static ListNode randomNode(ListNode _head, int _length)
        {
            int k = rand.Next(0, _length);
            int i = 0;
            ListNode result = _head;
            while (i < k)
            {
                result = result.Next;
                i++;
            }
            return result;
        }

        static void Main(string[] args)
        {
            
            int length = 7;

            
            ListNode head = new ListNode();
            ListNode tail = new ListNode();
            ListNode temp = new ListNode();

            head.Data = rand.Next(0, 1000).ToString();

            tail = head;

            for (int i = 1; i < length; i++)
                tail = addNode(tail);

            temp = head;

            
            for (int i = 0; i < length; i++)
            {
                temp.Rand = randomNode(head, length);
                temp = temp.Next;
            }

            
            ListRand first = new ListRand();
            first.Head = head;
            first.Tail = tail;
            first.Count = length;

            
            FileStream fs = new FileStream("dat.dat", FileMode.OpenOrCreate);
            first.Serialize(fs);

            
            ListRand second = new ListRand();
            try
            {
                fs = new FileStream("sample.dat", FileMode.Open);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Enter --> exit.");
                Console.Read();
                Environment.Exit(0);
            }
            second.Deserialize(fs);

            if (second.Tail.Data == first.Tail.Data) Console.WriteLine("Успешно");
            Console.Read();

        }
    }
}
