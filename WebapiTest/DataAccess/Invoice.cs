using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SAPbobsCOM;
using WebapiTest.Models;

namespace WebapiTest.DataAccess
{

    public interface IInvoiceRepository
    {
        Invoice GetById(string docEntry);
        List<Invoice> GetAll(string docDate);
        Invoice Add(Invoice invoice);
        Invoice Add(List<Invoice> invoices);
    }

    public class InvoiceRepository : IInvoiceRepository
    {
        public Invoice Add(Invoice invoice)
        {
            Documents invoiceSap;
            invoiceSap = (Documents)ApplicationContext.Db.GetBusinessObject(BoObjectTypes.oInvoices);


            invoiceSap.DocType = SAPbobsCOM.BoDocumentTypes.dDocument_Items;
            invoiceSap.DocDueDate = invoice.DocDueDate;
            invoiceSap.CardCode = invoice.CardCode;
            invoiceSap.DocCurrency = invoice.DocCurrency;
            invoiceSap.Address = invoice.Address;
            invoiceSap.DocCurrency = "QTZ";
            invoiceSap.Series = invoice.Series;


            foreach (InvoiceDetail invoiceDetail in invoice.invoiceDetails)
            {
                invoiceSap.Lines.ItemCode = invoiceDetail.ItemCode;
                invoiceSap.Lines.TaxCode = invoiceDetail.TaxCode;
                invoiceSap.Lines.WarehouseCode = invoiceDetail.WhsCode;
                invoiceSap.Lines.MeasureUnit = "1";
                invoiceSap.Add();

                //foreach (Lote lote in invoiceDetail.lotes)
                //{
                //    invoiceSap.Lines.BatchNumbers.BatchNumber = lote.BatchNum;
                //    invoiceSap.Lines.BatchNumbers.Quantity = lote.Quantity;
                //    invoiceSap.Lines.BatchNumbers.Add();
                //}
            }

            if (invoiceSap.Add() != 0)
            {
                throw new Exception("Error(" + ApplicationContext.Db.GetLastErrorCode() + "):" + ApplicationContext.Db.GetLastErrorDescription());
            }
            string key = ApplicationContext.Db.GetNewObjectKey();

            return invoice;
        }

        public Invoice Add(List<Invoice> invoices)
        {
            throw new NotImplementedException();
        }

        public List<Invoice> GetAll(string docDate)
        {
            string query = "select * from oinv where \"DocDate\" = \'" + docDate + "\'";
            Recordset recordset = ApplicationContext.Db.GetBusinessObject(BoObjectTypes.BoRecordset);
            recordset.DoQuery(query);

            if (recordset.RecordCount == 0) { throw new Exception("No se encontraron registros disponibles en la fecha " + docDate); }

            List<Invoice> invoices = new List<Invoice>();
            while (!recordset.EoF)
            {
                Invoice invoice = new Invoice
                {
                    DocEntry = recordset.Fields.Item("DocEntry").Value,
                    DocNum = recordset.Fields.Item("DocNum").Value,
                    DocCurrency = recordset.Fields.Item("DocCur").Value,
                    DocType = recordset.Fields.Item("DocType").Value,
                    DocDate = recordset.Fields.Item("DocDate").Value,
                    CardCode = recordset.Fields.Item("CardCode").Value,
                    CardName = recordset.Fields.Item("CardName").Value,
                    Address = recordset.Fields.Item("Address").Value,
                    Series = recordset.Fields.Item("Series").Value,
                    DocTotal = recordset.Fields.Item("DocTotal").Value
                };

                query = "select * from inv1 where  \"DocEntry\" = " + invoice.DocEntry;
                Recordset recordsetDetails = ApplicationContext.Db.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordsetDetails.DoQuery(query);

                if (recordsetDetails.RecordCount == 0)
                { throw new Exception("No se encontraron los detalles de la facturas en la fecha \"" + docDate + "\""); }

                invoice.invoiceDetails = new List<InvoiceDetail>();
                while (!recordsetDetails.EoF)
                {
                    InvoiceDetail invoiceDetail = new InvoiceDetail
                    {
                        DocEntry = recordsetDetails.Fields.Item("DocEntry").Value,
                        LineNum = recordsetDetails.Fields.Item("LineNum").Value,
                        ItemCode = recordsetDetails.Fields.Item("ItemCode").Value,
                        Dscription = recordsetDetails.Fields.Item("Dscription").Value,
                        Quantity = recordsetDetails.Fields.Item("Quantity").Value,
                        ShipDate = recordsetDetails.Fields.Item("ShipDate").Value,
                        Price = recordsetDetails.Fields.Item("Price").Value,
                        Currency = recordsetDetails.Fields.Item("Currency").Value,
                        LineTotal = recordsetDetails.Fields.Item("LineTotal").Value,
                        PriceAfVAT = recordsetDetails.Fields.Item("PriceAfVAT").Value,
                        WhsCode = recordsetDetails.Fields.Item("WhsCode").Value,
                        TaxCode = recordsetDetails.Fields.Item("TaxCode").Value
                    };

                    invoice.invoiceDetails.Add(invoiceDetail);
                    recordsetDetails.MoveNext();
                }

                invoices.Add(invoice);
                recordset.MoveNext();
            }
            return invoices;
        }

        public Invoice GetById(string docEntry)
        {
            string query = "select * from oinv where \"DocEntry\" = " + docEntry;
            Recordset recordset = ApplicationContext.Db.GetBusinessObject(BoObjectTypes.BoRecordset);
            recordset.DoQuery(query);

            if (recordset.RecordCount == 0) { throw new Exception("No se encontraron registros disponibles."); }

            Invoice invoice = new Invoice();

            invoice.DocEntry = recordset.Fields.Item("DocEntry").Value;
            invoice.DocNum = recordset.Fields.Item("DocNum").Value;
            invoice.DocCurrency = recordset.Fields.Item("DocCur").Value;
            invoice.DocType = recordset.Fields.Item("DocType").Value;
            invoice.DocDate = recordset.Fields.Item("DocDate").Value;
            invoice.CardCode = recordset.Fields.Item("CardCode").Value;
            invoice.CardName = recordset.Fields.Item("CardName").Value;
            invoice.Address = recordset.Fields.Item("Address").Value;
            invoice.Series = recordset.Fields.Item("Series").Value;
            invoice.DocTotal = recordset.Fields.Item("DocTotal").Value;

            query = "select * from inv1 where  \"DocEntry\" = " + docEntry;
            Recordset recordsetDetails = ApplicationContext.Db.GetBusinessObject(BoObjectTypes.BoRecordset);
            recordsetDetails.DoQuery(query);

            if (recordsetDetails.RecordCount == 0)
            { throw new Exception("No se encontraron los detalles de la factura \"" + docEntry + "\""); }

            invoice.invoiceDetails = new List<InvoiceDetail>();
            while (!recordsetDetails.EoF)
            {
                InvoiceDetail invoiceDetail = new InvoiceDetail();
                invoiceDetail.DocEntry = recordsetDetails.Fields.Item("DocEntry").Value;
                invoiceDetail.LineNum = recordsetDetails.Fields.Item("LineNum").Value;
                invoiceDetail.ItemCode = recordsetDetails.Fields.Item("ItemCode").Value;
                invoiceDetail.Dscription = recordsetDetails.Fields.Item("Dscription").Value;
                invoiceDetail.Quantity = recordsetDetails.Fields.Item("Quantity").Value;
                invoiceDetail.ShipDate = recordsetDetails.Fields.Item("ShipDate").Value;
                invoiceDetail.Price = recordsetDetails.Fields.Item("Price").Value;
                invoiceDetail.Currency = recordsetDetails.Fields.Item("Currency").Value;
                invoiceDetail.LineTotal = recordsetDetails.Fields.Item("LineTotal").Value;
                invoiceDetail.PriceAfVAT = recordsetDetails.Fields.Item("PriceAfVAT").Value;
                invoiceDetail.WhsCode = recordsetDetails.Fields.Item("WhsCode").Value;
                invoiceDetail.TaxCode = recordsetDetails.Fields.Item("TaxCode").Value;

                invoice.invoiceDetails.Add(invoiceDetail);
                recordsetDetails.MoveNext();
            }

            return invoice;
        }
    }
}