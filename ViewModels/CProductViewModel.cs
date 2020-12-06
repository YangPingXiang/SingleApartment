using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using sln_SingleApartment.Models;
namespace sln_SingleApartment.ViewModels
{
    public class CProductViewModel
    {
        [JsonIgnore]
        public Product entity { get; set; }
        [DisplayName("產品編號")]

        public int ProductID { get { return entity.ProductID; } }
        [DisplayName("子類別編號")]
        [Required(ErrorMessage = "請輸入子類別")]
        public int ProductSubCategoryID { get { return entity.ProductSubCategoryID; } }

        [DisplayName("產品名稱")]
        [Required(ErrorMessage = "請輸入產品名稱")]
        public string ProductName { get { return entity.ProductName; } }

        [DisplayName("產品單價")]
        [Required(ErrorMessage = "請輸入產品價格")]
        public Nullable<int> UnitPrice { get { return entity.UnitPrice; } }

        [DisplayName("產品停產")]
        public string Discontinued { get { return entity.Discontinued; } }

        [DisplayName("產品銷量")]
        public Nullable<int> Sales { get { return entity.Sales; } }

        [DisplayName("庫存")]
        [Required(ErrorMessage = "請輸入庫存")]
        public int Stock { get { return entity.Stock; } }

        [DisplayName("產品敘述")]
        [Required(ErrorMessage = "請輸入產品敘述")]
        public string Description { get { return entity.Description; } }
        //我需要的
        [DisplayName("商品子類別編號")]
        public int SubCategoryID { get { return entity.ProductSubCategory.ProductSubCategoryID; } }

        [DisplayName("商品子類別")]
        public string SubCategoryName { get { return entity.ProductSubCategory.ProductSubCategoryName; } }

        [DisplayName("商品主類別編號")]
        public int MainCategoryID { get { return entity.ProductSubCategory.ProductMainCategoryID; } }
        [DisplayName("商品主類別")]
        public string MainCategoryName { get { return entity.ProductSubCategory.ProductMainCategory.ProductMainCategoryName; } }
    }

    public class CProductMainCategoryViewModel
    {
        public ProductMainCategory entity_MainCategory { get; set; }
        public int ProductMainCategoryID { get { return entity_MainCategory.ProductMainCategoryID; } }
        public string ProductMainCategoryName { get { return entity_MainCategory.ProductMainCategoryName; } }
        public List<CProductSubCategoryViewModel> SubCategoryViewModels { get; set; }
        public int ProductCount { get; set; }
    }
    public class CProductSubCategoryViewModel
    {
        public ProductSubCategory entity_SubCategory { get; set; }
        public int ProductSubCategoryID { get { return entity_SubCategory.ProductSubCategoryID; } }
        public string ProductSubCategoryName { get { return entity_SubCategory.ProductSubCategoryName; } }
        public int ProductMainCategoryID { get { return entity_SubCategory.ProductMainCategoryID; } }
        public int ProductCount { get; set; }
    }

    public class ShopViewModel
    {
        public List<CProductViewModel> product { get; set; }
        public List<CProductMainCategoryViewModel> MainCategory { get; set; }
        public List<CProductSubCategoryViewModel> SubCategory { get; set; }

        
           
    }
}