using De.HsFlensburg.BookManager074.Business.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.BookManager074.Logic.UI.Wrapper
{
    /*
     * The class BookViewModel wraps a book by getting the reference 
     * of an instance of book in the constructor and having properties 
     * with getter and setters for all attributes.
     */

    public class BookViewModel
    {
        public Book wrappedBook;

        public BookViewModel(Book book)
        {
            wrappedBook = book;
        }

        public string BookAuthor
        {
            get { return wrappedBook.Author; }
            set { wrappedBook.Author = value; }
        }

        public string BookTitle
        {
            get { return wrappedBook.Title; }
            set { wrappedBook.Title = value; }
        }

        public int BookEdition
        {
            get { return wrappedBook.Edition; }
            set { wrappedBook.Edition = value; }
        }

        public Boolean BookAvailable
        {
            get { return wrappedBook.Available; }
            set { wrappedBook.Available = value; }
        }

        public int BookPubyear
        {
            get { return wrappedBook.PubYear; }
            set { wrappedBook.PubYear = value; }
        }

        /*
         * Checks if the given book is the same as the wrapped book 
         * by checking the references.
         * It is used for the synchronisation of the two collections of 
         * books (model and viewmodel).
         */
        public bool IsViewModelOf(Book book)
        {
            return book == wrappedBook;
        }
    }
}
