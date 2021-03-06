using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RunCPU
{
    class Program
    {
        const int GeneratedItems = 300;
        const int Times = 600;

        static async Task Main(string[] args)
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;

            var reservations = Generator.GetReservation(GeneratedItems);

            HttpClient client = new HttpClient();

            List<Task> tasks = new List<Task>();

            Stopwatch sw = Stopwatch.StartNew();

            Console.WriteLine("Enter service uri");
            var uri = Console.ReadLine();

            foreach (Reservation reservation in reservations)
            {
                var content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var task = client.PostAsync(uri + "/api/reservation/sign", content);
                for (int i = 0; i < Times; i++)
                {
                    task = task.ContinueWith(_ => client.PostAsync(uri + "/api/reservation/sign", content)).Unwrap();
                }
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);

            sw.Stop();

            Console.WriteLine($"Signing {GeneratedItems * Times} reservations takes {sw.Elapsed}");
        }
    }
}
