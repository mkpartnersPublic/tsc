using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Data.SqlClient;

namespace Inventory
{
    /// <summary>
    /// Summary description for Service1
    /// </summary>
    [WebService(Namespace = "http://tsc.org/Inventory")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Inventory : System.Web.Services.WebService
    {



        [WebMethod]
        public string returnConnString()
        {
            try
            {
                
               return HelperClass.RetrieveConnectionString();

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        [WebMethod]
        public InventoryResponse getVersePrice(int projectId, int languageId, Boolean isGroup)
        {
            try
            {
                InventoryResponse invResponse = new InventoryResponse();

                invResponse.initialize();

                BusinessRules bs = new BusinessRules();

                invResponse = bs.getVersePrice(projectId, languageId, isGroup);

                return invResponse;

            }
            catch (Exception ex)
            {
                InventoryResponse invErrResponce = new InventoryResponse();
                invErrResponce.error = ex.Message;
                return invErrResponce;
            }

        }


        [WebMethod]
        public InventoryResponse getVersesAssignedByDonationId(String sfdcDonationId)
        {
            try
            {
                InventoryResponse invResponse = new InventoryResponse();

                invResponse.initialize();

                BusinessRules bs = new BusinessRules();

                invResponse = bs.getVersesAssignedByDonationId(sfdcDonationId);

                return invResponse;

            }
            catch (Exception ex)
            {
                InventoryResponse invErrResponce = new InventoryResponse();
                invErrResponce.error = ex.Message;
                return invErrResponce;
            }
        }

        [WebMethod]
        public InventoryResponse getVersesAssignedByAuthId(int authId)
        {
            try
            {
                InventoryResponse invResponse = new InventoryResponse();

                invResponse.initialize();

                BusinessRules bs = new BusinessRules();

                invResponse = bs.getVersesAssignedByAuthId(authId);

                return invResponse;

            }
            catch (Exception ex)
            {
                InventoryResponse invErrResponce = new InventoryResponse();
                invErrResponce.error = ex.Message;
                return invErrResponce;
            }
        }

        [WebMethod]
        public InventoryResponse getVersesAssigned(String sfdcDonationId, int authId)
        {
            try
            {
                InventoryResponse invResponse = new InventoryResponse();

                invResponse.initialize();

                BusinessRules bs = new BusinessRules();

                if (sfdcDonationId.Length > 0)
                {
                    invResponse = bs.getVersesAssignedByDonationId(sfdcDonationId);
                }
                else
                {

                    if (authId.ToString().Length > 0)
                    {
                        invResponse = bs.getVersesAssignedByAuthId(authId);
                    }
                }
                return invResponse;

            }
            catch (Exception ex)
            {
                InventoryResponse invErrResponce = new InventoryResponse();
                invErrResponce.error = ex.Message;
                return invErrResponce;
            }
        }

        [WebMethod]
        public InventoryResponse releaseVersesAssignedByDonationId(String sfdcDonationId)
        {
            try
            {
                InventoryResponse invResponse = new InventoryResponse();

                invResponse.initialize();

                BusinessRules bs = new BusinessRules();

                invResponse = bs.releaseVersesAssignedByDonationId(sfdcDonationId);

                return invResponse;

            }
            catch (Exception ex)
            {
                InventoryResponse invErrResponce = new InventoryResponse();
                invErrResponce.error = ex.Message;
                return invErrResponce;
            }
        }

        [WebMethod]
        public InventoryResponse releaseVersesAssignedByAuthId(int authId)
        {
            try
            {
                InventoryResponse invResponse = new InventoryResponse();

                invResponse.initialize();

                BusinessRules bs = new BusinessRules();

                invResponse = bs.releaseVersesAssignedByAuthId(authId);

                return invResponse;

            }
            catch (Exception ex)
            {
                InventoryResponse invErrResponce = new InventoryResponse();
                invErrResponce.error = ex.Message;
                return invErrResponce;
            }
        }

        [WebMethod]
        public InventoryResponse assignVerses(int projectId, int languageId, String sfdcDonationId, String sfdcContactId, String fundId, DateTime paymentDate, int authId, int numberOfVerses)
        {
            try
            {
                InventoryResponse invResponse = new InventoryResponse();

                invResponse.initialize();

                BusinessRules bs = new BusinessRules();

                invResponse = bs.assignVerses(projectId, languageId, sfdcDonationId, sfdcContactId, fundId, paymentDate, authId, numberOfVerses);

                return invResponse;

            }
            catch (Exception ex)
            {
                InventoryResponse invErrResponce = new InventoryResponse();
                invErrResponce.error = ex.Message;
                return invErrResponce;
            }
        }

        private void InitializeComponent()
        {

        }
    }

}
