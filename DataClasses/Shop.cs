using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace KushBot
{
    public class Shop
    {
        [Key]
        public int BottomLevel { get; set; }
        public int TopLevel { get; set; }

        public int AmountOfLimitedItems { get; set;}

        public string NoLimitItems { get; set; }
        public string LimitedItems { get; set; }

        public DateTime Refilled { get; set; }

        public Shop()
        {
            //Refilled = DateTime.Now.AddHours(-8);
        }
    }
}
