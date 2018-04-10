using CTCT.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CCMassEmailNETStandard.Repository
{
    public interface IContactServiceRepository
    {
        IContactService CreateContactServiceInstance();
    }
}