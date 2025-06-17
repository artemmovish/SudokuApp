using Sudoku.Core.Models;
using Sudoku.UI.Pages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sudoku.Core.ViewModels.SinglTone
{
    public class PageStorage
    {
        private static PageStorage _instance;

        public MapPage MapPage { get; set; }
        public SettingsPage SettingsPage { get; set; }
        public int Difficulty { get; set; }
        public int OpenLockCount { get; set; }

        public int OpenMonogram { get; set; }

        public List<CompletedLevel> CompletedLevels { get; set; }

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

        public static void ResetStorage()
        {
            _instance = null;

            string filePath = Path.Combine("Saves", "completed_levels.json");

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        private void NewSave(string filePath)
        {
            // Если путь не найден
            CompletedLevels = new()
{
    new CompletedLevel { Name = "VitebskLevel_IronGift", CountLock = 3 },    // ЖД вокзал (Витебск)
    new CompletedLevel { Name = "VitebskLevel_Theatre", CountLock = 2 },     // Театр Якуба Колоса (Витебск)
    new CompletedLevel { Name = "MogilevLevel_Theatre", CountLock = 3 },     // Городской театр (Могилев)
    new CompletedLevel { Name = "MogilevLevel_IronGift", CountLock = 2 },    // ЖД вокзал (Могилев)
    new CompletedLevel { Name = "GomelLevel_Home", CountLock = 3 },          // Дом врача Александрова (Гомель)
    new CompletedLevel { Name = "GomelLevel_Bank", CountLock = 2 }           // Орловский коммерческий банк (Гомель)
};

            string json = JsonSerializer.Serialize(CompletedLevels, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            // Создаём папку, если её нет
            if (!Directory.Exists("Saves"))
            {
                Directory.CreateDirectory("Saves");
            }

            // Теперь записываем файл
            File.WriteAllText(filePath, json);
        }

        private PageStorage()
        {
            string filePath = Path.Combine("Saves", "completed_levels.json");

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                CompletedLevels = JsonSerializer.Deserialize<List<CompletedLevel>>(json);
            }
            else
            {
                NewSave(filePath);
            }

            filePath = Path.Combine("Saves", "completed_monogram.txt");

            if (File.Exists(filePath))
            {
                string content = File.ReadAllText(filePath);
                if (int.TryParse(content, out int savedMonogram))
                {
                    PageStorage.Instance.OpenMonogram = savedMonogram;
                }
                else
                {
                    // Обработка случая, когда в файле не число
                    Console.WriteLine("Файл содержит некорректные данные");
                    PageStorage.Instance.OpenMonogram = 0; // Значение по умолчанию
                }
            }
            Difficulty = 50;
            SettingsPage = new SettingsPage();
            OpenLockCount = 0;


            MapPage = new MapPage();
        }
    }
}
