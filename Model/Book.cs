using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace De.HsFlensburg.BookManager074.Business.Model
{
    /*
     * The model class for a book. In here are the attributes of the book 
     * difined as public properties with getter and setters and private 
     * backing fields.
     */
    public class Book
    { 
        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string author;
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        private int edition;
        public int Edition
        {
            get { return edition; }
            set { edition = value; }
        }

        private bool available;
        public bool Available
        {
            get { return available; }
            set { available = value; }
        }

        private int pubYear;
        public int PubYear
        {
            get { return pubYear; }
            set { pubYear = value; }
        }
    }
}
