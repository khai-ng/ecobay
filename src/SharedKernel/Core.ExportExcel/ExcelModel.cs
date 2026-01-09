using System.Reflection;

namespace SV.Utility.ExportExcel
{

	public class ExportExcelData<T>
    {
        public ExportExcelData() { }
        public static ExportExcelData<T> Init => new();
        public string Title { get; protected internal set; }
        public IEnumerable<GridHeader> Headers { get; protected internal set; }
        public GridPosition HeaderPosition { get; protected internal set; }

        public IEnumerable<T> Data { get; protected internal set; }
        public GridPosition DataPosition { get; protected internal set; }

        public IReadOnlyDictionary<GridPosition, string> Subject { get; protected internal set; } = new Dictionary<GridPosition, string>();

        public MemberInfo[] Members { get; protected internal set; } = typeof(T).GetMembers().Where(member => member.MemberType == MemberTypes.Property).ToArray();
    }

    public record GridHeader(
        string Title,
        GridStyle Style,
        string DataIndex,
        Func<object?, object?>? Transformer = null
        ) { }

    public record GridPosition(int Row, int Col)
    { }

    public class GridStyle
    {
        public bool? Bold { get; set; }
        public string? Format { get; set; }
        public double? Width { get; set; }
        public HorizontalAlignment HorizontalAlignment { get; set; } = HorizontalAlignment.Left;

        public static GridStyle Init => new();
    }

    public enum HorizontalAlignment
    {
        Left = 1,
        Center = 2,
        Right = 3,
    }
}
