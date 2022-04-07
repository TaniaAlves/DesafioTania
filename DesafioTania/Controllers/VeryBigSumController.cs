using DesafioTania.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeryBigSumController : ControllerBase
    {
        private readonly DataContext _context;
       

        public VeryBigSumController(DataContext context)
        {
            _context = context;
        }
        private static long Sum (VeryBigSum numbers)
        {
            List<long> sums = numbers.Input.TrimEnd().Split(' ').ToList().Select(x => Convert.ToInt64(x)).ToList();
            long result = sums.Sum();
            return result;
        }

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {
            VeryBigSum veryBigSum = new();
            try
            {
                if (veryBigSum.Id == System.Guid.Empty)
                    return BadRequest();

                veryBigSum.Input = input;
                veryBigSum.Output = Sum(veryBigSum).ToString();
                _context.VeryBigSum.Add(veryBigSum);
                _context.SaveChanges();

                return Ok(veryBigSum);
            }
            catch (Exception)
            {
                return BadRequest();
            }
            
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var result = _context.VeryBigSum.ToList().OrderByDescending(x => x.Date);

                if (result.Count() == 0)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            try
            {
                var result = _context.VeryBigSum.FirstOrDefault(x => x.Id == id);
                
                if (result == null)
                    return NoContent();
                
                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest();
            }
           
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            if (id == System.Guid.Empty)
                return BadRequest();
            try
            {
                var veryBigSum = _context.VeryBigSum.FirstOrDefault(w => w.Id == id);
                _context.VeryBigSum.Remove(veryBigSum);
                _context.SaveChanges();
                return Ok(true);
            }
            catch (Exception)
            {
                return NoContent();
            }
 
        }
    }
}
