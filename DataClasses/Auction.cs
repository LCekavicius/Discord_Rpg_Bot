using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace KushBot
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }
        public ulong SellerId { get; set; }
        public string SellerName { get; set; }
        public string SellingItem { get; set; }
        public int ItemPrice { get; set; }

        public Auction(int id, ulong sellerId, string sellerName, string sellingItem, int itemPrice)
        {
            Id = id;
            SellerId = sellerId;
            SellerName = sellerName;
            SellingItem = sellingItem;
            ItemPrice = itemPrice;
        }

        
    }
}
