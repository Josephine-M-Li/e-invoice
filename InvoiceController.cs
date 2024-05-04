using System;
using System.Xml;
using System.Web.Services.Protocols;
using System.Web.Mvc;
using InvoiceService; // 假設服務引用名稱為InvoiceService

namespace EInvoice.Controllers
{
    public class InvoiceController : Controller
    {
        public ActionResult Index()
        {
            // 準備要發送的資料
            string orderId = "123456";
            string orderDate = DateTime.Now.ToString("yyyy/MM/dd");
            string buyerIdentifier = "buyer_identifier";
            string buyerName = "buyer_name";
            string buyerEmailAddress = "buyer_email@example.com";
            string donateMark = "0";
            string invoiceType = "07";
            string taxType = "1";
            string payWay = "3";
            string productionCode = "product_code";
            string description = "Product A";
            int quantity = 1;
            decimal unitPrice = 100.00m;

            // 準備 XML 資料
            XmlDocument xmlDoc = new XmlDocument();
            XmlElement root = xmlDoc.CreateElement("InvoiceData");
            xmlDoc.AppendChild(root);

            XmlElement orderElement = xmlDoc.CreateElement("Order");
            orderElement.SetAttribute("OrderId", orderId);
            orderElement.SetAttribute("OrderDate", orderDate);
            orderElement.SetAttribute("BuyerIdentifier", buyerIdentifier);
            orderElement.SetAttribute("BuyerName", buyerName);
            orderElement.SetAttribute("BuyerEmailAddress", buyerEmailAddress);
            orderElement.SetAttribute("DonateMark", donateMark);
            orderElement.SetAttribute("InvoiceType", invoiceType);
            orderElement.SetAttribute("TaxType", taxType);
            orderElement.SetAttribute("PayWay", payWay);

            XmlElement productItem = xmlDoc.CreateElement("ProductItem");
            productItem.SetAttribute("ProductionCode", productionCode);
            productItem.SetAttribute("Description", description);
            productItem.SetAttribute("Quantity", quantity.ToString());
            productItem.SetAttribute("UnitPrice", unitPrice.ToString());

            orderElement.AppendChild(productItem);
            root.AppendChild(orderElement);

            // 準備發送至 API 的 XML 資料
            string xmlData = xmlDoc.InnerXml;

            // 使用 SOAP 客戶端呼叫外部服務
            try
            {
                InvoiceServiceClient client = new InvoiceServiceClient();
                string response = client.CreateInvoiceV3(xmlData, "1", "INVOICE_API_ID", "INVOICE_API_KEY");
                ViewBag.Response = response;
            }
            catch (SoapException ex)
            {
                ViewBag.Response = "SOAP Exception: " + ex.Message;
            }
            catch (Exception ex)
            {
                ViewBag.Response = "Exception: " + ex.Message;
            }

            return View();
        }
    }
}
