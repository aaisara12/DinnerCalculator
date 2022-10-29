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
using System.Collections.ObjectModel;

namespace DinnerCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Note: ObservableCollection is necessary for informing the
        // DataGrid object when a new element has been added so it knows
        // to prepare another item
        internal ObservableCollection<Participant> Participants
        {
            get => participants ?? (participants = new ObservableCollection<Participant>());
            set => participants = value;
        }
        private ObservableCollection<Participant> participants;
        public MainWindow()
        {
            InitializeComponent();
            myDataGrid.ItemsSource = Participants;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Participants.Add(new Participant { name = "", moneyOwed = 0 });
        }
    }
}
