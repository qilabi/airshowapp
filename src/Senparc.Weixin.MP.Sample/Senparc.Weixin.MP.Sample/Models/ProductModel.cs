using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Senparc.Weixin.MP.Sample.Models
{
    /// <summary>
    /// 商品实体类
    /// </summary>
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal RealPrice { get; set; }
        public string ImgPath { get; set; }
        public string Desc { get; set; }

        public ProductModel()
        {
        }

        public ProductModel(int id, string name, decimal price,decimal realPrice)
        {
            Id = id;
            Name = name;
            Price = price;
            RealPrice = realPrice;
        }


        private static List<ProductModel> ProductList { get; set; }

        public static List<ProductModel> GetFakeProductList()
        {
            var list = ProductList ?? new List<ProductModel>()
            {
                //new ProductModel(1,"成人票(电子票)",(decimal)500.00,(decimal)490.00),
                //new ProductModel(2,"老人儿童学生(电子票)",(decimal)300.00,(decimal)270.00)
                new ProductModel(1,"成人票(电子票)",(decimal)500.00,(decimal)0.2),
                new ProductModel(2,"老人儿童学生(电子票)",(decimal)300.00,(decimal)0.1)
            };
            ProductList = ProductList ?? list;

            return list;
        }
    }
}