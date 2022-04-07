using DesafioTania.Models;
using Microsoft.AspNetCore.Mvc;

namespace DesafioTania.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagonalSumController : ControllerBase
    {
        private readonly DataContext _context;

        public DiagonalSumController(DataContext context)
        {
            _context = context;
        }
        private long Sum(List<List<int>> listList)
        {
            int leftDiag = 0, rightDiag = 0;

            for (int i = 0; i < listList.Count; i++)
            {
                leftDiag += listList[i][i];
                rightDiag += listList[i][listList.Count - i - 1];
            }
            return Math.Abs(rightDiag - leftDiag);

        }

        [HttpPost]
        public IActionResult Post([FromBody] string input)
        {
            DiagonalSum diagonalSum = new();
            List<string> list = new();
            List<List<int>> arr = new();

            if (diagonalSum.Id == System.Guid.Empty)
            {
                return BadRequest();
            }

            try
            {
                list = input.TrimEnd().Split(',').ToList(); //separa em 3
                list.ForEach(x => arr.Add(x.TrimEnd().Split(' ').ToList().Select(b => Convert.ToInt32(b)).ToList())); //converte a list para int

                diagonalSum.Input = input;
                diagonalSum.Output = Sum(arr).ToString();
                _context.DiagonalSum.Add(diagonalSum);
                _context.SaveChanges();

                return Ok(diagonalSum);
                
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
                var result = _context.DiagonalSum.ToList().OrderByDescending(x => x.Date);

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
                var result = _context.DiagonalSum.FirstOrDefault(x => x.Id == id);

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
                var diagonalSum = _context.DiagonalSum.FirstOrDefault(w => w.Id == id);
                _context.DiagonalSum.Remove(diagonalSum);
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
