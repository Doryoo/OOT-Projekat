using System;
using System.Collections.Generic;
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
    /// Interaction logic for EditujDiskusiju.xaml
    /// </summary>
    public partial class EditujDiskusiju : Window
    {
        Diskusija diskusija;
        public EditujDiskusiju(Diskusija d)
        {
            diskusija = d;
            InitializeComponent();
            boxIme.Text = diskusija.Naziv;
        }

        private void saveChanges(object sender, RoutedEventArgs e)
        {
            diskusija.Naziv = boxIme.Text;
            Diskusija.SaveDiscussion(diskusija);
            this.Close();
        }
    }
}
