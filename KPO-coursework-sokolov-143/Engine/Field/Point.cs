namespace Engine.Field
{

    /// <summary>
    /// Точка на двумерной карте
    /// </summary>
    public class Point
    {
        /// <summary>
        /// Абсцисса точки
        /// </summary>
        private float _x = 0;

        /// <summary>
        /// Ордината точки
        /// </summary>
        private float _y = 0;

        /// <summary>
        /// Абсцисса точки
        /// </summary>
        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
            }
        }

        /// <summary>
        /// Ордината точки
        /// </summary>
        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
            }
        }

        /// <summary>
        /// Конструктор по умолчанию для точки на плоскости
        /// </summary>
        public Point()
        {

        }
        /// <summary>
        /// Конструктор точки на плоскости
        /// </summary>
        /// <param name="parX">Абсцисса точки</param>
        /// <param name="parY">Ордината точки</param>
        public Point(float parX, float parY)
        {
            _x = parX;
            _y = parY;
        }

        /// <summary>
        /// Конструктор копирования
        /// </summary>
        /// <param name="parOther">Копируемая точка</param>
        public Point(Point parOther)
        {
            _x = parOther.X;
            _y = parOther.Y;
        }

        /// <summary>
        /// Вычислить расстояние до заданной точки
        /// </summary>
        /// <param name="parTargetPoint">Вторая точка</param>
        /// <returns>Расстояние между точками</returns>
        public uint DistanceTo(Point parTargetPoint)
        {
            return (uint)Math.Sqrt(
                Math.Pow((Math.Max(X, parTargetPoint.X) - Math.Min(X, parTargetPoint.X)), 2) +
                Math.Pow((Math.Max(Y, parTargetPoint.Y) - Math.Min(Y, parTargetPoint.Y)), 2));
        }
    }
}
