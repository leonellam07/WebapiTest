using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebapiTest.Models
{
    public class Invoice
    {
        public int DocEntry { get; set; }
        public int DocNum { get; set; }
        public string DocType { get; set; }
        public string DocCurrency { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public DateTime DocDueDate { get; set; }
        public string Currency { get; set; }
        public string CardName { get; set; }
        public double VatSum { get; set; }
        public string Address { get; set; }
        public int Series { get; set; }
        public double DocTotal { get; set; }
        public List<InvoiceDetail> invoiceDetails { get; set; }
    }
}