namespace Engine.Records
{
    /// <summary>
    /// Запись в таблице рекордов
    /// </summary>
    [Serializable]
    public class Record
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Счет игрока
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Уровень игры
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Сложность игры
        /// </summary>
        public string Difficulty { get; set; }

        /// <summary>
        /// Результат завершения игры
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Стандартный метод для сравнения объекта с текущим
        /// </summary>
        /// <param name="parObj">Объект для сравнения</param>
        /// <returns>Истина, если объекты равны</returns>
        public override bool Equals(object parObj)
        {
            if (parObj == null)
            {
                return false;
            }
            if (parObj is Record otherRecord)
            {
                return Username == otherRecord.Username &&
                       Score == otherRecord.Score &&
                       Level == otherRecord.Level &&
                       Difficulty == otherRecord.Difficulty &&
                       Status == otherRecord.Status;
            }
            return false;
        }

        /// <summary>
        /// Получить хеш объекта
        /// </summary>
        /// <returns>Хеш объекта</returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Username, Score, Level, Difficulty, Status);
        }
    }
}
