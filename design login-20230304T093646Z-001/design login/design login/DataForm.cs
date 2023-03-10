using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace design_login
{
    public partial class DataForm : Form
    {

        MySqlConnection conn = connectionService.getConnection();
        DataTable dataTable = new DataTable();

        public DataForm()
        {
            InitializeComponent();
        }

        private void DataForm_Load(object sender, EventArgs e)
        {
            filldataTable();

            textBox7.Enabled = false;
            textBox6.Enabled = false;
            textBox5.Enabled = false;
            textBox1.Enabled = false;
        }

        public DataTable getNamaHewan()
        {
            dataTable.Reset();
            dataTable = new DataTable();
            using (MySqlCommand cmd = new MySqlCommand("SELECT * FROM toko", conn))
            {
                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();
                dataTable.Load(reader);
            }
            
            return dataTable;

        }

        public void filldataTable()
        {
            dgv_hewan.DataSource = getNamaHewan();

            DataGridViewButtonColumn colEdit = new DataGridViewButtonColumn();
            colEdit.UseColumnTextForButtonValue = true;
            colEdit.Text = "Edit";
            colEdit.Name = " ";
            dgv_hewan.Columns.Add(colEdit);

            DataGridViewButtonColumn colDelete = new DataGridViewButtonColumn();
            colDelete.UseColumnTextForButtonValue = true;
            colDelete.Text = "Delete";
            colDelete.Name = " ";
            dgv_hewan.Columns.Add(colDelete);
        }

        private void dgv_hp_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;

            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO toko(nama_barang, stok, harga) VALUE(@nama_barang, @stok, @harga)";
                cmd.Parameters.AddWithValue("@nama_barang", textBox2.Text);
                cmd.Parameters.AddWithValue("@stok", textBox3.Text);
                cmd.Parameters.AddWithValue("@harga", textBox4.Text);
                
                cmd.ExecuteNonQuery();

                conn.Close();

                dgv_hewan.Columns.Clear();
                dataTable.Clear();
                filldataTable();

                textBox7.Clear();
                textBox6.Clear();
                textBox5.Clear();
                textBox1.Clear();
            }
            catch (Exception ex)
            {

            }
        }

        private void dgv_hewan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == 4)
            {
                int id = Convert.ToInt32(dgv_hewan.CurrentCell.RowIndex.ToString());
                textBox7.Text = dgv_hewan.Rows[id].Cells[0].Value.ToString();
                textBox6.Text = dgv_hewan.Rows[id].Cells[1].Value.ToString();
                textBox5.Text = dgv_hewan.Rows[id].Cells[2].Value.ToString();
                textBox1.Text = dgv_hewan.Rows[id].Cells[3].Value.ToString();

                textBox7.Enabled = true;
                textBox6.Enabled = true;
                textBox5.Enabled = true;
                textBox1.Enabled = true;
            }

            if (e.ColumnIndex == 5)
            {
                int id = Convert.ToInt32(dgv_hewan.CurrentCell.RowIndex.ToString());

                MySqlCommand cmd;

                try
                {
                    cmd = conn.CreateCommand();
                    cmd.CommandText = "DELETE FROM toko WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", dgv_hewan.Rows[id].Cells[0].Value.ToString());
                    cmd.ExecuteNonQuery();

                    conn.Close();

                    dgv_hewan.Columns.Clear();
                    dataTable.Clear();
                    filldataTable();
                }
                
                catch (Exception ex)
                {

                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MySqlCommand cmd;

            try
            {
                cmd = conn.CreateCommand();
                cmd.CommandText = "UPDATE toko SET id = @id, nama_barang = @nama_barang, stok = @stok, harga = @harga WHERE id = @id";
                cmd.Parameters.AddWithValue("@id", textBox7.Text);
                cmd.Parameters.AddWithValue("@nama_barang", textBox6.Text);
                cmd.Parameters.AddWithValue("@stok", textBox5.Text);
                cmd.Parameters.AddWithValue("@harga", textBox1.Text);

                cmd.ExecuteNonQuery();

                conn.Close();

                dgv_hewan.Columns.Clear();
                dataTable.Clear();
                filldataTable();

                textBox7.Clear();
                textBox6.Clear();
                textBox5.Clear();
                textBox1.Clear();
            }
 
            catch (Exception ex)
            {

            }
        }

        public void searchData (string ValueToFind)
        {
            string searchQuery = "SELECT * FROM toko WHERE CONCAT(nama_barang,stok,harga) LIKE '%" + ValueToFind + "%'";
            MySqlDataAdapter adapter = new MySqlDataAdapter(searchQuery, conn);
            DataTable table = new DataTable();
            adapter.Fill(table);
            dgv_hewan.DataSource = table;
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            searchData(textBox8.Text);
        }
    }
}
