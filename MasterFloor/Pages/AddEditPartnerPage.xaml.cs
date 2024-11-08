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
            TypeCB.ItemsSource = Model.MasterFloorDBEntities.GetContext().PartnersTypes.ToList();
            if (SelectedPartner == null)
            {
                IsAddFlag = true;
                currentPartner = new Model.Partners();
            }
            else
            {
                currentPartner = Model.MasterFloorDBEntities.GetContext().Partners.Where(d => d.Id == SelectedPartner.Id).First();
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
                else
                {
                    if (Adress.Split(',').Length != 5)
                    {
                        errors.AppendLine("Адрес не по формату (Индекс,регион,город,улица,номер дома)");
                    }
                    else if (!Int32.TryParse(Adress.Split(',')[4].Trim(), out var res))
                    {
                        errors.AppendLine("Номер дома - целое число");
                    }
                }
                if (string.IsNullOrEmpty(fio)) { errors.AppendLine("Заполните ФИО партнера"); }
                else
                {
                    if (fio.Split(' ').Length != 3)
                    {
                        errors.AppendLine("ФИО заполнено неправльно");
                    }
                }
                if (typeId < 1) { errors.AppendLine("Выберите тип партнера"); }
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                currentPartner.DirectorName = fio.Split(' ')[1].Trim();
                currentPartner.DirectorSurname = fio.Split(' ')[0].Trim();
                currentPartner.DirectorPatronym = fio.Split(' ')[2].Trim();
                currentPartner.Email = email;
                currentPartner.PartnerTypeId = typeId;
                currentPartner.Phone = phone;
                currentPartner.IndividualTaxNumber = null;
                currentPartner.Rating = Convert.ToInt32(rating);
                var adressTemp = Adress.Split(',');
                currentPartner.AdresIndex = adressTemp[0];
                var regionTemp = Model.MasterFloorDBEntities.GetContext().Regions.ToList()
                    .Where(x => x.Name == adressTemp[1].Trim()).FirstOrDefault();
                if (regionTemp == null)
                {
                    Model.MasterFloorDBEntities.GetContext().Regions.Add(new Model.Regions() { Name = adressTemp[1].Trim() });
                    Model.MasterFloorDBEntities.GetContext().SaveChanges();
                    currentPartner.RegionId = Model.MasterFloorDBEntities.GetContext().Regions.ToList()
                        .Where(x => x.Name == adressTemp[1].Trim()).First().Id;
                }
                else
                {
                    currentPartner.RegionId = regionTemp.Id;
                }
                var cityTemp = Model.MasterFloorDBEntities.GetContext().Cities.ToList()
                    .Where(x => x.Name == adressTemp[2].Trim()).FirstOrDefault();
                if (cityTemp == null)
                {
                    Model.MasterFloorDBEntities.GetContext().Cities.Add(new Model.Cities() { Name = adressTemp[2].Trim() });
                    Model.MasterFloorDBEntities.GetContext().SaveChanges();
                    currentPartner.CityId = Model.MasterFloorDBEntities.GetContext().Cities.ToList()
                        .Where(x => x.Name == adressTemp[2].Trim()).First().Id;
                }
                else
                {
                    currentPartner.CityId = cityTemp.Id;
                }
                var streetTemp = Model.MasterFloorDBEntities.GetContext().Streets.ToList()
                    .Where(x => x.Name == adressTemp[3].Trim()).FirstOrDefault();
                if (streetTemp == null)
                {
                    Model.MasterFloorDBEntities.GetContext().Streets.Add(new Model.Streets() { Name = adressTemp[3].Trim() });
                    Model.MasterFloorDBEntities.GetContext().SaveChanges();
                    currentPartner.StreetId = Model.MasterFloorDBEntities.GetContext().Streets.ToList()
                        .Where(x => x.Name == adressTemp[3].Trim()).First().Id;
                }
                else
                {
                    currentPartner.StreetId = streetTemp.Id;
                }
                currentPartner.HouseNumber = Convert.ToInt32(adressTemp[4].Trim());
                if (isAddFlag)
                {
                    Model.MasterFloorDBEntities.GetContext().Partners.Add(currentPartner);
                    Model.MasterFloorDBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Успешно добавлен новый партнер!", "Информация!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Model.MasterFloorDBEntities.GetContext().SaveChanges();
                    MessageBox.Show("Партнер успешно отредактирован!", "Информация!", MessageBoxButton.OK, MessageBoxImage.Information);
                }

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
            return Regex.IsMatch(phone, @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$");
        }
    }
}
