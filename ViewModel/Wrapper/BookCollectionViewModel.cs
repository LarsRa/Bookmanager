using De.HsFlensburg.BookManager074.Business.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.BookManager074.Logic.UI.Wrapper
{
    /*
     * The class BookCollectionViewModel extends ObservableCollection and gets
     * instances of the typ BookViewModel. It wraps an instance of BookCollection
     * by getting the reference of the instance in the constructor.
     * It also implements the synchronization of the wrapped book collection
     * (BookCollectionViewModel) and the model book collection (BookCollection).
     */
    public class BookCollectionViewModel : ObservableCollection<BookViewModel>
    {
        public BookCollection bookCollection;
        private bool synchDisabled;
        private bool autoFetch = true;

        public BookCollectionViewModel (BookCollection books)
        {
            bookCollection = books;

            CollectionChanged += ViewModelCollectionChanged;

            // If model collection is observable register change
            // handling for synchronization from Models to ViewModels
            if (bookCollection is ObservableCollection<Book>)
            {
                var observableModels = bookCollection as ObservableCollection<Book>;
                observableModels.CollectionChanged += ModelCollectionChanged;
            }

         // Fecth ViewModels
        if (autoFetch)FetchFromModels();
        }

        /// CollectionChanged event of the ViewModelCollection
        public override sealed event NotifyCollectionChangedEventHandler CollectionChanged
        {
            add { base.CollectionChanged += value; }
            remove { base.CollectionChanged -= value; }
        }

        /*
         * Method for synchronization if the collection of the ViewModels changed with the
         * cases add, remove and reset.
         */
        private void ViewModelCollectionChanged(object bookCollectionVM, NotifyCollectionChangedEventArgs e)
        {
            if (synchDisabled) return;

            // Disable synchronization
            synchDisabled = true;

            switch (e.Action)
            {
                //add a new book to the model collection
                case NotifyCollectionChangedAction.Add:
                    foreach (var newBook in e.NewItems.OfType<BookViewModel>().Select(v => v.wrappedBook).OfType<Book>())
                        bookCollection.Add(newBook);
                    break;

                //remove a book from the model collection
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldBook in e.OldItems.OfType<BookViewModel>().Select(v => v.wrappedBook).OfType<Book>())
                        bookCollection.Remove(oldBook);
                    break;

                /*
                 * reset the model collection by clearing the collection and adding the models of the 
                 * saved viewmodels.
                 */
                case NotifyCollectionChangedAction.Reset:
                    bookCollection.Clear();
                    foreach (var book in e.NewItems.OfType<BookViewModel>().Select(v => v.wrappedBook).OfType<Book>())
                        bookCollection.Add(book);
                    break;
            }

            //Enable synchronization
            synchDisabled = false;
        }

        /*
         * Method for synchronization if the collection of the models changed with the
         * cases add, remove and reset.
         */
        private void ModelCollectionChanged(object modelCollection, NotifyCollectionChangedEventArgs e)
        {
            if (synchDisabled) return;
            synchDisabled = true;

            switch (e.Action)
            {
                //add a new book to the wrapped collection
                case NotifyCollectionChangedAction.Add:
                    foreach (var newBook in e.NewItems.OfType<Book>())
                        this.Add(new BookViewModel(newBook));
                    break;
                
                //remove a book from the wrapped collection
                case NotifyCollectionChangedAction.Remove:
                    foreach (var oldBook in e.OldItems.OfType<Book>())
                        this.Remove(GetViewModelOfModel(oldBook));
                    break;
                
                /*
                 * reset the wrapped collection by deleting all viewmodels and
                 * create new ones for each model.
                 */
                case NotifyCollectionChangedAction.Reset:
                    Clear();
                    FetchFromModels();
                    break;
            }

            synchDisabled = false;
        }

        /*
         * Gets the viewmodel of the given model
         */ 
        private BookViewModel GetViewModelOfModel(Book book)
        {
            return Items.OfType<BookViewModel>().FirstOrDefault(v => v.IsViewModelOf(book)) as BookViewModel;
        } 

        /// Load VM collection from model collection
        public void FetchFromModels()
        {
            // Deactivate change pushing
            synchDisabled = true;

            // Clear collection
            this.Clear();

            // Create and add new VM for each model
            foreach (var book in bookCollection)
                this.Add(new BookViewModel(book));

            // Reactivate change pushing
            synchDisabled = false;
        }
    }
}
