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
    public partial class Form11 : Form
    {
        public Form11()
        {
            InitializeComponent();
        }
        private void AddSuppliers()
        {
            string query = "InsertSuppliers";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@Supplier_name",
                Value = textBox1.Text
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(nameParam);

            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Успешно добавлено!");
        }
        DataBase dataBase = new DataBase();
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("Supplier_ID", "id_Поставщика");
            dataGridView1.Columns.Add("Supplier_name", "Название поставщика");
        }

        private void ReadSingleRow(DataGridView gridView, IDataRecord data)
        {
            gridView.Rows.Add(data.GetInt32(0), data.GetString(1));
        }

        private void RefreshDataGrid(DataGridView gridView)
        {
            gridView.Rows.Clear();
            string query = "select * from Suppliers_v";
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
            var deleteQuery = "DeleteSuppliers";
            SqlCommand sqlCommand = new SqlCommand(deleteQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter supParam = new SqlParameter
            {
                ParameterName = "@Supplier_ID",
                Value = id
            };
            sqlCommand.Parameters.Add(supParam);
            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Строка удалена!");
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();                           //для открытия второй формы
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Заполните все поля!");
            }
            else
            {
                AddSuppliers();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deletedRow();
        }

        private void Form11_Load(object sender, EventArgs e)
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

                textBox1.Text = row.Cells[1].Value.ToString();
            }
        }
        private void Change()
        {
            dataBase.openConnection();
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);
            var full_name = textBox1.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(id, full_name);
            }
            var changeQuery = "UpdateSup";
            SqlCommand sqlCommand = new SqlCommand(changeQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@Supplier_name",
                Value = full_name
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(nameParam);           
            SqlParameter idParam = new SqlParameter
            {
                ParameterName = "@Supplier_ID",
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
