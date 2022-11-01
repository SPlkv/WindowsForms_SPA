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
    public partial class Form14 : Form
    {
        public Form14()
        {
            InitializeComponent();
            GetClients();
            GetProcedures();
        }
        List<int> Clients = new List<int>();
        List<string> Procedures = new List<string>();
        private void GetClients()
        {

            string query = "select Client_ID,Full_name from Clients";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(reader.GetValue(1).ToString());
                Clients.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
            }
            reader.Close();
        }
        private void GetProcedures()
        {

            string query = "select Procedure_ID,Procedure_name from NamesProcedures";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                listBox2.Items.Add(reader.GetValue(1).ToString());
                Procedures.Add(reader.GetValue(1).ToString());
            }
            reader.Close();
        }
        DataBase dataBase = new DataBase();
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("Entry_ID", "id_Записи");
            dataGridView1.Columns.Add("DateOfEntry", "id_Дата появления записи");
            dataGridView1.Columns.Add("DateOfProcedure", "Дата процедуры");
            dataGridView1.Columns.Add("Full_name", "ФИО клиента");
            dataGridView1.Columns.Add("Procedure_name", "Название процедуры");
        }

        private void ReadSingleRow(DataGridView gridView, IDataRecord data)
        {
            gridView.Rows.Add(data.GetInt32(0), data.GetDateTime(1), data.GetDateTime(2), data.GetValue(3),data.GetValue(4));
        }

        private void RefreshDataGrid(DataGridView gridView)
        {
            gridView.Rows.Clear();
            string query = "select * from Entries_v";
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
            dataBase.openConnection();
            int index = dataGridView1.CurrentCell.RowIndex;
            var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
            var deleteQuery = "DeleteEntries";
            SqlCommand sqlCommand = new SqlCommand(deleteQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter conParam = new SqlParameter
            {
                ParameterName = "@Entry_ID",
                Value = id
            };
            sqlCommand.Parameters.Add(conParam);
            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Строка удалена!");
        }
        private void AddEntry()
        {
            string query = "InsertEntries";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            int id = listBox1.SelectedIndex;
            //int id2 = listBox2.SelectedItem;
            SqlParameter dateEntParam = new SqlParameter
            {
                ParameterName = "@DateOfEntry",
                Value = DateTime.Today
            };
            sqlCommand.Parameters.Add(dateEntParam);
            SqlParameter dateParam = new SqlParameter
            {
                ParameterName = "@DateOfProcedure",
                Value = dateTimePicker1.Value
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(dateParam);

            SqlParameter clientParam = new SqlParameter
            {
                ParameterName = "@Client_ID",
                Value = Clients[id]
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(clientParam);

            SqlParameter procidParam = new SqlParameter
            {
                ParameterName = "@Procedure_name",
                Value = listBox2.SelectedItem
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(procidParam);

            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Успешно добавлено!");
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();                           //для открытия второй формы
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex==-1&&listBox2.SelectedIndex==-1)
            {
                MessageBox.Show("Выберите все значения");
            }
            else
            {
                AddEntry();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            deletedRow();
        }

        private void Form14_Load(object sender, EventArgs e)
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

                dateTimePicker1.Value = (DateTime)row.Cells[2].Value;
                if(row.Cells[3].Value.ToString()!="")
                {
                    for (int i = 0; i < listBox1.Items.Count; i++)
                        if (listBox1.Items[i].ToString() == row.Cells[3].Value.ToString())
                            listBox1.SetSelected(i, true);
                }
                if (row.Cells[4].Value.ToString() != "")
                {
                    for (int i=0; i < listBox2.Items.Count;i++)
                        if (Convert.ToString(Procedures[i]) == row.Cells[4].Value.ToString())
                            listBox2.SetSelected(i,true);
                    
                    
                }

            }
        }
        private void Change()
        {
            dataBase.openConnection();
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);
            var date = dateTimePicker1.Value;
            int id_client = listBox1.SelectedIndex;
            var client = Clients[id_client];
            var proc = listBox2.SelectedItem;


            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(id, dataGridView1.Rows[selectedRowIndex].Cells[1].Value,date, client,proc);
            }
            var changeQuery = "UpdateEntry";
            SqlCommand sqlCommand = new SqlCommand(changeQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter masParam = new SqlParameter
            {
                ParameterName = "@Entry_ID",
                Value = id
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(masParam);
            // параметр для ввода номера
            SqlParameter conParam = new SqlParameter
            {
                ParameterName = "@DateOfProcedure",
                Value = date
            };

            sqlCommand.Parameters.Add(conParam);
            SqlParameter clientParam = new SqlParameter
            {
                ParameterName = "@Client_ID",
                Value = client
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(clientParam);
            // параметр для ввода номера
            SqlParameter procParam = new SqlParameter
            {
                ParameterName = "@Procedure_name",
                Value = proc
            };

            sqlCommand.Parameters.Add(procParam);
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
