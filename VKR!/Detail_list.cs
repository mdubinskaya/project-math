using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKR_
{
    public class Detail_list
    {
        public List<Detail> list;
        public List<Detail> work_list;
        public int current_detail;
        public Detail_list(int M)
        {
            list = new List<Detail>(M);
            current_detail = 0;
        }

        public void next()
        {
            if (work_list[current_detail].cb == 0)
            {
                work_list.RemoveAt(current_detail);
                if (current_detail > work_list.Count - 1)
                {
                    current_detail = work_list.Count - 1;

                }
                if (current_detail > 0 && work_list.Count - 1 == 0)
                {
                    current_detail = 0;
                    return;
                }

            }
            if (current_detail == work_list.Count - 1)
            {

                current_detail = 0;
                return;
            }

            else current_detail++;
        }

        public void prev()
        {
            if (current_detail == 0)
            {
                return;
            }
            for (int i = current_detail - 1; i >= 0; --i)
            {
                if (work_list[i].cb <= 0)
                    continue;
                else
                {
                    current_detail = i;
                    break;
                }
            }
        }

        public void add_detail(Detail d)
        {
            list.Add(d);
        }
        public void set_0()
        {
            foreach (Detail d in list)
            {
                d.cb = 0;
            }
        }
        public void reset_cb()
        {
            foreach (Detail d in list)
            {
                d.cb = d.b;
            }
        }

        public void update_work_list()
        {
            work_list = new List<Detail>(list);
        }
        public bool check_empty()  //Истина если пусто
        {
            bool empty = true;
            foreach (Detail d in list)
            {
                if (d.cb > 0)
                {
                    empty = false;
                    break;
                }
            }
            return empty;
        }
        public bool check_empty_wl()  //Истина если пусто
        {
            bool empty = true;
            foreach (Detail d in work_list)
            {
                if (d.cb > 0)
                {
                    empty = false;
                    break;
                }
            }
            return empty;
        }

        public void ClearAll()
        {
            list.Clear();
        }

        public void sort_by_l()
        {
            for (int i = 0; i < work_list.Count - 1; i++)
            {
                int max = i;
                for (int j = i + 1; j < work_list.Count; j++)
                {
                    if (work_list[j].l > work_list[max].l)
                    {
                        max = j;
                    }
                }
                Detail dummy = work_list[i];
                work_list[i] = work_list[max];
                work_list[max] = dummy;
            }
        }
        public void sort_by__y()
        {
            if (work_list.Count == 1 || work_list.Count == 0)
                return;
            for (int i = 0; i < work_list.Count - 1; i++)
            {
                int max = i;
                for (int j = i + 1; j < work_list.Count; j++)
                {
                    if (work_list[j]._y > work_list[max]._y)
                    {
                        max = j;
                    }
                }
                Detail dummy = work_list[i];
                work_list[i] = work_list[max];
                work_list[max] = dummy;
            }
        }
        
        public double calc_min()
        {
            if (work_list.Count == 0)
                return -1;
            double min = work_list[0].l;
            if (work_list.Count == 1)
                return min;
            for (int i = 1; i < work_list.Count; ++i)
            {
                if (work_list[i].l < min)
                    min = work_list[i].l;
            }
            return min;
        }
 


}
}
