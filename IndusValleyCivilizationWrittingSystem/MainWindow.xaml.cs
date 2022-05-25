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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Collections.ObjectModel;

namespace IndusValleyCivilizationWrittingSystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<SignObject> writtingList = new ObservableCollection<SignObject>();
        public static SignList Signs = new SignList();
        public SignObject currentSign=new SignObject();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            gdDetail.DataContext = currentSign;
            GetSigns();
        }

        private async void GetSigns()
        {
            await Task.Run(() =>
            {
                string[] strs = File.ReadAllLines(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "signlist.csv"));
                var list = SignObject.GetSignList(strs);
                writtingList = new ObservableCollection<SignObject>(list);
                lv.Dispatcher.Invoke(() =>
                {
                    lv.ItemsSource = writtingList;
                });
            });
        }

        private void lv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SignObject signObject = (SignObject)lv.SelectedItem;
            if (signObject == null)
                return;
            SignObject sign = new SignObject()
            {
                Index = signObject.Index,
                Name = signObject.Name,
                FilePath = signObject.FilePath,
                Meaning = signObject.Meaning,
                Notes = signObject.Notes,
                IsReadOnly = signObject.IsReadOnly,
                ShapeInDravidian = signObject.ShapeInDravidian
            };
            currentSign = sign;
            gdDetail.DataContext = currentSign;
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string str=tbFilter.Text;
            if (string.IsNullOrEmpty(str))
                return;
            if (writtingList.Any(x => x.Name.Contains(str)))
            {
               var v= writtingList.FirstOrDefault(x => x.Name.Contains(str));
               int idx= writtingList.IndexOf(v);
                lv.ScrollIntoView(v);
                lv.SelectedIndex = idx;
            }
        }

        private void btnMore_Click(object sender, RoutedEventArgs e)
        {
            MoreWindow moreWindow = new MoreWindow();
            moreWindow.Show();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            int idx = writtingList.Count+1;
            SignObject sign = new SignObject()
            {
                Index = idx,
                Name = currentSign.Name,
                FilePath = currentSign.FilePath,
                Meaning = currentSign.Meaning,
                Notes = currentSign.Notes,
                IsReadOnly = idx<25,
                ShapeInDravidian = currentSign.ShapeInDravidian
            };

            writtingList.Add(sign);
            File.AppendAllLines(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "signlist.csv"), new string[] {sign.ToString() });

        }

        private void btnDelet_Click(object sender, RoutedEventArgs e)
        {
           var val= lv.SelectedItem as SignObject;
            if(val==null||val.Index<25)
            {
                MessageBox.Show("This item can't be deleted!");
                return;
            }
            int idx=writtingList.IndexOf(val);
            if(idx<0)
            {
                return;
            }
            writtingList.RemoveAt(idx);
            StringBuilder sb = new StringBuilder();
            idx = 1;
            foreach(SignObject sign in writtingList)
            {
                sign.Index = idx++;
                sign.IsReadOnly = sign.Index < 25;
                sb.AppendLine(sign.ToString());
            }
            File.WriteAllText(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "signlist.csv"), sb.ToString());

        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }
    }
}
