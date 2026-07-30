using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {

        private IPeopleService _peopleService;

        public PeopleController([FromKeyedServices("peopleService")] IPeopleService peopleService)
        {
            _peopleService = peopleService;
        }

        [HttpGet("all")]
        public List<People> GetPeople() => Repository.People;

        [HttpGet("{id}")]
        public ActionResult<People> Get(int id)
        {
            var people = Repository.People.FirstOrDefault(people => people.Id == id);

            if (people == null)
            {
                return NotFound();
            }

            return Ok(people);
        }

        [HttpGet("search/{search}")]
        public List<People> Get(string search) =>
            Repository.People.Where(people => people.Name.Contains(search)).ToList();


        [HttpPost]
        public IActionResult Add(People people)
        {
            if(!_peopleService.validate(people))
            {
                return BadRequest();
            }
            Repository.People.Add(people);
            return NoContent();
        }
    }

    public class Repository
    {
        public static List<People> People = new List<People>
        {
            new People() {
                Id = 1, Name = "Pedro", Birthday = new DateTime(2000, 07, 02)
            },
            new People() {
                Id = 2, Name = "Jean", Birthday = new DateTime(2003, 07, 02)
            },
            new People() {
                Id = 3, Name = "Dolly", Birthday = new DateTime(2007, 07, 02)
            },

        };
    }


    public class People
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
    }
}
