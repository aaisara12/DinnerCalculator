using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace DinnerCalculator
{
    internal class Participant
    {
        public string name
        {
            get => _name;
            set => _name = value;
        }
        private string _name = String.Empty;

        public decimal moneyOwed 
        {
            get => _moneyOwed;
            set
            {
                _moneyOwed = value;
                OnMoneyOwedChanged?.Invoke(_moneyOwed);   
            }
        }
        private decimal _moneyOwed = 0;

        public event System.Action<decimal> OnMoneyOwedChanged;
    }
}
