using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Reflection;

namespace Core.ExportExcel
{
    public static class ExcelProcessor
    {
        private static readonly Color DefaultHeaderBgColor = Color.FromArgb(93, 138, 168);
        
        public static ExcelPackage Init()
        {
            const string org = "STEngineering";
            ExcelPackage.License.SetNonCommercialOrganization(org);
            return new ExcelPackage();
        }

        /// <summary>
        /// Shortcut single object export function
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="excelData"></param>
        /// <param name="libEngineInsertion"></param>
        /// <returns></returns>
        public static async Task<byte[]> ExportAsync<TModel>(
            this ExportExcelConfiguration<TModel> excelData, 
            bool libEngineInsertion = false)
        {
            using var excelPackage = Init();
            using var worksheet = excelPackage.Process(excelData, libEngineInsertion);

            return await excelPackage.ExportAsync();
        }

        public static async Task<byte[]> ExportAsync(this ExcelPackage excelPackage)
        {
            return await excelPackage.GetAsByteArrayAsync();
        }

        /// <summary>
        /// Process insert data within <see cref="ExcelWorksheet"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="worksheet"></param>
        /// <param name="excelConfigs"></param>
        /// <param name="libEngineInsertion"></param>
        /// <param name="offset"></param>
        /// <returns>Current <see cref="ExcelWorksheet"/></returns>
        public static ExcelWorksheet Process<TModel>(
            this ExcelWorksheet worksheet,
            ExportExcelConfiguration<TModel> excelConfigs,
            bool libEngineInsertion = false,
            GridPosition? offset = null)
        {
            var filteredHeader = excelConfigs._headers?.Where(h => !h._isHidden);

            // Add data for body
            if (libEngineInsertion)
                worksheet.AddBodyByLibEngine(
                    filteredHeader,
                    excelConfigs._data,
                    excelConfigs._dataPosition.Offset(offset),
                    excelConfigs._members);
            else
                worksheet.AddBody(
                    filteredHeader,
                    excelConfigs._data,
                    excelConfigs._dataPosition.Offset(offset),
                    excelConfigs._rowStyles);
            
            if (filteredHeader != null && filteredHeader.Any())
                worksheet.AddHeader(filteredHeader,
                    excelConfigs._headerPosition.Offset(offset));

            if (excelConfigs._subjects != null && excelConfigs._subjects.Any())
                worksheet.AddSubject(excelConfigs._subjects, offset);

            if (excelConfigs._postProcessing != null) excelConfigs._postProcessing?.Invoke(worksheet);

            return worksheet;
        }

        /// <summary>
        /// Process insert data within <see cref="ExcelPackage"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="excelPackage"></param>
        /// <param name="excelConfigs"></param>
        /// <param name="libEngineInsertion"></param>
        /// <returns>Current <see cref="ExcelWorksheet"/></returns>
        public static ExcelWorksheet Process<TModel>(
            this ExcelPackage excelPackage,
            ExportExcelConfiguration<TModel> excelConfigs,
            bool libEngineInsertion = false)
        {
            var worksheet = excelPackage.Workbook.Worksheets.Add(excelConfigs._title);
            return worksheet.Process(excelConfigs, libEngineInsertion);
        }

        /// <summary>
        /// Process insert data within <see cref="ExcelWorksheet"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TGroupKey"></typeparam>
        /// <param name="worksheet"></param>
        /// <param name="excelConfigs"></param>
        /// <param name="libEngineInsertion"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static ExcelWorksheet Process<TModel, TGroupKey>(
            this ExcelWorksheet worksheet,
            ExportExcelConfiguration<TModel, TGroupKey> excelConfigs,
            bool libEngineInsertion = false,
            GridPosition? offset = null)
        {
            List<TModel> groupedData = [];
            var groups = excelConfigs._data.GroupBy(excelConfigs._keyGroup!);

            foreach (var group in groups)
            {
                groupedData.AddRange(group);
                groupedData.Add(excelConfigs._groupDataDefinition!(group));
            }

            var newConfigs = (ExportExcelConfiguration<TModel>)excelConfigs;
            newConfigs.Data(groupedData, excelConfigs._dataPosition);

            return worksheet.Process(newConfigs, libEngineInsertion, offset);
        }

        /// <summary>
        /// Process insert data within <see cref="ExcelPackage"/>
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TGroupKey"></typeparam>
        /// <param name="excelPackage"></param>
        /// <param name="excelConfigs"></param>
        /// <param name="libEngineInsertion"></param>
        /// <returns></returns>
        public static ExcelWorksheet Process<TModel, TGroupKey>(
            this ExcelPackage excelPackage,
            ExportExcelConfiguration<TModel, TGroupKey> excelConfigs,
            bool libEngineInsertion = false)
        {
            var worksheet = excelPackage.Workbook.Worksheets.Add(excelConfigs._title);
            return worksheet.Process(excelConfigs, libEngineInsertion);
        }

        private static void AddSubject(
            this ExcelWorksheet worksheet, 
            IReadOnlyDictionary<GridPosition, string> subjects, 
            GridPosition? offset = null)
        {
            foreach (var subject in subjects)
            {
                var position = subject.Key.Offset(offset);
                worksheet.Cells[position.Row, position.Col].Value = subject.Value;
            }
        }

        private static void AddHeader(
            this ExcelWorksheet worksheet,
            IEnumerable<GridHeader> headers,
            GridPosition startPosition)
        {
            int curCol = startPosition.Col;
            foreach (var item in headers)
            {
                //header cell
                var curCell = worksheet.Cells[startPosition.Row, curCol];
                curCell.Value = item._title;
                curCell.Style.Font.Bold = true;
                curCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                curCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                curCell.Style.Fill.SetBackground((item._colStyle._bgColor != null ? (Color)item._colStyle._bgColor : DefaultHeaderBgColor));
                curCell.Style.Font.Color.SetColor(item._colStyle._color != null ? (Color)item._colStyle._color : Color.White);
                curCell.Style.WrapText = item._colStyle._wrapText ?? false;

                //column style
                var currentCol = worksheet.Column(curCol);
                if (!string.IsNullOrEmpty(item._colStyle._format))
                    currentCol.Style.Numberformat.Format = item._colStyle._format;
                if (item._colStyle._width != null)
                    currentCol.Width = (double)item._colStyle._width;
                else
                    currentCol.AutoFit(8, 100);

                currentCol.Style.HorizontalAlignment = item._colStyle._horizontalAlignment == HorizontalAlignment.Left
                    ? ExcelHorizontalAlignment.Left
                    : item._colStyle._horizontalAlignment == HorizontalAlignment.Center
                        ? ExcelHorizontalAlignment.Center
                        : ExcelHorizontalAlignment.Right;

                currentCol.Style.WrapText = item._colStyle._wrapText ?? false;

                curCol++;
            }
            return;
        }

        private static void AddBody<TModel>(
            this ExcelWorksheet worksheet,
            IEnumerable<GridHeader>? headers,
            IEnumerable<TModel> data,
            GridPosition startPosition,
            List<(Func<TModel, bool>, GridStyle)>? rowStyle)
        {
            if (headers == null || !headers.Any()) return;
            if (data == null || !data.Any()) return;

            var dataType = data.First()!.GetType();
            var properties = dataType!.GetProperties().ToDictionary(x => x.Name, x => x);
            var headerLength = headers.Count();

            var hasRowStyle = rowStyle != null && rowStyle.Any();
            Parallel.For(0, data.Count(), i =>
            { 
                for (var j = 0; j < headerLength; j++)
                {
                    var currentCell = worksheet.Cells[i + startPosition.Row, j + startPosition.Col];
                    var currentHeader = headers.ElementAt(j);
                    var currentData = data.ElementAt(i);
                    var value = properties[currentHeader._dataIndex]?.GetValue(currentData);

                    currentCell.Value = currentHeader._transformer != null
                        ? currentHeader._transformer!.Invoke(value)
                        : value;

                    if(hasRowStyle)
                    {
                        foreach (var (condition, style) in rowStyle!)
                        {
                            if (condition(currentData))
                            {
                                if (style._bold != null)
                                    currentCell.Style.Font.Bold = style._bold.Value;
                                if (style._color != null)
                                    currentCell.Style.Font.Color.SetColor((Color)style._color);
                                if (style._bgColor != null)
                                {
                                    currentCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    currentCell.Style.Fill.SetBackground((Color)style._bgColor);
                                }
                            }
                        }
                    }

                }
            });

            return;
        }

        private static void AddBodyByLibEngine<TModel>(
            this ExcelWorksheet worksheet,
            IEnumerable<GridHeader>? headers,
            IEnumerable<TModel> data,
            GridPosition startPosition,
            MemberInfo[] members)
        {
            if (data == null) return;

            var isHasHeaderConfig = headers != null && headers.Any();

            if (isHasHeaderConfig)
            {
                var headerIndexs = headers!.Select(i => i._dataIndex.ToLower()).ToArray();
                members = members.OrderBy(member => Array.IndexOf(headerIndexs, member.Name.ToLower())).ToArray();
            }

            worksheet.Cells[startPosition.Row, startPosition.Col].LoadFromCollection(
                data,
                PrintHeaders: !isHasHeaderConfig,
                null,
                BindingFlags.Instance | BindingFlags.Public,
                members);

			if(!isHasHeaderConfig)
                for (int col = startPosition.Col; col < startPosition.Col + members.Length; col++)
                    worksheet.Column(col).AutoFit();

        }
    }
}
