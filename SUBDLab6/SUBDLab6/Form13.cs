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
    public partial class Form13 : Form
    {
        public Form13()
        {
            InitializeComponent();
            GetMasters();
            GetConsumables();
            GetProc();
        }
        List<int> Masters = new List<int>();
        List<int> Consumables = new List<int>();
        List<int> Procedures = new List<int>();
        private void GetMasters()
        {

            string query = "select Master_ID,Full_name from Masters";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                listBox1.Items.Add(reader.GetValue(1).ToString());
                Masters.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
            }
            reader.Close();
        }
        private void GetConsumables()
        {

            string query = "select Consumable_ID,Name_consumable from Consumables";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                listBox2.Items.Add(reader.GetValue(1).ToString());
                Consumables.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
            }
            reader.Close();
        }
        private void GetProc()
        {

            string query = "select Procedure_ID from Procedures";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                listBox3.Items.Add(reader.GetValue(0).ToString());
                Procedures.Add(Convert.ToInt32(reader.GetValue(0).ToString()));
            }
            reader.Close();
        }
        private void AddProcedures()
        {
            string query = "InsertProcedures";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            int id = listBox1.SelectedIndex;
            int id2 = listBox2.SelectedIndex;
            SqlParameter masidParam = new SqlParameter
            {
                ParameterName = "@Master_ID",
                Value = Masters[id]
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(masidParam);

            SqlParameter conidParam = new SqlParameter
            {
                ParameterName = "@Consumable_ID",
                Value = Consumables[id2]
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(conidParam);

            SqlParameter durParam = new SqlParameter
            {
                ParameterName = "@Duration_procedure",
                Value = textBox3.Text
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(durParam);

            SqlParameter costParam = new SqlParameter
            {
                ParameterName = "@Cost_procedure",
                Value = textBox5.Text
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(costParam);

            SqlParameter countParam = new SqlParameter
            {
                ParameterName = "@Count_consumables",
                Value = textBox4.Text
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(countParam);

            SqlParameter priceParam = new SqlParameter
            {
                ParameterName = "@Price_procedure",
                Value = (Convert.ToInt32(textBox4.Text)*Convert.ToDouble(textBox5.Text)*0.3+500)        //высчет стоимости процедуры(расходники+работа мастера)
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(priceParam);

            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Успешно добавлено!");
        }
        private void AddNamesProcedure()
        {
            string query = "InsertNamesProcedures";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            int id = listBox3.SelectedIndex;
            SqlParameter nameParam = new SqlParameter
            {
                ParameterName = "@Procedure_name",
                Value = textBox1.Text
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(nameParam);

            SqlParameter procidParam = new SqlParameter
            {
                ParameterName = "@Procedure_ID",
                Value = Procedures[id]
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(procidParam);          

            sqlCommand.ExecuteScalar();
            RefreshDataGrid2(dataGridView2);
            MessageBox.Show("Успешно добавлено!");
        }
        DataBase dataBase = new DataBase();
        private void CreateColumns()
        {
            dataGridView1.Columns.Add("Procedure_ID", "id_Процедуры");
            dataGridView1.Columns.Add("Master_ID", "id_Мастера");
            dataGridView1.Columns.Add("Consumable_ID", "id_Расходника");
            dataGridView1.Columns.Add("Duration_procedure", "Длительность процедуры");
            dataGridView1.Columns.Add("Cost_procedure", "Себестоимость процедуры");
            dataGridView1.Columns.Add("Count_consumables", "Количество расходников");
            dataGridView1.Columns.Add("Price_procedure", "Цена процедуры");

            dataGridView2.Columns.Add("Procedure_ID", "id_Процедуры");
            dataGridView2.Columns.Add("Procedure_name", "Название процедуры");
        }
        

        private void ReadSingleRow(DataGridView gridView, IDataRecord data)
        {
            gridView.Rows.Add(data.GetInt32(0), data.GetInt32(1), data.GetValue(2), data.GetInt32(3), data.GetValue(4), data.GetInt32(5), data.GetValue(6));
        }
        private void ReadSingleRow2(DataGridView gridView, IDataRecord data)
        {
            gridView.Rows.Add(data.GetInt32(0), data.GetString(1));
        }
        private void RefreshDataGrid(DataGridView gridView)
        {
            gridView.Rows.Clear();
            string query = "select * from Procedures_v";
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
        private void RefreshDataGrid2(DataGridView gridView)
        {
            gridView.Rows.Clear();
            string query = "select * from ProceduresNames_v";
            SqlCommand sqlCommand = new SqlCommand(query, dataBase.getConnection());
            //sqlCommand.CommandType = CommandType.StoredProcedure;
            dataBase.openConnection();
            SqlDataReader reader = sqlCommand.ExecuteReader();
            while (reader.Read())
            {
                ReadSingleRow2(gridView, reader);
            }
            reader.Close();
        }
        private void deletedRow()
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            dataBase.openConnection();
            var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
            var deleteQuery = "DeleteProcedures";
            SqlCommand sqlCommand = new SqlCommand(deleteQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter procParam = new SqlParameter
            {
                ParameterName = "@Procedure_ID",
                Value = id
            };
            sqlCommand.Parameters.Add(procParam);
            RefreshDataGrid(dataGridView1);
            sqlCommand.ExecuteScalar();
            MessageBox.Show("Строка удалена!");

        }
        private void deletedRow2()
        {
            int index = dataGridView2.CurrentCell.RowIndex;
            dataBase.openConnection();
            var id = Convert.ToInt32(dataGridView1.Rows[index].Cells[0].Value);
            var deleteQuery = "DeleteProceduresNames";
            SqlCommand sqlCommand = new SqlCommand(deleteQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter procParam = new SqlParameter
            {
                ParameterName = "@Procedure_ID",
                Value = id
            };
            sqlCommand.Parameters.Add(procParam);
            sqlCommand.ExecuteScalar();
            RefreshDataGrid2(dataGridView2);
            MessageBox.Show("Строка удалена!");


        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
            {
                e.Handled = true;
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number) && number != 8 && number != 44) // цифры, клавиша BackSpace и запятая
            {
                e.Handled = true;
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number) && number != 8)
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
            if (textBox3.Text != "" || textBox4.Text != "" || textBox5.Text != "" || listBox1.SelectedIndex != -1 || listBox2.SelectedIndex != -1)
            {
                AddProcedures();
            }
            else
                MessageBox.Show("Заполните все поля!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
             deletedRow();

        }

        private void Form13_Load(object sender, EventArgs e)
        {
            CreateColumns();
            RefreshDataGrid(dataGridView1);
            RefreshDataGrid2(dataGridView2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            deletedRow2();
        }
        int selectedRow;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //string id, id2;
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                if (row.Cells[1].Value.ToString() != "")
                {
                    for (int i = 0; i < Masters.Count; i++)
                        if (Convert.ToString(Masters[i]) == row.Cells[1].Value.ToString())
                            listBox1.SetSelected(i, true);
                }

                if (row.Cells[2].Value.ToString() != "")
                {
                    for (int i = 0; i < Consumables.Count; i++)
                        if (Convert.ToString(Consumables[i]) == row.Cells[2].Value.ToString())
                            listBox2.SetSelected(i, true);
                }
                textBox3.Text = row.Cells[3].Value.ToString();
                textBox5.Text = row.Cells[4].Value.ToString();
                textBox4.Text = row.Cells[5].Value.ToString();
            }
        }
        private void Change()
        {
            dataBase.openConnection();
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            var id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);
            int id_mas = listBox1.SelectedIndex;
            var mas_id = Masters[id_mas];
            int id_con = listBox2.SelectedIndex;
            var con_id = Consumables[id_con];
            var duration = textBox3.Text;
            var count = textBox4.Text;
            var cost = textBox5.Text;
            var price = dataGridView1.Rows[selectedRowIndex].Cells[6].Value;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(id, mas_id, con_id,duration,cost,count,price);
            }
            var changeQuery = "UpdateProc";
            SqlCommand sqlCommand = new SqlCommand(changeQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter masParam = new SqlParameter
            {
                ParameterName = "@Master_ID",
                Value = Masters[id_mas]
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(masParam);
            // параметр для ввода номера
            SqlParameter conParam = new SqlParameter
            {
                ParameterName = "@Consumable_ID",
                Value = Consumables[id_con]
            };

            sqlCommand.Parameters.Add(conParam);
            SqlParameter durParam = new SqlParameter
            {
                ParameterName = "@Duration_procedure",
                Value = duration
            };

            sqlCommand.Parameters.Add(durParam);

            SqlParameter costParam = new SqlParameter
            {
                ParameterName = "@Cost_procedure",
                Value = cost
            };
            sqlCommand.Parameters.Add(costParam);

            SqlParameter countParam = new SqlParameter
            {
                ParameterName = "@Count_consumables",
                Value = count
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(countParam);
            SqlParameter iddParam = new SqlParameter
            {
                ParameterName = "@Procedure_ID",
                Value = id
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(iddParam);
            SqlParameter priceParam = new SqlParameter
            {
                ParameterName = "@Price_procedure",
                Value = price
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(priceParam);

            sqlCommand.ExecuteScalar();
            RefreshDataGrid(dataGridView1);
            MessageBox.Show("Успешно изменено!");
            dataBase.closedConnection();

        }
        private void Change2()
        {
            dataBase.openConnection();
            var selectedRowIndex = dataGridView2.CurrentCell.RowIndex;
            var id = listBox3.SelectedItem;
            string name = textBox1.Text;

            if (dataGridView2.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView2.Rows[selectedRowIndex].SetValues(id,name);
            }
            var changeQuery = "UpdateNameProc";
            SqlCommand sqlCommand = new SqlCommand(changeQuery, dataBase.getConnection());
            sqlCommand.CommandType = CommandType.StoredProcedure;
            SqlParameter masParam = new SqlParameter
            {
                ParameterName = "@Procedure_ID",
                Value = id
            };
            // добавляем параметр
            sqlCommand.Parameters.Add(masParam);
            // параметр для ввода номера
            SqlParameter conParam = new SqlParameter
            {
                ParameterName = "@Procedure_name",
                Value = name
            };

            sqlCommand.Parameters.Add(conParam);

            sqlCommand.ExecuteScalar();
            MessageBox.Show("Успешно изменено!");
            dataBase.closedConnection();

        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            selectedRow = e.RowIndex;
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[selectedRow];
                for (int i = 0; i < Procedures.Count; i++)
                    if (Convert.ToString(Procedures[i]) == row.Cells[0].Value.ToString())
                        listBox3.SetSelected(i, true);
                textBox1.Text= row.Cells[1].Value.ToString();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Change();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Change2();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddNamesProcedure();
        }
    }
}
