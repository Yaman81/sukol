﻿using System;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Sukol
{
    public partial class FormAna : Form
    {
        public int kullanici_id;
        Veritabani veritabani = new Veritabani();
        Gorevli gorevli = new Gorevli();
        public void gorevliGiris()
        {
            görevliToolStripMenuItem.Enabled = true;
            gorevli.erisim = true;
        }
        Ogretmen ogretmen = new Ogretmen();
        public void ogretmenGiris()
        {
            öğretmenToolStripMenuItem.Enabled = true;
            ogretmen.erisim = true;
        }
        Ogrenci ogrenci = new Ogrenci();
        public void ogrenciGiris(int numara)
        {
            öğrenciToolStripMenuItem.Enabled = true;
            ogrenci.erisim = true;
            ogrenci.numara = numara;
        }
        Kullanici kullanici = new Kullanici();
        public void kullaniciGiris(string isim, string soyisim, string profilfoto, bool ogrenci, bool ogretmen, bool gorevli)
        {
            kullanici.isim = isim;
            kullanici.soyisim = soyisim;

            labelIsim.Visible = true;
            labelIsimYazan.Text = isim;
            labelIsimYazan.Visible = true;
            labelSoyIsim.Visible = true;
            labelSoyIsimYazan.Text = soyisim;
            labelSoyIsimYazan.Visible = true;
            label_roller.Visible = true;
            string roller = "";
            if (ogrenci) roller += "öğrenci,";
            if (ogretmen) roller += "öğretmen,";
            if (gorevli) roller += "görevli,";
            roller = roller.Remove(roller.Length - 1);
            label_rollerYazan.Text = roller;
            label_rollerYazan.Visible = true;

            button_girisyap.Visible = false;
            button_cikisYap.Visible = true;

            kullanici.profilfoto = profilfoto;
            if (File.Exists(profilfoto)) pictureBox_profilFoto.Image = Image.FromFile(@profilfoto);
            else pictureBox_profilFoto.Image = null;

            button_profilFoto.Visible = true;
            pictureBox_profilFoto.Visible = true;
        }
        public FormAna()
        {
            InitializeComponent();

            new Loading(10, new string[] {"Bağlanıyor...", "Veritabanı yükleniyor", "Birşeyler oluyor", "Bitiyor...", "Bitmiyor"});

            panel_ana_sayfa.BringToFront();
        }

        private void girisyap_button_Click(object sender, EventArgs e)
        {
            FormGirisYap girisYap = new FormGirisYap();
            girisYap.Show();
        }

        private void websitemiz_button_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://sukol.herokuapp.com");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/sukol-dev/sukol");
        }

        private void button_cikisYap_Click(object sender, EventArgs e)
        {
            gorevli = new Gorevli();
            ogretmen = new Ogretmen();
            ogrenci = new Ogrenci();
            kullanici = new Kullanici();

            kullanici_id = 0;

            öğrenciToolStripMenuItem.Enabled = false;
            öğretmenToolStripMenuItem.Enabled = false;
            görevliToolStripMenuItem.Enabled = false;

            button_girisyap.Visible = true;
            button_cikisYap.Visible = false;

            labelIsim.Visible = false;
            labelIsimYazan.Visible = false;
            labelSoyIsim.Visible = false;
            labelSoyIsimYazan.Visible = false;
            label_roller.Visible = false;
            label_rollerYazan.Visible = false;

            button_profilFoto.Visible = false;
            pictureBox_profilFoto.Visible = false;
        }

        private void hakkimizda_button_Click(object sender, EventArgs e)
        {
            new AboutWindow("Unknown Productions", "Sukol", "1.0");
        }

        private void öğrenciToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_ogrenci.BringToFront();
        }

        private void anaSayfaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_ana_sayfa.BringToFront();
        }

        private void görevliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel_gorevli.BringToFront();
            string[] siniflar = gorevli.siniflar();
            listBox_siniflar.Items.Clear();
            for (int i = 0; i < siniflar.Length; i++)
                listBox_siniflar.Items.Add(siniflar[i]);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog_profilFoto.ShowDialog(this) == DialogResult.OK)
            {
                string path = openFileDialog_profilFoto.InitialDirectory + openFileDialog_profilFoto.FileName;
                veritabani.sorgu("UPDATE kullanicilar SET profilfoto=@profilfoto WHERE id=@kullanici_id");
                veritabani.parametreEkle("profilfoto", path);
                veritabani.parametreEkle("kullanici_id", kullanici_id.ToString());
                veritabani.baslat();
                veritabani.calistir();
                veritabani.kapat();

                kullanici.profilfoto = path;
                pictureBox_profilFoto.Image = Image.FromFile(@path);
            }
        }

        private void FormAna_Load(object sender, EventArgs e)
        {
            int[] dizi = new int[6];
            Random rnd = new Random();
            for (int i = 0; i < 6; i++)
            {
                dizi[i] = rnd.Next(1, 49);
            }
            Array.Sort(dizi);
            label_sansliSayi.Text = "Şanslı Sayılar:" + dizi[0].ToString() + "," + dizi[1].ToString() + "," + dizi[2].ToString() + "," + dizi[3].ToString() + "," + dizi[4].ToString() + "," + dizi[5].ToString();
        }

        private void button4_Click(object sender, EventArgs e)
        {

            int sayi1 = Convert.ToInt32(textBox1.Text);
            int sayi2 = Convert.ToInt32(textBox2.Text);
            double sonuc = 0;
            if (radioButton1.Checked)
                sonuc = sayi1 + sayi2;
            else if (radioButton2.Checked)
                sonuc = sayi1 - sayi2;
            else if (radioButton4.Checked)
                sonuc = sayi1 * sayi2;
            else if (radioButton3.Checked)
                if (sayi2 == 0)
                {
                    MessageBox.Show("Bölen 0 olamaz, başka bir rakam giriniz");
                    textBox2.Clear();
                }
                else
                    sonuc = sayi1 / sayi2;
            label_hesapmakSonuc.Text = "Sonuc:" + sonuc.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            new KullaniciEkleme.KullaniciEkle(gorevli).ShowDialog();
        }

        private void listBox_siniflar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox_siniflar.SelectedIndex == -1) return;
            listView_sinifOgrenci.Items.Clear();
            listView_sinifOgretmen.Items.Clear();
            listView_sinifOgrenci.Visible = true;
            listView_sinifOgretmen.Visible = true;
            label_sinifOgrenciListesi.Visible = true;
            label_sinifOgretmenListesi.Visible = true;
            label_sinifOgrenciListesi.Text = listBox_siniflar.SelectedItem.ToString() + " sınıfı öğrenci listesi";
            label_sinifOgretmenListesi.Text = listBox_siniflar.SelectedItem.ToString() + " sınıfı öğretmen listesi";

            veritabani.baslat();
            veritabani.sorgu("" +
                "select" +

                " kullanicilar.isim as isim," +
                " kullanicilar.soyisim as soyisim," +
                " kullanicilar.kullanici_adi as kullanici_adi," +
                " kullanicilar.sifre as sifre," +

                " ogrenciler.okul_no as okul_no" +

                " from kullanicilar" +
                " left join ogrenciler on kullanicilar.id=ogrenciler.kullanici_id" +

                " where ogrenciler.sinif=@sinif"
            );
            veritabani.parametreEkle("sinif", listBox_siniflar.SelectedItem.ToString());
            OleDbDataReader oku = veritabani.oku();
            while (oku.Read())
            {
                string[] bilgiler = {
                    oku["isim"].ToString()+" "+oku["soyisim"].ToString(),
                    oku["okul_no"].ToString(),
                    oku["kullanici_adi"].ToString(),
                    oku["sifre"].ToString()
                };
                ListViewItem list = new ListViewItem(bilgiler);
                listView_sinifOgrenci.Items.Add(list);
            }


            veritabani.sorgu("" +
                "select" +

                " kullanicilar.isim as isim," +
                " kullanicilar.soyisim as soyisim," +
                " kullanicilar.kullanici_adi as kullanici_adi," +
                " kullanicilar.sifre as sifre" +

                " from kullanicilar" +
                " left join ogretmenler on kullanicilar.id=ogretmenler.kullanici_id" +

                " where ogretmenler.sinif=@sinif"
            );
            veritabani.parametreEkle("sinif", listBox_siniflar.SelectedItem.ToString());
            oku = veritabani.oku();
            while (oku.Read())
            {
                string[] bilgiler = {
                    oku["isim"].ToString()+oku["soyisim"].ToString(),
                    oku["kullanici_adi"].ToString(),
                    oku["sifre"].ToString()
                };
                ListViewItem list = new ListViewItem(bilgiler);
                listView_sinifOgretmen.Items.Add(list);
            }
            veritabani.kapat();
        }

        private void button_YeniSinif_Click(object sender, EventArgs e)
        {
            if (textBox_yeniSinif.Text.Length > 2)
            {
                bool devam = true;
                veritabani.sorgu("" +
                    "select" +
                    " isim" +
                    " from siniflar" +
                    " where isim=@sinif"
                );
                veritabani.parametreEkle("sinif", textBox_yeniSinif.Text);
                veritabani.baslat();
                veritabani.calistir();
                OleDbDataReader oku = veritabani.oku();
                while (oku.Read())
                {
                    devam = false;
                }
                veritabani.kapat();
                if (!devam) { MessageBox.Show("Aynı isimde sınıf mevcut"); return; }
                veritabani.sorgu("INSERT INTO siniflar(isim) " +
                   "VALUES(@sinif);");
                veritabani.parametreEkle("sinif", textBox_yeniSinif.Text);
                veritabani.baslat();
                veritabani.calistir();
                veritabani.kapat();
                MessageBox.Show("Sınıf eklendi");
                textBox_yeniSinif.Text = "";
                listBox_siniflar.Items.Clear();
                string[] siniflar = gorevli.siniflar();
                listBox_siniflar.Items.Clear();
                for (int i = 0; i < siniflar.Length; i++)
                    listBox_siniflar.Items.Add(siniflar[i]);
            }
            else
            {
                MessageBox.Show("Daha uzun bir sınıf ismi girin");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (openFileDialog_radyo.ShowDialog(this) == DialogResult.OK)
            {
                string path = openFileDialog_radyo.InitialDirectory + openFileDialog_radyo.FileName;
                Properties.Settings.Default["okul_radyo"] = path;
                Properties.Settings.Default.Save();
            }
        }

        private void hesapMakinesiToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panel_hesapMakinesi.BringToFront();
        }

        private void okulRadyosuToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            panel_okulRadyo.BringToFront();
            if (File.Exists(Properties.Settings.Default["okul_radyo"].ToString())) axWindowsMediaPlayer1.URL = Properties.Settings.Default["okul_radyo"].ToString();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBar1.Value;
        }

        private void button_oynat_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void button_duraklat_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.Ctlcontrols.pause();
        }
    }
}
