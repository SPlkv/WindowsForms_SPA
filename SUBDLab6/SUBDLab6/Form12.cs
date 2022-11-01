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
    public partial class Form12 : Form
    {
        public Form12()
        {
            InitializeComponent();
            GetSup();
        }
        List<int> Suppliers = new List<int>();
        private void GetSup()
        {
             
            string query = "select Supplier_ID,Supplier_name from Suppliers";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(reader.GetValue(1).ToString());
                Suppliers.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
            }
            reader.Close();
        }
        private void AddConsumables()
        {
            string query = "InsertConsumables";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            int id = listBox1.SelectedIndex;
            SqlParameter supidParam = new SqlParameter
            {
                ParameterName = "@Supplier_ID",
                Value = Suppliers[id]
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(supidParam);

            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@Name_consumable",
                Value = textBox1.Text
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(nameParam);

            SqlParameter priceParam = new SqlParameter
            {
                ParameterName = "@Consumable_price",
                Value = textBox2.Text
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(priceParam);

            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Успешно добавлено!");
        }
        DataBase dataBase = new DataBase();


        private void CreateColumns()
        {
            dataGridView1.Columns.Add("Consumable_ID", "id_Расходника");
            dataGridView1.Columns.Add("Supplier_ID", "id_поставщика");
            dataGridView1.Columns.Add("Name_consumable", "Название расходника");
            dataGridView1.Columns.Add("Consumable_price", "Цена расходника");
        }

        private void ReadSingleRow(DataGridView gridView, IDataRecord data)
        {
            gridView.Rows.Add(data.GetInt32(0), data.GetInt32(1), data.GetString(2), data.GetValue(3));
        }

        private void RefreshDataGrid(DataGridView gridView)
        {
            gridView.Rows.Clear();
            string query = "select * from Consumables_v";
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
            var deleteQuery = "DeleteConsumables";
            SqlCommand sqlCommand = new SqlCommand(deleteQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter conParam = new SqlParameter
            {
                ParameterName = "@Consumable_ID",
                Value = id
            };
            sqlCommand.Parameters.Add(conParam);
            sqlCommand.ExecuteScalar();
            MessageBox.Show("Строка удалена!");

        }
        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44) // цифры, клавиша BackSpace и запятая
            {
                e.Handled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();                           //для открытия второй формы
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            if (textBox1.Text == "" || textBox2.Text == ""||listBox1.SelectedIndex==-1)
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                AddConsumables();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deletedRow();
            RefreshDataGrid(dataGridView1);
        }

        private void Form12_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
        }
        int selectedRow;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                textBox1.Text = row.Cells[2].Value.ToString();
                textBox2.Text = row.Cells[3].Value.ToString();
                for (int i = 0; i < Suppliers.Count; i++)
                    if (Convert.ToString(Suppliers[i]) == row.Cells[1].Value.ToString())
                        listBox1.SetSelected(i, true);
            }
        }
        private void Change()
        {
            dataBase.openConnection();
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);
            int id_sup = listBox1.SelectedIndex;
            var sup_id = Suppliers[id_sup];
            var name_consumable = textBox1.Text;
            var price = textBox2.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(id, sup_id,name_consumable,price);
            }
            var changeQuery = "UpdateCon";
            SqlCommand sqlCommand = new SqlCommand(changeQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@Supplier_ID",
                Value = Suppliers[id_sup]
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(nameParam);
            // параметр для ввода номера
            SqlParameter ageParam = new SqlParameter
            {
                ParameterName = "@Name_consumable",
                Value = name_consumable
            };

            sqlCommand.Parameters.Add(ageParam);
            SqlParameter employParam = new SqlParameter
            {
                ParameterName = "@Consumable_price",
                Value = price
            };

            sqlCommand.Parameters.Add(employParam);
         
            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@Consumable_ID",
                Value = id
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(idParam);

            sqlCommand.ExecuteScalar();
            MessageBox.Show("Успешно изменено!");
            dataBase.closedConnection();

        }
        private void button4_Click(object sender, EventArgs e)
        {
            Change();
        }
    }
}
