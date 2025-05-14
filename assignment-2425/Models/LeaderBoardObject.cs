using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace assignment_2425.Models
{
    public class LeaderBoardObject
    {
        public string UserName { get; set; }
        public int Wins { get; set; }
        public int Postition { get; set; }

        public LeaderBoardObject(string name, int score, int rank)
        {
            UserName = name;
            Wins = score;
            Postition = rank;
        }

    }
}
