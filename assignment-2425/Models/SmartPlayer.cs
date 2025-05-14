
namespace assignment_2425.ViewModels
{
    public class SmartPlayer
    {
        public double RiskVariable { get; set; } = 0.6;
        public bool IsSmart { get; set; } = false;
        public List<int> WLRatio { get; set; } = new List<int>();
        public bool MoveDown { get; set; } = true;

        //Runs calculation to determine if to lower or increase the RiskVariable
        public void Recalc()
        {
            //Reduces the size of w/l to reduce latency
            if (WLRatio.Count > 100)
            {
                int ls = WLRatio.Where(x => x == 0).Count()/10;
                int ws = WLRatio.Where(x => x== 1).Count()/10;
                var newList = WLRatio.Where(x => x == 0).Take(ls).ToList();

                newList.AddRange(WLRatio.Where(x => x == 1).Take(ws));
                WLRatio = newList;

            }
            //Check if losses are greater than wins
            if (WLRatio.Count > 1 && WLRatio.Where(x=>x == 1).Count() < WLRatio.Where(x => x == 0).Count())
            {
                var avgWins = CalculateMovingAverage(WLRatio.Where(x => x == 0).ToList(), 5);
                var avgLosses = CalculateMovingAverage(WLRatio.Where(x => x == 1).ToList(), 5);

                if(avgLosses > avgWins)
                {
                    if (RiskVariable - 0.05 < 0.4)
                    {
                        MoveDown = false;
                    }
                    else if(RiskVariable + 0.05 > 0.7)
                    {
                        MoveDown=true;
                    }

                    RiskVariable = (MoveDown) ? RiskVariable - 0.05 : RiskVariable + 0.05;
                }
            }
        }

        //Calculates average wins and losses
        public double CalculateMovingAverage(List<int> winsLosses, int every)
        {
            var count = winsLosses.Count;
            if (winsLosses.Count == 0)
            {
                return 0;
            }

            if (winsLosses.Count < every)
            {
                winsLosses.Average();
            }

            return winsLosses.Skip(Math.Max(0, count - every)).Take(every).Average();
        }
    }
}
