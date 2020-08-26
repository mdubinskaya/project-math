using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace VKR_
{
    public partial class Form1 : Form
    {
        Detail_list dl;
        double verh=0;
        int flag=1;
        public Form1()
        {
            
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && !((e.KeyChar == '.') && (((TextBox)sender).Text.IndexOf(".") == -1) && (((TextBox)sender).Text.Length != 0)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }

        }

        public Detail str_to_det(String s, int _i)
        {
            String l = "";
            String b = "";
            int i = 0;
            while (s[i] != '\t')
            {
                l += s[i];
                i++;
            }
            i++;
            for (int j = i; j < s.Length; ++j)
            {
                b += s[j];
            }
            return new Detail(Convert.ToDouble(l), Convert.ToInt32(b), _i);
        }


        private void LoadButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                String input = File.ReadAllText(openFileDialog1.FileName);
                input = input.Replace("\r", "");
                input = input.Replace(".", ",");
                String[] substrings = input.Split('\n');
                dl = new Detail_list(substrings.Length);
                for (int i = 0; i < substrings.Length; ++i)
                {
                    dl.add_detail(str_to_det(substrings[i], i));
                }
            }
            verh = 0;
            for (int i=0;i<dl.list.Count;i++)
            {
                verh += dl.list[i].b * dl.list[i].l;
            }
            flag = 0;
            for (int i = 0; i < dl.list.Count; ++i)
            {
                dgvInput.Rows.Add();
                dgvInput.Rows[i].Cells[0].Value = dl.list[i].l;
                dgvInput.Rows[i].Cells[1].Value = dl.list[i].b;
            }
            flag = 1;
        }

        public void outputDGV(Cutting c)
        {
            dgvOutput.Rows.Clear();
            dgvOutput.Columns.Clear();
            int n = c.cp_list.Count;
            for (int j = 0; j < c.dl.list.Count; ++j)
            {
                dgvOutput.Columns.Add(1.ToString(), c.dl.list[j].l.ToString());
            }
            dgvOutput.Columns.Add(1.ToString(), "Остаток");
            for (int i = 0; i < n; i++)
            {
                dgvOutput.Rows.Add();
                for (int j = 0; j < c.cp_list[i].map.Length; ++j)
                {
                    dgvOutput.Rows[i].Cells[j].Value = c.cp_list[i].map[j];
                }
                dgvOutput.Rows[i].Cells[c.cp_list[i].map.Length].Value = c.cp_list[i].h;
            }
        }

        public void output(Cutting c)
        {
            int n = c.cp_list.Count;
            string s = "";
            for (int j = 0; j < c.dl.list.Count; ++j)
            {
                s += c.dl.list[j].l.ToString();
                s += "\t";
            }
            s += "H";
            lb_output.Items.Add(s);
            s = "";
            for (int j = 0; j < c.dl.list.Count; ++j)
            {
                s += "-";
                s += "\t";
            }
            s += "-";
            lb_output.Items.Add(s);
            for (int i = 0; i < n; i++)
            {
                s = "";
                for (int j = 0; j < c.cp_list[i].map.Length; ++j)
                {
                    s += c.cp_list[i].map[j].ToString();
                    s += "\t";
                }
                s += c.cp_list[i].h;
                lb_output.Items.Add(s);
            }
        }

        private void button1_Click(object sender, EventArgs e)//Рассчитать 
        {
            lb_output.Items.Clear();
            Cutting cutting = new Cutting(Convert.ToDouble(tb_length.Text), dl);
            cutting.create_ffd_cutting_map();
            label2.Text = cutting.cp_list.Count.ToString();
            cutting.calc_botton_border();
            if (cutting.cp_list.Count > cutting.bottom_border)
            {
                int i = 0;
                while (true)
                {
                    cutting.S_task();

                    ++i;
                    if (cutting.cp_list.Count == cutting.bottom_border || i > 20000)
                        break;
                }
                      
                label3.Text = cutting.cp_list.Count.ToString();
            }
            //output(cutting);
            outputDGV(cutting);
            int niz = Convert.ToInt32(tb_length.Text) * Convert.ToInt32(label2.Text);
            label3.Text = Convert.ToString(Math.Round((verh / niz),4));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvInput.Columns.Add(1.ToString(), "Длина");
            dgvInput.Columns.Add(2.ToString(), "Количество");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            verh = 0;
            for (int i = 0; i < dgvInput.Rows.Count - 1; i++)
            {
                verh += Convert.ToDouble(dgvInput.Rows[i].Cells[0].Value) * Convert.ToInt32(dgvInput.Rows[i].Cells[1].Value);
            }
                if (tb_length.Text == "")
                MessageBox.Show("Введите длину бруска!", "Внимание!");
            if (dl!=null)
            {
                dl.ClearAll();
            }
            dl = new Detail_list(dgvInput.Rows.Count - 1);
            for (int i=0;i<dgvInput.Rows.Count-1 ;i++)
            {
                if (dgvInput.Rows[i].Cells[0].Value!=null && Convert.ToInt32(dgvInput.Rows[i].Cells[1].Value)!=0 )
                dl.add_detail( new Detail(Convert.ToDouble(dgvInput.Rows[i].Cells[0].Value), Convert.ToInt32(dgvInput.Rows[i].Cells[1].Value),i));
                else
                {
                    MessageBox.Show("Заполните данные!", "Внимание!");
                    dl.ClearAll();
                    break;
                }
            }
            
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if(dgvInput.SelectedRows.Count >0 && dgvInput.SelectedRows[0].Index != dgvInput.Rows.Count-1 )
            {
                dgvInput.Rows.RemoveAt(dgvInput.SelectedRows[0].Index);
            }
            dl.ClearAll();
            dl = new Detail_list(dgvInput.Rows.Count - 1);
            for (int i = 0; i < dgvInput.Rows.Count - 1; i++)
            {
                dl.add_detail(new Detail(Convert.ToDouble(dgvInput.Rows[i].Cells[0].Value), Convert.ToInt32(dgvInput.Rows[i].Cells[1].Value), i));
            }
        }

        private void dgvInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && !((e.KeyChar == '.') && (((TextBox)sender).Text.IndexOf(".") == -1) && (((TextBox)sender).Text.Length != 0)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
          
        }

        private void dgvOutput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar)) && !((e.KeyChar == '.') && (((TextBox)sender).Text.IndexOf(".") == -1) && (((TextBox)sender).Text.Length != 0)))
            {
                if (e.KeyChar != (char)Keys.Back)
                {
                    e.Handled = true;
                }
            }
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dgvInput_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(dgvOutput_KeyPress);
            e.Control.KeyPress += new KeyPressEventHandler(dgvOutput_KeyPress); 
        }

        private void tb_length_TextChanged(object sender, EventArgs e)
        {
            int result;
            if (!Int32.TryParse(tb_length.Text, out result))
            {
                MessageBox.Show("Введено некорректное число!");
            }
            
        }

        private void dgvInput_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (flag == 1)
            {
                if(dgvInput.CurrentCell.Value != null)
                {
                    int result;
                    if (!Int32.TryParse(dgvInput.CurrentCell.Value.ToString(), out result) || dgvInput.CurrentCell.Value.ToString() == "0")
                    {
                        MessageBox.Show("Введено некорректное число!");
                        dgvInput.CurrentCell.Value = null;
                        return;
                    }
                }
               
                
            }
            

        }
    }
}

