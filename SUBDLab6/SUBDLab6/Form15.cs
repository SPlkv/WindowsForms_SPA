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
    enum RowState
    {
        Existed,
        New,
        Modifed,
        ModifiedNew,
        Deleted
    }
    public partial class Form15 : Form
    {
        DataBase dataBase = new DataBase();

        int selectedRow;
        public Form15()
        {
            InitializeComponent();
        }

        private void CreateColumns()
        {
            //dataGridView1.Columns.Add("Entry_ID", "id_ЗАПИСИ");
            //dataGridView1.Columns.Add("DateOfEntry", "Дата появления записи");
            dataGridView1.Columns.Add("DateOfProcedure", "Дата процедуры");
            dataGridView1.Columns.Add("Full_name", "ФИО клиента");
            dataGridView1.Columns.Add("Procedure_name", "Название процедуры");
        }
        
        private void ReadSingleRow(DataGridView gridView,IDataRecord data)
        {
            gridView.Rows.Add(//data.GetInt32(0), //data.GetDateTime(1),
                                                data.GetDateTime(0), data.GetValue(1), data.GetString(2));
        }

        private void RefreshDataGrid(DataGridView gridView)
        {
            gridView.Rows.Clear();
            string query = "select DateOfProcedure,Full_name,Procedure_name from Entries_v";
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
        
        private void Form15_Load(object sender, EventArgs e)
        {
            


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "")
            {
                CreateColumns();
                RefreshDataGrid(dataGridView1);
                string name = textBox1.Text;
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    var s = dataGridView1.Rows[i].Cells[1].Value.ToString();
                    if (dataGridView1.Rows[i].Cells[1].Value.ToString() != name)
                        dataGridView1.Rows[i].Visible = false;
                }
            }
            else
                MessageBox.Show("Введите свое ФИО");
        }
    }
}
