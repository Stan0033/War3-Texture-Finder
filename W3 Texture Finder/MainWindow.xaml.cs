using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

namespace W3_Texture_Finder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Dictionary<string, string> FavouritesList = new Dictionary<string, string>();
        private string appTitle = "War3 Texture Finder for SD";
        public MainWindow()
        {
            InitializeComponent();
             
            MPQFinder.Find();
            MPQHelper.Initialize();
            foreach (string s in MPQHelper.Listfile_All)
            {
                Resultbox.Items.Add(new ListBoxItem() { Content = s, Margin = new Thickness(0, 0, 5, 0) });
            }
            CollectFavourites();
            Resized(null, null);
        }
        private void CollectFavourites()
        {
            string favsfile = System.IO.Path.Combine(MPQPaths.local, "Paths\\Favourites.txt");
            if (!File.Exists(favsfile) ) { return; }
            foreach (string line in File.ReadAllLines(favsfile))
                {
                string[] split = line.Split("|");
                FavouritesList.Add(split[0], split[1]);
                ListFavourites.Items.Add(new ListBoxItem() { Content = split[0], Margin = new Thickness(5,0,5,0) });
            }
        }
        private void CopyName(object sender, RoutedEventArgs e)
        {
            if (Resultbox.SelectedItems.Count != 1) { return; }
            Clipboard.SetText(GetSelected());
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            if (Resultbox.SelectedItems.Count != 1) { return; }
            string name = GetSelected();
            SaveFileDialog sv = new SaveFileDialog();

            sv.Filter = "BLP Files (*.blp)|*.blp"; ;
            if (sv.ShowDialog() == true)
            {
                string target = sv.FileName;
                MPQHelper.Export(name, target);
            }
        }
        private string GetSelected()
        {
            ListBoxItem item = Resultbox.SelectedItem as ListBoxItem;
            return item.Content.ToString();
        }
        private string GetSelectedFav()
        {
            ListBoxItem item = ListFavourites.SelectedItem as ListBoxItem;
            return item.Content.ToString();
        }
        private void SelectedItem(object sender, SelectionChangedEventArgs e)
        {
            if (Resultbox.SelectedItems.Count != 1) { return; }
            string name = GetSelected();
            DisplayImage.Source = MPQHelper.GetImageSource(name);
            Title = appTitle + " - " + name; 
        }

        private void ExportPNG(object sender, RoutedEventArgs e)
        {
            if (Resultbox.SelectedItems.Count != 1) { return; }
            string name = GetSelected();
            SaveFileDialog sv = new SaveFileDialog();

            sv.Filter = "PNG Files (*.png)|*.png"; ;
            if (sv.ShowDialog() == true)
            {
                string target = sv.FileName;
                MPQHelper.ExportPNG(MPQHelper.GetImageSource(name), target);
            }
        }

        private void Search(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Resultbox.Items.Clear();
                string searched = Searchbox.Text.Trim().ToLower();
                if (searched.Length == 0)
                {


                    foreach (string s in MPQHelper.Listfile_All)
                    {
                        if (Check_ExcludeIcons.IsChecked == true)
                        {
                            if (
                                s.ToLower().Contains("ReplaceableTextures\\CommandButtonsDisabled\\DISBTN".ToLower()) == false &&
                                s.ToLower().Contains("ReplaceableTextures\\CommandButtons\\BTN".ToLower()) == false &&
                                s.ToLower().Contains("ReplaceableTextures\\PassiveButtons\\PASBTN".ToLower()) == false &&
                                s.ToLower().Contains("UI\\Widgets\\BattleNet\\chaticons".ToLower()) == false
                                )
                            {
                                Resultbox.Items.Add(new ListBoxItem() { Content = s, Margin = new Thickness(0, 0, 5, 0) });
                            }

                        }
                        else
                        {
                            Resultbox.Items.Add(new ListBoxItem() { Content = s, Margin = new Thickness(0, 0, 5, 0) });
                        }
                    }
                }
                else
                {
                    foreach (string s in MPQHelper.Listfile_All)
                    {
                        if (s.ToLower() .Contains(searched))
                        {
                            if (Check_ExcludeIcons.IsChecked == true)
                            {
                                if (
                                    s.ToLower().Contains("ReplaceableTextures\\CommandButtonsDisabled\\DISBTN".ToLower()) == false &&
                                    s.ToLower().Contains("ReplaceableTextures\\CommandButtons\\BTN".ToLower()) == false &&
                                    s.ToLower().Contains("ReplaceableTextures\\PassiveButtons\\PASBTN".ToLower()) == false &&
                                    s.ToLower().Contains("UI\\Widgets\\BattleNet\\chaticons".ToLower()) == false 
                                    )
                                {
                                    Resultbox.Items.Add(new ListBoxItem() { Content = s, Margin = new Thickness(0, 0, 5, 0) });
                                }

                            }
                            else
                            {
                                Resultbox.Items.Add(new ListBoxItem() { Content = s, Margin = new Thickness(0, 0, 5, 0) });
                            }
                           
                        }
                    }
                }
            }
        }

        private void Resized(object sender, SizeChangedEventArgs e)
        {
            Resultbox.Height = LeftPanel.ActualHeight * 0.43;
            ListFavourites.Height = LeftPanel.ActualHeight * 0.20;
        }

        private void AddFav(object sender, RoutedEventArgs e)
        {
            if (Resultbox.SelectedItems.Count != 1) { return; }
            string name = GetSelected();
            Input i = new Input();
            if (i.ShowDialog() == true)
            {
                string given = i.box.Text;
                if (FavouritesList.ContainsKey(given))
                {
                    MessageBox.Show("Favourite with this note exists");
                }
                else
                {
                    FavouritesList.Add(given,name);
                    ListFavourites.Items.Add(new ListBoxItem() { Content = given, Margin = new Thickness(0, 0, 5, 0) }); ;
                    string favsfile = System.IO.Path.Combine(MPQPaths.local, "Paths\\Favourites.txt");
                    File.AppendAllLines(favsfile,new List<string>() { $"{given}|{name}" });
                }
            }
            //FavouritesList
        }

        private void del(object sender, MouseButtonEventArgs e)
        {
            string name = GetSelectedFav();
            string favsfile = System.IO.Path.Combine(MPQPaths.local, "Paths\\Favourites.txt");
           
            ListFavourites.Items.Remove(ListFavourites.SelectedItem);
            List<string> lines = File.ReadAllLines(favsfile).ToList();
            lines.RemoveAll(x=>x.Contains(FavouritesList[name]));
            FavouritesList.Remove(name);
            File.WriteAllText(favsfile,string.Join("\n", lines));
            
        }

        private void SelectedFav(object sender, SelectionChangedEventArgs e)
        {
            if (ListFavourites.SelectedItems.Count != 1) { return; }
            string name = GetSelectedFav();
            if (FavouritesList.ContainsKey(name))
            {
                DisplayImage.Source = MPQHelper.GetImageSource(FavouritesList[name]);
            }
        }

        private void CopyFav(object sender, RoutedEventArgs e)
        {
            if (ListFavourites.SelectedItems.Count != 1) { return; }
            string name = GetSelectedFav();
            Clipboard.SetText(FavouritesList[name]);
        }

        private void SelectedFav(object sender, RoutedEventArgs e)
        {
            if (ListFavourites.SelectedItems.Count != 1) { return; }
            string name = GetSelectedFav();
            if (FavouritesList.ContainsKey(name))
            {
                DisplayImage.Source = MPQHelper.GetImageSource(FavouritesList[name]);
            }
        }

        private void ExpandOnChecked(object sender, RoutedEventArgs e)
        {
           /* bool ExpandON = Checkbox_Expand.IsChecked == true;
            if (ExpandON)
            {
                ListFavourites.MaxWidth = double.PositiveInfinity; // No upper bound
                Resultbox.MaxWidth = double.PositiveInfinity;
            }
            else
            {
                ListFavourites.MaxWidth = 400; // Set max width to 400
                Resultbox.MaxWidth = 400;
                ListFavourites.Width = 300;
                Resultbox.Width = 300;
            }*/
        }

        private void ScrollToSearch(object sender, RoutedEventArgs e)
        {
            if (Resultbox.SelectedItems.Count != 1) { return; }
            Resultbox.ScrollIntoView(Resultbox.SelectedItem);
        }

        private void ScrollToFav(object sender, RoutedEventArgs e)
        {
            if (ListFavourites.SelectedItems.Count != 1) { return ; }
           ListFavourites.ScrollIntoView(ListFavourites.SelectedItem);
        }

        private void FindInResultBox(object sender, RoutedEventArgs e)
        {
            if (ListFavourites.SelectedItems.Count != 1) { return; }
            string selected = GetSelectedFav();
            string selectedPath = FavouritesList[selected];
           

           
           
            foreach (object sItem in Resultbox.Items)
            {

                ListBoxItem item = sItem as ListBoxItem;
                string name = item.Content .ToString();
                if (name == selectedPath)
                {

                    Resultbox.SelectedItem = sItem;
                    Resultbox.ScrollIntoView(sItem);
                }
            }
        }
    }
}