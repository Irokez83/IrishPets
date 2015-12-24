using System.Threading.Tasks;
using System.Web.Mvc;

namespace IrishPets.Controllers
{
    using Models;

    /// <summary> PayPal transaction processing </summary>
    [Authorize]
    public class PaymentController : Controller
    {
        private IrishPetsDb m_IrishPetsDb = IrishPetsDb.Create();

        public async Task<ActionResult> Success(string returnUrl, int? Id, int? t)
        {
            var __advert = await m_IrishPetsDb.PetAdverts.FindAsync(Id);
            if(null != __advert)
            {
                __advert.TypeOfSale = TypeOfSale.Commercial;

                m_IrishPetsDb.Entry<PetAdvert>(__advert).State = System.Data.Entity.EntityState.Modified;
                await m_IrishPetsDb.SaveChangesAsync();
            }
            
            return Redirect($"{returnUrl}?t={t}");
        }

        public ActionResult Failure(string returnUrl) =>  View();

        #region Instant Payment Notification(IPN)

        //After user completed payment, PayPal confirms it and sends to my webservice - Instant Payment Notification(IPN) is used.

        public ActionResult IPN() => View();

        public ActionResult SetExpressCheckout() => View();
        
        public ActionResult CreateIpnListener() => View();

        public ActionResult SendRequest() => View();

        public ActionResult GetPaymentType() => View();

        public ActionResult GetPostFromRawData() => View();

        #endregion Instant Payment Notification(IPN)


        #region Payment processing

        /// <summary> Payment has to be validated. Transaction ID received and processed. </summary>
        public ActionResult ProcessPayment() => View();

        #endregion Payment processing

        #region Payment validation

        public ActionResult ValidateTransaction() => View();

        #endregion Payment validation

    }
}