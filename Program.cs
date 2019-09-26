using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncBreakfast
{
    class Program
    {
        static async Task Main(string[] args)
        {

            //MakeSyncronousBreakfast();
            //Console.WriteLine();
            //await NotReallyAsyncBreakfast();
            //Console.WriteLine();
            
            using (var cancellationTokenSource = new CancellationTokenSource())
            {
                var keyBoardTask = Task.Run(() =>
                {
                    Console.WriteLine("Press enter to cancel correctly async breakfast");
                    Console.ReadKey();

                    // Cancel the task
                    cancellationTokenSource.Cancel();
                });

                try
                {
                    await CorrectlyAsyncBreakfast(cancellationTokenSource.Token);
                    Console.WriteLine();
                }
                catch (TaskCanceledException)
                {
                    Console.WriteLine("Task was cancelled. No breakfast for you!");
                    Console.ReadKey();
                    return;
                }

                await keyBoardTask;

            }
            
            Console.WriteLine("All breakfasts completed");
            Console.ReadKey();
        }

        static void MakeSyncronousBreakfast()
        {
            Console.WriteLine("Starting Sync Breakfast");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");
            Egg eggs = FryEggs(2);
            TakeOutTheBinAsync();

            Console.WriteLine("eggs are ready");
            Bacon bacon = FryBacon(3);
            Console.WriteLine("bacon is ready");
            Toast toast = ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");
            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            sw.Stop();
            Console.WriteLine("Sync Breakfast is ready!");
            Console.WriteLine("It took {0} seconds", sw.ElapsedMilliseconds / 1000M);
        }

        static async Task NotReallyAsyncBreakfast()
        {
            Console.WriteLine("Starting NotReallyAsync Breakfast");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");
            Egg eggs = await FryEggsAsync(2);
            TakeOutTheBinAsync();

            Bacon bacon = await FryBaconAsync(3);
            Toast toast = await ToastBreadAsync(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");
            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            sw.Stop();
            Console.WriteLine("NotReallyAsync Breakfast is ready!");
            Console.WriteLine("It took {0} seconds", sw.ElapsedMilliseconds / 1000M);
        }

        static async Task CorrectlyAsyncBreakfast(CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting CorrectlyAsync Breakfast");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");
            var eggsTask = FryEggsAsync(2);
            TakeOutTheBinAsync();
            var baconTask = FryBaconAsync(3);
            var toastTask = ToastBreadAsync(2);

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");

            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
            await eggsTask;
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
            await baconTask;
            if (cancellationToken.IsCancellationRequested)
                throw new TaskCanceledException();
            var toast = await toastTask;
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");

            sw.Stop();
            Console.WriteLine("CorrectlyAsync Breakfast is ready!");
            Console.WriteLine("It took {0} seconds", sw.ElapsedMilliseconds / 1000M);
        }


        private static async Task<Egg> FryEggsAsync(int v)
        {
            await Task.Delay(v * 1000);
            Console.WriteLine("eggs are ready");

            return new Egg();
        }

        private static Egg FryEggs(int v)
        {
            Thread.Sleep(v * 1000);
            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Thread.Sleep(1000);
            return new Coffee();
        }

        private static Juice PourOJ()
        {
            Thread.Sleep(1000);
            return new Juice();
        }

        private static void ApplyJam(Toast toast)
        {
            Thread.Sleep(1000);
        }

        private static void ApplyButter(Toast toast)
        {
            Thread.Sleep(1500);
        }

        private static async Task<Toast> ToastBreadAsync(int v)
        {
            await Task.Delay(v * 1000);
            return new Toast();
        }
        private static Toast ToastBread(int v)
        {
            Thread.Sleep(v * 1000);
            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int v)
        {
            await Task.Delay(v * 1000);
            Console.WriteLine("bacon is ready");

            return new Bacon();
        }
        private static Bacon FryBacon(int v)
        {
            Thread.Sleep(v * 1000);
            return new Bacon();
        }

        private static async void TakeOutTheBinAsync()
        {
            await Task.Delay(2000);
            Console.WriteLine("The bin is now outside");
        }
    }
}
