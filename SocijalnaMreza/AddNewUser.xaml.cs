using System.Windows;

namespace SocijalnaMreza
{
    /// <summary>
    /// Interaction logic for AddNewUser.xaml
    /// </summary>
    public partial class AddNewUser : Window
    {
        static List<Korisnik> allUsers;
        static List<string> allIDs;
        static Random idGen = new Random();

        public AddNewUser(List<Korisnik> AllUsers, List<string> AllIDs)
        {
            InitializeComponent();
            allUsers = AllUsers;
            allIDs = AllIDs;
        }

        private void dodajPrijatelja(object sender, RoutedEventArgs e)
        {
            //boxIme, boxPrezime
            if (boxIme.Text == "" || boxPrezime.Text == "")
            {
                MessageBox.Show("Morate uneti ime i prezime!");
                return;
            }
            Korisnik k = new Korisnik(GenerateNewUniqueID(),boxIme.Text, boxPrezime.Text);
            allUsers.Add(k);
            allIDs.Add(k.Id);
            Korisnik.SaveUser(k);
            boxIme.Text = "";
            boxPrezime.Text = "";
            this.Close();
            MessageBox.Show("Uspešno ste dodali korisnika!");
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
