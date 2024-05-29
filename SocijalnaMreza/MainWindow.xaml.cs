using Accessibility;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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
    public partial class MainWindow : Window
    {
        static Random idGen = new Random();
        static List<string> allIDs = new List<string>();
        static List<Korisnik> allUsers = new List<Korisnik>();
        static ObservableCollection<Korisnik> trenutniKorisnik = new ObservableCollection<Korisnik>();
        static ObservableCollection<Grupa> sveGrupe = new ObservableCollection<Grupa>();
        static ObservableCollection<Diskusija> sveDiskusije = new ObservableCollection<Diskusija>();

        Korisnik glavniKorisnik = new Korisnik(GenerateNewUniqueID(), "Nemanja", "Vojnov", "images/profileImage.png");
        Korisnik drugiKorisnik = new Korisnik(GenerateNewUniqueID(), "Nikola", "Kovac");
        Korisnik treci = new Korisnik(GenerateNewUniqueID(),"Treci", "kk");
        Korisnik cetvrti = new Korisnik(GenerateNewUniqueID(),"Cetvrti", "lol");



        Point startPoint = new Point();
        private Post _originalPost;

        public MainWindow()
        {
            InitializeComponent();

            allUsers.Add(glavniKorisnik);
            allUsers.Add(drugiKorisnik);
            allUsers.Add(treci);
            allUsers.Add(cetvrti);


            glavniKorisnik.dodajPost("cao svima 1");
            glavniKorisnik.dodajPost("cao svima 2");
            glavniKorisnik.dodajPost("cao svima 3");
            glavniKorisnik.dodajPost("cao svima 4");
            drugiKorisnik.dodajPost("Hello world!");
            treci.dodajPost("Hello world!");
            cetvrti.dodajPost("Hello world!");
            drugiKorisnik.dodajPost("Hello world!2");
            drugiKorisnik.dodajPost("Hello world!3");
            
            glavniKorisnik.DodajPrijatelja(drugiKorisnik);
            glavniKorisnik.DodajPrijatelja(treci);
            glavniKorisnik.DodajPrijatelja(cetvrti);
            glavniKorisnik.DodajPrijatelja(new Korisnik(GenerateNewUniqueID()));

            
            glavniKorisnik.initPrijatelji();

            trenutniKorisnik.Add(glavniKorisnik);
            List<Korisnik> listaTest = new List<Korisnik>();
            listaTest.Add(glavniKorisnik);
            listaTest.Add(drugiKorisnik);
            Grupa grupa1 = new Grupa(GenerateNewUniqueID(),"Grupica","Nema", listaTest);
            Grupa grupa2 = new Grupa(GenerateNewUniqueID(),"Druga","Nema", new List<Korisnik>());

            sveGrupe.Add(grupa1);
            sveGrupe.Add(grupa2);

            sveDiskusije.Add(new Diskusija(GenerateNewUniqueID(), "Diskusija1", DateOnly.FromDateTime(DateTime.Now), grupa1.Id));
            sveDiskusije.Add(new Diskusija(GenerateNewUniqueID(), "Diskusija3", DateOnly.FromDateTime(DateTime.Now), grupa1.Id));
            sveDiskusije.Add(new Diskusija(GenerateNewUniqueID(), "Diskusija4", DateOnly.FromDateTime(DateTime.Now), grupa1.Id));
            sveDiskusije.Add(new Diskusija(GenerateNewUniqueID(), "Diskusija2", DateOnly.FromDateTime(DateTime.Now), grupa2.Id));

            listaGrupa.ItemsSource = sveGrupe;

            ViewPostsGrid.ItemsSource = glavniKorisnik.getPosts();
            SviPrijatelji.ItemsSource = trenutniKorisnik;
            
            ProfileInfo.DataContext = glavniKorisnik;
            EditProfileInfo.DataContext = glavniKorisnik;
            ProfileImage.DataContext = glavniKorisnik;

            
        }

        static private string GenerateNewUniqueID()
        {
            string newID = idGen.Next(100000, 1000000).ToString();
            while(allIDs.Contains(newID))
            {
                newID = idGen.Next(100000, 1000000).ToString();
            }
            allIDs.Add(newID);
            return newID;
        }

        public Korisnik newUser() { 
            Korisnik novi = new Korisnik(GenerateNewUniqueID());
            allUsers.Add(novi);
            return novi;
        }
        public Korisnik newUser(string ime, string prezime) { 
            Korisnik novi = new Korisnik(GenerateNewUniqueID(),ime,prezime);
            allUsers.Add(novi);
            return novi;
        }
        public Korisnik newUser(string ime, string prezime, string profilnaSlikaPath) { 
            Korisnik novi = new Korisnik(GenerateNewUniqueID(),ime,prezime,profilnaSlikaPath);
            allUsers.Add(novi);
            return novi;
        }
        public Korisnik newUser(string ime, string prezime, DateOnly datumRodjenja, string profilnaSlikaPath) { 
            Korisnik novi = new Korisnik(GenerateNewUniqueID(),ime,prezime,datumRodjenja,profilnaSlikaPath);
            allUsers.Add(novi);
            return novi;
        }


        ////////////////////////////////////////////////
        ///////// OPERACIJE ZA UPLOAD POSTOVA //////////
        ////////////////////////////////////////////////
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
            if (UploadPostContent.Text.Length > 0) {
                glavniKorisnik.dodajPost(UploadPostContent.Text);
                UploadPostContent.Text = "";
                NewPostUpload.Visibility = Visibility.Hidden;
                UploadPostContent.Visibility = Visibility.Hidden;
            } else
            {
                MessageBox.Show("Ne moze se okaciti prazan post");
            }
            
        }
        // ########## KRAJ OPERACIJA ZA UPLOAD POSTOVA ##########



        ////////////////////////////////////////////////////////////////////
        ////////// OPERACIJE EDITOVANJA PROFILA GLAVNOG KORISNIKA //////////
        ////////////////////////////////////////////////////////////////////
        private void SelectPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    BitmapImage image = new BitmapImage(new Uri(openFileDialog.FileName));
                    ProfileImage.Source = image;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading image: " + ex.Message);
                }
            }
        }
        private void Edit_Profile_Click(object sender, RoutedEventArgs e)
        {
            ProfileInfo.Visibility = Visibility.Hidden;
            EditProfileInfo.Visibility = Visibility.Visible;
        }

        private void Save_Edit_Profile_Click(object sender, RoutedEventArgs e)
        {
            bool isInputValidated = true;

            if (!DateOnly.TryParseExact(EditedBirthDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _)) {
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
            if (isInputValidated) {
                glavniKorisnik.Ime = EditedName.Text;
                glavniKorisnik.Prezime = EditedSurname.Text;
                glavniKorisnik.DatumRodjenja = DateOnly.ParseExact(EditedBirthDate.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                ProfileInfo.Visibility = Visibility.Visible;
                EditProfileInfo.Visibility = Visibility.Hidden;
            }
            
        }
        // ########## KRAJ OPERACIJA ZA EDIT PROFILA GLAVNOG KORISNIKA ##########


        ////////////////////////////////////////////////
        ////////// OPERACIJE ZA DRAG AND DROP //////////
        ////////////////////////////////////////////////

        private void ListView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
        }

        private void ListView_MouseMove(object sender, MouseEventArgs e)
        {
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                 Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                // Uzimanje data grid elementa
                DataGrid dataGrid = sender as DataGrid;
                DataGridRow dataGridRow = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

                if (dataGridRow == null)
                    return;

                // Uzimanje podatka iza data grid elementa
                Post post = (Post)dataGridRow.Item;

                // Pocetak drag and drop operacije
                DataObject dragData = new DataObject("myFormat", post);
                DragDrop.DoDragDrop(dataGridRow, dragData, DragDropEffects.Move);
            }
        }

        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        private void ListView_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") || sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void ListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent("myFormat"))
            {
                DataGrid dataGrid = sender as DataGrid;
                Post droppedData = e.Data.GetData("myFormat") as Post;
                Point dropPosition = e.GetPosition(dataGrid);

                var targetItem = GetDataGridRowItem(dataGrid, dropPosition);
                if (targetItem != null)
                {
                    var posts = dataGrid.ItemsSource as IList<Post>;
                    int targetIndex = posts.IndexOf(targetItem);
                    int sourceIndex = posts.IndexOf(droppedData);

                    if (sourceIndex >= 0 && targetIndex >= 0)
                    {
                        // Remove from source index and insert at target index
                        posts.RemoveAt(sourceIndex);
                        posts.Insert(targetIndex, droppedData);
                    }
                }
            }
        }
        
        private Post GetDataGridRowItem(DataGrid dataGrid, Point position)
        {
            // Helper method to get the item from DataGridRow based on mouse position
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(dataGrid, position);
            DataGridRow dataGridRow = FindAncestor<DataGridRow>(hitTestResult.VisualHit);

            if (dataGridRow != null)
            {
                return dataGridRow.Item as Post;
            }

            return null;
        }
        // ########## KRAJ OPERACIJA ZA DRAG AND DROP ##########


        ///////////////////////////////////////////////
        ///////// OPERACIJE NAD FRIEND LISTOM /////////
        ///////////////////////////////////////////////
        private void AddFriend(object sender, RoutedEventArgs e)
        {
            if(DodajPrijatelja.Text != null)
            {
                glavniKorisnik.DodajPrijatelja(DodajPrijatelja.Text,allUsers);
                DodajPrijatelja.Text = "";
            }
        }

        private void RemoveFriend(object sender, RoutedEventArgs e)
        {
            var tmp = SviPrijatelji.SelectedItem as Korisnik;
            if(tmp != null)
            {
                glavniKorisnik.ukloniPrijatelja(tmp);
                
            }
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
                BitmapImage bitmap = new BitmapImage(new Uri("/images/defaultImage.jpg",UriKind.RelativeOrAbsolute));
                ProfilePic.Source= bitmap;
                
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

        private void SearchButt(object sender, RoutedEventArgs e)
        {
            if(SearchContent.Text != null)
            {
                glavniKorisnik.SearchFor(SearchContent.Text);
            }
            SearchContent.Text = "";
        }

        // ########## KRAJ OPERACIJA NAD FRIEND LISTOM ##########


        ///////////////////////////////////////////////
        /////////  OPERACIJE NAD DATA GRID-OM /////////
        ///////////////////////////////////////////////
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewPostsGrid.SelectedItem != null)
            {
                var post = ViewPostsGrid.SelectedItem as Post;

                if (post != null)
                {
                    glavniKorisnik.obrisiPost(post);
                }

                EditRowButton.IsEnabled = false;
                DeleteRowButton.IsEnabled = false;
                UpdateRowButton.IsEnabled = false;
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewPostsGrid.SelectedItem != null)
            {
                EditContentTextBox.Visibility = Visibility.Visible;
                _originalPost = (Post)ViewPostsGrid.SelectedItem;
                EditContentTextBox.Text = _originalPost.Sadrzaj;

                EditRowButton.IsEnabled = false;
                DeleteRowButton.IsEnabled = false;
                UpdateRowButton.IsEnabled = true;
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewPostsGrid.SelectedItem != null)
            {
                Post post = (Post)ViewPostsGrid.SelectedItem;

                glavniKorisnik.editujPost(post, EditContentTextBox.Text);

                EditContentTextBox.Visibility = Visibility.Hidden;

                EditRowButton.IsEnabled = false;
                DeleteRowButton.IsEnabled = false;
                UpdateRowButton.IsEnabled = false;
            }
        }
        
        private void ViewPostsGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            EditRowButton.IsEnabled = true;
            DeleteRowButton.IsEnabled = true;
            UpdateRowButton.IsEnabled = false;
        }

        private void ViewPostsGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ViewPostsGrid.SelectedItem != null)
            {
                ViewPostsGrid.IsReadOnly = false;
                _originalPost = (Post)ViewPostsGrid.SelectedItem;
                ViewPostsGrid.BeginEdit();
            }
        }




        // ########## OPERACIJA NAD DATA GRID-OM ##########

        //////////////////////////////
        /////////  TRECI TAB /////////
        //////////////////////////////

        private void listaGrupa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (listaGrupa.SelectedItem != null)
            {
                Grupa? tmp = listaGrupa.SelectedItem as Grupa;
                if (tmp != null)
                {
                    //MessageBox.Show(tmp.ToString());
                    ObservableCollection<Diskusija> vezaneDiskusije = new ObservableCollection<Diskusija>();
                    for (int i = 0; i < sveDiskusije.Count; i++)
                    {
                        if (sveDiskusije[i].IdGrupe == tmp.Id)
                        {
                            sveDiskusije[i].BrojClanovaGrupe = tmp.BrojClanova;
                            vezaneDiskusije.Add(sveDiskusije[i]);
                        }
                    }
                    PregledDiskusija.ItemsSource = vezaneDiskusije;
                }
            }
        }

        private void RemoveGroup(object sender, RoutedEventArgs e)
        {

        }

        private void AddGroupButton(object sender, RoutedEventArgs e)
        {

        }


        //editovanje selektovanog itema u trecem tabu
        private void EditSelectedT(object sender, RoutedEventArgs e)
        {
            /*koristimo, kako bi napravili mesta za textbox koji nam sluzi za editovanje // ipak ne treba, za ovu trenutnu verziju xd
            Grid.SetRowSpan(listaGrupa, 1);
            Grid.SetRowSpan(PregledDiskusija, 1);*/
            if(PregledDiskusija.SelectedItem!= null)
            {
                Diskusija? tmp = PregledDiskusija.SelectedItem as Diskusija;
                if (tmp != null)
                {
                    //MessageBox.Show(tmp.ToString());
                    LabelZaMenjanje.Visibility = Visibility.Visible;
                    EditBox.Visibility = Visibility.Visible;
                    EditBox.IsEnabled = true;
                }
            }
            else if(listaGrupa.SelectedItem != null)
            {
                Grupa? tmp = listaGrupa.SelectedItem as Grupa;
                if (tmp != null)
                {
                    //MessageBox.Show(tmp.ToString());
                    LabelZaMenjanje.Visibility = Visibility.Visible;
                    EditBox.Visibility = Visibility.Visible;
                    EditBox.IsEnabled = true;
                    EditBox.Text = "";
                }
            }
            else
            {
                LabelZaMenjanje.Visibility = Visibility.Hidden;
                EditBox.Visibility = Visibility.Hidden;
                EditBox.IsEnabled = false;
                EditBox.Text = "";
            }
        }

        private void EditDone(object sender, RoutedEventArgs e)
        {

            //ovde treba da se izvrsi editovanje selektovanog itema
            if (PregledDiskusija.SelectedItem != null)
            { 
            
            } 
            else if (listaGrupa.SelectedItem != null) 
            {
            
            }

                LabelZaMenjanje.Visibility = Visibility.Hidden;
            EditBox.Visibility = Visibility.Hidden;
            EditBox.IsEnabled = false;
            EditBox.Text = "";
        }

        private void ExportCSV(object sender, RoutedEventArgs e)
        {

        }

        private void RemoveDiscussion(object sender, RoutedEventArgs e)
        {

        }

        private void AddDiscussionButton(object sender, RoutedEventArgs e)
        {

        }

        


        // ########## TRECI TAB ##########
    }
}