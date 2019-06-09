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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace SecurityBag
{


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
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
                SHA256 mySHA256 = SHA256.Create();
                var query = db.users.Where(x => x.username == txtUser.Text).First();
                byte[] bytes1 = Encoding.ASCII.GetBytes(txtPass.Password + query.salt);
                byte[] bytes2 = mySHA256.ComputeHash(bytes1);
                string pass = Encoding.ASCII.GetString(bytes2);

                if (db.users.Where(x => x.username == txtUser.Text && x.password == pass).Any())
                {
                    new Main(txtUser.Text,txtPass.Password).Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("نام کاربری یا رمز عبور اشتباه است");
                }
            }
            catch
            {
                MessageBox.Show("خطا در اتصال");
            }


        }
        public MainWindow()
        {


            InitializeComponent();

        }

        private void lblSignUp(object sender, MouseButtonEventArgs e)
        {
            new SignUp().Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
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
