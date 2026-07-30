using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SomeController : ControllerBase
    {
        [HttpGet("sync")]
        public IActionResult GetSync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            Thread.Sleep(1000);
            Console.WriteLine("Conectando a la base de datos");


            Thread.Sleep(1000);
            Console.WriteLine("Enviando el Email");

            Console.WriteLine("Todo ha terminado");
            stopwatch.Stop();
            return Ok(stopwatch.Elapsed);
        }

        [HttpGet("async")]
        public async Task<IActionResult> GetAsync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            var task1 = new Task<int>(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Conectando a la base de datos");
                return 8;
            });
            var task2 = new Task<int>(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Enviando el Email");
                return 8;
            });

            task1.Start();
            task2.Start();

            Console.WriteLine("Estoy haciendo algo más");

            var returnOfTask1 = await task1;
            var returnOfTask2 = await task2;
            Console.WriteLine("Todo terminó");
            stopwatch.Stop();
            return Ok(returnOfTask1 + " " + " " + returnOfTask2 + stopwatch.Elapsed); 
        }
    }
}
