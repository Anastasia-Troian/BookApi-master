using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Dto.ResultDto
{
    public class ResultLoginDto : ResultDto
    {
        public string Token { get; set; }
    }
}
