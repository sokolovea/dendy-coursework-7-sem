namespace Engine.StaticObjects
{
    /// <summary>
    /// Блок штаба
    /// </summary>
    public class Staff : AbstractStaticObject
    {
        /// <summary>
        /// Создает блок штаба (непроходим танком и пулей, но разрушим)
        /// </summary>
        public Staff() : base(false, false, true)
        {

        }
    }
}
