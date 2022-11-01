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
    public partial class Form10 : Form
    {
        public Form10()
        {
            InitializeComponent();
        }
        private void AddClient()
        {
            string query = "InsertClients";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@Full_name",
                Value = textBox1.Text
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(nameParam);
            // параметр для ввода номера
            SqlParameter addressParam = new SqlParameter
            {
                ParameterName = "@Address_client",
                Value = textBox2.Text
            };

            sqlCommand.Parameters.Add(addressParam);
            SqlParameter phoneParam = new SqlParameter
            {
                ParameterName = "@Phone_number",
                Value = textBox3.Text
            };

            sqlCommand.Parameters.Add(phoneParam);

            SqlParameter emailParam = new SqlParameter
            {
                ParameterName = "@Email",
                Value = textBox2.Text
            };

            sqlCommand.Parameters.Add(emailParam);          

            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Успешно добавлено!");
        }
        DataBase dataBase = new DataBase();
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("Client_ID", "id_Клиента");
            dataGridView1.Columns.Add("Full_name", "ФИО клиента");
            dataGridView1.Columns.Add("Address_client", "Адрес клиента");
            dataGridView1.Columns.Add("Phone_number", "Номер клиента");
            dataGridView1.Columns.Add("Email", "Эмэйл");
        }

        private void ReadSingleRow(DataGridView gridView, IDataRecord data)
        {
            gridView.Rows.Add(data.GetInt32(0), data.GetString(1), data.GetString(2), data.GetString(3), data.GetString(4));
        }

        private void RefreshDataGrid(DataGridView gridView)
        {
            gridView.Rows.Clear();
            string query = "select * from Clients_v";
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
            var deleteQuery = "DeleteClients";
            SqlCommand sqlCommand = new SqlCommand(deleteQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter clientParam = new SqlParameter
            {
                ParameterName = "@Client_ID",
                Value = id
            };
            sqlCommand.Parameters.Add(clientParam);
            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Строка удалена!");
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            string Symbol = e.KeyChar.ToString();
            char number = e.KeyChar;
            if (!Regex.Match(Symbol, @"[а-яА-Я]|[a-zA-Z]").Success && number != 8 && number != 32&& number!=46)
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number)&& number!=8 && number != 32)
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                AddClient();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();                           //для открытия второй формы
            Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deletedRow();

        }

        private void Form10_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }
        private void Change()
        {
            dataBase.openConnection();
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);
            var full_name = textBox1.Text;
            var address = textBox2.Text;
            var phone_number= textBox3.Text;
            var email = textBox4.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(id, full_name,address,phone_number,email);
            }
            var changeQuery = "UpdateClients";
            SqlCommand sqlCommand = new SqlCommand(changeQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@Full_name",
                Value = full_name
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(nameParam);
            // параметр для ввода номера
            SqlParameter ageParam = new SqlParameter
            {
                ParameterName = "@Address_client",
                Value = address
            };

            sqlCommand.Parameters.Add(ageParam);
            SqlParameter employParam = new SqlParameter
            {
                ParameterName = "@Phone_number",
                Value = phone_number
            };

            sqlCommand.Parameters.Add(employParam);

            SqlParameter salaryParam = new SqlParameter
            {
                ParameterName = "@Email",
                Value = email
            };

            sqlCommand.Parameters.Add(salaryParam);
            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@Client_ID",
                Value = id
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(idParam);

            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Успешно изменено!");
            dataBase.closedConnection();
            
        }
        
        int selectedRow;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox1.Text = row.Cells[1].Value.ToString();
                textBox2.Text = row.Cells[2].Value.ToString();
                textBox3.Text = row.Cells[3].Value.ToString();
                textBox4.Text = row.Cells[4].Value.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Change();
        }
    }
}
