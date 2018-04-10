using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CTCT;
using CTCT.Components;
using CTCT.Components.EmailCampaigns;
using CTCT.Services;

namespace CCMassEmailNETStandard.Repository
{
    public class ContactServiceRepository : IContactServiceRepository
    {
        public EmailCampaign AddCampaign(EmailCampaign campaign)
        {
            throw new NotImplementedException();
        }

        public Schedule AddSchedule(string campaignId, Schedule schedule)
        {
            throw new NotImplementedException();
        }

        // private IUserServiceContext userServiceContext = null;
        //public ContactServiceRepository(IUserServiceContext userServiceContext)
        //{
        //    this.userServiceContext = userServiceContext;
        //}

        public IContactService CreateContactServiceInstance()
        {
            string _apiKey = "";
            string _accessToken = "";

            IUserServiceContext userServiceContext = new UserServiceContext(_accessToken, _apiKey);
            ConstantContactFactory serviceFactory = new ConstantContactFactory(userServiceContext);
            var contactService = serviceFactory.CreateContactService();
            return contactService;
        }


    }
}