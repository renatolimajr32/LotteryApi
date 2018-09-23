using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static LotteryApi.Constants;

namespace LotteryApi.Models
{
    public class DrawnNumber
    {
        public long Id { get; set; }
        [NotMapped]
        public List<int> Numbers
        {
            get
            {
                if (string.IsNullOrEmpty(NumbersString))
                {
                    return new List<int>();
                }

                return NumbersString.Split(LottoConstants.DELIMITER).Select(Int32.Parse).ToList();
            }
            set { }
        }
        public string NumbersString { get; set; }
    }
}
