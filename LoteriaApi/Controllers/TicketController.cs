using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using LotteryApi.Models;
using System;
using FluentValidation;
using static LotteryApi.Constants;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Controllers
{
    [Route("api/ticket")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        public  LottoContext _context;
        private Random _rand = new Random();

        public TicketController(LottoContext context)
        {
            _context = context;       
        }

        [HttpGet]
        public ActionResult<List<Ticket>> GetAll()
        {
            return _context.Tickets.ToList();
        }

        [HttpGet("{id}", Name = "GetTicket")]
        public ActionResult<Ticket> GetById(long id)
        {
            var item = _context.Tickets.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return item;
        }

        [HttpPost]
        public IActionResult Create(Ticket item)
        {        
            _context.Tickets.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetTicket", new { id = item.Id }, item);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();
            return NoContent();
        }

        [Route("winners/{drawn:long}/{hits:int}")]
        public ActionResult<List<Ticket>> GetWinners(long drawn, int hits)
        {
            var winners = CalculateWinners(drawn, hits);

            if(!winners.Any())
            {
                return NotFound();
            }

            return winners;
        }

        [Route("randomTicket")]    
        public ActionResult<Ticket> GenerateRandomTicket()
        {
            Ticket ticket = new Ticket();
            var list = Enumerable.Range(LottoConstants.MINORNUMBER, LottoConstants.MAXNUMBER).OrderBy(x => _rand.Next(LottoConstants.MINORNUMBER, LottoConstants.MAXNUMBER)).Take(LottoConstants.SELECTEDNUMBERS).ToList();

            foreach (int n in list)
            {
                ticket.LottoString += n.ToString() + LottoConstants.DELIMITER;
            }

            ticket.LottoString = ticket.LottoString.Remove(ticket.LottoString.Length - 1);
            ticket.TimeStamp = DateTime.Now;

            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return CreatedAtRoute("GetTicket", new { id = ticket.Id }, ticket);           
        }

        public List<Ticket> CalculateWinners(long drawn, int hits)
        {
            List<Ticket> winners = new List<Ticket>();

            var selectedDrawn = _context.DrawnNumbers.Find(drawn);

            if (selectedDrawn == null)
            {
                return winners;
            }

            foreach (Ticket ticket in _context.Tickets)
            {
                var result = ticket.Lotto.Select(i => i).Intersect(selectedDrawn.Numbers);
                if (result.ToList().Count == hits)
                {
                    winners.Add(ticket);
                }
            }

            return winners;
        }
    }
}