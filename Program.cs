using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace os_lab_3
{
    internal static class Program
    {
        private const int Producers = 3;
        private const int ProducersTimeout = 300;
        private const int Consumers = 2;
        private const int ConsumersTimeout = 500;
        private const int Capacity = 200;
        private static bool _stopProducers = false;
        private static bool _pauseProducers = false;

        private static readonly BlockingCollection<int> Queue = new BlockingCollection<int>(Capacity);
        struct Th
        {
            public Th(Thread thre)
            {
                thre.Start();
            }
        }
        static void Main(string[] args)
        {
            int realPr = 0;
            int realCons = 0;
            Console.WriteLine("FACTORIO SIMULATOR v. 0.0.0.0.0.0.0.0.0.0.0.0.0.5.2.4.89.8.4 pre-alpha early access no embargo\n\n\n");
            Console.ReadLine();
            List <Th> prodThreads = new List<Th>();
            if (prodThreads == null) throw new ArgumentNullException(nameof(prodThreads));
            List <Th> consThreads = new List<Th>();
            if (consThreads == null) throw new ArgumentNullException(nameof(consThreads));

            void AddProd(int cnt)
            {
                for (var i = 0; i < cnt; i++)
                {
                    realPr++;
                    prodThreads.Add(new Th (new Thread(Produce) { Name = $"P-{realPr}" }));
                }
            }

            void AddCons(int cnt)
            {
                for (var i = 0; i < Consumers; i++)
                {
                    realCons++;
                    consThreads.Add(new Th(new Thread(Consume) { Name = $"C-{realCons}" }));
                }
            }

            AddProd(Producers);
            AddCons(Consumers);

            while(true)
            {
                if (Queue.Count >= 100 && !_pauseProducers)
                {
                    _pauseProducers = true;
                    Console.WriteLine("\n\t\t\tPRODUCERS PAUSED!\n");
                }
                if (Queue.Count <= 80 && _pauseProducers)
                {
                    _pauseProducers = false;
                    Console.WriteLine("\n\t\t\tPRODUCERS STARTED!\n");
                }
                //------------------------------------------------------
                if (Console.ReadKey(true).Key == ConsoleKey.UpArrow)
                {
                    AddProd(1);
                    Console.WriteLine("\n\t\t\tPRODUCER ADDED!\n");
                }
                if (Console.ReadKey(true).Key == ConsoleKey.RightArrow)
                {
                    AddCons(1);
                    Console.WriteLine("\n\t\t\tCONSUMER ADDED!\n");
                }
                if (Console.ReadKey(true).Key == ConsoleKey.X)
                {
                    _pauseProducers = !_pauseProducers;
                }
                if (Console.ReadKey(true).Key == ConsoleKey.Q)
                {
                    Console.WriteLine("\n\t\t\tPRODUCERS STOPPED!\n");
                    _stopProducers = true;
                    break;
                }
            }
        }

        private static void Produce()
        {
            var rnd = new Random();

            while (true)
            {
                if (!_pauseProducers)
                {
                    Queue.Add(rnd.Next(1, 100));
                    Console.WriteLine($"Element produced by {Thread.CurrentThread.Name}\t\t({Queue.Count}/{Capacity})");
                    Thread.Sleep(ProducersTimeout);
                }

                if (_stopProducers)
                    return;
            }
        }

        private static void Consume()
        {
            while (true)
            {
                var n = Queue.Take();
                Console.WriteLine($"Element consumed by {Thread.CurrentThread.Name}\t\t({Queue.Count}/{Capacity})");
                Thread.Sleep(ConsumersTimeout);

                if (Queue.Count <= 0)
                    return;
            }
        }
    }
}