using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Laba_2
{
    public class Paging
    {
        public int CurrentPage { get; set; }
        public List<List<DataUnit>> pages = new List<List<DataUnit>>();

        public Paging() { }

        public Paging(List<DataUnit> data)
        {
            CurrentPage = 0;
            CreatePages(data);
        }

        public void CreatePages(List<DataUnit> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                if (i % 15 == 0)
                    pages.Add(new List<DataUnit>());
                pages.Last().Add(data[i]);
            }
        }

        public List<DataUnit> ToPreviousPage()
        {
            if (CurrentPage > 0)
            {
                CurrentPage--;
            }
            return pages[CurrentPage];
        }

        public List<DataUnit> ToNextPage()
        {
            if (CurrentPage < pages.Count - 1)
            {
                CurrentPage++;
            }
            return pages[CurrentPage];
        }

        public List<DataUnit> ToFirstPage()
        {
            CurrentPage = 0;
            return pages[CurrentPage];
        }

        public List<DataUnit> ToLastPage()
        {
            CurrentPage = pages.Count - 1;
            return pages[CurrentPage];
        }
    }
}
