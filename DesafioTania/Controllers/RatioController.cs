using DesafioTania.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatioController : ControllerBase
    {
        private readonly DataContext _context;

        public RatioController(DataContext context)
        {
            _context = context;
        }
        private string CalculateRatio(Ratio numbers)
        {
            List<int> nums = numbers.Input.TrimEnd().Split(' ').ToList().Select(x => Convert.ToInt32(x)).ToList();

            decimal quantity = nums.Count(), positive= 0, zeros=0, negatives=0;

            foreach (var item in nums)
            {
                if (item > 0)
                    positive++;
                else if (item == 0)
                    zeros++;
                else if(item < 0)   
                    negatives++;
            }

            return $"{(positive / nums.Count).ToString("N6")} {(negatives / nums.Count).ToString("N6")} {(zeros / nums.Count).ToString("N6")}";
        }

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {
            Ratio ratio = new();
            try
            {
                if (ratio.Id == System.Guid.Empty)
                    return BadRequest();

                ratio.Input = input;    
                ratio.Output = CalculateRatio(ratio).ToString();
                _context.Ratio.Add(ratio);
                _context.SaveChanges();

                return Ok(ratio);
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
                var result = _context.Ratio.ToList().OrderByDescending(x => x.Date);

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
                var result = _context.Ratio.FirstOrDefault(x => x.Id == id);

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
                var ratio = _context.Ratio.FirstOrDefault(w => w.Id == id);
                _context.Ratio.Remove(ratio);
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
