using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using LibraryAPI.Application.Repositories.Interfaces;
using LibraryAPI.Domain.DataTransferObjects.BorrowedBooks;
using LibraryAPI.Domain.Entities;
using LibraryAPI.Extensions;

namespace ExcelExportService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var borrowedBookService = scope.ServiceProvider.GetService<IBorrowedBookRepository>();
            var borrowedBooks = await borrowedBookService!.GetBorrowedBooks() as List<BorrowedBook>;

            borrowedBooks!.Sort((a, b) => a.BorrowedDate.CompareTo(b.BorrowedDate));

            var borrowedBookDetails = borrowedBooks.Select(bb => bb.ToDetailsDto());
            
            ExportBorrowedBooksToExcel(borrowedBookDetails, @"BorrowedBooks.xlsx");
            

            if (_logger.IsEnabled(LogLevel.Information))
            {
                Console.WriteLine(borrowedBooks.Count);
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        }
    }

    private static void ExportBorrowedBooksToExcel(IEnumerable<BorrowedBookDetailsDto> borrowedBooks, string filePath)
    {
        using (var spreadsheetDocument = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
        {
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            workbookPart.Workbook = new Workbook();

            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            worksheetPart.Worksheet = new Worksheet(new SheetData());

            var sheets = spreadsheetDocument.WorkbookPart!.Workbook.AppendChild(new Sheets());

            var sheet = new Sheet()
            {
                Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                SheetId = 1,
                Name = "BorrowedBooks"
            };
            sheets.Append(sheet);

            var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            var headerRow = new Row();
            headerRow.Append(
                new Cell { CellValue = new CellValue("Book Title"), DataType = CellValues.String },
                new Cell { CellValue = new CellValue("Borrower Name"), DataType = CellValues.String },
                new Cell { CellValue = new CellValue("Borrowed Date"), DataType = CellValues.String },
                new Cell { CellValue = new CellValue("Due Date"), DataType = CellValues.String },
                new Cell { CellValue = new CellValue("Has Returned The Book"), DataType = CellValues.String },
                new Cell { CellValue = new CellValue("Penalty Price"), DataType = CellValues.String}
            );
            sheetData!.AppendChild(headerRow);

            foreach (var book in borrowedBooks)
            {
                var row = new Row();
                row.Append(
                    new Cell { CellValue = new CellValue(book.BookName!), DataType = CellValues.String },
                    new Cell
                    {
                        CellValue = new CellValue(book.BorrowerName!),
                        DataType = CellValues.String
                    },
                    new Cell
                    {
                        CellValue = new CellValue(book.BorrowingDate.ToString("yyyy-MM-dd")),
                        DataType = CellValues.String
                    },
                    new Cell
                    {
                        CellValue = new CellValue(book.DueDate.ToString() ?? " "), DataType = CellValues.String
                    },
                    new Cell { CellValue = new CellValue(book.IsReturned.ToString()!), DataType = CellValues.String },
                    new Cell { CellValue = new CellValue(book.PenaltyPrice), DataType = CellValues.String }
                );
                sheetData.AppendChild(row);
            }

            workbookPart.Workbook.Save();
        }
    }
}