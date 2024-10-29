using MasterFloor.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
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
            List<PartnersWithDiscount> list = new List<PartnersWithDiscount>();
            foreach (Model.Partners item in Model.MasterFloorDBEntities1.GetContext().Partners.ToList())
            {
                list.Add(new PartnersWithDiscount(item));
            }
            PartnersListView.ItemsSource = list;
        }

        private void EditThisPartner_Click(object sender, RoutedEventArgs e)
        {
            Utils.Navigation.CurrentFrame.Navigate(new Pages.AddEditPartnerPage((sender as Button).DataContext as PartnersWithDiscount));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Utils.Navigation.CurrentFrame.Navigate(new Pages.AddEditPartnerPage(null));
        }

        private void PartnersListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult NotificationOnPreviewHistory = MessageBox.Show("Посмотреть историю реализации продукции?", "Уведомление!", MessageBoxButton.YesNo, MessageBoxImage.Information);
            if(NotificationOnPreviewHistory == MessageBoxResult.No)
            {
                return;
            }
            else
            {
                Utils.Navigation.CurrentFrame.Navigate(new Pages.HistoryView((sender as ListViewItem).DataContext as PartnersWithDiscount));
            }
        }
    }
    public class PartnersWithDiscount : Model.Partners
    {
        public decimal Discount { get; set; }

        public PartnersWithDiscount(Model.Partners partner)
        {
            this.Streets = partner.Streets;
            this.Discount = GetDiscount(partner);
            this.Regions = partner.Regions;
            this.Cities = partner.Cities;
            this.AdresIndex = partner.AdresIndex;
            this.CityId = partner.CityId;
            this.DirectorName = partner.DirectorName;
            this.DirectorSurname = partner.DirectorSurname;
            this.DirectorPatronym = partner.DirectorPatronym;
            this.Rating = partner.Rating;
            this.PartnerTypeId = partner.PartnerTypeId;
            this.PartnerName = partner.PartnerName;
            this.PartnerProducts = partner.PartnerProducts;
            this.HouseNumber = partner.HouseNumber;
            this.IndividualTaxNumber = partner.IndividualTaxNumber;
            this.StreetId = partner.StreetId;
            this.RegionId = partner.RegionId;
            this.Phone = partner.Phone;
            this.Id = partner.Id;
            this.PartnersTypes = partner.PartnersTypes;
            this.Email = partner.Email;
        }
        private int GetDiscount(Model.Partners partner)
        {
            decimal? totalCost =
                (from p in Model.MasterFloorDBEntities1.GetContext().Partners
                 join pp in Model.MasterFloorDBEntities1.GetContext().PartnerProducts on p.Id equals pp.PartnerId into ppGroup
                 from pp in ppGroup.DefaultIfEmpty()
                 join pr in Model.MasterFloorDBEntities1.GetContext().Products on pp.ProductId equals pr.Id into prGroup
                 from pr in prGroup.DefaultIfEmpty()
                 where p.PartnerName == partner.PartnerName
                 select (decimal?)(pp.Count) * (decimal?)(pr.MinimalCost))
                 .Sum();
            if (totalCost >= 10000 && totalCost < 50000)
            {
                return 5;
            }
            else if (totalCost >= 50000 && totalCost < 300000)
            {
                return 10;
            }
            else if (totalCost >= 300000)
            {
                return 15;
            }

            return 0;
        }
    }
}
