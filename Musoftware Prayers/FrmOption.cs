using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Permissions;
using System.IO;
[assembly: RegistryPermissionAttribute(SecurityAction.RequestMinimum,
    All = "HKEY_CURRENT_USER")]

namespace Musoftware_Prayers
{
    public partial class FrmOption : Form
    {
        public FrmOption()
        {
            InitializeComponent();
        }
     
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            RegistryKey MyKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\run", true);
            string filename = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MUSPrayer\MUSPrayer.exe";
            string Dirname = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MUSPrayer";
            string PDirname = Path.GetDirectoryName(Application.ExecutablePath);


            

            if (checkBox1.Checked == false)
            {
                try { MyKey.DeleteValue("MUSPrayer"); }
                catch (System.Exception) { }

                try { File.Delete(filename); }
                catch (System.Exception) { }

                return;
            }


            
            System.Security.Permissions.FileIOPermission permission = new
            System.Security.Permissions.FileIOPermission(
            System.Security.Permissions.FileIOPermissionAccess.Write,
            filename);

            try { Directory.CreateDirectory(Dirname); }
            catch (System.Exception) { }
            try { File.Delete(filename); }
            catch (System.Exception) { }
            try
            { File.Copy(Application.ExecutablePath, filename); }
            catch (System.Exception) { }
            try
            { File.Copy(PDirname + @"\Prayers.dll", Dirname + @"\Prayers.dll"); }
            catch (System.Exception) { }
            try
            { File.Copy(PDirname + @"\Azan.mp3", Dirname + @"\Azan.mp3"); }
            catch (System.Exception) { }
            try
            { File.Copy(PDirname + @"\AzanFajr.wav", Dirname + @"\AzanFajr.wav"); }
            catch (System.Exception) { }



            MyKey.SetValue("MUSPrayer", filename + " /hide");

        }

        private void FrmOption_Load(object sender, EventArgs e)
        {
            RegistryKey MyKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\run");
            try
            {
                if (!(MyKey.GetValue("MUSPrayer") == null))
                    checkBox1.Checked = true;

            }
            catch (System.Exception)
            {
            }
        }
    }
}
