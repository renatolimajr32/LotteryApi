using LoteriaEntities.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace LoteriaApi
{
    public class LottoContext : DbContext
    {
        public LottoContext(DbContextOptions<LottoContext> options) : base(options) { }
        public DbSet<Aposta> Aposta { get; set; }
        public DbSet<Concurso> Concurso { get; set; }

        //public void MarkAsModified(Ticket item)
        //{
        //    throw new System.NotImplementedException();
        //}

        //public void MarkAsModified(DrawnNumber item)
        //{
        //    throw new System.NotImplementedException();
        //}
    }

    //public class ValidatorActionFilter : IActionFilter
    //{
    //    public void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        if (!filterContext.ModelState.IsValid)
    //        {
    //            filterContext.Result = new BadRequestObjectResult(filterContext.ModelState);
    //        }
    //    }

    //    public void OnActionExecuted(ActionExecutedContext filterContext)
    //    {

    //    }
    //}

}
