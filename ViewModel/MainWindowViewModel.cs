using De.HsFlensburg.BookManager074.Business.Model;
using De.HsFlensburg.BookManager074.Logic.UI.Wrapper;
using De.HsFlensburg.BookManager074.Logic.UI.Messages;
using De.HsFlensburg.BookManager074.Service.ServiceBus;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace De.HsFlensburg.BookManager074.Logic.UI
{
    public class MainWindowViewModel
    {
        public BookViewModel MyWrappedBook { get; set; }

        public BookCollectionViewModel MyWrappedBookCollection { get; set; }

        private RelayCommand openFileDialogCommand;       
        public ICommand FileDialog { get { return openFileDialogCommand; } }

        private RelayCommand exitApplicationCommand;
        public ICommand Shutdown { get { return exitApplicationCommand; } }

        private RelayCommand addNewBookCommand;
        public ICommand NewBook { get { return addNewBookCommand; } }


        public MainWindowViewModel()
        {
            /*
             * create book collection and add three example books
             */
            BookCollection MyBookCollection = new BookCollection();

            MyBookCollection.Add(new Book());
            MyBookCollection.ElementAt<Book>(0).Author = "Lars Raschke";
            MyBookCollection.ElementAt<Book>(0).Title = "How to program last minute";
            MyBookCollection.ElementAt<Book>(0).Edition = 1;
            MyBookCollection.ElementAt<Book>(0).PubYear = 2018;
            MyBookCollection.ElementAt<Book>(0).Available = true;

            MyBookCollection.Add(new Book());
            MyBookCollection.ElementAt<Book>(1).Author = "Dieter Bohlen";
            MyBookCollection.ElementAt<Book>(1).Title = "Weg zum Erfolg";
            MyBookCollection.ElementAt<Book>(1).Edition = 3;
            MyBookCollection.ElementAt<Book>(1).PubYear = 1995;
            MyBookCollection.ElementAt<Book>(1).Available = false;

            MyBookCollection.Add(new Book());
            MyBookCollection.ElementAt<Book>(2).Author = "Beate Uhse";
            MyBookCollection.ElementAt<Book>(2).Title = "Gummibaum statt Gummifrauen";
            MyBookCollection.ElementAt<Book>(2).Edition = 2;
            MyBookCollection.ElementAt<Book>(2).PubYear = 2010;
            MyBookCollection.ElementAt<Book>(2).Available = true;

            // create a wrapped book collection of the book collection
            MyWrappedBookCollection = new BookCollectionViewModel(MyBookCollection);

            // creating RelayCommands with the given methods
            openFileDialogCommand = new RelayCommand(OpenNewFileDialog);
            exitApplicationCommand = new RelayCommand(ExitCurrentApplication);

            addNewBookCommand = new RelayCommand(() =>
            Messenger.Instance.Send<OpenNewBookWindowMessage>(new OpenNewBookWindowMessage()));
        }

        /*
         * Open file dialog on clicking the open menu item
         */
        private void OpenNewFileDialog()
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == true)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                }
            }
        }
        /*
         * Method for exiting the application on clicking the exit menu item
         */
        private void ExitCurrentApplication()
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
