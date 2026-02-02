using System.Drawing;

namespace AIMS.V2.ExportExcel
{
    public class GridHeader
    {
        internal string _title;
        internal GridStyle _colStyle = GridStyle.Init();
        internal string _dataIndex;
        internal bool _isHidden = false;
        internal Func<object?, object?>? _transformer;

        public string Index() => _dataIndex;
        protected GridHeader() { }
        public static GridHeader Init() => new();

        public GridHeader Title(string title)
        {
            _title = title;
            return this;
        }
        public GridHeader ColStyle(GridStyle style)
        {
            _colStyle = style;
            return this;
        }
        public GridHeader DataIndex(string dataIndex)
        {
            _dataIndex = dataIndex;
            return this;
        }
        public GridHeader IsHidden(bool isHidden)
        {
            _isHidden = isHidden;
            return this;
        }
        public GridHeader Color(Color color)
        {
            _colStyle._color = color;
            return this;
        }
        public GridHeader BackgroundColor(Color bgColor)
        {
            _colStyle._bgColor = bgColor;
            return this;
        }
        public GridHeader Transformer(Func<object?, object?> transformer)
        {
            _transformer = transformer;
            return this;
        }
    }

}
