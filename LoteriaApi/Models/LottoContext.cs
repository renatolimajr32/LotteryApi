using LotteryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace LotteryApi.Models
{
    public class LottoContext: DbContext
    {
        public LottoContext(DbContextOptions<LottoContext> options): base(options) {}
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<DrawnNumber> DrawnNumbers { get; set; }

        public void MarkAsModified(Ticket item)
        {
            throw new System.NotImplementedException();
        }

        public void MarkAsModified(DrawnNumber item)
        {
            throw new System.NotImplementedException();
        }
    }

    public class ValidatorActionFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}