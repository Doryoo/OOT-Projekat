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
    /// Interaction logic for EditujGrupu.xaml
    /// </summary>
    public partial class EditujGrupu : Window
    {
        Grupa tmp;
        public EditujGrupu(Grupa Tmp)
        {
            tmp = Tmp;
            InitializeComponent();
            boxIme.Text = tmp.Naziv;
            boxOpis.Text = tmp.Opis;
        }

        private void saveChanges(object sender, RoutedEventArgs e)
        {
            tmp.Naziv = boxIme.Text;
            tmp.Opis = boxOpis.Text;
            Grupa.SaveGroup(tmp);
            this.Close();
        }
    }
}
