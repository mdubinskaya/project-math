using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKR_
{
    public class Cutting
    {
        public int M;
        public double L;
        public double barier;
        public int bottom_border, up_border, N;
        public Detail_list dl;
        public List<Cutting_Pattern> cp_list;
        public Cutting(double _L, Detail_list _dl)
        {
            M = _dl.list.Count;
            L = _L;
            dl = _dl;
            dl.update_work_list();
            dl.sort_by_l();
            cp_list = new List<Cutting_Pattern>();
        }
        public void create_ffd_cutting_map()
        {
            while (!dl.check_empty_wl())  
            {
                Cutting_Pattern cp = new Cutting_Pattern(M, L);
                int current_detail = 0;
                bool next = false;
                while (!next)
                {
                    if (cp.add_detail(dl.work_list[current_detail])) //Если можно добавить текущую деталь
                    {
                        dl.work_list[current_detail].cb--;
                        continue;
                    }
                    else
                    {
                        if (cp.h == 0 || cp.h < dl.work_list[dl.work_list.Count - 1].l)
                            next = true;
                        else
                        {
                            if (current_detail < dl.work_list.Count - 1)
                                current_detail++;
                            else next = true;
                        }
                    }
                }
                cp_list.Add(cp);
            }
        }
        public void calc_y_first()
        {
            for (int i = 0; i < dl.list.Count; ++i)
            {
                dl.list[i].y = 0;
                for (int j = 0; j < cp_list.Count; ++j)
                {
                    dl.list[i].y += ((dl.list[i].l * cp_list[j].h) / (L - cp_list[j].h) + dl.list[i].l) * cp_list[j].map[dl.list[i].i];
                }
                dl.list[i].y /= dl.list[i].b;
            }
            calc__y_dl();
            dl.update_work_list();
        }
        public void calc_y()
        {
            for (int i = 0; i < dl.work_list.Count - 1; ++i)
            {
                double old_y = dl.work_list[i].y;
                dl.work_list[i].y = 0;
                 for (int j = 0; j < cp_list.Count; ++j)
                {
                    dl.work_list[i].y += ((dl.work_list[i].l * cp_list[j].h) / (L - cp_list[j].h) + dl.work_list[i].l) * cp_list[j].map[dl.work_list[i].i]; //+old_y * dl.list[i].b;
                }
                dl.work_list[i].y += old_y * dl.work_list[i].cb;
                dl.work_list[i].y /= dl.work_list[i].b;
            }
            calc__y_wl();
        }
        public void calc__y_wl()
        {
            for (int i = 0; i < dl.work_list.Count; ++i)
            {
                dl.work_list[i]._y = dl.work_list[i].y / dl.work_list[i].l;
            }
        }
        public void calc__y_dl()
        {
            for (int i = 0; i < M; ++i)
            {
                dl.list[i]._y = dl.list[i].y / dl.list[i].l;
            }
        }
        public void calc_barier()
        {
            double _f = dl.work_list[0]._y * L;
            /*for (int i = 0; i < M; ++i)
            {
                _f += dl.list[i]._y;
            }*/
            barier = (L + _f) / 2;
        }
        public void calc_botton_border()
        {
            double sum = 0;
            for (int i = 0; i < M; ++i)
            {
                sum += dl.list[i].b * dl.list[i].l;
            }
            bottom_border = Convert.ToInt32(Math.Ceiling(sum / L));
        }
        public bool check_solution()
        {
            if (cp_list.Count == bottom_border)
                return true;
            else return false; //Здесь корректировку?
        }
        public void S_task()
        {
            dl.reset_cb();
            dl.update_work_list();
            calc_y_first();
            cp_list.Clear();
            while (!dl.check_empty_wl())
            {
                dl.sort_by__y();
                calc_barier();
                Cutting_Pattern cp = new Cutting_Pattern(M, L);
                double _f;
                double fi;
                double old_barier = 0;
                int place_count = 1;
                int prev = 0;
                dl.current_detail = 0;
                bool was_step_back = false;
                while (cp.h >= dl.calc_min() && !dl.check_empty_wl())
                {
                    if (cp.h < dl.work_list[dl.current_detail].l && !was_step_back)
                    {
                        dl.next();
                        continue;
                    }
                    _f = dl.work_list[dl.current_detail]._y * cp.h;
                    if (_f >= barier) // step forward //запатчтил остаток
                    {
                        if (dl.work_list[dl.current_detail].cb >= place_count)
                        {
                            for (int i = 0; i < place_count; ++i)
                            {
                                cp.add_detail(dl.work_list[dl.current_detail]);
                            }
                            dl.work_list[dl.current_detail].cb -= place_count;
                        }
                        else
                        {
                            for (int i = 0; i < dl.work_list[dl.current_detail].cb; ++i)
                            {
                                cp.add_detail(dl.work_list[dl.current_detail]);
                            }
                            dl.work_list[dl.current_detail].cb = 0;
                        }
                        fi = dl.work_list[dl.current_detail].y * cp.map[dl.work_list[dl.current_detail].i];
                        old_barier = barier;
                        prev = dl.current_detail;
                        barier -= fi;
                        dl.next();
                        was_step_back = false;
                        place_count = 1;
                    }
                    else // step back
                    {
                        if (was_step_back)
                        {
                            barier *= 0.8;
                            continue;
                        }
                        dl.current_detail = prev;
                        cp.remove_detail(dl.work_list[dl.current_detail]);
                        dl.work_list[dl.current_detail].cb += place_count;
                        barier = old_barier;
                        place_count++;
                        was_step_back = true; 
                    }
                }
                cp_list.Add(cp);

                if (!dl.check_empty_wl())
                    calc_y();
                //else
            }
        }


}
}
