using System.Collections.ObjectModel;
using System.ComponentModel;
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
            
            //glavniKorisnik.DodajPrijatelja(drugiKorisnik);
            //glavniKorisnik.DodajPrijatelja(treci);
            //glavniKorisnik.DodajPrijatelja(cetvrti);

            ViewPostsGrid.ItemsSource = glavniKorisnik.getPosts();
            //SviPrijatelji.ItemsSource = glavniKorisnik.getFriends();
            
        }

        private void Toggle_Upload_Visibility_Click(object sender, RoutedEventArgs e)
        {
            if (NewPostUpload.Visibility == Visibility.Hidden)
            {
                NewPostUpload.Visibility = Visibility.Visible;
                UploadPostContent.Visibility = Visibility.Visible;
            } else
            {
                NewPostUpload.Visibility = Visibility.Hidden;
                UploadPostContent.Visibility = Visibility.Hidden;
            }
        }
        private void Upload_Post_Click(object sender, RoutedEventArgs e)
        {
            glavniKorisnik.dodajPost(UploadPostContent.Text);
            UploadPostContent.Text = "";
        }

        private void FriendClicked(object sender, RoutedEventArgs e)
        {

        }

        private void ViewPostsGridMreza_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Edit_Profile_Click(object sender, RoutedEventArgs e)
        {
            ProfileInfo.Visibility = Visibility.Hidden;
            EditProfileInfo.Visibility = Visibility.Visible;
        }

        private void Save_Edit_Profile_Click(object sender, RoutedEventArgs e)
        {
            NameSurname.Text = EditedNameSurname.Text;
            BirthDate.Text = EditedBirthDate.Text;

            ProfileInfo.Visibility = Visibility.Visible;
            EditProfileInfo.Visibility = Visibility.Hidden;
        }
    }
}