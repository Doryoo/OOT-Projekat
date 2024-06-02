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
    /// Interaction logic for DodajPrijateljaWindow.xaml
    /// </summary>
    public partial class DodajPrijateljaWindow : Window
    {
        static List<Korisnik> allUsers;

        static Korisnik glavniKorisnik;

        public DodajPrijateljaWindow(List<Korisnik> AllUsers, Korisnik GlavniKorisnik)
        {
            InitializeComponent();
            allUsers = AllUsers;   
            glavniKorisnik = GlavniKorisnik;
        }

        private void dodajPrijatelja(object sender, RoutedEventArgs e)
        {
            boxIme.Text = boxIme.Text.Trim();
            boxPrezime.Text = boxPrezime.Text.Trim();
            boxId.Text = boxId.Text.Trim();
            if(boxIme.Text == "" && boxPrezime.Text == "" && boxId.Text == "")
            {
                MessageBox.Show("Morate uneti bar jedno polje!");
                return;
            }
            string[] s = { boxIme.Text, boxPrezime.Text, boxId.Text };
            if(glavniKorisnik.DodajPrijateljaV2(s, allUsers)){
                Korisnik.SaveUser(glavniKorisnik);
                MessageBox.Show("Uspešno ste dodali prijatelja!");
                boxId.Text = "";
                boxIme.Text = "";
                boxPrezime.Text = "";
                this.Hide();
            }
            else
            {
                Korisnik.SaveUser(glavniKorisnik);
                boxId.Text = "";
                boxIme.Text = "";
                boxPrezime.Text = "";
                MessageBox.Show("Ne postoji korisnik sa tim podacima!");
            }
        }
    }
}
