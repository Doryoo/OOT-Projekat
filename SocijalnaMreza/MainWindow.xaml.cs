using Accessibility;
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
using System.Windows.Media.TextFormatting;
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
        ObservableCollection<Korisnik> mreza = new ObservableCollection<Korisnik>();


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
            mreza.Add(glavniKorisnik);
            ViewPostsGrid.ItemsSource = glavniKorisnik.getPosts();
            SviPrijatelji.ItemsSource = mreza;
            
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
            NewPostUpload.Visibility = Visibility.Hidden;
            UploadPostContent.Visibility = Visibility.Hidden;
        }

        private void FriendClicked(object sender, RoutedEventArgs e)
        {


            var tmp = SviPrijatelji.SelectedItem as Korisnik;

            if (tmp != null)
            {
                ViewPostsGridMreza.ItemsSource = tmp.getPosts();
                ViewPostsGridMreza.Visibility = Visibility.Visible;
                ProfileInfoMreza.Visibility = Visibility.Visible;
                IDMrezaSelektovano.Text = tmp.Id;
                NameSurnameMreza.Text = tmp.Ime + " " + tmp.Prezime;
                BirthDateMreza.Text = tmp.DatumRodjenja.ToString();
                //IDMreza.Binding = ;
                //                LajkoviMreza = tmp.
                //                MessageBox.Show(tmp.ToString());
            }
            else
            {
                ViewPostsGridMreza.Visibility = Visibility.Hidden;
                ProfileInfoMreza.Visibility = Visibility.Hidden;
                IDMrezaSelektovano.Text = "";
                NameSurnameMreza.Text = "";
                BirthDateMreza.Text = "";
            }
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

        private void AddFriend(object sender, RoutedEventArgs e)
        {
            if(DodajPrijatelja.Text != null)
            {   
                for(int i = 0; i<mreza.Count();i++)
                {
                    if (mreza[i].Id == DodajPrijatelja.Text) 
                    {
                        mreza[i].DodajPrijatelja(glavniKorisnik);
                        glavniKorisnik.DodajPrijatelja(mreza[i]);
                        
                    }
                }
                DodajPrijatelja.Text = "";
            }
        }

        private void RemoveFriend(object sender, RoutedEventArgs e)
        {
            var tmp = sender as Korisnik;
            if(tmp != null)
            {
                glavniKorisnik.ukloniPrijatelja(tmp);
                for(int i = 0; i < mreza.Count(); i++)
                {
                    if(tmp == mreza[i])
                    {
                        mreza[i].ukloniPrijatelja(glavniKorisnik);
                    }
                }
            }
        }
    }
}