using System.Reflection;

namespace SV.Utility.ExportExcel
{

	public static class ExportExcelDataExtensions
    {
        public static ExportExcelData<T> Title<T>(this ExportExcelData<T> master,
            string title)
        {
            master.Title = title;
            return master;
        }
        public static ExportExcelData<T> Header<T>(this ExportExcelData<T> master, 
            IEnumerable<GridHeader> headers, 
            GridPosition postition)
        {
            master.Headers = headers;
            master.HeaderPosition = postition;
            return master;
        }

        public static ExportExcelData<T> Data<T>(this ExportExcelData<T> master, 
            IEnumerable<T> data, 
            GridPosition postition)
        {
            master.Data = data;
            master.DataPosition = postition;
            return master;
        }

        public static ExportExcelData<T> Subject<T>(this ExportExcelData<T> master, IReadOnlyDictionary<GridPosition, string> data)
        {
            master.Subject = data;
            return master;
        }

        public static ExportExcelData<T> Members<T>(this ExportExcelData<T> master, MemberInfo[] members)
		{
			master.Members = members;
			return master;
		}
	}

    public static class GridStyleExtesions
    {
        public static GridStyle Format(this GridStyle style, string format)
        {
            style.Format = format;
            return style;
        }
        public static GridStyle Width(this GridStyle style, double width)
        {
            style.Width = width;
            return style;
        }

        public static GridStyle Alignment(this GridStyle style, HorizontalAlignment alignment)
        {
            style.HorizontalAlignment = alignment;
            return style;
        }
    }
}
