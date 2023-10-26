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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-KPC6PV7\SQLEXPRESS;Initial Catalog=dbbankatest;Integrated Security=True");

        public string hesap;
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Form1 frm =new Form1();
            frm.Show();
            this.Hide();
        }
        int bakiye = 0;
        int havale = 0;
        private void Form2_Load(object sender, EventArgs e)
        {
            //HESAP HAREKETLERİNİ CEKME 
            baglanti.Open();
            SqlCommand comm = new SqlCommand("select gonderen,alıcı from tblhareket where gonderen=@y1", baglanti);
            comm.Parameters.AddWithValue("@y1",hesap);
            SqlDataAdapter da = new SqlDataAdapter(comm);
            DataTable ds = new DataTable();
            da.Fill(ds);
            dataGridView1.DataSource = ds;

            baglanti.Close();

            lblhesapno.Text = hesap;

            baglanti.Open();
            SqlCommand komut= new SqlCommand("select *from tblkisiler where hesapno=@p1",baglanti);
            komut.Parameters.AddWithValue("@p1",hesap);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                lbladsoyad.Text = dr[1] + " " + dr[2];
                lbltcno.Text = dr[3].ToString();
                lbltelefon.Text = dr[4].ToString();

            }
            baglanti.Close();
            //bakiye sorgulama
            baglanti.Open();
            SqlCommand kmt = new SqlCommand("select *from tblhesap where hesapno=@p1", baglanti);
            kmt.Parameters.AddWithValue("@p1", hesap);
            SqlDataReader dataReader = kmt.ExecuteReader();
            if (dataReader.Read())
            {
                bakiye = Convert.ToInt32(dataReader[1]);
            }
            baglanti.Close();

            

            LBLBAKİYE.Text = Convert.ToString(bakiye);
            label6.Text = Convert.ToString(havale);

        }

        private void btngonder_Click(object sender, EventArgs e)
        {
            havale = Convert.ToInt32(txttutar.Text);
            
            //bakiye sorgulama
            baglanti.Open();
            SqlCommand kmt = new SqlCommand("select *from tblhesap where hesapno=@p1",baglanti);
            kmt.Parameters.AddWithValue("@p1",hesap);
            SqlDataReader dr = kmt.ExecuteReader();
            if (dr.Read())
            {
                bakiye = Convert.ToInt32(dr[1]);
            }
            baglanti.Close();
            
            havale = Convert.ToInt32(txttutar.Text);

            LBLBAKİYE.Text = Convert.ToString(bakiye);
            label6.Text=Convert.ToString(havale);

            // sorgulama işlemi 
               if(havale<=bakiye)
                {
                    baglanti.Close();
                    // gönderilen hesabın para artışı
                    baglanti.Open();
                    SqlCommand komut = new SqlCommand("update tblhesap set bakiye=bakiye+@p1 where hesapno=@p2 ", baglanti);
                    komut.Parameters.AddWithValue("@p1", decimal.Parse(txttutar.Text));
                    komut.Parameters.AddWithValue("@p2", mskhesapno.Text);
                    komut.ExecuteNonQuery();
                    baglanti.Close();

                    //gönderen hesabın para azalışı 
                    baglanti.Open();
                    SqlCommand komut2 = new SqlCommand("update tblhesap set bakiye=bakiye-@k1 where hesapno=@k2", baglanti);
                    komut2.Parameters.AddWithValue("@k1", decimal.Parse(txttutar.Text));
                    komut2.Parameters.AddWithValue("@k2", hesap);
                    komut2.ExecuteNonQuery();
                    baglanti.Close();
                    MessageBox.Show("İŞLEM GERÇEKLEŞTİ", "BİLGİLENDİRME", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    {
                        MessageBox.Show("YETERSİZ BAKİYE","UYARI",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
            //bakiye sorgulama
            baglanti.Open();
            SqlCommand sorgu = new SqlCommand("select *from tblhesap where hesapno=@p1", baglanti);
            sorgu.Parameters.AddWithValue("@p1", hesap);
            SqlDataReader dataR = sorgu.ExecuteReader();
            if (dataR.Read())
            {
                bakiye = Convert.ToInt32(dataR[1]);
            }
            baglanti.Close();

            LBLBAKİYE.Text = bakiye.ToString();

            //İŞLEM GEÇMİŞİ 
            baglanti.Open();
            SqlCommand com = new SqlCommand("insert into tblhareket (gonderen,alıcı,tutar) values (@p1,@p2,@p3)", baglanti);
            com.Parameters.AddWithValue("@p1",lblhesapno.Text);
            com.Parameters.AddWithValue("@p2",mskhesapno.Text);
            com.Parameters.AddWithValue("@p3",txttutar.Text);
            com.ExecuteNonQuery();
            baglanti.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Size = new Size(318, 584);
        }
    }
}
