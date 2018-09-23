using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using LotteryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using static LotteryApi.Constants;

namespace LotteryApi.Models
{
    public class Ticket
    {
        public long Id { get; set; }
        public string LottoString { get; set; }
        public DateTime TimeStamp { get; set; }
        [NotMapped]
        public List<int> Lotto
        {
            get
            {
                if(string.IsNullOrEmpty(LottoString))
                {
                    return new List<int>();
                }

                return LottoString.Split(LottoConstants.DELIMITER).Select(Int32.Parse).ToList();
            }
            set { }           
         } 
    }
}

public class TicketValidator : AbstractValidator<Ticket>
{
    public TicketValidator()
    {
        RuleFor(x => x.LottoString).Must(ValidLotto).WithMessage("The lotto string is not in a valid format");
        RuleFor(x => x.Lotto).Must(ValidLottoCount).WithMessage("The amount of elements selected does not agree with the ticket.");
        RuleForEach(x => x.Lotto).InclusiveBetween(LottoConstants.MINORNUMBER, LottoConstants.MAXNUMBER);
        RuleFor(x => x.TimeStamp).Must(ValidDate).WithMessage("Timestamp is required");
    }

    protected bool ValidLotto(string lotto)
    {
        bool result = true;
        try
        {
            var numbers = lotto.Split(';').Select(Int32.Parse).ToList();    
        }
        catch
        {
            result = false;
        }

        return result;
    }

    protected bool ValidLottoCount(List<int> lotto)
    {
        if(lotto.Count != LottoConstants.SELECTEDNUMBERS)
        {
            return false;
        }

        return true;
    }

    private bool ValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}

