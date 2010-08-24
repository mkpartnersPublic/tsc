using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;

namespace Inventory
{


    public struct VersePrice
    {
        private int m_projectid;
        public int projectid
        {
            get
            {
                return m_projectid;
            }
            set
            {
                m_projectid = value;
            }

        }

        //languageId

        private int m_languageId;
        public int languageId
        {
            get
            {
                return m_languageId;
            }
            set
            {
                m_languageId = value;
            }

        }

        private Decimal m_versePrice;
        public Decimal versePrice
        {
            get
            {
                return m_versePrice;
            }
            set
            {
                m_versePrice = value;
            }

        }

        private int m_groupId;
        public int groupId
        {
            get
            {
                return m_groupId;
            }
            set
            {
                m_groupId = value;
            }

        }


        private Boolean m_sucess;
        public Boolean sucess
        {
            get
            {
                return m_sucess;
            }
            set
            {
                m_sucess = value;
            }

        }


    }
    public struct InventoryResponse
    {
        private string m_error;
        public string error
        {
            get
            {
                return m_error;
            }
            set
            {
                m_error = value;
            }

        }


        private Boolean m_sucess;
        public Boolean sucess
        {
            get
            {
                return m_sucess;
            }
            set
            {
                m_sucess = value;
            }

        }




        private List<VersePrice> m_VersePrice;


        public List<VersePrice> VersePrice
        {
            get
            {
                return m_VersePrice;
            }

            set
            {
                m_VersePrice = value;
            }
        }

        public string addVersePrice(VersePrice myVersePrice)
        {
            try
            {
                m_VersePrice.Add(myVersePrice);
                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }


        private List<Verses> m_verse;

        public Boolean initialize()
        {
            m_verse = new List<Verses>();
            m_VersePrice = new List<VersePrice>();
            return true;
        }

        public List<Verses> verse
        {
            get
            {
                return m_verse;
            }

            set
            {
                m_verse = value;
            }
        }

        public string addVerse(Verses myVerse)
        {
            try
            {
                m_verse.Add(myVerse);
                return "True";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }



    }
    public struct Verses
    {
        /// <summary>
        /// Backing Store for error
        /// </summary>
        private string m_error;

        /// <summary>
        /// Width of rectangle
        /// </summary>
        public string error
        {
            get
            {
                return m_error;
            }
            set
            {
                m_error = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string m_returnValue;

        /// <summary>
        /// 
        /// </summary>
        public string returnValue
        {
            get
            {
                return m_returnValue;
            }
            set
            {
                m_returnValue = value;
            }
        }
        private int m_id;
        public int id
        {
            get
            {
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }
        private string m_book;
        public string book
        {
            get
            {
                return m_book;
            }
            set
            {
                m_book = value;
            }
        }
        private string m_text;
        public string text
        {
            get
            {
                return m_text;
            }
            set
            {
                m_text = value;
            }
        }

        private int m_chapter;

        public int chapter
        {
            get
            {
                return m_chapter;
            }
            set
            {
                m_chapter = value;
            }
        }

        private int m_verse;

        public int verse
        {
            get
            {
                return m_verse;
            }
            set
            {
                m_verse = value;
            }
        }

        private int m_verseGroupId;

        public int verseGroupId
        {
            get
            {
                return m_verseGroupId;
            }
            set
            {
                m_verseGroupId = value;
            }
        }
    }


    public class InventoryInterfaces
    {

    }
}

