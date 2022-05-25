using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace IndusValleyCivilizationWrittingSystem
{
    public class SignObject : INotifyPropertyChanged,IComparable<SignObject>
    {
        private int index;
        public int Index
        {
            get { return index; }
            set
            {
                index = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Index"));
            }
        }
        private string filePath;
        public string FilePath
        {
            get => filePath; set
            {
                filePath = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FilePath"));
            }
        }
        private string name;
        public string Name
        {
            get => name; set
            {
                name = value;
                PropertyChanged?.Invoke(value, new PropertyChangedEventArgs("Name"));
            }
        }
        private string meaning;
        public string Meaning
        {
            get => meaning; set
            {
                meaning = value;
                PropertyChanged?.Invoke(value, new PropertyChangedEventArgs("Meaning"));
            }
        }
        private string shapeInDravidian;
        public string ShapeInDravidian
        {
            get => shapeInDravidian; set
            {
                shapeInDravidian = value;
                PropertyChanged?.Invoke(value, new PropertyChangedEventArgs("ShapeInDravidian"));
            }
        }
        private string notes;
        public string Notes
        {
            get => notes; set
            {
                notes = value;
                PropertyChanged?.Invoke(value, new PropertyChangedEventArgs("Notes"));
            }
        }
        private bool isReadOnly;
        public bool IsReadOnly
        {
            get => isReadOnly; set
            {
                isReadOnly = value;
                PropertyChanged?.Invoke(value, new PropertyChangedEventArgs("IsReadOnly"));
            }
        }

        public Visibility Visibility=>IsReadOnly ? Visibility.Collapsed : Visibility.Visible;

        public string ImageFile //=> FilePath; 
        {
            get
            {
                if (string.IsNullOrEmpty(FilePath))
                    return "";
                string[] strs = FilePath.Split(' ');
                StringBuilder sb = new StringBuilder();
                foreach (string str in strs)
                {
                    sb.Append((char)int.Parse(str,System.Globalization.NumberStyles.HexNumber));
                }
                return sb.ToString();
            }
        }
        //new BitmapImage(new Uri(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img",FilePath))); // System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory,FilePath)

        public event PropertyChangedEventHandler PropertyChanged;

        public static List<SignObject> GetSignList(string[] strs)
        {
            List<SignObject> signList = new List<SignObject>();
            foreach (string str in strs)
            {
                SignObject signObject = new SignObject();
                string [] vals=str.Split(',');
                signObject.index = int.Parse(vals[0]);
                signObject.FilePath = vals[1];
                signObject.Name = vals[2];
                signObject.ShapeInDravidian = vals[4];
                signObject.Meaning=vals[3];
                signObject.Notes = vals[5];
                signObject.IsReadOnly=vals[6].ToLower()=="true";
                signList.Add(signObject);   
            }
            return signList;
        }

        public int CompareTo(SignObject other)
        {
            return index.CompareTo(other.index);
        }
        public override string ToString()
        {
            return $"{index},{FilePath},{Name},{Meaning.Replace(",", " ")},{ShapeInDravidian.Replace(",", " ")},{Notes.Replace(",", " ")},{IsReadOnly}";
        }
    }
}
