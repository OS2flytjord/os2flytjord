using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;

namespace Niras.Jordflytning.Library.Helpers
{
	/// <summary>
	///   Class to help to build an Excel workbook
	/// </summary>
	/// <remarks>
	///   The following code was written by Mike Wendelius and can be found on Code Project at:
	///   http://www.codeproject.com/Articles/371203/Creating-basic-Excel-workbook-with-Open-XML
	/// </remarks>
	public static class ExcelHelper
	{

		private const string OldNyLinie = @"<br/>";
		private const string NyLinie = "";
		private static readonly Regex LeadingInteger = new Regex(@"^(-?\d+)");

		public static byte[] CreateExcelFile(string model, string data, string title)
		{
			byte[] file;
			using (var stream = new MemoryStream())
			{
				/* Create the worksheet. */

				var spreadsheet = CreateWorkbook(stream);
				AddBasicStyles(spreadsheet);
				AddAdditionalStyles(spreadsheet);
				var workSheetData = AddWorksheet(spreadsheet, title);
				var worksheet = spreadsheet.WorkbookPart.WorksheetParts.First().Worksheet;


				/* Get the information needed for the worksheet */

				var modelObject = JsonConvert.DeserializeObject<dynamic>(model);
				//var dataObject = JsonConvert.DeserializeObject<dynamic>(data);

                var jsJson = new JavaScriptSerializer();
                jsJson.MaxJsonLength = int.MaxValue;
                var dataObject = jsJson.Deserialize<dynamic>(data);





				/* Add the column titles to the worksheet. */

				// For each column...
				for (var mdx = 0; mdx < modelObject.Count; mdx++)
				{
					// If the column has a title, use it.  Otherwise, use the field name.
					var colTitle = "-";

					if (modelObject[mdx].title == null || modelObject[mdx].title == "&nbsp;")
					{
						if (modelObject[mdx].field != null)
							colTitle = modelObject[mdx].field.ToString();
					}
					else
					{
						if (modelObject[mdx].title != null)
							colTitle = modelObject[mdx].title.ToString();
					}
					colTitle = colTitle.Replace(OldNyLinie, NyLinie);

					SetColumnHeadingValue(spreadsheet, worksheet, Convert.ToUInt32(mdx + 1), colTitle, false, false);

					// Is there are column width defined?
					int colWidth;
					if (modelObject[mdx].width != null)
						colWidth = int.Parse(LeadingInteger.Match(modelObject[mdx].width.ToString()).Value)/4;
					else
						colWidth = 25;

					SetColumnWidth(worksheet, mdx + 1, colWidth);
				}

				/* Add the data to the worksheet. */

                //// For each row of data...
                //for (var idx = 0; idx < dataObject.Length; idx++)
                //{
                //    // For each column...
                //    for (var mdx = 0; mdx < modelObject.Count; mdx++)
                //    {
                //        if (modelObject[mdx].field == null)
                //            continue;

                //        string fieldTitle = modelObject[mdx].field.ToString();
                //        var tValue = dataObject[idx][fieldTitle] ?? "";
                //        string fieldValue = tValue.ToString();
                //        fieldValue = fieldValue.Replace(OldNyLinie, NyLinie);

                //        // Set the field value in the spreadsheet for the current row and column.
                //        SetCellValue(spreadsheet, worksheet, Convert.ToUInt32(mdx + 1), Convert.ToUInt32(idx + 2), fieldValue, false, false);
                //    }
                //}

                // For each row of data...
                for (var idx = 0; idx < dataObject.Length; idx++)
                {
                    var rowcount = Convert.ToUInt32(idx + 2);
                    var row = new Row { RowIndex = rowcount };

                    // For each column...
                    for (var mdx = 0; mdx < modelObject.Count; mdx++)
                    {
                        if (modelObject[mdx].field == null)
                            continue;

                        string fieldTitle = modelObject[mdx].field.ToString();
                        var tValue = dataObject[idx][fieldTitle] ?? "";
                        string fieldValue = tValue.ToString();
                        fieldValue = fieldValue.Replace(OldNyLinie, NyLinie);

                        // Set the field value in the spreadsheet for the current row and column.
                        //SetCellValue(spreadsheet, worksheet, Convert.ToUInt32(mdx + 1), Convert.ToUInt32(idx + 2), fieldValue, false, false);

                        row.Append(CreateTextCell(ColumnNameFromIndex(Convert.ToUInt32(mdx + 1)), rowcount, fieldValue));
                       
                    }

                    workSheetData.AppendChild(row);
                }




				worksheet.Save();
				spreadsheet.Close();
				file = stream.ToArray();
			}
			return file;
		}


        // Create a text cell
        private static Cell CreateTextCell(string header, UInt32 index, string text)
        {
            var cell = new Cell
            {
                DataType = CellValues.String,
                CellReference = header + index,
                CellValue = new CellValue(text)
            };

            return cell;
        }
            

            
		#region *** Private methods ***

		/// <summary>
		///   Adds a new worksheet to the workbook
		/// </summary>
		/// <param name="spreadsheet">Spreadsheet to use</param>
		/// <param name="name">Name of the worksheet</param>
		/// <returns>True if succesful</returns>
        private static SheetData AddWorksheet(SpreadsheetDocument spreadsheet, string name)
		{
			var sheets = spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>();

			// Add the worksheetpart
			var worksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
		    var sheetData = new SheetData();
            worksheetPart.Worksheet = new Worksheet(sheetData);
			worksheetPart.Worksheet.Save();

			// Add the sheet and make relation to workbook
			var sheet = new Sheet
				{
					Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart),
					SheetId = (uint)(spreadsheet.WorkbookPart.Workbook.Sheets.Count() + 1),
					Name = name
				};
			sheets.Append(sheet);
			spreadsheet.WorkbookPart.Workbook.Save();

            return sheetData;
		}

		/// <summary>
		///   Adds the basic styles to the workbook
		/// </summary>
		/// <param name="spreadsheet">Spreadsheet to use</param>
		/// <returns>True if succesful</returns>
		private static bool AddBasicStyles(SpreadsheetDocument spreadsheet)
		{
			var stylesheet = spreadsheet.WorkbookPart.WorkbookStylesPart.Stylesheet;

			// Numbering formats (x:numFmts)
			stylesheet.InsertAt(new NumberingFormats(), 0);
			// Currency
			stylesheet.GetFirstChild<NumberingFormats>().InsertAt<NumberingFormat>(
				new NumberingFormat
				{
					NumberFormatId = 164,
					FormatCode = "#,##0.00"
											 + "\\ \"" + CultureInfo.CurrentUICulture.NumberFormat.CurrencySymbol + "\""
				}, 0);

			// Fonts (x:fonts)
			stylesheet.InsertAt(new Fonts(), 1);
			stylesheet.GetFirstChild<Fonts>().InsertAt<Font>(
				new Font
				{
					FontSize = new FontSize
					{
						Val = 11
					},
					FontName = new FontName
					{
						Val = "Calibri"
					}
				}, 0);

			// Fills (x:fills)
			stylesheet.InsertAt(new Fills(), 2);
			stylesheet.GetFirstChild<Fills>().InsertAt<Fill>(
				new Fill
				{
					PatternFill = new PatternFill
					{
						PatternType = new EnumValue<PatternValues>
						{
							Value = PatternValues.None
						}
					}
				}, 0);
			stylesheet.GetFirstChild<Fills>().InsertAt<Fill>(
				new Fill
				{
					PatternFill = new PatternFill
					{
						PatternType = new EnumValue<PatternValues>
						{
							Value = PatternValues.Gray125
						}
					}
				}, 1);

			// Borders (x:borders)
			stylesheet.InsertAt(new Borders(), 3);
			stylesheet.GetFirstChild<Borders>().InsertAt<Border>(
				new Border
				{
					LeftBorder = new LeftBorder(),
					RightBorder = new RightBorder(),
					TopBorder = new TopBorder(),
					BottomBorder = new BottomBorder(),
					DiagonalBorder = new DiagonalBorder()
				}, 0);

			// Cell style formats (x:CellStyleXfs)
			stylesheet.InsertAt(new CellStyleFormats(), 4);
			stylesheet.GetFirstChild<CellStyleFormats>().InsertAt<CellFormat>(
				new CellFormat
				{
					NumberFormatId = 0,
					FontId = 0,
					FillId = 0,
					BorderId = 0
				}, 0);

			// Cell formats (x:CellXfs)
			stylesheet.InsertAt(new CellFormats(), 5);
			// General text
			stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
				new CellFormat
				{
					FormatId = 0,
					NumberFormatId = 0
				}, 0);
			// Date
			stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
				new CellFormat
				{
					ApplyNumberFormat = true,
					FormatId = 0,
					NumberFormatId = 22,
					FontId = 0,
					FillId = 0,
					BorderId = 0
				},
				1);
			// Currency
			stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
				new CellFormat
				{
					ApplyNumberFormat = true,
					FormatId = 0,
					NumberFormatId = 164,
					FontId = 0,
					FillId = 0,
					BorderId = 0
				},
				2);
			// Percentage
			stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
				new CellFormat
				{
					ApplyNumberFormat = true,
					FormatId = 0,
					NumberFormatId = 10,
					FontId = 0,
					FillId = 0,
					BorderId = 0
				},
				3);

			stylesheet.Save();

			return true;
		}

		/// <summary>
		///   Add a single string to shared strings table.
		///   Shared string table is created if it doesn't exist.
		/// </summary>
		/// <param name="spreadsheet">Spreadsheet to use</param>
		/// <param name="stringItem">string to add</param>
		/// <param name="save">Save the shared string table</param>
		/// <returns></returns>
		private static bool AddSharedString(SpreadsheetDocument spreadsheet, string stringItem, bool save = true)
		{
			var sharedStringTable = spreadsheet.WorkbookPart.SharedStringTablePart.SharedStringTable;

			if (0 == sharedStringTable.Where(item => item.InnerText == stringItem).Count())
			{
				sharedStringTable.AppendChild(
					new SharedStringItem(
						new Text(stringItem)));

				// Save the changes
				if (save)
				{
					sharedStringTable.Save();
				}
			}

			return true;
		}

		/// <summary>
		///   Returns the index of a shared string.
		/// </summary>
		/// <param name="spreadsheet">Spreadsheet to use</param>
		/// <param name="stringItem">String to search for</param>
		/// <returns>Index of a shared string. -1 if not found</returns>
		private static int IndexOfSharedString(SpreadsheetDocument spreadsheet, string stringItem)
		{
			var sharedStringTable = spreadsheet.WorkbookPart.SharedStringTablePart.SharedStringTable;
			var found = false;
			var index = 0;

			foreach (var sharedString in sharedStringTable.Elements<SharedStringItem>())
			{
				if (sharedString.InnerText == stringItem)
				{
					found = true;
					break;
					
				}
				index++;
			}

			return found ? index : -1;
		}

		/// <summary>
		///   Converts a column number to column name (i.e. A, B, C..., AA, AB...)
		/// </summary>
		/// <param name="columnIndex">Index of the column</param>
		/// <returns>Column name</returns>
		private static string ColumnNameFromIndex(uint columnIndex)
		{
			var columnName = "";

			while (columnIndex > 0)
			{
				uint remainder = (columnIndex - 1) % 26;
				columnName = Convert.ToChar(65 + remainder).ToString() + columnName;
				columnIndex = ((columnIndex - remainder) / 26);
			}

			return columnName;
		}

		/// <summary>
		///   Sets a column heading to a cell
		/// </summary>
		/// <param name="spreadsheet">Spreadsheet to use</param>
		/// <param name="worksheet">Worksheet to use</param>
		/// <param name="columnIndex">Index of the column</param>
		/// <param name="stringValue">String value to set</param>
		/// <param name="useSharedString">Use shared strings? If true and the string isn't found in shared strings, it will be added</param>
		/// <param name="save">Save the worksheet</param>
		/// <returns>True if succesful</returns>
		private static bool SetColumnHeadingValue(SpreadsheetDocument spreadsheet, Worksheet worksheet, uint columnIndex, string stringValue, bool useSharedString,
																							bool save = true)
		{
			var columnValue = stringValue;
			CellValues cellValueType;

			// Add the shared string if necessary
			if (useSharedString)
			{
				if (IndexOfSharedString(spreadsheet, stringValue) == -1)
				{
					AddSharedString(spreadsheet, stringValue, true);
				}
				columnValue = IndexOfSharedString(spreadsheet, stringValue).ToString();
				cellValueType = CellValues.SharedString;
			}
			else
			{
				cellValueType = CellValues.String;
			}

			return SetCellValue(spreadsheet, worksheet, columnIndex, 1, cellValueType, columnValue, 4, save);
		}

		/// <summary>
		///   Sets a string value to a cell
		/// </summary>
		/// <param name="spreadsheet">Spreadsheet to use</param>
		/// <param name="worksheet">Worksheet to use</param>
		/// <param name="columnIndex">Index of the column</param>
		/// <param name="rowIndex">Index of the row</param>
		/// <param name="stringValue">String value to set</param>
		/// <param name="useSharedString">Use shared strings? If true and the string isn't found in shared strings, it will be added</param>
		/// <param name="save">Save the worksheet</param>
		/// <returns>True if succesful</returns>
		private static bool SetCellValue(SpreadsheetDocument spreadsheet, Worksheet worksheet, uint columnIndex, uint rowIndex, string stringValue,
																		 bool useSharedString, bool save = true)
		{
			var columnValue = stringValue;
			CellValues cellValueType;

			// Add the shared string if necessary
			if (useSharedString)
			{
				if (IndexOfSharedString(spreadsheet, stringValue) == -1)
				{
					AddSharedString(spreadsheet, stringValue, true);
				}
				columnValue = IndexOfSharedString(spreadsheet, stringValue).ToString();
				cellValueType = CellValues.SharedString;
			}
			else
			{
				cellValueType = CellValues.String;
			}

			return SetCellValue(spreadsheet, worksheet, columnIndex, rowIndex, cellValueType, columnValue, null, save);
		}

		/// <summary>
		///   Sets the column width
		/// </summary>
		/// <param name="worksheet">Worksheet to use</param>
		/// <param name="columnIndex">Index of the column</param>
		/// <param name="width">Width to set</param>
		/// <returns>True if succesful</returns>
		private static bool SetColumnWidth(Worksheet worksheet, int columnIndex, int width)
		{
			Columns columns;
			Column column;

			// Get the column collection exists
			columns = worksheet.Elements<Columns>().FirstOrDefault();
			if (columns == null)
			{
				return false;
			}
			// Get the column
			column = columns.Elements<Column>().Where(item => item.Min == columnIndex).FirstOrDefault();
			if (columns == null)
			{
				return false;
			}
			column.Width = width;
			column.CustomWidth = true;

			worksheet.Save();

			return true;
		}

		/// <summary>
		///   Sets a cell value. The row and the cell are created if they do not exist. If the cell exists, the contents of the cell is overwritten
		/// </summary>
		/// <param name="spreadsheet">Spreadsheet to use</param>
		/// <param name="worksheet">Worksheet to use</param>
		/// <param name="columnIndex">Index of the column</param>
		/// <param name="rowIndex">Index of the row</param>
		/// <param name="valueType">Type of the value</param>
		/// <param name="value">The actual value</param>
		/// <param name="styleIndex">Index of the style to use. Null if no style is to be defined</param>
		/// <param name="save">Save the worksheet?</param>
		/// <returns>True if succesful</returns>
		private static bool SetCellValue(SpreadsheetDocument spreadsheet, Worksheet worksheet, uint columnIndex, uint rowIndex, CellValues valueType, string value,
																		 uint? styleIndex, bool save = true)
		{
			var sheetData = worksheet.GetFirstChild<SheetData>();
			Row row;
			Row previousRow = null;
			Cell cell;
			Cell previousCell = null;
			Column previousColumn = null;
			var cellAddress = ColumnNameFromIndex(columnIndex) + rowIndex;

			// Check if the row exists, create if necessary
			if (sheetData.Elements<Row>().Count(item => item.RowIndex == rowIndex) != 0)
			{
				row = sheetData.Elements<Row>().First(item => item.RowIndex == rowIndex);
			}
			else
			{
				row = new Row { RowIndex = rowIndex };
				//sheetData.Append(row);
				for (var counter = rowIndex - 1; counter > 0; counter--)
				{
					previousRow = sheetData.Elements<Row>().FirstOrDefault(item => item.RowIndex == counter);
					if (previousRow != null)
					{
						break;
					}
				}
				sheetData.InsertAfter(row, previousRow);
			}

			// Check if the cell exists, create if necessary
			if (row.Elements<Cell>().Any(item => item.CellReference.Value == cellAddress))
			{
				cell = row.Elements<Cell>().First(item => item.CellReference.Value == cellAddress);
			}
			else
			{
				// Find the previous existing cell in the row
				for (var counter = columnIndex - 1; counter > 0; counter--)
				{
					previousCell = row.Elements<Cell>().FirstOrDefault(item => item.CellReference.Value == ColumnNameFromIndex(counter) + rowIndex);
					if (previousCell != null)
					{
						break;
					}
				}
				cell = new Cell { CellReference = cellAddress };
				row.InsertAfter(cell, previousCell);
			}

			// Check if the column collection exists
			var columns = worksheet.Elements<Columns>().FirstOrDefault();
			if (columns == null)
			{
				columns = worksheet.InsertAt(new Columns(), 0);
			}
			// Check if the column exists
			if (columns.Elements<Column>().All(item => item.Min != columnIndex))
			{
				// Find the previous existing column in the columns
				for (var counter = columnIndex - 1; counter > 0; counter--)
				{
					previousColumn = columns.Elements<Column>().FirstOrDefault(item => item.Min == counter);
					if (previousColumn != null)
					{
						break;
					}
				}
				columns.InsertAfter(
					new Column
					{
						Min = columnIndex,
						Max = columnIndex,
						CustomWidth = true,
						Width = 9
					}, previousColumn);
			}

			// Add the value
			cell.CellValue = new CellValue(value);
			if (styleIndex != null)
			{
				cell.StyleIndex = styleIndex;
			}
			if (valueType != CellValues.Date)
			{
				cell.DataType = new EnumValue<CellValues>(valueType);
			}

			if (save)
			{
				worksheet.Save();
			}

			return true;
		}

		/// <summary>
		///   Creates the workbook in memory.
		/// </summary>
		/// <returns>Spreadsheet created</returns>
		private static SpreadsheetDocument CreateWorkbook(Stream stream)
		{
			SpreadsheetDocument spreadSheet = null;
			SharedStringTablePart sharedStringTablePart;
			WorkbookStylesPart workbookStylesPart;

			// Create the Excel workbook
			spreadSheet = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, false);

			// Create the parts and the corresponding objects

			// Workbook
			spreadSheet.AddWorkbookPart();
			spreadSheet.WorkbookPart.Workbook = new Workbook();
			spreadSheet.WorkbookPart.Workbook.Save();

			// Shared string table
			sharedStringTablePart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();
			sharedStringTablePart.SharedStringTable = new SharedStringTable();
			sharedStringTablePart.SharedStringTable.Save();

			// Sheets collection
			spreadSheet.WorkbookPart.Workbook.Sheets = new Sheets();
			spreadSheet.WorkbookPart.Workbook.Save();

			// Stylesheet
			workbookStylesPart = spreadSheet.WorkbookPart.AddNewPart<WorkbookStylesPart>();
			workbookStylesPart.Stylesheet = new Stylesheet();
			workbookStylesPart.Stylesheet.Save();

			return spreadSheet;
		}

		/// <summary>
		///   Adds additional styles to the workbook
		/// </summary>
		/// <param name="spreadsheet">Spreadsheet to use</param>
		/// <returns>True if succesful</returns>
		private static bool AddAdditionalStyles(SpreadsheetDocument spreadsheet)
		{
			var stylesheet = spreadsheet.WorkbookPart.WorkbookStylesPart.Stylesheet;

			// Additional Font for Column Heder.
			stylesheet.GetFirstChild<Fonts>().InsertAt<Font>(
				new Font
				{
					FontSize = new FontSize
					{
						Val = 12
					},
					FontName = new FontName
					{
						Val = "Calibri"
					},
					Bold = new Bold
					{
						Val = true
					}
				}, 1);

			// Additional Fill for Column Header.
			stylesheet.GetFirstChild<Fills>().InsertAt<Fill>(
				new Fill
				{
					PatternFill = new PatternFill
					{
						PatternType = new EnumValue<PatternValues>
						{
							Value = PatternValues.Solid
						},
						BackgroundColor = new BackgroundColor
						{
							Indexed = 64U
						},
						ForegroundColor = new ForegroundColor
						{
							Rgb = "F2F2F2"
						}
					}
				}, 2);

			// Column Header
			stylesheet.GetFirstChild<CellFormats>().InsertAt<CellFormat>(
				new CellFormat
				{
					FormatId = 0,
					NumberFormatId = 0,
					FontId = 1,
					FillId = 2,
					ApplyFill = true,
					ApplyFont = true
				}, 4);

			stylesheet.Save();

			return true;
		}

	}

		#endregion *** Private methods ***
}