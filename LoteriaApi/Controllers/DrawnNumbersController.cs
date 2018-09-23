using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LotteryApi.Models;
using Microsoft.AspNetCore.Mvc;
using static LotteryApi.Constants;

namespace LotteryApi.Controllers
{
    [Route("api/drawn")]
    [ApiController]
    public class DrawnNumbersController : ControllerBase
    {
        private readonly LottoContext _context;
        private Random _rand = new Random();

        public DrawnNumbersController(LottoContext context)
        {
            _context = context;   
        }

        [HttpGet]
        public ActionResult<DrawnNumber> Create()
        {            
            var numbers = GenerateDrawnNumbers();

            _context.DrawnNumbers.Add(numbers);
            _context.SaveChanges();

            return CreatedAtRoute("GetDrawnNumbers", new { id = numbers.Id }, numbers);
        }

        [HttpGet("{id}", Name = "GetDrawnNumbers")]
        public ActionResult<DrawnNumber> GetById(long id)
        {
            var item = _context.DrawnNumbers.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        public DrawnNumber GenerateDrawnNumbers()
        {
            DrawnNumber numbers = new DrawnNumber();            
            var list = Enumerable.Range(LottoConstants.MINORNUMBER, LottoConstants.MAXNUMBER).OrderBy(x => _rand.Next(LottoConstants.MINORNUMBER, LottoConstants.MAXNUMBER)).Take(LottoConstants.SELECTEDNUMBERS).ToList();

            foreach(int n in list)
            {
                numbers.NumbersString +=  n.ToString() + LottoConstants.DELIMITER;
            }
            numbers.NumbersString = numbers.NumbersString.Remove(numbers.NumbersString.Length - 1);
            return numbers;
        }    
    }
}