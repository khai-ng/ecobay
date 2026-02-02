using OfficeOpenXml;
using System.Reflection;

namespace AIMS.V2.ExportExcel
{
    public class ExportExcelConfiguration<TModel>
    {
        internal string _title;
        internal IEnumerable<GridHeader> _headers;
        internal GridPosition _headerPosition;
        internal IEnumerable<TModel> _data;
        internal GridPosition _dataPosition;
        internal IReadOnlyDictionary<GridPosition, string>? _subjects = null;
        internal MemberInfo[] _members = typeof(TModel).GetMembers().Where(member => member.MemberType == MemberTypes.Property).ToArray();
        internal Action<ExcelWorksheet>? _postProcessing = null;
        internal List<(Func<TModel, bool>, GridStyle)>? _rowStyles;

        protected ExportExcelConfiguration() { }
        //protected internal ExportExcelConfiguration(ExportExcelConfiguration<TModel> config)
        //{
        //    if (config == null) throw new ArgumentNullException("config");

        //    _title = config._title;
        //    _headers = config._headers;
        //    _headerPosition = config._headerPosition;
        //    _data = config._data;
        //    _dataPosition = config._dataPosition;
        //    _subjects = config._subjects;
        //    _members = config._members;
        //    _postProcessing = config._postProcessing;
        //}

        public static ExportExcelConfiguration<TModel> Init() => new();
        public ExportExcelConfiguration<TModel> Title(string title)
        {
            _title = title;
            return this;
        }
        public ExportExcelConfiguration<TModel> Header(
            IEnumerable<GridHeader> headers,
            GridPosition position)
            {
                _headers = headers;
            _headerPosition = position;
            return this;
        }

        public ExportExcelConfiguration<TModel> Data(
            IEnumerable<TModel> data,
            GridPosition position)
        {
            _data = data;
            _dataPosition = position;
            return this;
        }

        public ExportExcelConfiguration<TModel> Subject(
            IReadOnlyDictionary<GridPosition, string> data)
        {
            _subjects = data;
            return this;
        }

        public ExportExcelConfiguration<TModel> PostProcess(
            Action<ExcelWorksheet> postProcessing)
        {
            _postProcessing = postProcessing;
            return this;
        }

        public ExportExcelConfiguration<TModel> RowStyle(
            Func<TModel, bool> filter,
            GridStyle style)
        {
            if (_rowStyles == null)
                _rowStyles = [];
            
            _rowStyles.Add((filter, style));
            return this;
        }
    }

    public class ExportExcelConfiguration<TModel, TGroupKey> : ExportExcelConfiguration<TModel>
    {
        internal Func<TModel, TGroupKey> _keyGroup;
        internal Func<IGrouping<TGroupKey, TModel>, TModel> _groupDataDefinition;

        public ExportExcelConfiguration(ExportExcelConfiguration<TModel> config)
        {
            if (config == null) throw new ArgumentNullException("config");

            _title = config._title;
            _headers = config._headers;
            _headerPosition = config._headerPosition;
            _data = config._data;
            _dataPosition = config._dataPosition;
            _subjects = config._subjects;
            _members = config._members;
            _postProcessing = config._postProcessing;
            _rowStyles = config._rowStyles;
        }

        public ExportExcelConfiguration<TModel, TGroupKey> Group(
            Func<TModel, TGroupKey> groupBy,
            Func<IGrouping<TGroupKey, TModel>, TModel> groupDefinition)
        {
            _keyGroup = groupBy;
            _groupDataDefinition = groupDefinition;
            return this;
        }
    }

    public enum HorizontalAlignment
    {
        Left = 1,
        Center = 2,
        Right = 3,
    }
}
