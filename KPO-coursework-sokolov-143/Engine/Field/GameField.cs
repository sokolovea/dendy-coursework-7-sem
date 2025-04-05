using Engine.DynamicObjects;
using Engine.DynamicObjects.Tank;

namespace Engine.Field
{
    /// <summary>
    /// Игровое поле, на котором происходят бои танков
    /// </summary>
    public class GameField
    {
        /// <summary>
        /// Игровое поле, состоящее из ячеек со статическими объектами
        /// </summary>
        private Cell[,] _cellField;

        /// <summary>
        /// Множество танков
        /// </summary>
        private HashSet<AbstractTank> _tanks = new HashSet<AbstractTank>();

        /// <summary>
        /// Множество пуль
        /// </summary>
        private HashSet<Bullet> _bullets = new HashSet<Bullet>();

        /// <summary>
        /// Относительная ширина клетки (в масштабах координат)
        /// </summary>
        private int _cellSize;

        /// <summary>
        /// Игровое поле, состоящее из ячеек
        /// </summary>
        public Cell[,] CellField
        {
            get
            {
                return _cellField;
            }
            private set
            {
                _cellField = value;
            }
        }

        /// <summary>
        /// Множество танков
        /// </summary>
        public HashSet<AbstractTank> Tanks
        {
            get
            {
                return _tanks;
            }
            private set
            {
                _tanks = value;
            }
        }

        /// <summary>
        /// Множество пуль
        /// </summary>
        public HashSet<Bullet> Bullets
        {
            get
            {
                return _bullets;
            }
            private set
            {
                _bullets = value;
            }
        }

        /// <summary>
        /// Относительная ширина клетки (в масштабах координат)
        /// </summary>
        public int CellSize
        {
            get
            {
                return _cellSize;
            }
            private set
            {
                _cellSize = value;
            }
        }

        /// <summary>
        /// Конструктор игрового поля с заданной шириной и высотой
        /// </summary>
        /// <param name="parWidthCells">Ширина игрового поля в клетках</param>
        /// <param name="parHeightCells">Высота игрового поля в клетках</param>
        /// <param name="parCellSize">Относительная ширина клетки (в масштабах координат)</param>
        public GameField(int parWidthCells, int parHeightCells, int parCellSize)
        {
            _cellField = new Cell[parWidthCells, parHeightCells];
            for (int i = 0; i < parWidthCells; i++)
            {
                for (int j = 0; j < parHeightCells; j++)
                {
                    CellField[i, j] = new Cell();
                }
            }
            _cellSize = parCellSize;
        }

        /// <summary>
        /// Проверка, находится ли координата внутри карты
        /// </summary>
        /// <param name="parPosition">Координата для проверки</param>
        /// <returns>Истина, если координата принадлежит карте</returns>
        public bool IsWithinBounds(Point parPosition)
        {
            long fieldWidth = _cellField.GetLength(0) * _cellSize;
            long fieldHeight = _cellField.GetLength(1) * _cellSize;
            return parPosition.X < fieldWidth && parPosition.Y < fieldHeight;
        }

        /// <summary>
        /// Получить ячейку по координатам в пикселях
        /// </summary>
        /// <param name="parX">Абсцисса клетки</param>
        /// <param name="parY">Ордината ячейки</param>
        /// <returns>Клетка по заданным координатам</returns>
        public Cell GetCellByCoordinates(int parX, int parY)
        {
            int fieldWidth = (int)_cellField.GetLength(0) * _cellSize;
            int fieldHeight = (int)_cellField.GetLength(1) * _cellSize;

            int xCell = parX / (int)_cellSize;
            int yCell = parY / (int)_cellSize;

            if (xCell > _cellField.GetLength(0) - 1)
            {
                xCell = _cellField.GetLength(0) - 1;
            }
            if (yCell > _cellField.GetLength(1) - 1)
            {
                yCell = _cellField.GetLength(1) - 1;
            }

            return CellField[xCell, yCell];
        }

        /// <summary>
        /// Получить ширину поля в клетках
        /// </summary>
        /// <returns>Ширина поля в клетках</returns>
        public int GetWidthInCells()
        {
            return _cellField.GetLength(0);
        }

        /// <summary>
        /// Получить высоту поля в клетках
        /// </summary>
        /// <returns>Высота поля в клетках</returns>
        public int GetHeightInCells()
        {
            return _cellField.GetLength(1);
        }

        /// <summary>
        /// Получить ширину поля в клетках
        /// </summary>
        /// <returns>Ширина поля в клетках</returns>
        public int GetWidthInPixels()
        {
            return GetWidthInCells() * _cellSize;
        }

        /// <summary>
        /// Получить высоту поля в клетках
        /// </summary>
        /// <returns>Высота поля в клетках</returns>
        public int GetHeightInPixels()
        {
            return GetHeightInCells() * _cellSize;
        }
    }
}
