using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SocijalnaMreza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static Random idGen = new Random();
        Korisnik glavniKorisnik = new Korisnik(idGen.Next().ToString(), "Nemanja", "Vojnov", DateOnly.FromDateTime(DateTime.Now), null);
        Korisnik drugiKorisnik = new Korisnik(idGen.Next().ToString(),"Nikola", "Kovac",DateOnly.FromDateTime(DateTime.Now),null);
        Korisnik treci = new Korisnik(idGen.Next().ToString(),"random1", "kk",DateOnly.FromDateTime(DateTime.Now),null);
        Korisnik cetvrti = new Korisnik(idGen.Next().ToString(),"random2", "lol",DateOnly.FromDateTime(DateTime.Now),null);

        public MainWindow()
        {
            InitializeComponent();
            
            glavniKorisnik.dodajPost("cao svima");
            glavniKorisnik.dodajPost("cao svima");
            glavniKorisnik.dodajPost("cao svima");
            glavniKorisnik.dodajPost("cao svima");
            
            glavniKorisnik.DodajPrijatelja(drugiKorisnik);
            glavniKorisnik.DodajPrijatelja(treci);
            glavniKorisnik.DodajPrijatelja(cetvrti);

            ViewPostsGrid.ItemsSource = glavniKorisnik.getPosts();
            SviPrijatelji.ItemsSource = glavniKorisnik.getFriends();
            
        }

        private void Toggle_Upload_Visibility_Click(object sender, RoutedEventArgs e)
        {
            if (NewPost.Visibility == Visibility.Hidden)
            {
                NewPost.Visibility = Visibility.Visible;
            } else
            {
                NewPost.Visibility = Visibility.Hidden;
            }
        }
        private void Upload_Post_Click(object sender, RoutedEventArgs e)
        {
            glavniKorisnik.dodajPost(Upload_Post_Content.Text);
            Upload_Post_Content.Text = "";
        }
    }
}