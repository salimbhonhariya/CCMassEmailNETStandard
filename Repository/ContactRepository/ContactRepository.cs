using CTCT.Components.Contacts;
using CTCT.Services;
using Email.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCMassEmailNETStandard.Repository
{
    
    public class ContactRepository : IContactRepository
    {
        private IContactServiceRepository _contactServiceRepository;

        public ContactRepository(IContactServiceRepository contactServiceRepository)
        {
            _contactServiceRepository = contactServiceRepository;
        }
        public Contact GetContact(string id)
        {
            Contact contact;
            try
            {
                IContactService contactService = _contactServiceRepository.CreateContactServiceInstance();
                contact = contactService.GetContact(id);
            }
            catch (Exception ex)
            {
                throw new ContactRepositoryException(ex.ToString());
                //throw new ContactRepositoryException("Contact not found");
            }
            return contact;
        }
    }
}