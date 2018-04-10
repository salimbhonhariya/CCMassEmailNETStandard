using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CCMassEmailNETStandard.Models;
using CCMassEmailNETStandard.Repository;
using CTCT;
using CTCT.Components.Contacts;
using CTCT.Services;

namespace CCMassEmailNETStandard.Repository
{
    public interface IContactRepository
    {
        Contact GetContact(string id);
    }
}