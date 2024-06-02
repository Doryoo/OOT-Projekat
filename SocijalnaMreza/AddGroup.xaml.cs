using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddGroup.xaml
    /// </summary>
    public partial class AddGroup : Window
    {
        static Random idGen = new Random();
        static ObservableCollection<Grupa> sveGrupe;
        static List<string> allIDs;
        Korisnik glavniKorisnik;
        public AddGroup(ObservableCollection<Grupa> SveGrupe, List<string> AllIDs, Korisnik GlavniKorisnik)
        {
            InitializeComponent();
            sveGrupe = SveGrupe;
            allIDs = AllIDs;
            glavniKorisnik = GlavniKorisnik;
        }

        private void dodajGrupu(object sender, RoutedEventArgs e)
        {
            boxIme.Text = boxIme.Text.Trim();
            boxID.Text = boxID.Text.Trim();
            if (boxIme.Text == "" && boxID.Text == "")
            {
                MessageBox.Show("Morate uneti bar jedno polje!");
                return;
            }

            bool uspeh = true;
            foreach (var item in sveGrupe)
            {
                if (item.Id == boxID.Text || item.Naziv.Contains(boxIme.Text) && boxIme.Text != "" || boxIme.Text.Contains(item.Naziv) && boxIme.Text != "")
                {
                    item.DodajClana(glavniKorisnik);
                    Grupa.SaveGroup(item);
                    MessageBox.Show(boxID.Text + " " + item.Id);
                    uspeh = false;
                }
            }
            if(uspeh)
                MessageBox.Show("Vasa grupa nije nadjena");
            //treba dodati da se cuva korisnik

            this.Close();
        }
        static public string GenerateNewUniqueID()
        {
            string newID = idGen.Next(100000, 1000000).ToString();
            while (allIDs.Contains(newID))
            {
                newID = idGen.Next(100000, 1000000).ToString();
            }
            allIDs.Add(newID);
            return newID;
        }
    }
}
