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
using System.Collections.Specialized;
using System.ComponentModel;

namespace DinnerCalculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Note: ObservableCollection is necessary for informing the
        // DataGrid object when a new element has been added so it knows
        // to prepare another item (a simple List doesn't have add events)
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

            Participants.CollectionChanged += SubscribeToNewParticipants;

            subtotalText.Text = (0).ToString("C");
            totalText.Text = (0).ToString("C");
        }

        private void SubscribeToNewParticipants(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach (Participant p in e.NewItems)
                {
                    p.OnMoneyOwedChanged += HandleMoneyOwedChanged;
                }
            }
            if(e.OldItems != null)
            {
                foreach(Participant p in e.OldItems)
                {
                    p.OnMoneyOwedChanged -= HandleMoneyOwedChanged;
                }
            }
            
        }

        // Wrapper around UpdateTotals to respond to money owed changed event
        private void HandleMoneyOwedChanged(decimal newAmt)
        {
            UpdateTotals();
        }

        // Recompute the totals
        private void UpdateTotals()
        {
            decimal subtotal = 0;
            foreach(Participant p in Participants)
            {
                subtotal += p.moneyOwed;
            }
            

            decimal tax = decimal.Parse(taxPercentText.Text) * 0.01m;
            decimal tip = decimal.Parse(tipPercentText.Text) * 0.01m;

            decimal total =  subtotal * (1 + (tax + tip));

            subtotalText.Text = subtotal.ToString("C");
            totalText.Text = total.ToString("C");
        }

        private void AddPerson_Button_Click(object sender, RoutedEventArgs e)
        {
            Participants.Add(new Participant { name = "", moneyOwed = 0 }); 
        }
    }
}
