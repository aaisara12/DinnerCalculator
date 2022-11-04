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
            Participants.CollectionChanged += HandleParticipantsListChanged;

            subtotalText.Text = (0).ToString("C");
            totalText.Text = (0).ToString("C");

        }

        private void HandleParticipantsListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateTotals();
        }

        private void SubscribeToNewParticipants(object sender, NotifyCollectionChangedEventArgs e)
        {
            if(e.NewItems != null)
            {
                foreach (Participant p in e.NewItems)
                {
                    p.PropertyChanged += HandlePropertyChanged;
                }
            }
            if(e.OldItems != null)
            {
                foreach(Participant p in e.OldItems)
                {
                    p.PropertyChanged -= HandlePropertyChanged;
                }
            }
            
        }


        // Wrapper around UpdateTotals to respond to money owed changed event
        private void HandlePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "expenses" || e.PropertyName == "isPaying")
                UpdateTotals();
        }

        // Recompute the totals
        private void UpdateTotals()
        {
            // Make sure all the text components are ready -- is there a better way to do this?
            if (tipPercentText == null || 
                taxPercentText == null ||
                totalText == null ||
                subtotalText == null 
                )
                return;

            decimal subtotal = 0;
            decimal costToDistribute = 0;   // Expenses of non-paying participants
            int numPaying = 0;

            foreach(Participant p in Participants)
            {
                subtotal += p.expenses;

                if (!p.isPaying)
                    costToDistribute += p.expenses;
                else
                    numPaying++;
            }

            decimal distributedCostPerParticipant = costToDistribute / numPaying; // How much extra each paying participant pays
            
            decimal tax = decimal.Parse(taxPercentText.Text) * 0.01m;
            decimal tip = decimal.Parse(tipPercentText.Text) * 0.01m;

            decimal total =  subtotal * (1 + (tax + tip));

            // TODO: True money owed column
            // 1. Two more attributes of Participant class: isPaying and expenses
            // 2. Make expenses column readonly
            // 3. Update expenses after values changed (including isPaying)

            subtotalText.Text = subtotal.ToString("C");
            totalText.Text = total.ToString("C");

            foreach (Participant p in Participants)
            {
                p.moneyOwed = (subtotal == 0 || !p.isPaying)? 0 : ((p.expenses + distributedCostPerParticipant)/ subtotal) * total;
            }
        }

        private void AddPerson_Button_Click(object sender, RoutedEventArgs e)
        {
            Participants.Add(new Participant { name = "", expenses = 0 }); 
        }

        private void tipPercentText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotals();
        }

        private void taxPercentText_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateTotals();
        }
    }
}
