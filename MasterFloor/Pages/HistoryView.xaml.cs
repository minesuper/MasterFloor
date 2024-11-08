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
    /// Логика взаимодействия для HistoryView.xaml
    /// </summary>
    public partial class HistoryView : Page
    {
        public HistoryView(Model.Partners user)
        {
            InitializeComponent();
            var History = Model.MasterFloorDBEntities.GetContext().PartnerProducts.Where(d => d.PartnerId == user.Id).ToList();
            if (History.Count > 0)
            {
                HistoryListView.ItemsSource = History;
                HistoryListView.Visibility = Visibility.Visible;
                EmptyHistoryTB.Visibility = Visibility.Hidden;
            }
            
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if(Utils.Navigation.CurrentFrame.CanGoBack)
            {
                Utils.Navigation.CurrentFrame.GoBack();
            }
        }
    }
}
