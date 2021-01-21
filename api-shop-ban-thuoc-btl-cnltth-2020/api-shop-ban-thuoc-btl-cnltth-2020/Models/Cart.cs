using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api_shop_ban_thuoc_btl_cnltth_2020.Models
{
    [Serializable]
    public class CartItem
    {
        public SANPHAM Thuoc { get; set; }
        public int Quantity { set; get; }
        public int ThanhTien { set; get; }
    }
    public class Cart
    {
        private List<CartItem> lineCollection = new List<CartItem>();

        public void AddItem(SANPHAM sp)
        {
            CartItem line = lineCollection.Where(p => p.Thuoc.MaSP == sp.MaSP).FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartItem
                {
                    Thuoc = sp,
                    Quantity = 1, ThanhTien = (int) sp.GiaBan
                });
            }
            else
            {
                line.Quantity += 1;
                line.ThanhTien = (int) sp.GiaBan * line.Quantity;
                if (line.Quantity <= 0)
                {
                    lineCollection.RemoveAll(l => l.Thuoc.MaSP == sp.MaSP);
                }
            }
        }
        public void UpdateItem(SANPHAM sp, int quantity)
        {
            CartItem line = lineCollection.Where(p => p.Thuoc.MaSP == sp.MaSP).FirstOrDefault();

            if (line != null)
            {
                if (quantity > 0)
                {
                    line.Quantity = quantity;
                    line.ThanhTien = (int) sp.GiaBan * quantity;
                }
                else
                {
                    lineCollection.RemoveAll(l => l.Thuoc.MaSP == sp.MaSP);
                }
            }
        }
        public void RemoveLine(SANPHAM sp)
        {
            lineCollection.RemoveAll(l => l.Thuoc.MaSP == sp.MaSP);
        }

        public int? ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Thuoc.GiaBan * e.Quantity);

        }
        public int? ComputeTotalProduct()
        {
            return lineCollection.Sum(e => e.Quantity);

        }
        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartItem> Lines
        {
            get { return lineCollection; }
        }
    }
}