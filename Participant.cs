using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DinnerCalculator
{
    internal class Participant : INotifyPropertyChanged
    {
        public string name
        {
            get => _name;
            set => _name = value;
        }
        private string _name = String.Empty;

        public decimal expenses 
        {
            get => _expenses;
            set
            {
                _expenses = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("expenses"));
            }
        }
        private decimal _expenses = 0;

        public bool isPaying
        {
            get => _isPaying;
            set
            {
                _isPaying = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("isPaying"));
            }
        }
        private bool _isPaying = true;

        public decimal moneyOwed
        {
            get => _moneyOwed;
            set
            {
                _moneyOwed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("moneyOwed"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("moneyOwedString"));
            }
        }
        private decimal _moneyOwed = 0;

        public string moneyOwedString
        {
            get => _moneyOwed.ToString("C");
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
