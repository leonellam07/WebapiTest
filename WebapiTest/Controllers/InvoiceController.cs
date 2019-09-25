using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using WebapiTest.DataAccess;
using WebapiTest.Models;
using WebapiTest.Utilities;

namespace WebapiTest.Controllers
{
    [Route("Invoice")]
    public class InvoiceController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get(string DocEntry)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotAcceptable);
            
            try
            {
                InvoiceRepository repository = new InvoiceRepository();
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(repository.GetById(DocEntry)));
            }
            catch (Exception ex)
            {

                Error error = new Error
                {
                    Mensaje = ex.Message,
                    MensajeInterno = ex.InnerException != null ? ex.InnerException.Message : null
                };

                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(error));
            }
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return ResponseMessage(response);
        }


        [HttpPost]
        public IHttpActionResult Post([FromBody] Invoice invoice)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotAcceptable);

            try
            {
                InvoiceRepository repository = new InvoiceRepository();
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(repository.Add(invoice)));
            }
            catch (Exception ex)
            {

                Error error = new Error
                {
                    Mensaje = ex.Message,
                    MensajeInterno = ex.InnerException != null ? ex.InnerException.Message : null
                };

                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(error));
            }
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return ResponseMessage(response);
        }


        [HttpGet]
        [Route("Invoice/GetByDate")]
        public IHttpActionResult GetByDate(string DocDate)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.NotAcceptable);

            try
            {
                InvoiceRepository repository = new InvoiceRepository();
                response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(JsonConvert.SerializeObject(repository.GetAll(DocDate)));
            }
            catch (Exception ex)
            {

                Error error = new Error
                {
                    Mensaje = ex.Message,
                    MensajeInterno = ex.InnerException != null ? ex.InnerException.Message : null
                };

                response = Request.CreateResponse(HttpStatusCode.InternalServerError);
                response.Content = new StringContent(JsonConvert.SerializeObject(error));
            }
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return ResponseMessage(response);
        }
    }
}
