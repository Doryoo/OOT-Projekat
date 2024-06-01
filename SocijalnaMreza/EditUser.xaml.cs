using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        private Korisnik glavniKorisnik;
        public EditUser(Korisnik k)
        {
            InitializeComponent();
            EditProfileInfo.DataContext = k;
            glavniKorisnik = k;
        }

        private void Save_Edit_Profile_Click(object sender, RoutedEventArgs e)
        {
            bool isInputValidated = true;

            if (!DateOnly.TryParseExact(EditedBirthDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                MessageBox.Show("Nije dobar datum");
                isInputValidated = false;
            }
            if (EditedName.Text.Length == 0)
            {
                MessageBox.Show("Ime ne moze biti prazno");
                isInputValidated = false;
            }
            if (EditedSurname.Text.Length == 0)
            {
                MessageBox.Show("Prezime ne moze biti prazno");
                isInputValidated = false;
            }
            if (isInputValidated)
            {
                glavniKorisnik.Ime = EditedName.Text;
                glavniKorisnik.Prezime = EditedSurname.Text;
                glavniKorisnik.DatumRodjenja = DateOnly.ParseExact(EditedBirthDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                Korisnik.SaveUser(glavniKorisnik);
                MessageBox.Show("Profile updated successfully!");
                this.Close();
            }

        }
    }
}
