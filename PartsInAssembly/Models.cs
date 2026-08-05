namespace CSharp
{
    /// <summary>
    /// Информация об объекте, полученная от get-prop-objects / get-linked-fast
    /// </summary>
    public class ObjectInfo
    {
        public int idLink { get; set; }
        public int idVersion { get; set; }
        public string? type { get; set; }
        public string? product { get; set; }
        public string? version { get; set; }
        public double minCalc { get; set; }
        public double maxCalc { get; set; }
    }

    /// <summary>
    /// Атрибут версии, полученный от get-info-about-version-mode-3
    /// </summary>
    public class Attributes
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? value { get; set; }
    }

    /// <summary>
    /// Строка итогового отчёта — одна деталь в составе сборочной единицы
    /// </summary>
    public class ReportRow
    {
        public int IdVersion { get; set; }
        public int IdLink { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string VersionNumber { get; set; } = string.Empty;
    }
}
