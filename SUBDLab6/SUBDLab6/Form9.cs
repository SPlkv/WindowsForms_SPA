using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SUBDLab6
{
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
        }
        enum RowState
        {
            Existed,
            New,
            Modifed,
            ModifiedNew,
            Deleted
        }
        int selectedRow;
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void AddMaster()
        {
            string query = "InsertMasters";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@Full_name",
                Value = textBox4.Text
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(nameParam);
            // параметр для ввода номера
            SqlParameter ageParam = new SqlParameter
            {
                ParameterName = "@Phone_number",
                Value = textBox3.Text
            };

            sqlCommand.Parameters.Add(ageParam);
            SqlParameter employParam = new SqlParameter
            {
                ParameterName = "@Employment_date",
                Value = dateTimePicker1.Value
            };

            sqlCommand.Parameters.Add(employParam);

            SqlParameter salaryParam = new SqlParameter
            {
                ParameterName = "@Salary",
                Value = textBox2.Text
            };

            sqlCommand.Parameters.Add(salaryParam);
            SqlParameter serviceTypeParam = new SqlParameter
            {
                ParameterName = "@Service_type",
                Value = textBox1.Text
            };

            sqlCommand.Parameters.Add(serviceTypeParam);
            
            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Успешно добавлено!");

        }
        DataBase dataBase = new DataBase();
        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text==""|| textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                AddMaster();
            }
              
        }


        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number)&&number!=8)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44 && number != 32) // цифры, клавиша BackSpace и запятая
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();
            char number = e.KeyChar;
            if (!Regex.Match(Symbol, @"[а-яА-Я]|[a-zA-Z]").Success && number != 8 && number != 32 && number!=46)
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();
            char number = e.KeyChar;
            if (!Regex.Match(Symbol, @"[а-яА-Я]|[a-zA-Z]").Success && number != 8 && number != 32)
            {
                e.Handled = true;
            }
        }
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("Master_ID", "id_Мастера");
            dataGridView1.Columns.Add("Full_name", "ФИО мастера");
            dataGridView1.Columns.Add("Phone_number", "Номер мастера");
            dataGridView1.Columns.Add("Employment_date", "Дата поступления на работу");
            dataGridView1.Columns.Add("Salary", "Оклад");
            dataGridView1.Columns.Add("Service_type", "Тип процедуры");
        }

        private void ReadSingleRow(DataGridView gridView, IDataRecord data)
        {
            gridView.Rows.Add(data.GetInt32(0), data.GetString(1), data.GetString(2), data.GetDateTime(3).Date, data.GetValue(4),data.GetString(5));
        }

        private void RefreshDataGrid(DataGridView gridView)
        {
            gridView.Rows.Clear();
            string query = "select * from Masters_v";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow(gridView, reader);
            }
            reader.Close();
        }
        
        private void deletedRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataBase.openConnection();
            var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
            var deleteQuery = "DeleteMasters";
            SqlCommand sqlCommand = new SqlCommand(deleteQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter masterParam = new SqlParameter
            {
                ParameterName = "@Master_ID",
                Value = id
            };
            sqlCommand.Parameters.Add(masterParam);
            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Строка удалена!");
            dataBase.closedConnection();

        }

        private void Change()
        {
            dataBase.openConnection();
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);
            var full_name = textBox4.Text;
            var phone_number = textBox3.Text;
            var calendar = dateTimePicker1.Value;
            var salary = textBox2.Text;
            var type = textBox1.Text;
            
            if(dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString()!=string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(id,full_name, phone_number, calendar, salary, type);
            }
            var changeQuery = "UpdateMasters";
            SqlCommand sqlCommand = new SqlCommand(changeQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@Full_name",
                Value = textBox4.Text
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(nameParam);
            // параметр для ввода номера
            SqlParameter ageParam = new SqlParameter
            {
                ParameterName = "@Phone_number",
                Value = textBox3.Text
            };

            sqlCommand.Parameters.Add(ageParam);
            SqlParameter employParam = new SqlParameter
            {
                ParameterName = "@Employment_date",
                Value = dateTimePicker1.Value
            };

            sqlCommand.Parameters.Add(employParam);

            SqlParameter salaryParam = new SqlParameter
            {
                ParameterName = "@Salary",
                Value = textBox2.Text
            };

            sqlCommand.Parameters.Add(salaryParam);
            SqlParameter serviceTypeParam = new SqlParameter
            {
                ParameterName = "@Service_type",
                Value = textBox1.Text
            };

            sqlCommand.Parameters.Add(serviceTypeParam);
            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@Master_ID",
                Value = id
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(idParam);

            sqlCommand.ExecuteScalar();
            MessageBox.Show("Успешно изменено!");
            dataBase.closedConnection();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();                           //для открытия второй формы
            Hide();
        }

        private void Form9_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deletedRow();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Change();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if(e.RowIndex>=0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox4.Text = row.Cells[1].Value.ToString();
                textBox3.Text = row.Cells[2].Value.ToString();
                dateTimePicker1.Value = (DateTime)row.Cells[3].Value;
                textBox2.Text = row.Cells[4].Value.ToString();
                textBox1.Text = row.Cells[5].Value.ToString();
            }
        }
    }
}
