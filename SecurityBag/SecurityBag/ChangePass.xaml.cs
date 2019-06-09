using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace SecurityBag
{
    /// <summary>
    /// Interaction logic for ChangePass.xaml
    /// </summary>
    public partial class ChangePass : Window
    {

        string user = "";
        SecurityBagEntities db = new SecurityBagEntities();
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void btnConfirm(object sender, RoutedEventArgs e)
        {
            try
            {
                SHA256 mySHA256 = SHA256.Create();

                var query = db.users.Where(x => x.username == user).First();
                byte[] bytes = Encoding.ASCII.GetBytes(txtPass.Password + query.salt);
                bytes = mySHA256.ComputeHash(bytes);
                string passOld = Encoding.ASCII.GetString(bytes);

                if (db.users.Where(a => a.username == user && a.password == passOld).Any())
                {
                    if (txtNewPass.Password == txtNewPass2.Password && txtNewPass.Password.Length > 0)
                    {


                        SHA256 mySHA256_2 = SHA256.Create();
                        Aes myAes = Aes.Create();
                        byte[] bytes_2 = Encoding.ASCII.GetBytes(txtPass.Password + txtPass.Password);
                        bytes_2 = mySHA256.ComputeHash(bytes_2);

                        var decryptor_2 = myAes.CreateDecryptor
                            (bytes_2,
                            System.Text.Encoding.Default.GetBytes("¬ÐL%¼(ü?5r%>­Oˆ"));

                        byte[] tempKey = decryptor_2.TransformFinalBlock(
                            System.Text.Encoding.Default.GetBytes(query.key),
                            0,
                            System.Text.Encoding.Default.GetBytes(query.key).Length);
                        byte[] tempIv = decryptor_2.TransformFinalBlock(
                            System.Text.Encoding.Default.GetBytes(query.iv),
                            0,
                            System.Text.Encoding.Default.GetBytes(query.iv).Length);

                        bytes_2 = Encoding.ASCII.GetBytes(txtNewPass.Password + txtNewPass.Password);
                        bytes_2 = mySHA256.ComputeHash(bytes_2);

                        var encryptor = myAes.CreateEncryptor
                           (bytes_2,
                           System.Text.Encoding.Default.GetBytes("¬ÐL%¼(ü?5r%>­Oˆ"));

                        byte[] keyUserByte = encryptor.TransformFinalBlock(tempKey, 0, tempKey.Length);
                        byte[] ivUserByte = encryptor.TransformFinalBlock(tempIv, 0, tempIv.Length);


                        string key_2 = System.Text.Encoding.Default.GetString(keyUserByte);
                        string iv_2 = System.Text.Encoding.Default.GetString(ivUserByte);


                        bytes = Encoding.ASCII.GetBytes(txtNewPass.Password + query.salt);
                        bytes = mySHA256.ComputeHash(bytes);
                        string pass = Encoding.ASCII.GetString(bytes);
                        var user1 = db.users.Where(a => a.username == user).FirstOrDefault();
                        user1.password = pass;
                        user1.key = key_2;
                        user1.iv = iv_2;
                        db.SaveChanges();
                        MessageBox.Show("رمز با موفقیت تغییر گردید");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("در رمز عبور جدید دقت کنید");
                    }
                }
                else
                {
                    MessageBox.Show("خطا");
                }
            }
            catch
            {
                MessageBox.Show("خطا در اتصال");
            }


        }
        public ChangePass(string s)
        {
            user = s;

            InitializeComponent();
        }



        private void btnCancel(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
