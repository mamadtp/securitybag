using Microsoft.Win32;
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
using System.IO;

namespace SecurityBag
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        string user = "";
        string password = "";
        SecurityBagEntities db = new SecurityBagEntities();

        byte[] key = null;
        byte[] iv = null;

        public Main(string s,string pass)
        {
            user = s;
            password = pass;
            InitializeComponent();
            refresh();

            string keyStr = null;
            string ivStr = null;

            //get key from data base
            var u = db.users.Where(a => a.username == user).FirstOrDefault();
            keyStr = u.key;
            ivStr = u.iv;
            //and decrypt it by main key
            Aes myAes = Aes.Create();
            SHA256 mySHA256 = SHA256.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(password + password);
            bytes = mySHA256.ComputeHash(bytes);
            var decryptor = myAes.CreateDecryptor
                            (bytes,
                            System.Text.Encoding.Default.GetBytes("¬ÐL%¼(ü?5r%>­Oˆ"));

            key = decryptor.TransformFinalBlock(
                System.Text.Encoding.Default.GetBytes(keyStr), 0, System.Text.Encoding.Default.GetBytes(keyStr).Length);
            iv = decryptor.TransformFinalBlock(
                System.Text.Encoding.Default.GetBytes(ivStr), 0, System.Text.Encoding.Default.GetBytes(ivStr).Length);

        }


        private void menuitem3_Click(object sender, RoutedEventArgs e)
        {
            new ChangePass(user).Show();
        }

        private void btndel(object sender, RoutedEventArgs e)
        {
            object item = datagrid.SelectedItem;
            int id = int.Parse((datagrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text);
            if (File.Exists("D:\\securitybag\\" + id ))
            {
                File.Delete("D:\\securitybag\\" + id );
                MessageBox.Show("فایل حذف شد");
            }
            else
            {
                MessageBox.Show("فایل توسط شخص دیگری حذف شده است");
            }


            db.deleteFile(id);


            refresh();
        }

        private void btnDec(object sender, RoutedEventArgs e)
        {
            Aes myAes = Aes.Create();
            SHA256 mySHA256 = SHA256.Create();
            var decryptor = myAes.CreateDecryptor(key, iv);
            object item = datagrid.SelectedItem;
            int id = int.Parse((datagrid.SelectedCells[1].Column.GetCellContent(item) as TextBlock).Text);



            var query = db.files.Where(x => x.Id == id).FirstOrDefault();

            string temp = query.iv;
            byte[] fileIV = System.Text.Encoding.Default.GetBytes(temp);
            fileIV = decryptor.TransformFinalBlock(fileIV, 0, fileIV.Length);

            decryptor = myAes.CreateDecryptor(key, fileIV);



            if (File.Exists("D:\\securitybag\\" + id ))
            {

                byte[] file = File.ReadAllBytes("D:\\securitybag\\" + id);


                try
                {
                    byte[] decFile = decryptor.TransformFinalBlock(file, 0, file.Length);
                    byte[] hash = mySHA256.ComputeHash(decFile);

                    string strHash = System.Text.Encoding.Default.GetString(hash);

                    if (db.files.Where(x => x.Id == id && x.hash == strHash).Any())
                    {
                        SaveFileDialog save = new SaveFileDialog();
                        if (save.ShowDialog() == true)
                        {
                            File.WriteAllBytes(save.FileName, decFile);
                            MessageBox.Show("فایل با موفقیت رمزگشایی شد");
                        }
                    }
                    else
                    {
                        MessageBox.Show("فایل توسط شخص دیگری خراب است");
                    }
                }
                catch
                {
                    MessageBox.Show("فایل توسط شخص دیگری خراب است");
                }
                    
  

            }
            else
            {
                MessageBox.Show("فایل توسط شخص دیگری حذف شده است");
            }
        }




        private void btnAdd(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    //encrypt file with user key and new iv
                    string strTime = DateTime.Now.ToString("HH:mm:ss");
                    string strDate = DateTime.Today.ToString("dd/MM/yyyy");

                    Aes myAes = Aes.Create();
                    SHA256 mySHA256 = SHA256.Create();
                    byte[] fileIV = myAes.IV;
                    string strFileIV = System.Text.Encoding.Default.GetString(fileIV);

                    var encryptor = myAes.CreateEncryptor(key, fileIV);

                    byte[] file = File.ReadAllBytes(openFileDialog.FileName);
                    byte[] hashedFile = mySHA256.ComputeHash(file);
                    byte[] encFile = encryptor.TransformFinalBlock(file, 0, file.Length);

                    //encrypt detail for database with key user and defualt iv
                    encryptor = myAes.CreateEncryptor(key, iv);
                    byte[] userEnc = encryptor.TransformFinalBlock(
                        System.Text.Encoding.Default.GetBytes(user), 0, System.Text.Encoding.Default.GetBytes(user).Length);

                    byte[] nameEnc = encryptor.TransformFinalBlock(
                        System.Text.Encoding.Default.GetBytes(System.IO.Path.GetFileName(openFileDialog.FileName))
                        , 0
                        , System.Text.Encoding.Default.GetBytes(System.IO.Path.GetFileName(openFileDialog.FileName)).Length);

                    byte[] timeEnc = encryptor.TransformFinalBlock(
                        System.Text.Encoding.Default.GetBytes(strTime), 0, System.Text.Encoding.Default.GetBytes(strTime).Length);
                    byte[] dateEnc = encryptor.TransformFinalBlock(
                        System.Text.Encoding.Default.GetBytes(strDate), 0, System.Text.Encoding.Default.GetBytes(strDate).Length);

                    byte[] ivEnc = encryptor.TransformFinalBlock(
                        System.Text.Encoding.Default.GetBytes(strFileIV), 0, System.Text.Encoding.Default.GetBytes(strFileIV).Length);


                    string strUser = System.Text.Encoding.Default.GetString(userEnc);
                    string strName = System.Text.Encoding.Default.GetString(nameEnc);
                    strTime = System.Text.Encoding.Default.GetString(timeEnc);
                    strDate = System.Text.Encoding.Default.GetString(dateEnc);
                    string hash = System.Text.Encoding.Default.GetString(hashedFile);

                    string strIV = System.Text.Encoding.Default.GetString(ivEnc);
                    //and insert detail into files table
                    db.insertFile(strUser, strName.ToString(), strTime.ToString(), strDate.ToString(), strIV, hash);

                    var files = db.files.OrderByDescending(x => x.Id).First();

                    //save encrypt file
                    File.WriteAllBytes("D:\\securitybag\\" + files.Id , encFile);




                    MessageBox.Show("فایل رمزگزاری شد");
                    refresh();
                }
                catch
                {
                    MessageBox.Show("خطا در رمزگزاری");
                }


            }
        }

        private void menuitem10_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("ایا کاربر حذف شود؟", "تایید حذف کاربر", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Aes myAes = Aes.Create();
                var encryptor = myAes.CreateEncryptor(key, iv);

                byte[] vs = encryptor.TransformFinalBlock(
                    System.Text.Encoding.Default.GetBytes(user), 0, System.Text.Encoding.Default.GetBytes(user).Length);

                string userEnc = System.Text.Encoding.Default.GetString(vs);

                var query = db.files.Where(x => x.username == userEnc).ToList();

                foreach (var temp in query)
                {
                    if (File.Exists("D:\\securitybag\\" + temp.Id + ".txt"))
                    {
                        File.Delete("D:\\securitybag\\" + temp.Id + ".txt");
                    }
                    db.deleteFile(temp.Id);
                }
                db.delteUser(user);

                new MainWindow().Show();
                this.Close();
            }
        }

        private void refresh()
        {

            string keyStr = null;
            string ivStr = null;

            var u = db.users.Where(a => a.username == user).FirstOrDefault();
            keyStr = u.key;
            ivStr = u.iv;
            //and decrypt it by main key
            Aes myAes = Aes.Create();
            SHA256 mySHA256_2 = SHA256.Create();

            byte[] bytes_2 = Encoding.ASCII.GetBytes(password + password);
            bytes_2 = mySHA256_2.ComputeHash(bytes_2);

            var decryptor = myAes.CreateDecryptor
                            (bytes_2,
                            System.Text.Encoding.Default.GetBytes("¬ÐL%¼(ü?5r%>­Oˆ"));

            key = decryptor.TransformFinalBlock(
                System.Text.Encoding.Default.GetBytes(keyStr), 0, System.Text.Encoding.Default.GetBytes(keyStr).Length);
            iv = decryptor.TransformFinalBlock(
                System.Text.Encoding.Default.GetBytes(ivStr), 0, System.Text.Encoding.Default.GetBytes(ivStr).Length);


            var encryptor = myAes.CreateEncryptor(key, iv);

            byte[] vs = encryptor.TransformFinalBlock(
                System.Text.Encoding.Default.GetBytes(user), 0, System.Text.Encoding.Default.GetBytes(user).Length);



            decryptor = myAes.CreateDecryptor(key, iv);
            string temp = System.Text.Encoding.Default.GetString(vs);

            var list = db.files.Where(a => a.username == temp).ToList();
            List<file> help = list;
            foreach (var q in help)
            {

                Aes myAes2 = Aes.Create();
                decryptor = myAes2.CreateDecryptor(key, iv);
                try
                {
                    byte[] col3 = decryptor.TransformFinalBlock(System.Text.Encoding.Default.GetBytes(q.namefile), 0, q.namefile.Length);
                    q.namefile = System.Text.Encoding.Default.GetString(col3);
                }
                catch
                {

                }

                decryptor = myAes2.CreateDecryptor(key, iv);
                try
                {
                    byte[] col4 = decryptor.TransformFinalBlock(
                   System.Text.Encoding.Default.GetBytes(q.time.ToString()), 0, q.time.Length);
                    q.time = System.Text.Encoding.Default.GetString(col4);
                }
                catch
                {

                }
                decryptor = myAes2.CreateDecryptor(key, iv);
                try
                {
                    byte[] col5 = decryptor.TransformFinalBlock(
                   System.Text.Encoding.Default.GetBytes(q.date.ToString()), 0, q.date.Length);
                    q.date = System.Text.Encoding.Default.GetString(col5);
                }
                catch
                {

                }



            }

            datagrid.ItemsSource = list;

        }

        private void menuitem11_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
