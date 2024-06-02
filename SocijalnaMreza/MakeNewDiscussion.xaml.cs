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
    /// Interaction logic for MakeNewDiscussion.xaml
    /// </summary>
    public partial class MakeNewDiscussion : Window
    {
        static Random idGen = new Random();
        static ObservableCollection<Grupa> sveGrupe;
        static ObservableCollection<Diskusija> sveDiskusije;
        static List<string> allIDs;
        static Grupa? input, output;
        public MakeNewDiscussion( ObservableCollection<Grupa> SveGrupe,ObservableCollection<Diskusija> SveDiskusije, List<string> AllIDs, Grupa Input, Grupa Output)
        {
            InitializeComponent();
            sveGrupe = SveGrupe;
            sveDiskusije = SveDiskusije;
            allIDs = AllIDs;
            input = Input;
            output = Output;
        }

        private void makeDiscussion(object sender, RoutedEventArgs e)
        {
            boxId.Text = boxId.Text.Trim();
            boxIme.Text = boxIme.Text.Trim();
            bool uspeo = true;
            if(boxIme.Text == "")
            {
                MessageBox.Show("Unesite ime grupe!");
                return;
            }
            if(boxId.Text == "")
            {
                if (input == null)
                {
                    MessageBox.Show("Ni jedna grupa nije bila selektovana, niti ste odabrali ID grupa za koju se vezuje diskusija");
                }
                else
                {
                    sveDiskusije.Add(new Diskusija(GenerateNewUniqueID(), boxIme.Text, DateOnly.FromDateTime(DateTime.Now), input.Id));
                }
            }
            else
            {
                foreach (var grupa in sveGrupe)
                {
                    if (grupa.Id == boxId.Text)
                    {
                        sveDiskusije.Add(new Diskusija(GenerateNewUniqueID(), boxIme.Text, DateOnly.FromDateTime(DateTime.Now), grupa.Id));
                        output = grupa;
                        uspeo = false;
                    }
                }
                if (uspeo)
                {
                    MessageBox.Show("Nijedna grupa sa unetim ID-om nije pronadjena");
                }
            }

            this.Close();
            return;
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
