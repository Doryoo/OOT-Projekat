using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for MakeNewGrup.xaml
    /// </summary>
    ///
    public partial class MakeNewGrup : Window
    {
        static Random idGen = new Random();
        static ObservableCollection<Grupa> sveGrupe;
        static List<string> allIDs;
        public MakeNewGrup(ObservableCollection<Grupa> SveGrupe, List<string> AllIDs)
        {
            InitializeComponent();
            sveGrupe = SveGrupe;
            allIDs = AllIDs;
        }
        private void napraviGrupu(object sender, RoutedEventArgs e)
        {
            boxIme.Text = boxIme.Text.Trim();
            boxOpis.Text = boxOpis.Text.Trim();
            if (boxIme.Text == "" && boxOpis.Text == "")
            {
                MessageBox.Show("Morate uneti bar jedno polje!");
                return;
            }

            Grupa g = new Grupa(GenerateNewUniqueID(), boxIme.Text, boxOpis.Text);
            sveGrupe.Add(g);
            Grupa.SaveGroup(g);
            this.Close();
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
    }
}
