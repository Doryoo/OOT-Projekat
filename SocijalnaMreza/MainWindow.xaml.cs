using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
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
       
        static List<Korisnik> allUsers = Korisnik.LoadAllUsers();
        static ObservableCollection<Grupa> sveGrupe = Grupa.LoadAllGroups();
        static ObservableCollection<Diskusija> sveDiskusije = Diskusija.LoadAllDiscussions();
        static List<string> allIDs = LoadAllIDs();
        Korisnik glavniKorisnik = SelectMainUser();



        Point startPoint = new();
        private Post? _originalPost;

        static Grupa? tmpGrupa = null;
        static Diskusija? tmpDiskusija = null;
        public MainWindow()
        {
            InitializeComponent();

            LoadAllFriendsForUser(glavniKorisnik);
            LoadAllUsersForGroups();

            glavniKorisnik.initPrijatelji();

            SyncShownGroups(); // koristimo funkciju za prikazivanje svih groupa u kojima je glavni korisnik

            ViewPostsGrid.ItemsSource = glavniKorisnik.getPosts();
            SviPrijatelji.ItemsSource = new ObservableCollection<Korisnik>() { glavniKorisnik };

            ProfileInfo.DataContext = glavniKorisnik;
            ProfileImage.DataContext = glavniKorisnik;

            Uri resourceUri = new Uri(Path.Join(glavniKorisnik.ProfilnaSlikaPath) , UriKind.Relative);
            ProfileImage.Source = new BitmapImage(resourceUri);

           }


        /////////////////////////////////////////////////////////////
        ///////// OPERACIJE ZA LOADOVANJE PODATAKA IZ BAZE //////////
        /////////////////////////////////////////////////////////////
        static public void LoadAllUsersForGroups()
        {
            foreach (var grupa in sveGrupe)
            {
                foreach (var id in grupa.ListaClanovaIDs)
                {
                    foreach (var user in allUsers)
                    {
                        if (user.Id == id)
                        {
                            grupa.DodajClana(user);
                        }
                    }
                }
            }
        }
        static public List<string> LoadAllIDs()
        {
            List<string> ids = new List<string>();

            foreach (var user in allUsers)
            {
                ids.Add(user.Id);
                foreach (var post in user.getPosts())
                {
                    ids.Add(post.Id);
                }
            }
            foreach (var disc in sveDiskusije)
            {
                ids.Add(disc.Id);
            }
            foreach (var gr in sveGrupe)
            {
                ids.Add(gr.Id);
            }
            return ids;
        }
        static public void LoadAllFriendsForUser(Korisnik k)
        {
            k.ListaPrijateljskihIDs.ForEach(x => {

                
                foreach (Korisnik kor in allUsers)
                {
                    if (kor.Id == x)
                    {
                        k.DodajPrijatelja(kor);
                    }
                }
            });
        }
        static public Korisnik SelectMainUser()
        {
            Korisnik tmp = new("");
            foreach(Korisnik k in allUsers)
            {
                if (k.Id == "964964")
                {
                    tmp = k; break;
                }
            }
            return tmp;
        }

        //////////////////////////////////////
        ///////// KORISNE OPERACIJE //////////
        //////////////////////////////////////
        
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



        ////////////////////////////////////////////////
        ///////// OPERACIJE ZA UPLOAD POSTOVA //////////
        ////////////////////////////////////////////////
        private void Toggle_Upload_Visibility_Click(object sender, RoutedEventArgs e)
        {
            UploadPost uploadPostWindow = new UploadPost(glavniKorisnik);
            uploadPostWindow.ShowDialog();
        }
        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Button)sender).Cursor = Cursors.Cross;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Button)sender).Cursor = Cursors.Arrow; // Reset to default cursor
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

                    glavniKorisnik.ProfilnaSlikaPath = destinationFilePath;
                    Korisnik.SaveUser(glavniKorisnik);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading or saving image: " + ex.Message);
                }
            }
        }

        private void Edit_Profile_Click(object sender, RoutedEventArgs e)
        {
            EditUser editProfileWindow = new EditUser(glavniKorisnik);
            editProfileWindow.ShowDialog();
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
            // Pomocna metoda za dobijanje item-a iz DataGridRow na osnovu pozicije misa
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
        
        //Poziva dugme za dodavanje vec postojeceg prijatelja
        private void AddFriend(object sender, RoutedEventArgs e)
        {
            DodajPrijateljaWindow dodajPrijateljaWindow = new DodajPrijateljaWindow(allUsers,glavniKorisnik);
            dodajPrijateljaWindow.ShowDialog();
            Korisnik.SaveUser(glavniKorisnik);

        }

        private void RemoveFriend(object sender, RoutedEventArgs e)
        {
            var tmp = SviPrijatelji.SelectedItem as Korisnik;
            if (tmp != null)
            {
                glavniKorisnik.ukloniPrijatelja(tmp);
                ObrisiPrijatelja.IsEnabled = false;

            }
            Korisnik.SaveUser(glavniKorisnik);
        }

        //Prizove se kada se bilo koji prijatelj selektuje, kako bi se prikazale sve informacije o njemu
        private void FriendClicked(object sender, RoutedEventArgs e)
        {

            var tmp = SviPrijatelji.SelectedItem as Korisnik;

            if (tmp != null)
            {
                if (tmp!= glavniKorisnik) { 
                    ObrisiPrijatelja.IsEnabled = true;
                }
                else
                {
                    ObrisiPrijatelja.IsEnabled = false;
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

        private void SearchButtn(object sender, RoutedEventArgs e)
        {
            if (SearchContent.Text != null)
            {
                glavniKorisnik.SearchFor(SearchContent.Text);
            }
            SearchContent.Text = "";
        }


        private void AddNewUser(object sender, RoutedEventArgs e)
        {
            AddNewUser addNewUserWindow = new AddNewUser(allUsers, allIDs);
            addNewUserWindow.ShowDialog();
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
                Korisnik.SaveUser(glavniKorisnik);
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewPostsGrid.SelectedItem != null)
            {
                Post selectedPost = (Post)ViewPostsGrid.SelectedItem;
                EditPost editPostWindow = new EditPost(selectedPost, glavniKorisnik);
                editPostWindow.ShowDialog();

                // Optionally refresh the grid or other UI elements here
            }
        }

        private void ViewPostsGrid_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            EditRowButton.IsEnabled = true;
            DeleteRowButton.IsEnabled = true;
        }

        // ########## KRAJ OPERACIJA NAD DATA GRID-OM ##########


        //////////////////////////////
        /////////  TRECI TAB /////////
        //////////////////////////////

        private void listaGrupa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Grupa? tmp = listaGrupa.SelectedItem as Grupa;
            if (tmp != null)
            {
                delGroup.IsEnabled = true;
                SyncShownDiscussions(tmp);
                PregledDiskusija.Visibility = Visibility.Visible;
                addDiscButton.IsEnabled = true;
                CSVExport.IsEnabled = true;
            }
            else
            {
                delGroup.IsEnabled = false;
                CSVExport.IsEnabled = false;
                PregledDiskusija.Visibility = Visibility.Hidden;
                addDiscButton.IsEnabled = false;
            }
        }

        private void diskusijeChanged(object sender, SelectionChangedEventArgs e)
        {
            Diskusija? tmp = PregledDiskusija.SelectedItem as Diskusija;
            if (tmp != null)
            {
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
                File.Delete(Path.Join("groups/", tmp.Id + ".txt"));
            }
        }

        private void AddNewGroupButton(object sender, RoutedEventArgs e)
        {
            MakeNewGrup makeNewGrupWindow = new MakeNewGrup(sveGrupe, allIDs);
            makeNewGrupWindow.ShowDialog();
            SyncShownGroups();
        }
        private void AddGroupButton(object sender, RoutedEventArgs e)
        {
            AddGroup addGroupWindow = new AddGroup(sveGrupe, allIDs, glavniKorisnik);
            addGroupWindow.ShowDialog();
            SyncShownGroups();
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

        private void RemoveDiscussion(object sender, RoutedEventArgs e) 
        {
            if(PregledDiskusija.SelectedItem != null) { 
                var tmp = PregledDiskusija.SelectedItem as Diskusija;

                if (tmp != null)
                {
                    sveDiskusije.Remove(tmp);
                    SyncShownDiscussions(tmp);
                    File.Delete(Path.Join("discussions/", tmp.Id + ".txt"));
                }
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
                        EditujGrupu editujGrupuWindow = new EditujGrupu(tmp);
                        editujGrupuWindow.ShowDialog();
                        SyncShownGroups();
                    }
                }
            }
            else
            {
                if (PregledDiskusija.SelectedItem != null)
                {
                    Diskusija? tmp = PregledDiskusija.SelectedItem as Diskusija;
                    if (tmp != null)
                    {
                        EditujDiskusiju editujDiskusijuWindow = new EditujDiskusiju(tmp);
                        editujDiskusijuWindow.ShowDialog();
                        SyncShownDiscussions(tmp);
                    }
                }
            }
        }

        private void AddNewDiscussion(object sender, RoutedEventArgs e)
        {
            Grupa? tmp = null;
            if (listaGrupa.SelectedItem != null)
                tmp = listaGrupa.SelectedItem as Grupa;

            Grupa? output = null;
            
            MakeNewDiscussion makeNewDiscussionWindow = new MakeNewDiscussion(sveGrupe,sveDiskusije, allIDs,tmp, output);
            makeNewDiscussionWindow.ShowDialog();
            if(tmp != null)
            {
                SyncShownDiscussions(tmp);
            }
        }





        // ########## TRECI TAB ##########


        //public Korisnik newUser()
        //{
        //    Korisnik novi = new Korisnik(GenerateNewUniqueID());
        //    allUsers.Add(novi);
        //    return novi;
        //}
        //public Korisnik newUser(string ime, string prezime)
        //{
        //    Korisnik novi = new Korisnik(GenerateNewUniqueID(), ime, prezime);
        //    allUsers.Add(novi);
        //    return novi;
        //}
        //public Korisnik newUser(string ime, string prezime, string profilnaSlikaPath)
        //{
        //    Korisnik novi = new Korisnik(GenerateNewUniqueID(), ime, prezime, profilnaSlikaPath);
        //    allUsers.Add(novi);
        //    return novi;
        //}
        //public Korisnik newUser(string ime, string prezime, DateOnly datumRodjenja, string profilnaSlikaPath)
        //{
        //    Korisnik novi = new Korisnik(GenerateNewUniqueID(), ime, prezime, datumRodjenja, profilnaSlikaPath);
        //    allUsers.Add(novi);
        //    return novi;
        //}

    }
}