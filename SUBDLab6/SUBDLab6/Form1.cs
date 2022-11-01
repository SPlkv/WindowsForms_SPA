using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SUBDLab6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            
        }
        
        private void button6_Click(object sender, EventArgs e)  //запись на процедуру
        {
            Form8 form8 = new Form8();
            form8.Show();                           //для открытия второй формы
            Hide();
        }

        private void button5_Click(object sender, EventArgs e)  //добавление процедуры
        {
            Form3 form3 = new Form3();
            form3.Show();                           //для открытия второй формы
            Hide();
        }

        private void button4_Click(object sender, EventArgs e)  //добавить расходник
        {
            Form4 form4 = new Form4();
            form4.Show();                           //для открытия второй формы
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)  //добавить поставщика
        {
            Form5 form5 = new Form5();
            form5.Show();                           //для открытия второй формы
            Hide();
        }

        private void button2_Click(object sender, EventArgs e)  //добавить клиента
        {
            Form6 form6 = new Form6();
            form6.Show();                           //для открытия второй формы
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)  //добавить мастера
        {
            Form7 form7 = new Form7();
            form7.Show();                           //для открытия второй формы
            Hide();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked==true)
            {
                textBox1.Visible = true;
                label2.Visible = true;
            }
            else
            {
                textBox1.Visible = false;
                label2.Visible = false;
                
            }
            
        }

        public string getdata;
        private void button1_Click_1(object sender, EventArgs e)
        {
            if ((checkBox1.Checked == true && checkBox2.Checked == true && checkBox3.Checked == true)|| (checkBox1.Checked == true && checkBox2.Checked == true) || (checkBox1.Checked == true && checkBox3.Checked == true) || (checkBox2.Checked == true && checkBox3.Checked == true))
                MessageBox.Show("Выберите только одно значение!");

            else if(textBox1.Text=="Админ")
            {
                Form2 form2 = new Form2();
                form2.Show();                           //для открытия второй формы
                Hide();
            }
            else if(textBox1.Text=="Оператор")
            {
                Form2 form2 = new Form2();
                form2.Show();                           //для открытия второй формы
                Hide();
            }
            else
            {
                Form15 form15 = new Form15();
                form15.Show();                           //для открытия второй формы
                Hide();
                
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                textBox1.Visible = true;
                label2.Visible = true;
            }
            else
            {
                textBox1.Visible = false;
                label2.Visible = false;
             
            }
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                
            }
            else
            {
                textBox1.Visible = false;
                label2.Visible = false;
                
            }
        }
    }
}
