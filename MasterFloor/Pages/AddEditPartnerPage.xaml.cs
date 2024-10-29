using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для AddEditPartnerPage.xaml
    /// </summary>
    public partial class AddEditPartnerPage : Page
    {
        private bool isAddFlag = false;
        private Model.Partners currentPartner { get; set; }

        public bool IsAddFlag
        {
            get { return isAddFlag; }
            set { isAddFlag = value; }
        }

        public AddEditPartnerPage(PartnersWithDiscount SelectedPartner)
        {
            InitializeComponent();
            OnStart(SelectedPartner);
            DataContext = currentPartner;
        }
        private void OnStart(PartnersWithDiscount SelectedPartner)
        {
            TypeCB.ItemsSource = Model.MasterFloorDBEntities1.GetContext().PartnersTypes.ToList();
            if (SelectedPartner == null)
            {
                IsAddFlag = true;
                currentPartner = new Model.Partners();
            }
            else
            {
                currentPartner = Model.MasterFloorDBEntities1.GetContext().Partners.Where(d => d.Id == SelectedPartner.Id).First();
                NameTB.Text = currentPartner.PartnerName;
                RatingTB.Text = currentPartner.Rating.ToString();
                AdressTB.Text = $"{currentPartner.AdresIndex},{currentPartner.Regions.Name},{currentPartner.Cities.Name},{currentPartner.Streets.Name},{currentPartner.HouseNumber}";
                FIOTB.Text = $"{currentPartner.DirectorSurname} {currentPartner.DirectorName} {currentPartner.DirectorPatronym}";
                PhoneTB.Text = currentPartner.Phone;
                EmailTB.Text = currentPartner.Email;
                TypeCB.SelectedIndex = currentPartner.PartnerTypeId - 1;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Utils.Navigation.CurrentFrame.CanGoBack)
            {
                Utils.Navigation.CurrentFrame.GoBack();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();
                string title = NameTB.Text;
                string rating = RatingTB.Text;
                string phone = PhoneTB.Text;
                string email = EmailTB.Text;
                string Adress = AdressTB.Text;
                string fio = FIOTB.Text;
                int typeId = TypeCB.SelectedIndex + 1;
                if (string.IsNullOrEmpty(title)) { errors.AppendLine("Заполните название партнера"); }
                if (string.IsNullOrEmpty(rating)) { errors.AppendLine("Заполните рейтинг партнера"); }
                else
                {
                    if (!int.TryParse(rating, out var res))
                    {
                        errors.AppendLine("Рейтинг - целое число");
                    }
                    else if (int.Parse(rating) < 0)
                    {
                        errors.AppendLine("Рейтинг - неотрицательное число");
                    }
                }
                if (string.IsNullOrEmpty(phone)) { errors.AppendLine("Заполните телефон партнера"); }
                else
                {
                    if (!CheckPhonePattern(phone))
                    {
                        errors.AppendLine("Телефон не по паттерну +7(###)-###-##-##");
                    }
                }
                if (string.IsNullOrEmpty(email)) { errors.AppendLine("Заполните email партнера"); }
                if (string.IsNullOrEmpty(Adress)) { errors.AppendLine("Заполните адрес партнера"); }
                if (string.IsNullOrEmpty(fio)) { errors.AppendLine("Заполните ФИО партнера"); }
                if (typeId < 1) { errors.AppendLine("Выберите тип партнера"); }
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (isAddFlag)
                {
                    Model.Partners NewPartner = new Model.Partners();

                    NewPartner.PartnerName = title;
                    NewPartner.DirectorName = fio.Split(' ')[1];
                    NewPartner.DirectorSurname = fio.Split(' ')[0];
                    NewPartner.DirectorPatronym = fio.Split(' ')[2];
                    NewPartner.Email = email;
                    NewPartner.PartnerTypeId = typeId;
                    NewPartner.Phone = phone;
                    NewPartner.IndividualTaxNumber = null;
                    NewPartner.Rating = Convert.ToInt32(rating);
                    string[] adressTemp = Adress.Split(',');
                    NewPartner.AdresIndex = adressTemp[0];
                    var regionTemp = Model.MasterFloorDBEntities1.GetContext().Regions.ToList().Where(x => x.Name == adressTemp[1]).FirstOrDefault();
                    if(regionTemp == null)
                    {
                        Model.MasterFloorDBEntities1.GetContext().Regions.Add(new Model.Regions() { Name = adressTemp[1] });
                        Model.MasterFloorDBEntities1.GetContext().SaveChanges();
                        NewPartner.RegionId = Model.MasterFloorDBEntities1.GetContext().Regions.ToList().Where(x => x.Name == adressTemp[1]).First().Id;
                    }
                    var cityTemp = Model.MasterFloorDBEntities1.GetContext().Cities.ToList().Where(x => x.Name == adressTemp[2]).FirstOrDefault();
                    if (cityTemp == null)
                    {
                        Model.MasterFloorDBEntities1.GetContext().Cities.Add(new Model.Cities() { Name = adressTemp[2] });
                        Model.MasterFloorDBEntities1.GetContext().SaveChanges();
                        NewPartner.CityId = Model.MasterFloorDBEntities1.GetContext().Cities.ToList().Where(x => x.Name == adressTemp[2]).First().Id;
                    }
                    var streetTemp = Model.MasterFloorDBEntities1.GetContext().Streets.ToList().Where(x => x.Name == adressTemp[3]).FirstOrDefault();
                    if (streetTemp == null)
                    {
                        Model.MasterFloorDBEntities1.GetContext().Streets.Add(new Model.Streets() { Name = adressTemp[3] });
                        Model.MasterFloorDBEntities1.GetContext().SaveChanges();
                        NewPartner.StreetId = Model.MasterFloorDBEntities1.GetContext().Streets.ToList().Where(x => x.Name == adressTemp[3]).First().Id;
                    }
                    NewPartner.StreetId = Model.MasterFloorDBEntities1.GetContext().Streets.ToList().Where(x => x.Name == adressTemp[3]).FirstOrDefault().Id;
                    NewPartner.HouseNumber = Convert.ToInt32(adressTemp[4]);

                    Model.MasterFloorDBEntities1.GetContext().Partners.Add(NewPartner);
                    Model.MasterFloorDBEntities1.GetContext().SaveChanges();
                }
                else
                {
                    currentPartner.DirectorName = fio.Split(' ')[1];
                    currentPartner.DirectorSurname = fio.Split(' ')[0];
                    currentPartner.DirectorPatronym = fio.Split(' ')[2];
                    currentPartner.Email = email;
                    currentPartner.PartnerTypeId = typeId;
                    currentPartner.Phone = phone;
                    currentPartner.IndividualTaxNumber = null;
                    currentPartner.Rating = Convert.ToInt32(rating);
                    var adressTemp = Adress.Split(',');
                    currentPartner.AdresIndex = adressTemp[0];
                    var regionTemp = Model.MasterFloorDBEntities1.GetContext().Regions.ToList().Where(x => x.Name == adressTemp[1]).FirstOrDefault();
                    if (regionTemp == null)
                    {
                        Model.MasterFloorDBEntities1.GetContext().Regions.Add(new Model.Regions() { Name = adressTemp[1] });
                        Model.MasterFloorDBEntities1.GetContext().SaveChanges();
                        currentPartner.RegionId = Model.MasterFloorDBEntities1.GetContext().Regions.ToList().Where(x => x.Name == adressTemp[1]).First().Id;
                    }
                    else
                    {
                        currentPartner.RegionId = regionTemp.Id;
                    }
                    var cityTemp = Model.MasterFloorDBEntities1.GetContext().Cities.ToList().Where(x => x.Name == adressTemp[2]).FirstOrDefault();
                    if (cityTemp == null)
                    {
                        Model.MasterFloorDBEntities1.GetContext().Cities.Add(new Model.Cities() { Name = adressTemp[2] });
                        Model.MasterFloorDBEntities1.GetContext().SaveChanges();
                        currentPartner.CityId = Model.MasterFloorDBEntities1.GetContext().Cities.ToList().Where(x => x.Name == adressTemp[2]).First().Id;
                    }
                    else
                    {
                        currentPartner.CityId = cityTemp.Id;
                    }
                    var streetTemp = Model.MasterFloorDBEntities1.GetContext().Streets.ToList().Where(x => x.Name == adressTemp[3]).FirstOrDefault();
                    if (streetTemp == null)
                    {
                        Model.MasterFloorDBEntities1.GetContext().Streets.Add(new Model.Streets() { Name = adressTemp[3] });
                        Model.MasterFloorDBEntities1.GetContext().SaveChanges();
                        currentPartner.StreetId = Model.MasterFloorDBEntities1.GetContext().Streets.ToList().Where(x => x.Name == adressTemp[3]).First().Id;
                    }
                    else
                    {
                        currentPartner.StreetId = streetTemp.Id;
                    }
                    currentPartner.HouseNumber = Convert.ToInt32(adressTemp[4]);
                }

                Model.MasterFloorDBEntities1.GetContext().SaveChanges();
                Utils.Navigation.CurrentFrame.RemoveBackEntry();
                Utils.Navigation.CurrentFrame.Navigate(new Pages.PartnersPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private bool CheckPhonePattern(string phone)
        {
            return Regex.IsMatch(phone, @"^\+7\(\d{3}\)-\d{3}-\d{2}-\d{2}$");
        }
    }
}
