using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKR_
{
    public class Cutting_Pattern
    {
        public double h, L;
        public int[] map;
        public Cutting_Pattern(int M, double _L)
        {
            map = new int[M];
            L = _L;
            h = L;
        }
        public bool add_detail(Detail d)
        {
            if (h >= d.l && d.cb > 0)
            {
                h -= d.l;
                map[d.i] += 1;
                return true;
            }
            return false;
        }
        public void remove_detail(Detail d)
        {
            h += d.l;
            map[d.i] -= 1;
        }

    }
}
