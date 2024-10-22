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
    /// Логика взаимодействия для AddEditPartnerPage.xaml
    /// </summary>
    public partial class AddEditPartnerPage : Page
    {
        private bool isAddFlag = false;
        private Model.Partners newPartner { get; set; }

        public bool IsAddFlag
        {
            get { return isAddFlag; }
            set { isAddFlag = value; }
        }

        public AddEditPartnerPage(Model.Partners SelectedPartner)
        {
            InitializeComponent();
            OnStart(SelectedPartner);
        }
        private void OnStart(Model.Partners SelectedPartner)
        {
            TypeCB.ItemsSource = Model.MasterFloorDBEntities.GetContext().PartnersTypes.ToList();
            if (SelectedPartner == null)
            {
                IsAddFlag = true;
                newPartner = new Model.Partners();
            }
            else
            {
                newPartner = SelectedPartner;
                TitleTB.Text = newPartner.PartnerName;
                RatingTB.Text = newPartner.Rating.ToString();
                AdressTB.Text = $"{newPartner.Regions.Name}, г. {newPartner.Cities.Name}, ул. {newPartner.Streets.Name}, д. {newPartner.HouseNumber}";
                FIOTB.Text = $"{newPartner.DirectorSurname} {newPartner.DirectorName} {newPartner.DirectorPatronym}";
                PhoneTB.Text = newPartner.Phone;
                EmailTB.Text = newPartner.Email;
                TypeCB.SelectedIndex = newPartner.PartnerTypeId - 1;
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
                string title = TitleTB.Text;
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
                if (string.IsNullOrEmpty(email)) { errors.AppendLine("Заполните email партнера"); }
                if (string.IsNullOrEmpty(Adress)) { errors.AppendLine("Заполните адрес партнера"); }
                if (string.IsNullOrEmpty(fio)) { errors.AppendLine("Заполните ФИО партнера"); }
                if (typeId < 1) { errors.AppendLine("Выберите тип партнера"); }
                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
