using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;

namespace Consumo_de_Combustível
{
    public partial class Consumo_de_combustivel : Form
    {

        private SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\Database1.mdf;Integrated Security=True");
        private SqlCommand cmd = new SqlCommand();



        public Consumo_de_combustivel()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            display();
        }
        public void display() //MOSTRAR DADOS DA TABELA NO DATAGRIDVIEW
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();

            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = @"select * from [Table]";

            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            DataGridView1.DataSource = dt;
        }
        private void DataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) // PEGAR ID DO ROW SELECIONADO NO DATAGRIDVIEW
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();
            Int32 i;
            i = Convert.ToInt32(DataGridView1.SelectedCells[0].Value.ToString());

            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * from table where id = '" + i + "'";
            cmd.ExecuteNonQuery();

            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            SqlDataReader dr;
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read())
            {
                Label8.Text = Convert.ToString(dr.GetInt32(0));
                TextBox1.Text = Convert.ToString(dr.GetInt32(1));
                TextBox2.Text = Convert.ToString(dr.GetInt32(2));
                TextBox3.Text = Convert.ToString(dr.GetString(3));
                Label1.Text = Convert.ToString(dr.GetInt32(4));
            }




            // SE TEM COMBUSTIVEL, RODAR, SE NÃO, NÃO TEM COMBUSTIVEL

            if (int.Parse(Label1.Text) > 0)
            {
                Label7.Text = "rodar";
                Label7.ForeColor = Color.Green;
            }
            else
            {
                Label7.Text = "Sem combustivel";
                Label7.ForeColor = Color.Red;
            }
        }

        private void Button1_Click(object sender, EventArgs e) // INSERIR CARRO NA TABELA
        {
            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();

            int numero_de_serie = Convert.ToInt32(TextBox1.Text);
            int capacidade = Convert.ToInt32(TextBox2.Text);
            int litros = Convert.ToInt32("0");
            string portador = TextBox3.Text;


            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO [table](numero_de_serie, capacidade, portador, litros) " +
                "Values('" + numero_de_serie + "', '" + capacidade + "', '" + portador + "', '" + litros + "')";
            cmd.ExecuteNonQuery();
                        
                        
            display();

            TextBox1.Text = "";
            TextBox2.Text = "";
            TextBox3.Text = "";
        }

        private void Button2_Click(object sender, EventArgs e) //INSERIR CONSUMO DE COMBUSTIVEL + CALCULAR CONSUMO
        {
            Int32 litros;

            litros = int.Parse(Label1.Text);

            Int32 calculo;
            calculo = litros + int.Parse(TextBox4.Text) - int.Parse(TextBox5.Text);

            if (con.State == ConnectionState.Open)
                con.Close();
            con.Open();

            cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "update [table] set litros = '" + calculo + "' WHERE id = '" + Label8.Text + "'";
            cmd.ExecuteNonQuery();

            TextBox4.Text = "0";
            TextBox5.Text = "0";
            display();
        }

       
    }
}
