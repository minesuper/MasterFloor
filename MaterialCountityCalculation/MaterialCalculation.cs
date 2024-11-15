using System;
using System.Linq;

namespace MaterialCountityCalculation
{
    public static class MaterialCalculation
    {
        public static int CountityMaterialCalc(int IdProductType, int IdMaterialType, int ResultProductCount, decimal par1, decimal par2)
        {
            var Context = MasterFloor.Model.MasterFloorDBEntities.GetContext();
            if (Context.ProductTypes
                .Any(i => IdProductType == i.Id) 
                && Context.MaterialTypes
                .Any(i => IdMaterialType == i.Id) 
                && ResultProductCount > 0 
                && par1 >= 0
                && par2 >= 0)
            {
                return Convert.ToInt32(Math.Ceiling(par1 * par2 * ResultProductCount * Context.ProductTypes
                    .Where(i => i.Id == IdProductType).FirstOrDefault().ProductMultiply * (Context.MaterialTypes
                    .Where(i => i.Id == IdMaterialType).FirstOrDefault().DefectPercent + 1)));
            }
            return -1;
        }
    }
}
