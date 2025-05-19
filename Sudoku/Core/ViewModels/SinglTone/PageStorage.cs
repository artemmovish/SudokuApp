using Sudoku.UI.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku.Core.ViewModels.SinglTone
{
    public class PageStorage
    {
        private static PageStorage _instance;

        public MapPage MapPage { get; set; }
        public SettingsPage SettingsPage { get; set; }

        public static PageStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PageStorage();
                }
                return _instance;
            }
        }

        private PageStorage()
        {
            MapPage = new MapPage();
            SettingsPage = new SettingsPage();
        }
    }
}
