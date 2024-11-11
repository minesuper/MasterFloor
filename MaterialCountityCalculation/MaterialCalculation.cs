using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialCountityCalculation
{
    public static class MaterialCalculation
    {
        public static int CountityMaterialCalc(int IdProductType, int IdMaterialType, int ResultProductCount)
        {
            var Context = MasterFloor.Model.MasterFloorDBEntities.GetContext();
            if (Context.Products.Any(i => IdProductType == i.Id) && Context.ProductTypes.Any(i => IdMaterialType == i.Id) && ResultProductCount > 0)
            {
                var Product = Context.Products.Where(i => IdProductType == i.Id).FirstOrDefault();
                return Convert.ToInt32(Math.Ceiling(Product.ProductTypes.ProductMultiply * ResultProductCount * (Product.MaterialTypes.DefectPercent+1)));
            }
            return -1;
        }
    }
}
