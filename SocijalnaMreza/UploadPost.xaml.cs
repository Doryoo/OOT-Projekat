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

namespace SocijalnaMreza
{
    /// <summary>
    /// Interaction logic for UploadPost.xaml
    /// </summary>
    public partial class UploadPost : Window
    {
        private Korisnik glavniKorisnik;
        public UploadPost(Korisnik korisnik)
        {
            InitializeComponent();
            glavniKorisnik = korisnik;
        }
        private void Upload_Post_Click(object sender, RoutedEventArgs e)
        {
            if (UploadPostContent.Text.Length > 0)
            {
                glavniKorisnik.dodajPost(UploadPostContent.Text);
                Korisnik.SaveUser(glavniKorisnik);
                MessageBox.Show("Post successfully uploaded!");
                UploadPostContent.Text = "";
                this.Close();
            }
            else
            {
                MessageBox.Show("Ne moze se okaciti prazan post");
            }
        }
    }
}
