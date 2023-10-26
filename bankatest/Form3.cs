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


namespace bankatest
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 fr = new Form1();
            fr.Show();
            this.Hide();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-KPC6PV7\SQLEXPRESS;Initial Catalog=dbbankatest;Integrated Security=True");
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnkaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into tblkisiler (ad,soyad,tc,telefon,hesapno,sifre) values(@p1,@p2,@p3,@p4,@p5,@p6)", baglanti);
            komut.Parameters.AddWithValue("@p1", txtad.Text);
            komut.Parameters.AddWithValue("@p2", txtsoyad.Text);
            komut.Parameters.AddWithValue("@p3", msktc.Text);
            komut.Parameters.AddWithValue("@p4", msktelefon.Text);
            komut.Parameters.AddWithValue("@p5", mskhesapno.Text);
            komut.Parameters.AddWithValue("@p6", txtsifre.Text);

            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("KAYIT İŞLEMİ BAŞARILI");
            
            //hesap veri tabanı aktarma 

            baglanti.Open();
            SqlCommand hsp = new SqlCommand("insert into tblhesap (hesapno,bakiye)values(@m1,@m2)",baglanti);
            hsp.Parameters.AddWithValue("@m1",mskhesapno.Text);
            hsp.Parameters.AddWithValue("@m2",0);
            hsp.ExecuteNonQuery();
            baglanti.Close();

        }

        private void btnhesapno_Click(object sender, EventArgs e)
        {
            Random rasgele = new Random();
            int sayi = rasgele.Next(100000, 1000000);
            mskhesapno.Text = sayi.ToString();  
        }
    }
}
