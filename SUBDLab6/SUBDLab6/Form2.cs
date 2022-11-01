using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SUBDLab6
{
    public partial class Form2 : Form
    {
        DataBase dataBase = new DataBase();
        public Form2()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form9 form9  = new Form9();
            form9.Show();                           //для открытия второй формы
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form10 form10 = new Form10();
            form10.Show();                           //для открытия второй формы
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form11 form11 = new Form11();
            form11.Show();                           //для открытия второй формы
            Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form12 form12 = new Form12();
            form12.Show();                           //для открытия второй формы
            Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form13 form13 = new Form13();
            form13.Show();                           //для открытия второй формы
            Hide();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form14 form14 = new Form14();
            form14.Show();                           //для открытия второй формы
            Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Админ")
            {
                Form3 form3 = new Form3();
                form3.Show();                           //для открытия второй формы
                Hide();
            }
        }
    }
}
