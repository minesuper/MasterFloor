using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MasterFloor.Pages
{
    /// <summary>
    /// Логика взаимодействия для PartnersPage.xaml
    /// </summary>
    public partial class PartnersPage : Page
    {
        private List<PartnersWithDiscount> currentPartners;

        public List<PartnersWithDiscount> CurrentPartners { get => currentPartners; set => currentPartners = value; }
        public PartnersPage()
        {
            InitializeComponent();
            PartnersListView.ItemsSource = Model.MasterFloorDBEntities.GetContext().Partners.ToList();
        }
    }
    public class PartnersWithDiscount : Model.Partners
    {
        public int Discount { get; set; }
        private List<PartnersWithDiscount> AddDiscount(List<Model.Partners> Partners)
        {
            List<PartnersWithDiscount> NewList = new List<PartnersWithDiscount>();
            foreach (var partner in Partners)
            {
                PartnersWithDiscount NewDiscount = new PartnersWithDiscount();
                NewDiscount.Streets = partner.Streets;
                NewDiscount.Discount = 14;
                NewDiscount.Regions = partner.Regions;
                NewDiscount.Cities = partner.Cities;
                NewDiscount.AdresIndex = partner.AdresIndex;
                NewDiscount.CityId = partner.CityId;
                NewDiscount.DirectorName = partner.DirectorName;
                NewDiscount.DirectorSurname = partner.DirectorSurname;
                NewDiscount.DirectorPatronym = partner.DirectorPatronym;
                NewDiscount.Rating = partner.Rating;
                NewDiscount.PartnerTypeId = partner.PartnerTypeId;
                NewDiscount.PartnerName = partner.PartnerName;
                NewDiscount.PartnerProducts = partner.PartnerProducts;
                NewDiscount.HouseNumber = partner.HouseNumber;
                NewDiscount.IndividualTaxNumber = partner.IndividualTaxNumber;
                NewDiscount.StreetId = partner.StreetId;
                NewDiscount.RegionId = partner.RegionId;
                NewDiscount.Phone = partner.Phone;
                NewDiscount.Id = partner.Id;
                NewDiscount.PartnersTypes = partner.PartnersTypes;
                NewList.Add(NewDiscount);
            }
            return NewList;
        }
        private int GetDiscount(Model.Partners partner)
        {
            int temp = Model.MasterFloorDBEntities.GetContext().PartnerProducts.
            var sale = (from p in Model.MasterFloorDBEntities.GetContext().PartnerProducts
                       let request = from c in Model.MasterFloorDBEntities.GetContext().Products
                                     where c.Id == p.ProductId
                                     select c.MinimalCost*p.Count
                                     where request.Any()
                                     select p).ToList();
            return sale.Count;
        }
    }
}
