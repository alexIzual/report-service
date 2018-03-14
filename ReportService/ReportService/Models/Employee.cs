namespace ReportService.Domain
{
    /// <summary>
    /// Модель сотрудника.
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// ФИО сотрудника.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Название отдела.
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// ИНН.
        /// </summary>
        public string  Inn { get; set; }

        /// <summary>
        /// Зарплата в месяц.
        /// </summary>
        public decimal? Salary { get; set; }

        /// <summary>
        /// Код сотрудника.
        /// </summary>
        public string BuhCode { get; set; }
    }
}
