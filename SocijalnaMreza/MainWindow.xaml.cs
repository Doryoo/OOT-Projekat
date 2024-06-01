using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        Korisnik drugiKorisnik = new Korisnik(GenerateNewUniqueID(), "Nikola", "Kovac", "images/user1.png");
        Korisnik treci = new Korisnik(GenerateNewUniqueID(), "Treci", "kk", "images/user2.png");
        Korisnik cetvrti = new Korisnik(GenerateNewUniqueID(), "Cetvrti", "lol", "images/user3.png");

        Point startPoint = new Point();
        private Post _originalPost;

        static public Grupa? tmpGrupa = null;
        //static DateTime tmpVremeGrupe = DateTime.Now;
        static public Diskusija? tmpDiskusija = null;
        //static DateTime tmpVremeDiskusije = tmpVremeGrupe;
        static bool oderOr;
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
            List<Korisnik> listaTest2 = new List<Korisnik>();
            listaTest.Add(glavniKorisnik);
            listaTest2.Add(glavniKorisnik);
            listaTest.Add(drugiKorisnik);
            Grupa grupa1 = new Grupa(GenerateNewUniqueID(), "Grupica", "Nema", listaTest);
            Grupa grupa2 = new Grupa(GenerateNewUniqueID(), "Druga", "Nema", listaTest2);

            sveGrupe.Add(grupa1);
            sveGrupe.Add(grupa2);

            Diskusija d1 = new Diskusija(GenerateNewUniqueID(), "Diskusija1", DateOnly.FromDateTime(DateTime.Now), grupa1.Id);
            Diskusija d2 = new Diskusija(GenerateNewUniqueID(), "Diskusija2", DateOnly.FromDateTime(DateTime.Now), grupa1.Id);
            Diskusija d3 = new Diskusija(GenerateNewUniqueID(), "Diskusija3", DateOnly.FromDateTime(DateTime.Now), grupa1.Id);
            Diskusija d4 = new Diskusija(GenerateNewUniqueID(), "Diskusija4", DateOnly.FromDateTime(DateTime.Now), grupa1.Id);
            sveDiskusije.Add(d1);
            sveDiskusije.Add(new Diskusija(GenerateNewUniqueID(), "Diskusija3", DateOnly.FromDateTime(DateTime.Now), grupa1.Id));
            sveDiskusije.Add(new Diskusija(GenerateNewUniqueID(), "Diskusija4", DateOnly.FromDateTime(DateTime.Now), grupa1.Id));
            sveDiskusije.Add(new Diskusija(GenerateNewUniqueID(), "Diskusija2", DateOnly.FromDateTime(DateTime.Now), grupa2.Id));

            SyncShownGroups(); // koristimo funkciju za prikazivanje svih groupa u kojima je glavni korisnik

            ViewPostsGrid.ItemsSource = glavniKorisnik.getPosts();
            SviPrijatelji.ItemsSource = trenutniKorisnik;

            ProfileInfo.DataContext = glavniKorisnik;
            EditProfileInfo.DataContext = glavniKorisnik;
            ProfileImage.DataContext = glavniKorisnik;

            Uri resourceUri = new Uri(glavniKorisnik.ProfilnaSlikaPath, UriKind.Relative);
            ProfileImage.Source = new BitmapImage(resourceUri);

            Korisnik.SaveUser(glavniKorisnik);
            Korisnik.SaveUser(drugiKorisnik);
            Korisnik.SaveUser(treci);
            Korisnik.SaveUser(cetvrti);
            Korisnik novi = Korisnik.LoadUser("glavniKorisnik.txt");

            Grupa.SaveGroup(grupa1);
            Grupa.SaveGroup(grupa2);
            Grupa newGroup = Grupa.LoadGroup("grupa1.txt");

            Diskusija.SaveDiscussion(d1);
            Diskusija.SaveDiscussion(d2);
            Diskusija.SaveDiscussion(d3);
            Diskusija.SaveDiscussion(d4);
            //Diskusija d2 = Diskusija.LoadDiscussion("disc1.txt");

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

        public Korisnik newUser()
        {
            Korisnik novi = new Korisnik(GenerateNewUniqueID());
            allUsers.Add(novi);
            return novi;
        }
        public Korisnik newUser(string ime, string prezime)
        {
            Korisnik novi = new Korisnik(GenerateNewUniqueID(), ime, prezime);
            allUsers.Add(novi);
            return novi;
        }
        public Korisnik newUser(string ime, string prezime, string profilnaSlikaPath)
        {
            Korisnik novi = new Korisnik(GenerateNewUniqueID(), ime, prezime, profilnaSlikaPath);
            allUsers.Add(novi);
            return novi;
        }
        public Korisnik newUser(string ime, string prezime, DateOnly datumRodjenja, string profilnaSlikaPath)
        {
            Korisnik novi = new Korisnik(GenerateNewUniqueID(), ime, prezime, datumRodjenja, profilnaSlikaPath);
            allUsers.Add(novi);
            return novi;
        }

        public void SyncShownGroups()
        {

            ObservableCollection<Grupa> shownGroups = new ObservableCollection<Grupa>();
            foreach (Grupa grupa in sveGrupe)
            {
                if (grupa.ListaClanova.Contains(glavniKorisnik))
                {
                    shownGroups.Add(grupa);
                }
            }
            listaGrupa.ItemsSource = shownGroups;
        }

        public void SyncShownDiscussions(Grupa tmp)
        {
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

        public void SyncShownDiscussions(Diskusija tmpDiskusija)
        {
            ObservableCollection<Diskusija> vezaneDiskusije = new ObservableCollection<Diskusija>();
            for (int i = 0; i < sveDiskusije.Count; i++)
            {
                if (sveDiskusije[i].IdGrupe == tmpDiskusija.IdGrupe)
                {
                    sveDiskusije[i].BrojClanovaGrupe = tmpDiskusija.BrojClanovaGrupe;
                    vezaneDiskusije.Add(sveDiskusije[i]);
                }
            }
            PregledDiskusija.ItemsSource = vezaneDiskusije;
        }


        /*
         zanimljiv recommendation od intellisense-a 
         
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Da li ste sigurni da zelite da zatvorite aplikaciju?", "Zatvaranje aplikacije", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
        }
        
         */

        ////////////////////////////////////////////////
        ///////// OPERACIJE ZA UPLOAD POSTOVA //////////
        ////////////////////////////////////////////////
        private void Toggle_Upload_Visibility_Click(object sender, RoutedEventArgs e)
        {
            if (NewPostUpload.Visibility == Visibility.Hidden)
            {
                NewPostUpload.Visibility = Visibility.Visible;
                UploadPostContent.Visibility = Visibility.Visible;
            }
            else
            {
                NewPostUpload.Visibility = Visibility.Hidden;
                UploadPostContent.Visibility = Visibility.Hidden;
            }
        }
        private void Upload_Post_Click(object sender, RoutedEventArgs e)
        {
            if (UploadPostContent.Text.Length > 0)
            {
                glavniKorisnik.dodajPost(UploadPostContent.Text);
                UploadPostContent.Text = "";
                NewPostUpload.Visibility = Visibility.Hidden;
                UploadPostContent.Visibility = Visibility.Hidden;
            }
            else
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
                    string selectedFilePath = openFileDialog.FileName;
                    BitmapImage image = new BitmapImage(new Uri(selectedFilePath));
                    ProfileImage.Source = image;

                    // Create directory if it doesn't exist
                    string directoryPath = "images";
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    // Save the image to the images directory
                    string fileName = System.IO.Path.GetFileName(selectedFilePath);
                    string destinationFilePath = System.IO.Path.Combine(directoryPath, fileName);
                    File.Copy(selectedFilePath, destinationFilePath, true);

                    MessageBox.Show("Image saved to " + destinationFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading or saving image: " + ex.Message);
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
            if (DodajPrijatelja.Text != null)
            {
                string num = glavniKorisnik.DodajPrijatelja(DodajPrijatelja.Text, allUsers, allIDs);
                if (num != "0") 
                { 
                    allIDs.Add(num);
                    ObservableCollection<Korisnik> tmp = glavniKorisnik.ListaPrijatelja;
                    foreach(Korisnik korisnik in tmp)
                    {
                        if (korisnik.Id == num)
                        {
                            allUsers.Add(korisnik);
                        }
                    }
                }
                DodajPrijatelja.Text = "";
            }
        }

        private void RemoveFriend(object sender, RoutedEventArgs e)
        {
            var tmp = SviPrijatelji.SelectedItem as Korisnik;
            if (tmp != null)
            {
                glavniKorisnik.ukloniPrijatelja(tmp);
                ObrisiPrijatelja.IsEnabled = false;

            }
        }

        private void FriendClicked(object sender, RoutedEventArgs e)
        {

            var tmp = SviPrijatelji.SelectedItem as Korisnik;

            if (tmp != null)
            {
                if (tmp!= glavniKorisnik) { 
                    ObrisiPrijatelja.IsEnabled = true;
                }
                ViewPostsGridMreza.ItemsSource = tmp.getPosts();
                ViewPostsGridMreza.Visibility = Visibility.Visible;
                ProfileInfoMreza.Visibility = Visibility.Visible;
                IDMrezaSelektovano.Text = tmp.Id;
                NameSurnameMreza.Text = tmp.Ime + " " + tmp.Prezime;
                BirthDateMreza.Text = tmp.DatumRodjenja.ToString();

                BitmapImage bitmap = new BitmapImage(new Uri(tmp.ProfilnaSlikaPath, UriKind.Relative));
                CurrentUserPicture.Source = bitmap;

                //IDMreza.Binding = ;
                //                LajkoviMreza = tmp.
                //                MessageBox.Show(tmp.ToString());
            }
            else
            {
                ObrisiPrijatelja.IsEnabled = false;
                ViewPostsGridMreza.Visibility = Visibility.Hidden;
                ProfileInfoMreza.Visibility = Visibility.Hidden;
                IDMrezaSelektovano.Text = "";
                NameSurnameMreza.Text = "";
                BirthDateMreza.Text = "";
            }
        }

        private void SearchButt(object sender, RoutedEventArgs e)
        {
            if (SearchContent.Text != null)
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
            Grupa? tmp = listaGrupa.SelectedItem as Grupa;
            if (tmp != null)
            {
                //tmpVremeGrupe = DateTime.Now;

                delGroup.IsEnabled = true;
                //MessageBox.Show(tmp.ToString());
                SyncShownDiscussions(tmp);
                PregledDiskusija.Visibility = Visibility.Visible;
                AddDiscussionBox.IsEnabled = true;
                addDiscButton.IsEnabled = true;
                CSVExport.IsEnabled = true;

            }
            else
            {
                delGroup.IsEnabled = false;
                CSVExport.IsEnabled = false;
                PregledDiskusija.Visibility = Visibility.Hidden;
                AddDiscussionBox.IsEnabled = false;
                addDiscButton.IsEnabled = false;
            }
        }

        private void diskusijeChanged(object sender, SelectionChangedEventArgs e)
        {
            Diskusija? tmp = PregledDiskusija.SelectedItem as Diskusija;
            if (tmp != null)
            {
                //MessageBox.Show(listaGrupa.IsKeyboardFocusWithin.ToString());
                //tmpVremeDiskusije = DateTime.Now;
                CSVExport.IsEnabled = true;
                delDiscussion.IsEnabled = true;
            }
            else
            {
                delDiscussion.IsEnabled = false;
            }
        }

        private void RemoveGroup(object sender, RoutedEventArgs e)
        {
            Grupa? tmp = listaGrupa.SelectedItem as Grupa;
            if (tmp != null)
            {
                tmp.UkloniClana(glavniKorisnik);
                delDiscussion.IsEnabled = false;
                SyncShownGroups();
            }
        }

        private void AddGroupButton(object sender, RoutedEventArgs e)
        {
            if (AddGroupBox.Text != null)
            {
                foreach (Grupa grupa in sveGrupe)
                {
                    if (grupa.Id == AddGroupBox.Text)
                    {
                        grupa.DodajClana(glavniKorisnik);
                        SyncShownGroups();
                        return;
                    }
                }
            }
        }


        //editovanje selektovanog itema u trecem tabu
        private void EditSelectedT(object sender, RoutedEventArgs e)
        {
            /*koristimo, kako bi napravili mesta za textbox koji nam sluzi za editovanje // ipak ne treba, za ovu trenutnu verziju xd
            Grid.SetRowSpan(listaGrupa, 1);
            Grid.SetRowSpan(PregledDiskusija, 1);
            //MessageBox.Show(listaGrupa.PreviewMouseLeftButtonDown);

            bool barJedan = true;

            if (PregledDiskusija.SelectedItem != null)
            {
                Diskusija? tmp = PregledDiskusija.SelectedItem as Diskusija;
                if (tmp != null)
                {
                    tmpDiskusija = tmp;
                    //MessageBox.Show(tmp.ToString());
                    LabelZaMenjanje.Visibility = Visibility.Visible;
                    EditBox.Visibility = Visibility.Visible;
                    EditBox.IsEnabled = true;
                    LabelZaMenjanje.Content = "Promeni diskusiju " + tmp.Naziv + " :";
                    barJedan = false;
                }
            }

            if (listaGrupa.SelectedItem != null)
            {
                Grupa? tmp = listaGrupa.SelectedItem as Grupa;
                if (tmp != null)
                {
                    tmpGrupa = tmp;

                    //MessageBox.Show(tmp.ToString());
                    LabelZaMenjanje.Visibility = Visibility.Visible;
                    EditBox.Visibility = Visibility.Visible;
                    EditBox.IsEnabled = true;
                    LabelZaMenjanje.Content = "Promeni grupu " + tmp.Naziv + " :";
                    barJedan = false;
                }
            }

            if (barJedan)
            {
                LabelZaMenjanje.Visibility = Visibility.Hidden;
                EditBox.Visibility = Visibility.Hidden;
                EditBox.IsEnabled = false;
                EditBox.Text = "";
            }
            */
        }

        //Dugme koje se klikne kada zavrsimo editovanje selektovanog itema na trecem tabu
        private void EditDone(object sender, RoutedEventArgs e)
        {

            //ovde treba da se izvrsi editovanje selektovanog itema

            if(tmpGrupa != null)
            {
                tmpGrupa.Naziv = EditBox.Text;
                SyncShownGroups();
            }
            else if (tmpDiskusija != null)
            {
                tmpDiskusija.Naziv = EditBox.Text;

                SyncShownDiscussions(tmpDiskusija); 
            }


            LabelZaMenjanje.Visibility = Visibility.Hidden;
            EditBox.Visibility = Visibility.Hidden;
            EditBox.IsEnabled = false;
            EditBox.Text = "";
        }

        private void ExportCSV(object sender, RoutedEventArgs e)
        {
            if (PregledDiskusija.IsKeyboardFocusWithin)
            {
                if (PregledDiskusija.SelectedItem != null)
                {
                    Diskusija d = PregledDiskusija.SelectedItem as Diskusija;
                    if (d != null)
                    {
                        d.exportToCsv();
                    }
                }
            }
            else
            {
                if (listaGrupa.SelectedItem != null)
                {
                    Grupa g = listaGrupa.SelectedItem as Grupa;
                    if (g != null)
                    {
                        g.exportToCsv(sveDiskusije);
                    }
                }
            }
        }

        private void RemoveDiscussion(object sender, RoutedEventArgs e) //not done well.
        {
            var tmp = PregledDiskusija.SelectedItem as Diskusija;

            if (tmp != null)
            {
                sveDiskusije.Remove(tmp);
                SyncShownDiscussions(tmp);
            }
        }

        private void AddDiscussionButton(object sender, RoutedEventArgs e)
        {
            var tmp = listaGrupa.SelectedItem as Grupa;
            if (tmp != null && AddDiscussionBox.Text != null && AddDiscussionBox.Text != "")
            {
                sveDiskusije.Add(new Diskusija(GenerateNewUniqueID(), AddDiscussionBox.Text, DateOnly.FromDateTime(DateTime.Now), tmp.Id));
                AddDiscussionBox.Text = "";
                SyncShownDiscussions(tmp);
            }
        }

        private void EditSelectedT(object sender, MouseButtonEventArgs e)
        {
            // Proveravamo kojeg tipa je poslednji selektovani item
            if (listaGrupa.IsKeyboardFocusWithin)
            {
                if (listaGrupa.SelectedItem != null)
                {
                    Grupa? tmp = listaGrupa.SelectedItem as Grupa;
                    if (tmp != null)
                    {
                        tmpGrupa = tmp;
                        tmpDiskusija = null;
                        EditBox.Text = tmp.Naziv;
                        //MessageBox.Show(tmp.ToString());
                        LabelZaMenjanje.Visibility = Visibility.Visible;
                        EditBox.Visibility = Visibility.Visible;
                        EditBox.IsEnabled = true;
                        LabelZaMenjanje.Content = "Promeni grupu " + tmp.Naziv + " :";
                    }
                    else
                    {
                        tmpGrupa = null;
                        tmpDiskusija = null;
                    }
                }
                else
                {
                    LabelZaMenjanje.Visibility = Visibility.Hidden;
                    EditBox.Visibility = Visibility.Hidden;
                    EditBox.IsEnabled = false;
                    EditBox.Text = "";
                    tmpGrupa = null;
                    tmpDiskusija = null;
                }
            }
            else
            {
                if (PregledDiskusija.SelectedItem != null)
                {
                    Diskusija? tmp = PregledDiskusija.SelectedItem as Diskusija;
                    if (tmp != null)
                    {
                        tmpGrupa = null;
                        tmpDiskusija = tmp;
                        EditBox.Text = tmp.Naziv;
                        //MessageBox.Show(tmp.ToString());
                        LabelZaMenjanje.Visibility = Visibility.Visible;
                        EditBox.Visibility = Visibility.Visible;
                        EditBox.IsEnabled = true;
                        LabelZaMenjanje.Content = "Promeni diskusiju " + tmp.Naziv + " :";
                    }
                    else
                    {
                        tmpGrupa = null;
                        tmpDiskusija = null;
                    }
                }
                else
                {
                    LabelZaMenjanje.Visibility = Visibility.Hidden;
                    EditBox.Visibility = Visibility.Hidden;
                    EditBox.IsEnabled = false;
                    EditBox.Text = "";
                    tmpGrupa = null;
                    tmpDiskusija = null;
                }
            }
        }


        // ########## TRECI TAB ##########
    }
}