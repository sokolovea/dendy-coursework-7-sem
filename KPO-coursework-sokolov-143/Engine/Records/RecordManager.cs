using System.Xml.Serialization;

namespace Engine.Records
{
    /// <summary>
    /// Менеджер рекордов
    /// </summary>
    public class RecordManager
    {
        /// <summary>
        /// Имя файла с рекордами
        /// </summary>
        private const string FilePath = "records.xml";

        /// <summary>
        /// Путь к файлу с рекордами
        /// </summary>
        private readonly string recordsFilePath;

        /// <summary>
        /// Список рекордов
        /// </summary>
        public HashSet<Record> Records { get; private set; }

        /// <summary>
        /// Получить лучшие рекорды
        /// </summary>
        /// <param name="parMaxCount">Максимальное количество рекордов</param>
        /// <returns></returns>
        public List<Record> GetTopRecords(int parMaxCount = 10)
        {
            return Records
                .OrderByDescending(record => record.Score)
                .ThenBy(record => record.Status)
                .ThenByDescending(record => record.Difficulty)
                .Take(parMaxCount)
                .ToList();
        }

        /// <summary>
        /// Создает настроенный экземпляр класса менеджера рекордов
        /// </summary>
        public RecordManager()
        {
            string localAppDataPath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), "BattleCity");

            Directory.CreateDirectory(localAppDataPath);

            recordsFilePath = Path.Combine(localAppDataPath, FilePath);

            if (!File.Exists(recordsFilePath))
            {
                CreateEmptyFile();
            }

            Records = LoadRecords().ToHashSet();
        }

        /// <summary>
        /// Создает пустой файл рекордов
        /// </summary>
        private void CreateEmptyFile()
        {
            try
            {
                var emptyRecords = new List<Record>();
                using (var stream = new FileStream(recordsFilePath, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(List<Record>));
                    serializer.Serialize(stream, emptyRecords);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании файла: {ex.Message}");
            }
        }

        /// <summary>
        /// Загрузить рекорды из файла рекордов
        /// </summary>
        /// <returns>Список рекордов</returns>
        private List<Record> LoadRecords()
        {
            try
            {
                if (!File.Exists(recordsFilePath) || new FileInfo(recordsFilePath).Length == 0)
                {
                    Console.WriteLine("Файл отсутствует или пустой. Создание нового списка записей.");
                    return new List<Record>();
                }

                using (var stream = new FileStream(recordsFilePath, FileMode.Open))
                {
                    var serializer = new XmlSerializer(typeof(List<Record>));
                    return (List<Record>)serializer.Deserialize(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки записей: {ex.Message}");
                return new List<Record>();
            }
        }

        /// <summary>
        /// Сохранение записей с рекордами в XML-документ
        /// </summary>
        public void SaveRecords()
        {
            try
            {
                using (var stream = new FileStream(recordsFilePath, FileMode.Create))
                {
                    var serializer = new XmlSerializer(typeof(HashSet<Record>));
                    serializer.Serialize(stream, Records);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения записей: {ex.Message}");
            }
        }

        /// <summary>
        /// Добавление новой записи с рекордом
        /// </summary>
        /// <param name="parUsername">Логин игрока</param>
        /// <param name="parScore">Счет игрока</param>
        /// <param name="parLevel">Уровень игрока</param>
        /// <param name="parStatus">Победа или поражение в игре</param>
        /// <param name="parDifficulty">Сложность игры</param>
        public void AddRecord(string parUsername, int parScore, int parLevel, string parStatus, string parDifficulty)
        {
            Records.Add(new Record
            {
                Username = parUsername,
                Score = parScore,
                Level = parLevel,
                Status = parStatus,
                Difficulty = parDifficulty
            });
            SaveRecords();
        }
    }
}
