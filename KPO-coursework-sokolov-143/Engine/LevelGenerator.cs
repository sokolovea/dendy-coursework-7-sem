using Engine.ConcreteStrategy;
using Engine.Enum;
using Engine.Field;
using System.Reflection;

namespace Engine
{
    /// <summary>
    /// Класс для создания уровней (используется как контекст для шаблона проектирования "Стратегия")
    /// </summary>
    public class LevelGenerator
    {
        /// <summary>
        /// Ширина поля в клетках
        /// </summary>
        private const int FIELD_WIDTH = 50;

        /// <summary>
        /// Высота поля в клетках
        /// </summary>
        private const int FIELD_HEIGHT = 40;

        /// <summary>
        /// Сторона клетки в пиклелях
        /// </summary>
        private const int CELL_SIZE = 16;

        /// <summary>
        /// Словарь стратегий для генераторов ячеек для разных цветов пикселов (блоки уровня хранятся в bmp)
        /// </summary>
        private readonly Dictionary<(byte R, byte G, byte B), ICellCreatorFabric> _colorCellCreators;

        /// <summary>
        /// Создает объект класса, заполняет словарь генератора ячеек
        /// </summary>
        public LevelGenerator()
        {
            _colorCellCreators = new Dictionary<(byte, byte, byte), ICellCreatorFabric>();

            RegisterColorCellCreator((0, 0, 0), new EmptyCellCreator()); // Пустая ячейка
            RegisterColorCellCreator((255, 0, 0), new WallCellCreator()); // Кирпичная стена
            RegisterColorCellCreator((128, 128, 128), new IndestructibleWallCellCreator()); // Нерушимая стена
            RegisterColorCellCreator((0, 255, 0), new BushCellCreator()); // Кусты
            RegisterColorCellCreator((0, 0, 255), new WaterCellCreator()); // Вода
            RegisterColorCellCreator((255, 255, 255), new StaffCellCreator()); // Штаб
        }

        /// <summary>
        /// Регистрация стратегии для конкретного цвета пикселя
        /// </summary>
        /// <param name="parColor">Цвет пикселя (RGB)</param>
        /// <param name="parCreatorStrategy">Стратегия для создания ячейки</param>
        private void RegisterColorCellCreator((byte R, byte G, byte B) parColor, ICellCreatorFabric parCreatorStrategy)
        {
            _colorCellCreators.Add(parColor, parCreatorStrategy);
        }

        /// <summary>
        /// Метод для заполнения поля блоками для заданного уровня
        /// </summary>
        /// <param name="parLevelEnum">Уровень игры</param>
        /// <returns>Заполненное блоками игровое поле</returns>
        public GameField CreateLevel(LevelEnum parLevelEnum)
        {
            GameField gameField = null;
            switch (parLevelEnum)
            {
                case LevelEnum.First:
                    gameField = CreateConcreteLevel("Engine.Levels.level1.bmp");
                    break;
                case LevelEnum.Second:
                    gameField = CreateConcreteLevel("Engine.Levels.level2.bmp");
                    break;
                case LevelEnum.Third:
                    gameField = CreateConcreteLevel("Engine.Levels.level3.bmp");
                    break;

            }
            return gameField;
        }

        /// <summary>
        /// Метод для заполнения поля блоками для конкретного уровня с заданным именем ресурса
        /// </summary>
        /// <param name="parResourceName">Заданное имя ресурса</param>
        /// <returns> Заполненное блоками поле</returns>
        /// <exception cref="InvalidOperationException">Выбрасывается, если ресурс не найден</exception>
        private GameField CreateConcreteLevel(String parResourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resources = assembly.GetManifestResourceNames();
            if (Array.IndexOf(resources, parResourceName) == -1)
            {
                throw new InvalidOperationException($"Ресурс {parResourceName} не найден. Доступные ресурсы: {string.Join(", ", resources)}");
            }

            using (var stream = assembly.GetManifestResourceStream(parResourceName))
            {
                if (stream == null)
                {
                    throw new InvalidOperationException($"Не удалось загрузить ресурс {parResourceName}.");
                }

                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    return LoadLevelFromBitmap(memoryStream.ToArray());
                }
            }
        }

        /// <summary>
        /// Загружает уровень из двоичного bmp
        /// </summary>
        /// <param name="parfileBinary">bmp файл в виде массива байт</param>
        /// <returns>Заполненное блоками игровое поле</returns>
        /// <exception cref="ArgumentException"></exception>
        public GameField LoadLevelFromBitmap(byte[] parfileBinary)
        {
            if (parfileBinary[0] != 'B' || parfileBinary[1] != 'M')
                throw new ArgumentException("Файл не является BMP изображением.");

            int width = BitConverter.ToInt32(parfileBinary, 18);
            int height = BitConverter.ToInt32(parfileBinary, 22);

            if (width != FIELD_WIDTH || height != FIELD_HEIGHT)
                throw new ArgumentException($"Размер изображения должен быть {FIELD_WIDTH}x{FIELD_HEIGHT}. Получено: {width}x{height}");

            int pixelArrayOffset = BitConverter.ToInt32(parfileBinary, 10);

            var field = new GameField(FIELD_WIDTH, FIELD_HEIGHT, CELL_SIZE);

            // Чтение самого изображение в BGR с учетом выравниваний
            int rowSize = ((24 * width + 31) / 32) * 4;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int pixelIndex = pixelArrayOffset + (height - 1 - y) * rowSize + x * 3;
                    byte b = parfileBinary[pixelIndex];
                    byte g = parfileBinary[pixelIndex + 1];
                    byte r = parfileBinary[pixelIndex + 2];

                    var color = (R: r, G: g, B: b);
                    if (_colorCellCreators.TryGetValue(color, out var cellCreator))
                    {
                        field.CellField[x, y] = cellCreator.CreateCell();
                    }
                    else
                    {
                        field.CellField[x, y] = new Cell();
                    }
                }
            }

            return field;
        }

    }
}
