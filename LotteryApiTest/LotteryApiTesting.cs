using Xunit;
using LotteryApi.Models;
using LotteryApi.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Testing;
using static LotteryApi.Constants;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace LotteryApiTest
{
    public class UnitTest1
    {
        [Fact]
        public void RandomTicket()
        {
            var _dbContext = getContext();

            TicketController ticketController = new TicketController(_dbContext);

            ticketController.GenerateRandomTicket();
            ticketController.GenerateRandomTicket();
            ticketController.GenerateRandomTicket();

            var list =  _dbContext.Tickets.ToList();

            Assert.Equal(3, list.Count);
        }

        [Fact]
        public void DrawnNumbers()
        {
            var _dbContext = getContext();

            DrawnNumbersController drawnController = new DrawnNumbersController(_dbContext);

            drawnController.Create();
            var drawn = _dbContext.DrawnNumbers.Find((long)1);

            Assert.Equal(drawn.Numbers.Count, LotteryApi.Constants.LottoConstants.SELECTEDNUMBERS);
        }

        [Fact]
        public void CheckWinners()
        {
            var _dbContext = getContext();

            DrawnNumbersController drawnController = new DrawnNumbersController(_dbContext);
            TicketController ticketController = new TicketController(_dbContext);

            _dbContext.Add(new Ticket { Id = 1, LottoString = "1;2;3;4;5;6", TimeStamp = DateTime.Now });
            _dbContext.Add(new Ticket { Id = 2, LottoString = "1;56;43;57;5;6", TimeStamp = DateTime.Now });
            _dbContext.Add(new Ticket { Id = 3, LottoString = "1;21;33;44;5;6", TimeStamp = DateTime.Now });

            _dbContext.Add(new DrawnNumber { Id = 1, NumbersString = "1;2;3;4;5;6"});
            _dbContext.SaveChanges();

            var winners = ticketController.CalculateWinners(1, 6);

            Assert.Single(winners);
            Assert.Equal(1, winners[0].Id);
        }

        public LottoContext getContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<LottoContext>();
            optionsBuilder.UseInMemoryDatabase();
            var _dbContext = new LottoContext(optionsBuilder.Options);

            return _dbContext;
        }
    }
}
