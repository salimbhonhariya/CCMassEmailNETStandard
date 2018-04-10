using CCMassEmailNETStandard.Models;
using CCMassEmailNETStandard.Repository;
using CTCT;
using CTCT.Components.Contacts;
using CTCT.Components.EmailCampaigns;
using CTCT.Services;
using Email.CustomException;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.Office.Interop.Excel;
using System.Xml;
using System.IO;
using System.Linq;
using CCMassEmailNETStandard.ViewModel;
using System.Text.RegularExpressions;

namespace Email.Controllers
{
    public class CCController : Controller
    {
        IContactRepository _contactRepository;
        IContactServiceRepository _contactServiceRepository;

        private const string CustomerEmail = "";

        private const string ApiKey = "";
        private const string AccessToken = "";
        private IUserServiceContext userServiceContext = new UserServiceContext(AccessToken, ApiKey);

        public CCController(IContactRepository contactRepository, IContactServiceRepository contactServiceRepository)
        {
            _contactRepository = contactRepository;
            _contactServiceRepository = contactServiceRepository;
        }
        // Before implementing the MVC IoC Unity Container
        //public CCController()
        //{

        //    _contactServiceRepository = new ContactServiceRepository();
        //    _contactRepository = new ContactRepository(_contactServiceRepository);
        //}


        [System.Web.Mvc.HttpGet()]
        [System.Web.Mvc.ActionName("SendEmailNow")]
        public ActionResult LiveEmailCampaign()
        {
            var cc = new ConstantContactFactory(userServiceContext);
            var emailCampaignService = cc.CreateEmailCampaignService();
            var campaignScheduleService = cc.CreateCampaignScheduleService();
            var camp = new EmailCampaign
            {
                EmailContent = "<html><body>EMAIL CONTENT.</body></html>",
                Subject = "campaign subject",
                FromName = "my company",
                FromEmail = CustomerEmail,
                ReplyToEmail = CustomerEmail,
                Name = "campaign_" + DateTime.Now.ToString("yyMMddHHmmss"),
                TextContent = "email campaign text content",
                GreetingString = "Dear ",
                //TemplateType = TemplateType.CUSTOM,
                Status = CampaignStatus.DRAFT,
                EmailContentFormat = CampaignEmailFormat.HTML,
                StyleSheet = "",
                MessageFooter = new MessageFooter
                {
                    OrganizationName = "my organization",
                    AddressLine1 = "123 Mapple Street",
                    AddressLine2 = "Suite 1",
                    AddressLine3 = "",
                    City = "Boston",
                    State = "MA",
                    PostalCode = "02101",
                    Country = "US",
                    IncludeForwardEmail = true,
                    ForwardEmailLinkText = "forward link",
                    IncludeSubscribeLink = true,
                    SubscribeLinkText = "subscribe link"
                }
                    ,
                Lists = new List<SentContactList> { new SentContactList { Id = "1588639466" } }
            };
            camp = emailCampaignService.AddCampaign(camp);
            if (camp != null)
            {
                string campId = camp.Id;
            }
            var test = new TestSend
            {
                Format = EmailFormat.HTML_AND_TEXT.ToString(),
                PersonalMessage = "This is a test send of the email campaign message.",
                EmailAddresses = new List<string> { CustomerEmail }
            };

            var testSend = campaignScheduleService.SendTest(camp.Id, test);
            if (testSend != null)
            {
                string testFormat = testSend.Format;
            }
            string Internalerrormessage = "Campaign email sent successfully";
            TempData["Internalmessage"] = Internalerrormessage;
            return RedirectToAction("ListAll", "CC", new { errormsg = TempData["Internalmessage"] });
        }

        [System.Web.Mvc.HttpGet()]
        [System.Web.Mvc.ActionName("ListAll")]
        public ActionResult ListAll()
        {
            ViewBag.Message = Convert.ToString(TempData["Internalmessage"]);

            IContactService contactService = _contactServiceRepository.CreateContactServiceInstance();
            IList<ContactViewModel> lstcontactViewModels = new List<ContactViewModel>();

            foreach (var item in contactService.GetContacts(null).Results)
            {
                var contactviewmodel = new ContactViewModel
                {
                    Addresses = GetAddressAttributes(item),
                    EmailAddresses = GetEmailAddressesAttributes(item),
                    CustomFields = GetCustomFieldAttributes(item),
                    Lists = GetListAttributes(item),
                    Notes = GetNotesAttributes(item),

                    CellPhone = item.CellPhone,
                    CompanyName = item.CompanyName,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    HomePhone = item.HomePhone,
                    WorkPhone = item.WorkPhone,
                    Fax = item.Fax,
                    JobTitle = item.JobTitle,
                    PrefixName = item.PrefixName,
                    SourceDetails = item.SourceDetails,
                    Id = item.Id
                };

                lstcontactViewModels.Add(contactviewmodel);
            }
            return View(lstcontactViewModels);
        }

        [System.Web.Mvc.ActionName("GetContact")]
        [System.Web.Mvc.HttpGet()]
        public ActionResult Get(string id)
        {
            Contact contact;
            try
            {
                contact = _contactRepository.GetContact(id);
            }
            catch (ContactRepositoryException e)
            {
                throw new HttpException(e.Message.ToString());
                //throw new HttpException(404, "Contact not found");
                //throw;
            }
            catch (Exception ex)
            {
                throw new HttpException(ex.ToString());
                //throw;
            }
            ContactModel contactModel = new ContactModel
            {
                Addresses = contact.Addresses,
                CellPhone = contact.CellPhone,
                CompanyName = contact.CompanyName,
                Confirmed = contact.Confirmed,
                CustomFields = contact.CustomFields,
                DateCreated = contact.DateCreated,
                DateModified = contact.DateModified,
                EmailAddresses = contact.EmailAddresses as EmailAddress,
                Fax = contact.Fax,
                FirstName = contact.FirstName,
                HomePhone = contact.HomePhone,
                JobTitle = contact.JobTitle,
                LastName = contact.LastName,
                Lists = contact.Lists,
                MiddleName = contact.MiddleName,
                Notes = contact.Notes,
                PrefixName = contact.PrefixName,
                Source = contact.Source,
                SourceDetails = contact.SourceDetails,
                Status = contact.Status,
                Id = contact.Id,
                WorkPhone = contact.WorkPhone
            };

            return View("EditContact", contactModel);
        }

        [System.Web.Mvc.HttpGet()]
        [System.Web.Mvc.ActionName("CreateShow")]
        public ActionResult Post(string errormsg)
        {
            //if (Request.HttpMethod == "GET")
            //{
            ViewBag.Message = errormsg;
            return View("~/Views/Home/Create.cshtml");
            //}
            //else
            //{
            //    return View("~/Views/Home/ListAll.cshtml");
            //}

            //ViewBag.Message = errormsg;
            //return View("~/Views/Home/Create.cshtml");
        }

        [System.Web.Mvc.HttpPost()]
        [System.Web.Mvc.ActionName("Create")]
        public ActionResult Post([FromBody]ContactModel contactModel)
        {
            string Internalerrormessage = string.Empty;

            if (!ModelState.IsValid)
            {
                foreach (ModelState modelstate in ViewData.ModelState.Values)
                {
                    foreach (ModelError modelerror in modelstate.Errors)
                    {
                        string errormessage = modelerror.ErrorMessage;
                        // logerrormessage
                    }
                }
            }
            // to create a error to test it
            //Contact contact = new Contact();
            try
            {
                Contact contact = new Contact();
                IContactService contactService = _contactServiceRepository.CreateContactServiceInstance();
                //string BaseUrl = "https://api.constantcontact.com/v2/lists";
                #region CreateContactsFromFile
                // //Contact contact = new Contact();
                // // Don't care about the id value
                // // contact.Id = "1";

                //// Read contact from XML file and add each contact to list on Constant Contact.
                // var contacts = ReadEmailsFromXMLFile();
                // foreach (var item in contacts)
                // {
                //     Contact contact1 = new Contact();
                //     contact1.LastName = item.Name;
                //     contact1.EmailAddresses = IList<EmailAddress> { new EmailAddress { EmailAddr = item.Email}};
                //     PrepareContactObject(contactModel, contact1);
                //     Internalerrormessage = AddOrUpdateContact(contactModel, Internalerrormessage, contact1, contactService);
                // }
                #endregion CreateContactsFromFile

                PrepareContactObject(contactModel, contact);
                Internalerrormessage = AddOrUpdateContact(contactModel, Internalerrormessage, contact, contactService);

            }
            catch (Exception ex)
            {
                Internalerrormessage = "Error happed while creating" + ex.Message;
                TempData["Internalmessage"] = Internalerrormessage;
                return View("~/Views/Home/Create.cshtml", contactModel);
                //return RedirectToAction("CreateShow", "CC", new { errormsg = Internalerrormessage });
                // throw;
            }

            // return View();
            TempData["Internalmessage"] = Internalerrormessage;
            // return RedirectToAction("ListAll", "CC", new { errormsg = TempData["Internalmessage"] });
            return Redirect("http://localhost:59072/CC/ListAll");
        }

        private string AddOrUpdateContact(ContactModel contactModel, string Internalerrormessage, Contact contact, IContactService contactService)
        {
            Contact CheckIfExists = _contactRepository.GetContact(contactModel.Id);
            if (CheckIfExists != null)
            {
                Contact NewContact = contactService.AddContact(contact, true);
                if (NewContact != null)
                {
                    Internalerrormessage = "Contact created successfully";
                }
            }
            else
            {
                contactService.UpdateContact(contact, false);
            }

            return Internalerrormessage;
        }

        private void PrepareContactObject(ContactModel contactModel, Contact contact)
        {
            contact.EmailAddresses.Add(new EmailAddress()
            {
                EmailAddr = contactModel.EmailAddresses.EmailAddr, //"CreateedUsingCreateAPInew" + contactModel.LastName.Replace(" ", string.Empty) + DateTime.Now.ToString("yyyyMMddHHmmss") + "@gmail.com",
                ConfirmStatus = ConfirmStatus.NoConfirmationRequired,
                Status = Status.Active,
            });
            contactModel.LastName.Replace(" ", string.Empty);
            contactModel.FirstName.Replace(" ", string.Empty);
            contactModel.MiddleName.Replace(" ", string.Empty);
            contact.Lists.Add(new ContactList()
            {
                Id = "1588639466",
                Status = Status.Active,
            });
            if (Request.HttpMethod == "POST")
            {
                ViewBag.Emails = contact.EmailAddresses;
            }
        }

        [System.Web.Mvc.HttpGet()]
        public ActionResult Put([FromBody]ContactModel contactModel)
        {
            return RedirectToAction("GetContact", "CC", new { Id = contactModel.Id });
            // return View("~/Views/CC/EditContact.cshtml" );
        }

        [System.Web.Mvc.HttpPost()]
        public void EditContact([FromBody]ContactModel contactModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (ModelState modelstate in ViewData.ModelState.Values)
                {
                    foreach (ModelError modelerror in modelstate.Errors)
                    {
                        string errormessage = modelerror.ErrorMessage;
                    }
                }
            }

            IContactService contactService = _contactServiceRepository.CreateContactServiceInstance();
            Contact existingContact = contactService.GetContact(contactModel.Id.ToString());
            if (existingContact != null)
            {
                //Contact contact = new Contact
                //{
                //    // Don't care about the id value
                //    //Id = id
                //};
                //contact.EmailAddresses.Add(new EmailAddress()
                //{
                //    EmailAddr = "Modified@gmail.com",
                //    ConfirmStatus = ConfirmStatus.NoConfirmationRequired,
                //    Status = Status.Active
                //});
                //existingContact.Lists.Add(new ContactList()
                //{
                //    Id = "1588639466",
                //    Status = Status.Active
                //});

                existingContact.Addresses = contactModel.Addresses;
                existingContact.CellPhone = contactModel.CellPhone;
                existingContact.CompanyName = contactModel.CompanyName;
                existingContact.Confirmed = contactModel.Confirmed;
                existingContact.CustomFields = contactModel.CustomFields;
                existingContact.DateCreated = contactModel.DateCreated;
                existingContact.DateModified = contactModel.DateModified;

                existingContact.EmailAddresses.Clear();
                existingContact.EmailAddresses = new List<EmailAddress> { new EmailAddress
                {
                    EmailAddr = "CreateedUsingCreateAPInew" + contactModel.LastName.Replace(" ", string.Empty) + DateTime.Now.ToString("yyyyMMddHHmmss") + "@gmail.com",
                    ConfirmStatus = ConfirmStatus.NoConfirmationRequired,
                    Status = Status.Active
                } };
                existingContact.Fax = contactModel.Fax;
                existingContact.FirstName = contactModel.FirstName;
                existingContact.HomePhone = contactModel.HomePhone;
                existingContact.JobTitle = contactModel.JobTitle;
                existingContact.LastName = contactModel.LastName;
                existingContact.Lists.Add(new ContactList()
                {
                    Id = "1588639466",
                    Status = Status.Active
                });
                existingContact.MiddleName = contactModel.MiddleName;
                existingContact.Notes = contactModel.Notes;
                existingContact.PrefixName = contactModel.PrefixName;
                existingContact.Source = contactModel.Source;
                existingContact.SourceDetails = contactModel.SourceDetails;
                existingContact.Status = contactModel.Status;
                existingContact.Id = contactModel.Id;
                existingContact.WorkPhone = contactModel.WorkPhone;


                contactService.UpdateContact(existingContact, true);
            }
        }


        //[System.Web.Http.HttpGet()]
        //public ActionResult Delete(string Id)
        //{
        //    if (Id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    IContactService contactService = _contactServiceRepository.CreateContactServiceInstance();
        //    Contact existingContact = contactService.GetContact(Id);

        //    if (existingContact == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(existingContact);
        //}



        [System.Web.Http.HttpPost]
        public ActionResult Delete([FromBody]ContactModel contactModel)
        {
            IContactService contactService = _contactServiceRepository.CreateContactServiceInstance();
            Contact existingContact = contactService.GetContact(contactModel.Id);
            if (existingContact != null)
            {
                contactService.DeleteContact(existingContact);
            }
            return View("~/Views/CC/Deleted.cshtml");
        }

        #region Helper Methods


        [System.Web.Mvc.HttpGet()]
        [System.Web.Mvc.ActionName("ReadEmails")]
        public ActionResult ReadEmailFromXMLFile()
        {
            Dictionary<string, string> dctEmails = new Dictionary<string, string>();

#pragma warning disable CS0618 // Type or member is obsolete
            XmlDataDocument xmldoc = new XmlDataDocument();
#pragma warning restore CS0618 // Type or member is obsolete
            string wanted_path = "C:\\Users\\sbhonhariya\\Documents\\GitHub\\CCMassEmailNETStandard";
            FileStream fs = new FileStream(wanted_path + "\\CCEmailXML.xml", FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);

            var nodes = xmldoc.SelectNodes("//record");
            foreach (XmlNode node in nodes)
            {
                dctEmails.Add(node["Name"].InnerText, node["Email"].InnerText);
            }

            List<KeyValuePair<string, string>> list = dctEmails.ToList();

            IEnumerable<EmailViewModel> model = list
                .Select(c => new EmailViewModel
                {
                    Name = c.Key,
                    Email = c.Value,
                });
            return View("EmailListView", model);
        }

        public IEnumerable<EmailViewModel> ReadEmailsFromXMLFile()
        {
            Dictionary<string, string> dctEmails = new Dictionary<string, string>();

#pragma warning disable CS0618 // Type or member is obsolete
            XmlDataDocument xmldoc = new XmlDataDocument();
#pragma warning restore CS0618 // Type or member is obsolete
            string wanted_path = "C:\\Users\\sbhonhariya\\Documents\\GitHub\\CCMassEmailNETStandard";
            FileStream fs = new FileStream(wanted_path + "\\CCEmailXML.xml", FileMode.Open, FileAccess.Read);
            xmldoc.Load(fs);

            var nodes = xmldoc.SelectNodes("//record");
            foreach (XmlNode node in nodes)
            {
                dctEmails.Add(node["Name"].InnerText, node["Email"].InnerText);
            }

            List<KeyValuePair<string, string>> list = dctEmails.ToList();

            IEnumerable<EmailViewModel> model = list
                .Select(c => new EmailViewModel
                {
                    Name = c.Key,
                    Email = c.Value,
                });
            return model;
        }
        private IList<CustomFieldViewModel> GetCustomFieldAttributes(Contact contact)
        {
            List<CustomFieldViewModel> customFieldViewModels = new List<CustomFieldViewModel>();

            foreach (var item in contact.CustomFields)
            {
                CustomFieldViewModel contactNotesViewModel = new CustomFieldViewModel
                {
                    Name = item.Name,
                    Value = item.Value
                };

                customFieldViewModels.Add(contactNotesViewModel);
            };

            return customFieldViewModels;
        }

        private IList<ContactNotesViewModel> GetNotesAttributes(Contact contact)
        {
            List<ContactNotesViewModel> lstcontactNotesViewModels = new List<ContactNotesViewModel>();

            foreach (var item in contact.Notes)
            {
                ContactNotesViewModel contactNotesViewModel = new ContactNotesViewModel
                {
                    Content = item.Content,
                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate
                };

                lstcontactNotesViewModels.Add(contactNotesViewModel);
            };

            return lstcontactNotesViewModels;
        }

        private IList<ContactListViewModel> GetListAttributes(Contact contact)
        {
            List<ContactListViewModel> lstcontactListViewModels = new List<ContactListViewModel>();

            foreach (var item in contact.Lists)
            {
                ContactListViewModel contactListViewModel = new ContactListViewModel
                {
                    ContactCount = item.ContactCount,
                    DateCreated = item.DateCreated,
                    DateModified = item.DateModified,
                    Name = item.Name,
                    Status = item.Status
                };

                lstcontactListViewModels.Add(contactListViewModel);
            };

            return lstcontactListViewModels;
        }

        private IList<EmailAddressViewModel> GetEmailAddressesAttributes(Contact contact)
        {
            List<EmailAddressViewModel> lstemailAddressViewModels = new List<EmailAddressViewModel>();

            foreach (var item in contact.EmailAddresses)
            {
                EmailAddressViewModel emailAddressViewModel = new EmailAddressViewModel
                {
                    ConfirmStatus = item.ConfirmStatus,
                    Status = item.Status,
                    EmailAddr = item.EmailAddr,
                    OptInDate = item.OptInDate,
                    OptInSource = item.OptInSource,
                    OptOutDate = item.OptOutDate,
                    OptOutSource = item.OptOutSource
                };

                lstemailAddressViewModels.Add(emailAddressViewModel);
            };

            return lstemailAddressViewModels;
        }

        private List<AddressViewModel> GetAddressAttributes(Contact contact)
        {
            List<AddressViewModel> lstaddressViewModels = new List<AddressViewModel>();

            foreach (var item in contact.Addresses)
            {
                AddressViewModel contactmodel = new AddressViewModel
                {
                    AddressType = item.AddressType,
                    City = item.City,
                    CountryCode = item.CountryCode,
                    Line1 = item.Line1,
                    Line2 = item.Line2,
                    Line3 = item.Line3,
                    PostalCode = item.PostalCode,
                    StateName = item.StateName,
                    SubPostalCode = item.SubPostalCode
                };

                lstaddressViewModels.Add(contactmodel);
            };

            return lstaddressViewModels;
        }

        #endregion Helper Methods
    }
}