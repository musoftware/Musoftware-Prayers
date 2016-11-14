using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Musoftware_Prayers
{
    public partial class Frmmain : Form
    {
        public Frmmain()
        {
            InitializeComponent();
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            FrmAbout D = new FrmAbout();
            D.ShowDialog();
        }

        private void menuItem5_Click(object sender, EventArgs e)
        {
            FrmOption D = new FrmOption();
            D.ShowDialog();
        }

        private void menuItem2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Frmmain_Load(object sender, EventArgs e)
        {
            checkinstance();
            timer1_Tick(null, null);
            toolStripStatusLabel2.Text = "Program Loaded";
            toolStripStatusLabel4.Text = "تم تحميل البرنامج";
        }
        private void Re(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            { this.Visible = false; this.ShowInTaskbar = false; }

            if (this.WindowState == FormWindowState.Normal)
            { this.Visible = true; this.ShowInTaskbar = true; }
        }

        private void ShowAgain(object sender, MouseEventArgs e)
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
        }

        public void checkinstance()
        {
            Process[] thisnameprocesslist;
            string modulename, processname;
            Process p = Process.GetCurrentProcess();
            modulename = p.MainModule.ModuleName.ToString();
            processname = System.IO.Path.GetFileNameWithoutExtension(modulename);
            thisnameprocesslist = Process.GetProcessesByName(processname);
            if (thisnameprocesslist.Length > 1)
            {
                MessageBox.Show("Instance of this application is already running.");
                Application.Exit();
            }
        }
        Prayers.ClsPrayers Giza = new Prayers.ClsPrayers();

        private DateTime[] _prayers = new DateTime[5];
        Prayers.clsMultimedia AzanMultiMedia = new Prayers.clsMultimedia();


        private void timer1_Tick(object sender, EventArgs e)
        {
            
            Giza.Latitude = 29.8221;
            Giza.Longitude = 31.0554;
            Giza.Zone = 2;
            Giza.LocalDay = DateTime.Now.Day;
            Giza.LocalMonth = DateTime.Now.Month;
            Giza.LocalYear = DateTime.Now.Year;
            Giza.ArcEsha = -17.5;
            Giza.ArcFajer = -19.5;

            _prayers[0] = Convert.ToDateTime(Giza.Time_Fajer);
            _prayers[1] = Convert.ToDateTime(Giza.Time_Dohr);
            _prayers[2] = Convert.ToDateTime(Giza.Time_Asr_Shafee);
            _prayers[3] = Convert.ToDateTime(Giza.Time_Magrib);
            _prayers[4] = Convert.ToDateTime(Giza.Time_Esha);

            textBox1.Text = Convert.ToString(_prayers[0]);
            textBox2.Text = Convert.ToString(_prayers[1]);
            textBox3.Text = Convert.ToString(_prayers[2]);
            textBox4.Text = Convert.ToString(_prayers[3]);
            textBox5.Text = Convert.ToString(_prayers[4]);

            if (ChkTime(_prayers[1]) | ChkTime(_prayers[2]) | ChkTime(_prayers[3]) | ChkTime(_prayers[4]))
            {
                AzanMultiMedia.mmOpen(Path.GetDirectoryName(Application.ExecutablePath) + "\\Azan.mp3");
                AzanMultiMedia.mmPlay();
            }
            else if (ChkTime(_prayers[0]))
            {
                AzanMultiMedia.mmOpen(Path.GetDirectoryName(Application.ExecutablePath) + "\\AzanFajr.wav");
                AzanMultiMedia.mmPlay();
            }

            if (BtwnTime(_prayers[0]) | BtwnTime(_prayers[1]) | BtwnTime(_prayers[2]) | BtwnTime(_prayers[3]) | BtwnTime(_prayers[4]))
            {
                string Eng,Ara;

                Ara = "حان الان موعد اذان";
                if (BtwnTime(_prayers[0])) { Ara += " الفجر"; }
                if (BtwnTime(_prayers[1])) { Ara += " الظهر"; }
                if (BtwnTime(_prayers[2])) { Ara += " العصر"; }
                if (BtwnTime(_prayers[3])) { Ara += " المغرب"; }
                if (BtwnTime(_prayers[4])) { Ara += " العشاء"; }

                Eng = "Now the time has come";
                if (BtwnTime(_prayers[0])) { Eng += " Fajr"; }
                if (BtwnTime(_prayers[1])) { Eng += " Dohr"; }
                if (BtwnTime(_prayers[2])) { Eng += " Easr"; }
                if (BtwnTime(_prayers[3])) { Eng += " Magreb"; }
                if (BtwnTime(_prayers[4])) { Eng += " Esha"; }


                toolStripStatusLabel2.Text = Eng;
                toolStripStatusLabel4.Text = Ara;
            }
            else{

                toolStripStatusLabel2.Text = "";
                toolStripStatusLabel4.Text = "";

            }
            



        }

        bool ChkTime(DateTime va)
        {
            if (va.Hour == DateTime.Now.Hour)
                if (va.Minute == DateTime.Now.Minute)
                    if (va.Second == DateTime.Now.Second) return true;

            return false;
        }
        bool BtwnTime(DateTime va)
        {
            DateTime vai = va.AddSeconds(50);

            if (((DateTime.Now - va).TotalSeconds > 0) & ((DateTime.Now - vai).TotalSeconds < 0))
                return true;

            return false;
        }

 



    }
}
