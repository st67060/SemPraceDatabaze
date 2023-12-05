
using IronPdf;
using System.ComponentModel;

namespace Aplikace.data.Entity
{
    public class Document : INotifyPropertyChanged
    {
        private int id;
        private PdfDocument file;
        private string fileName;
        private string fileSuffix;

        public int Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public PdfDocument File
        {
            get { return file; }
            set
            {
                if (file != value)
                {
                    file = value;
                    OnPropertyChanged(nameof(File));
                }
            }
        }

        public string FileName
        {
            get { return fileName; }
            set
            {
                if (fileName != value)
                {
                    fileName = value;
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }

        public string FileSuffix
        {
            get { return fileSuffix; }
            set
            {
                if (fileSuffix != value)
                {
                    fileSuffix = value;
                    OnPropertyChanged(nameof(FileSuffix));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public Document(int id, PdfDocument file, string fileName, string fileSuffix)
        {
            Id = id;
            File = file;
            FileName = fileName;
            FileSuffix = fileSuffix;
            
        }
    }
}
