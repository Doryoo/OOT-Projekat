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
        ObservableCollection<Korisnik> trenutniKorisnik = new ObservableCollection<Korisnik>();
        Point startPoint = new Point();
        private Post _originalPost;

        public MainWindow()
        {
            InitializeComponent();

            
            
                        



            glavniKorisnik.dodajPost("cao svima");
            glavniKorisnik.dodajPost("cao svima");
            glavniKorisnik.dodajPost("cao svima");
            glavniKorisnik.dodajPost("cao svima");
            drugiKorisnik.dodajPost("Hello world!");
            treci.dodajPost("Hello world!");
            cetvrti.dodajPost("Hello world!");
            drugiKorisnik.dodajPost("Hello world!2");
            drugiKorisnik.dodajPost("Hello world!3");
            
            glavniKorisnik.DodajPrijatelja(drugiKorisnik);
            glavniKorisnik.DodajPrijatelja(treci);
            glavniKorisnik.DodajPrijatelja(cetvrti);
            trenutniKorisnik.Add(glavniKorisnik);
            ViewPostsGrid.ItemsSource = glavniKorisnik.getPosts();
            SviPrijatelji.ItemsSource = trenutniKorisnik;
            
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

        // Drag and drop in same table functionality
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
                DataGrid dataGrid = sender as DataGrid;
                DataGridRow dataGridRow = FindAncestor<DataGridRow>((DependencyObject)e.OriginalSource);

                if (dataGridRow == null)
                    return;

                Post post = (Post)dataGridRow.Item;

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

        // Helper method to get the item from DataGridRow based on mouse position
        private Post GetDataGridRowItem(DataGrid dataGrid, Point position)
        {
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(dataGrid, position);
            DataGridRow dataGridRow = FindAncestor<DataGridRow>(hitTestResult.VisualHit);

            if (dataGridRow != null)
            {
                return dataGridRow.Item as Post;
            }

            return null;
        }

        private void AddFriend(object sender, RoutedEventArgs e)
        {
            if(DodajPrijatelja.Text != null)
            {
                glavniKorisnik.DodajPrijatelja(DodajPrijatelja.Text);
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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewPostsGrid.SelectedItem != null)
            {
                var post = ViewPostsGrid.SelectedItem as Post;

                if (post != null)
                {
                    glavniKorisnik.obrisiPost(post);
                }
            }
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
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewPostsGrid.SelectedItem != null)
            {
                EditContentTextBox.Visibility = Visibility.Visible;
                _originalPost = (Post)ViewPostsGrid.SelectedItem;
                EditContentTextBox.Text = _originalPost.Sadrzaj;
            }
        }

        private void ViewPostsGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var editingElement = e.EditingElement as TextBox;
                if (editingElement != null && e.Column is DataGridBoundColumn boundColumn)
                {
                    var binding = boundColumn.Binding as Binding;
                    if (binding != null)
                    {
                        var bindingPath = binding.Path.Path;
                        var propertyInfo = typeof(Post).GetProperty(bindingPath);
                        if (propertyInfo != null)
                        {
                            propertyInfo.SetValue(_originalPost, Convert.ChangeType(editingElement.Text, propertyInfo.PropertyType), null);
                        }
                    }
                }
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewPostsGrid.SelectedItem != null)
            {
                Post post = (Post)ViewPostsGrid.SelectedItem;

                glavniKorisnik.editujPost(post, EditContentTextBox.Text);

                EditContentTextBox.Visibility = Visibility.Hidden;
            }
        }
    }
}