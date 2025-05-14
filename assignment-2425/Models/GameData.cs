using assignment_2425.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_2425.ViewModels
{
    public partial class GameData : ObservableObject
    {
        [ObservableProperty]
        public BlackJack blackjack = new BlackJack();

        public Settings Settings { get; set; } = new Settings();
        public void Reset()
        {
            Blackjack = new BlackJack();
        }
    }
}
