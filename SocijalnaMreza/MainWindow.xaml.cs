﻿using System.Collections.ObjectModel;
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
        Point startPoint = new Point();


        public MainWindow()
        {
            InitializeComponent();

            ObservableCollection<Korisnik> mreza = new ObservableCollection<Korisnik>();
            mreza.Add(glavniKorisnik);
                        



            glavniKorisnik.dodajPost("cao svima");
            glavniKorisnik.dodajPost("cao svima");
            glavniKorisnik.dodajPost("cao svima");
            glavniKorisnik.dodajPost("cao svima");
            
            glavniKorisnik.DodajPrijatelja(drugiKorisnik);
            glavniKorisnik.DodajPrijatelja(treci);
            glavniKorisnik.DodajPrijatelja(cetvrti);

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
            if(tmp != null)
            {
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
    }
}