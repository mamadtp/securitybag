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
    /// Interaction logic for SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {


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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (db.users.Where(x => x.username == txtUser.Text).Any())
                {
                    MessageBox.Show("این نام کاربری قبلا ثبت نام کرده است");
                }
                else
                {
                    if (txtPass1.Password == txtPass2.Password && txtPass1.Password.Length > 0)
                    {

                        SHA256 mySHA256 = SHA256.Create();

                        Aes myAes = Aes.Create();


                        byte[] bytes = Encoding.ASCII.GetBytes(txtPass1.Password + txtPass1.Password);
                        bytes = mySHA256.ComputeHash(bytes);


                        var encryptor = myAes.CreateEncryptor
                            (bytes,
                            System.Text.Encoding.Default.GetBytes("¬ÐL%¼(ü?5r%>­Oˆ"));


                        byte[] keyUserByte = encryptor.TransformFinalBlock(myAes.Key, 0, myAes.Key.Length);
                        byte[] ivUserByte = encryptor.TransformFinalBlock(myAes.IV, 0, myAes.IV.Length);

                        string key = System.Text.Encoding.Default.GetString(keyUserByte);
                        string iv = System.Text.Encoding.Default.GetString(ivUserByte);


                        string salt = RandomString(30, true);
                        bytes = Encoding.ASCII.GetBytes(txtPass1.Password + salt);
                        bytes = mySHA256.ComputeHash(bytes);
                        string pass = Encoding.ASCII.GetString(bytes);
                        db.insertUser(txtUser.Text, pass, key, iv, salt);
                        db.SaveChanges();

                        MessageBox.Show("عملیات با موفقیت به اتمام رسید");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("در رمز عبور  دقت کنید");
                    }
                }
            }
            catch
            {
                MessageBox.Show("خطا در اتصال");
            }

        }
        public SignUp()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 1; i < size + 1; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            else
                return builder.ToString();
        }
    }
}
