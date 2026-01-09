using LinqKit;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Reflection;

namespace SV.Utility.ExportExcel
{
	public static class ExcelExtensions
	{
		public static async Task<byte[]> ExportAsync<T>(this ExportExcelData<T> excelData, bool isBulkInsert = false)
		{
			try
			{
				ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
				using var excelPackage = new ExcelPackage();
				using var worksheet = excelPackage.Workbook.Worksheets.Add(excelData.Title);

				if (excelData.Subject.Any())
				{
					await worksheet.AddSubjectAsync(excelData.Subject);
				}

				// Add data for body
				if (isBulkInsert)
				{
					worksheet.BatchInsertBody(excelData.Headers, excelData.Data, excelData.DataPosition, excelData.Members);
				}
				else
				{
					await worksheet.AddBodyAsync(excelData.Headers, excelData.Data, excelData.DataPosition);
				}

				await worksheet.AddHeaderAsync(excelData.Headers, excelData.HeaderPosition);

				return await excelPackage.GetAsByteArrayAsync();
			}
			catch (Exception ex)
			{
				return null;
			}
		}

        public static async Task<IEnumerable<T>> ImportExcelToObjects<T>(string filePath) where T : class, new()
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"File not found at path: {filePath}");

            // Set EPPlus license context
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            return await Task.Run(() => InitialMapping<T>(filePath));
        }

        private static List<T> InitialMapping<T>(string filePath) where T : class, new()
        {
            var result = new List<T>();

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new InvalidOperationException("The Excel file contains no worksheets.");

                int rows = worksheet.Dimension.Rows;
                int columns = worksheet.Dimension.Columns;

                // Read header row to map property names
                var headers = new List<string>();
                for (int col = 1; col <= columns; col++)
                {
                    var header = worksheet.Cells[1, col].Text.Trim();
                    headers.Add(header.Replace(" ", "").ToLower()); // Normalize headers
                }

                // Map rows to objects
                for (int row = 2; row <= rows; row++)
                {
                    var obj = new T();
                    foreach (var prop in typeof(T).GetProperties())
                    {
                        // Check for custom mapping attribute
                        var columnName = prop.GetCustomAttribute<ExcelColumnAttribute>()?.ColumnName
                            ?? prop.Name;

                        int colIndex = headers.FindIndex(h => h.Equals(columnName.Replace(" ", "").ToLower(), StringComparison.OrdinalIgnoreCase)) + 1;

                        if (colIndex > 0)
                        {
                            var cellValue = worksheet.Cells[row, colIndex].Text.Trim();
                            try
                            {
                                if (!string.IsNullOrEmpty(cellValue))
                                {
                                    var convertedValue = Convert.ChangeType(cellValue.Replace(" ","-"), prop.PropertyType);
                                    prop.SetValue(obj, convertedValue);
                                }
                            }
                            catch
                            {
                                // Handle type conversion errors
                                prop.SetValue(obj, GetDefaultValue(prop.PropertyType));
                            }
                        }
                    }
                    result.Add(obj);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the default value for a given type.
        /// </summary>
        private static object GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

        private static Task AddSubjectAsync(this ExcelWorksheet worksheet, IReadOnlyDictionary<GridPosition, string> subjects)
        {
            subjects.ForEach(subject => worksheet.Cells.SetCellValue(subject.Key.Row, subject.Key.Col, subject.Value));
            return Task.CompletedTask;
        }

        private static Task AddHeaderAsync(
            this ExcelWorksheet worksheet,
            IEnumerable<GridHeader> headers,
            GridPosition startPosition)
        {
            int curCol = startPosition.Col;
            foreach (var item in headers)
            {
                //column
                var currentCol = worksheet.Column(curCol);
                if (!string.IsNullOrEmpty(item.Style.Format))
                    currentCol.Style.Numberformat.Format = item.Style.Format;
                if (item.Style.Width != null)
                    currentCol.Width = (double)item.Style.Width;
                else
                    currentCol.AutoFit(20, 100);
                if (item.Style.Format != null)
                    currentCol.Style.Numberformat.Format = item.Style.Format;

				currentCol.Style.HorizontalAlignment = item.Style.HorizontalAlignment == HorizontalAlignment.Left
					? ExcelHorizontalAlignment.Left
					: item.Style.HorizontalAlignment == HorizontalAlignment.Center
						? ExcelHorizontalAlignment.Center
						: ExcelHorizontalAlignment.Right;

                //cell
                var curCell = worksheet.Cells[startPosition.Row, curCol];
                curCell.Value = item.Title;
                curCell.Style.Font.Bold = true;
                curCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                curCol++;
            }
            return Task.CompletedTask;
        }

        private static Task AddBodyAsync<T>(
            this ExcelWorksheet worksheet,
            IEnumerable<GridHeader> headers,
            IEnumerable<T> data,
            GridPosition startPosition)
        {
            Parallel.For(0, data.Count(), i =>
            {
                var dataType = data.ElementAt(i)?.GetType();
                for (var j = 0; j < headers.Count(); j++)
                {
                    var currentCell = worksheet.Cells[i + startPosition.Row, j + startPosition.Col];
                    var currentHeader = headers.ElementAt(j);
                    var value = dataType?
                        .GetProperty(currentHeader.DataIndex)?
                        .GetValue(data.ElementAt(i));
                    currentCell.Value = currentHeader.Transformer != null
                        ? currentHeader.Transformer!.Invoke(value)
                        : value;
                }
            });

			return Task.CompletedTask;
		}

		/// <summary>
		/// Batch insert data
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="headers"></param>
		/// <param name="worksheet"></param>
		/// <param name="data"></param>
		/// <param name="startPosition"></param>
		/// <returns></returns>
		private static void BatchInsertBody<T>(
			this ExcelWorksheet worksheet,
			IEnumerable<GridHeader> headers,
			IEnumerable<T> data,
			GridPosition startPosition,
			MemberInfo[] members)
		{
			var headerIndexs = headers.Select(i => i.DataIndex.ToLower())
				.ToArray();
			var memberWithHeader = members.OrderBy(member => Array.IndexOf(headerIndexs, member.Name.ToLower()))
				.ToArray();

			worksheet.Cells[startPosition.Row, startPosition.Col].LoadFromCollection(
				data,
				PrintHeaders: false,
				null,
				BindingFlags.Instance | BindingFlags.Public,
				memberWithHeader);
		}
	}
}
