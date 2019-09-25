using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebapiTest.Models
{
    public class InvoiceDetail
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string ItemCode { get; set; }
        public string Dscription { get; set; }
        public double Quantity { get; set; }
        public DateTime ShipDate { get; set; }
        public double Price { get; set; }
        public string Currency { get; set; }
        public double LineTotal { get; set; }
        public double PriceAfVAT { get; set; }
        public string WhsCode { get; set; }
        public string TaxCode { get; set; }
        public List<Lote> lotes { get; set; }
    }
}