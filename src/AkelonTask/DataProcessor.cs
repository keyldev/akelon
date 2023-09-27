using AkelonTask.Models;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AkelonTask
{
    internal class DataProcessor
    {
        private readonly string _filePath = string.Empty;

        public DataProcessor(string filePath)
        {
            this._filePath = File.Exists(filePath) ? filePath: throw new FileNotFoundException("Файл с данными не найден.");
        }

        //2. По наименованию товара выводить информацию о клиентах, заказавших этот товар, с указанием информации по количеству товара, цене и дате заказа.
        public void FindClientsInfoByProductName(string? productName)
        {
            var product = FindProductByName(productName);
            var clientsRequests = FindClientsByProductCode(product.Code);
            var clients = GetClientsInfo(clientsRequests);

            Console.WriteLine($"Наименование товара - {productName}, цена - {product.Price}rub. за {product.UnitOfMeasurement}\n");
            clients.ForEach(client => Console.WriteLine($"ФИО:{client.ClientName}, организация: {client.OrganizationName}, адрес: {client.Address}, количество заказанного товара: {clientsRequests.FirstOrDefault(cr => cr.ClientCode == client.Code).Amount}"));
        }

        private Product FindProductByName(string? productName)
        {
            var product = new Product();
            using (var workbook = new XLWorkbook(_filePath))
            {
                IXLWorksheet productsWorksheet = workbook.Worksheet(1);
                var rows = productsWorksheet.RowsUsed();

                int productCodeIndex = GetColumnIndex(productsWorksheet, "Код товара");
                int productNameIndex = GetColumnIndex(productsWorksheet, "Наименование");
                int unitOfMeasureIndex = GetColumnIndex(productsWorksheet, "Ед. измерения");
                int priceIndex = GetColumnIndex(productsWorksheet, "Цена товара за единицу");

                foreach (var row in rows)
                {
                    if (row.Cell(productNameIndex).Value.ToString() == productName)
                    {
                        string productCode = row.Cell(productCodeIndex).Value.ToString();
                        string unitOfMeasure = row.Cell(unitOfMeasureIndex).Value.ToString();
                        string price = row.Cell(priceIndex).Value.ToString();
                        string name = productName;

                        product.Code = Convert.ToInt32(productCode);
                        product.UnitOfMeasurement = unitOfMeasure;
                        product.Price = Convert.ToDouble(price);
                        product.Name = productName;


                        #region DEBUG_INFO
                        Debug.WriteLine($"Код товара: {product.Code}.Наименование: {productName}. Единица измерения: {unitOfMeasure}. Цена за единицу товара: {price}");
                        Debug.WriteLine($"");
                        #endregion

                    }
                }
                return product;

            }
        }
        private List<Request> FindClientsByProductCode(int? productCode)
        {
            var clientsInfo = new List<Request>();
            using (var workbook = new XLWorkbook(_filePath))
            {
                IXLWorksheet requestsWorksheet = workbook.Worksheet(3);
                var requestsRows = requestsWorksheet.RowsUsed();

                var requestProductCodeIndex = GetColumnIndex(requestsWorksheet, "Код товара");
                var requestClientCodeIndex = GetColumnIndex(requestsWorksheet, "Код клиента");
                var requestCountIndex = GetColumnIndex(requestsWorksheet, "Требуемое количество");
                var requestDateIndex = GetColumnIndex(requestsWorksheet, "Дата размещения");

                foreach (var row in requestsRows)
                {
                    if (row.Cell(requestProductCodeIndex).Value.ToString() == productCode.ToString())
                    {
                        string productCount = row.Cell(requestCountIndex).Value.ToString();
                        string requestDate = row.Cell(requestDateIndex).Value.ToString();
                        string clientId = row.Cell(requestClientCodeIndex).Value.ToString();

                        clientsInfo.Add(new Request
                        {
                            ClientCode = Convert.ToInt32(clientId),
                            Amount = Convert.ToInt32(productCount),
                            Date = Convert.ToDateTime(requestDate)
                        });

                        #region DEBUG_INFO
                        Debug.WriteLine($"Количество: {productCount}. Дата заказа: {requestDate}. Id клиента {clientId}");
                        Debug.WriteLine($"");
                        #endregion

                    }
                }
                return clientsInfo;
            }
        }

        private List<Client> GetClientsInfo(List<Request> clientsRequests)
        {
            var clients = new List<Client>();

            using (var workbook = new XLWorkbook(_filePath))
            {
                IXLWorksheet clientWorksheet = workbook.Worksheet(2);
                var clientRows = clientWorksheet.RowsUsed();

                var clientCodeIndex = GetColumnIndex(clientWorksheet, "Код клиента");
                var clientOrgNameIndex = GetColumnIndex(clientWorksheet, "Наименование организации");
                var clientAddressIndex = GetColumnIndex(clientWorksheet, "Адрес");
                var clientContactNameIndex = GetColumnIndex(clientWorksheet, "Контактное лицо (ФИО)");
                Debug.WriteLine($"Поиск информации о клиентах...");
                foreach (var row in clientRows)
                {
                    // cr - clientRequest
                    if (clientsRequests.Any(cr => cr.ClientCode.ToString() == row.Cell(clientCodeIndex).Value.ToString()))
                    {
                        string orgName = row.Cell(clientOrgNameIndex).Value.ToString();
                        string address = row.Cell(clientAddressIndex).Value.ToString();
                        string contactName = row.Cell(clientContactNameIndex).Value.ToString();

                        clients.Add(new Client
                        {
                            Code = Convert.ToInt32(row.Cell(clientCodeIndex).Value.ToString()),
                            ClientName = contactName,
                            Address = address,
                            OrganizationName = orgName,
                        });

                        Debug.WriteLine($"Имя клиента: {contactName}Название организации:{orgName}\n.Адрес: {address}");
                    }

                }

                return clients;
            }
        }
        public void SetContactName(string? orgName, string? newContactName)
        {

            using (var workbook = new XLWorkbook(_filePath))
            {
                IXLWorksheet clientWorksheet = workbook.Worksheet(2);
                var clientRows = clientWorksheet.RowsUsed();

                var clientCodeIndex = GetColumnIndex(clientWorksheet, "Код клиента");
                var clientOrgNameIndex = GetColumnIndex(clientWorksheet, "Наименование организации");
                var clientAddressIndex = GetColumnIndex(clientWorksheet, "Адрес");
                var clientContactNameIndex = GetColumnIndex(clientWorksheet, "Контактное лицо (ФИО)");
                Debug.WriteLine($"Поиск информации о клиентах...");
                foreach (var row in clientRows)
                {
                    if (row.Cell(clientOrgNameIndex).Value.ToString().Equals(orgName))
                    {
                        row.Cell(clientContactNameIndex).Value = newContactName;

                        Console.WriteLine($"ФИО нового контактного лица для {orgName} успешно заменено на {newContactName}");
                    }
                }
                workbook.Save();
            }
        }
        public void FindGoldenClient(int year, int month)
        {
            var requests = new List<Request>();
            using (var workbook = new XLWorkbook(_filePath))
            {
                IXLWorksheet requestsWorksheet = workbook.Worksheet(3);
                var requestsRows = requestsWorksheet.RowsUsed();

                var requestProductCodeIndex = GetColumnIndex(requestsWorksheet, "Код товара");
                var requestClientCodeIndex = GetColumnIndex(requestsWorksheet, "Код клиента");
                var requestCountIndex = GetColumnIndex(requestsWorksheet, "Требуемое количество");
                var requestDateIndex = GetColumnIndex(requestsWorksheet, "Дата размещения");

                foreach (var row in requestsRows.Skip(1))
                {
                    // рабочий код
                    DateTime date = Convert.ToDateTime(row.Cell(requestDateIndex).Value.ToString());
                    if (date.Year == year && date.Month == month)
                    {
                        string amount = row.Cell(requestCountIndex).Value.ToString();
                        int clientId = Convert.ToInt32(row.Cell(requestClientCodeIndex).Value.ToString());


                        requests.Add(new Request
                        {
                            Amount = Convert.ToInt32(amount),
                            ClientCode = clientId,

                        });

                        Debug.WriteLine($"Amount: {amount}");
                    }
                }
            }
            var clients = GetClientsInfo(requests);
            var client = clients.FirstOrDefault(c => c.Code == requests.OrderByDescending(c => c.Amount).First().ClientCode);

            Console.WriteLine($"Золотой клиент найден. ID: {client?.Code}. ФИО: {client?.ClientName}");
        }
        private int GetColumnIndex(IXLWorksheet worksheet, string columnName)
        {
            var cell = worksheet.FirstRow().CellsUsed(c => string.Equals(c.Value.ToString(), columnName, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            return cell?.Address.ColumnNumber ?? 0;
        }

    }
}
