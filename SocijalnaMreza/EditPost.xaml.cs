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
    /// Interaction logic for EditPost.xaml
    /// </summary>
    public partial class EditPost : Window
    {
        private Post _originalPost;
        private Korisnik _glavniKorisnik;

        public EditPost(Post post, Korisnik korisnik)
        {
            InitializeComponent();
            _originalPost = post;
            _glavniKorisnik = korisnik;

            EditContentTextBox.Text = _originalPost.Sadrzaj;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (EditContentTextBox.Text == _originalPost.Sadrzaj)
            {
                MessageBox.Show("Neophodno je uneti promenu");
            }
            else if (EditContentTextBox.Text == "")
            {
                MessageBox.Show("Promena ne sme biti prazna");
            }
            else
            {
                _glavniKorisnik.editujPost(_originalPost, EditContentTextBox.Text);
                Korisnik.SaveUser(_glavniKorisnik);
                this.Close();
            }
            
        }
    }
}
