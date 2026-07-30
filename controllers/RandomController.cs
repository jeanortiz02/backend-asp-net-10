using Backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend
{
    [Route("api/[controller]")]
    [ApiController]
    public class RandomController : ControllerBase
    {

        private IRandomService _randomServiceSingleton;
        private IRandomService _randomServiceScope;
        private IRandomService _randomServiceTransient;
        private IRandomService _randomServiceSingleton2;
        private IRandomService _randomServiceScope2;
        private IRandomService _randomServiceTransient2;


        public RandomController(
            [FromKeyedServices("randomSingleton")] IRandomService randomSingleton,
            [FromKeyedServices("randomScope")] IRandomService randomScope,
            [FromKeyedServices("randomTransient")] IRandomService randomTransient,

            [FromKeyedServices("randomSingleton")] IRandomService randomSingleton2,
            [FromKeyedServices("randomScope")] IRandomService randomScope2,
            [FromKeyedServices("randomTransient")] IRandomService randomTransient2

            )
        {
            _randomServiceSingleton = randomSingleton;
            _randomServiceScope = randomScope;
            _randomServiceTransient = randomTransient;

            _randomServiceSingleton2 = randomSingleton2;
            _randomServiceScope2 = randomScope2;
            _randomServiceTransient2 = randomTransient2;
        }

        [HttpGet]
        public ActionResult<Dictionary<string, int>> Get()
        {
            var result = new Dictionary<string, int>();

            result.Add("Singleton 1", _randomServiceSingleton.Value);
            result.Add("Scoped 1", _randomServiceScope.Value);
            result.Add("Transient 1", _randomServiceTransient.Value);

            result.Add("Singleton 2", _randomServiceSingleton2.Value);
            result.Add("Scoped 2", _randomServiceScope2.Value);
            result.Add("Transient 2", _randomServiceTransient2.Value);

            return result;
        }
    }
}
