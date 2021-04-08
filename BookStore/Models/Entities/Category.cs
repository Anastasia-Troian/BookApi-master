using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /*Navigation Properties*/
        public ICollection<Book> Books { get; set; }
    }
}
