using System.Drawing;

namespace AIMS.V2.ExportExcel
{
    public class GridStyle
    {
        public bool? _bold { get; set; }
        public string? _format { get; set; }
        public double? _width { get; set; }
        public HorizontalAlignment _horizontalAlignment { get; set; } = HorizontalAlignment.Left;
        public bool? _wrapText { get; set; } = false;
        internal Color? _color;
        internal Color? _bgColor;

        protected GridStyle() { }
        public static GridStyle Init() => new();

        public GridStyle Format(string format)
        {
            _format = format;
            return this;
        }
        public GridStyle Width(double width)
        {
            _width = width;
            return this;
        }

        public GridStyle Alignment(HorizontalAlignment alignment)
        {
            _horizontalAlignment = alignment;
            return this;
        }
        public GridStyle Bold(bool bold)
        {
            _bold = bold;
            return this;
        }
        public GridStyle WrapText(bool wrapText)
        {
            _wrapText = wrapText;
            return this;
        }

        public GridStyle Color(Color color)
        {
            _color = color;
            return this;
        }

        public GridStyle Background(Color bgColor)
        {
            _bgColor = bgColor;
            return this;
        }
    }

}
