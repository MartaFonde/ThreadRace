using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadRace
{
    class Program
    {
        static readonly private object l = new object();

        static void Main(string[] args)
        {
            int point = 0;
            bool running = true;

            Thread t1 = new Thread(() =>
            {
                while (running)
                {
                    lock (l)
                    {
                        point++;
                        if (point <= 1000)
                        {
                            Console.WriteLine("Thread 1: " + point);
                        }
                        else
                        {
                            running = false;
                            Monitor.Pulse(l);
                        }
                    }
                }
            });

            Thread t2 = new Thread(() =>
            {
                while (running)
                {
                    lock (l)
                    {
                        point--;
                        if (point >= -1000)
                        {
                            Console.WriteLine("Thread 2: " + point);
                        }
                        else
                        {
                            running = false;
                            Monitor.Pulse(l);
                        }
                    }
                }
            });

            t1.Start();
            t2.Start();

            lock (l)
            {
                while (running)
                {
                    Monitor.Wait(l);
                }
                Console.WriteLine("Ganador: " + (point > 0 ? "Thread 1" : "Thread 2"));
                Console.ReadKey();
            }
        }
    }
}