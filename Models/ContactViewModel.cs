using System;
using System.Collections.Generic;

namespace CCMassEmailNETStandard.Models
{
    public class ContactViewModel
    {
        public string Id { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Fax { get; set; }
        public string CellPhone { get; set; }
        public string WorkPhone { get; set; }
        public string HomePhone { get; set; }
        public string CompanyName { get; set; }
        public string PrefixName { get; set; }
        public string JobTitle { get; set; }
        public string SourceDetails { get; set; }
      
        public IList<ContactNotesViewModel> Notes { get; set; }
        public IList<ContactListViewModel> Lists { get; set; }
        public IList<AddressViewModel> Addresses { get; set; }
        public IList<EmailAddressViewModel> EmailAddresses { get; set; }
        public IList<CustomFieldViewModel> CustomFields { get; set; }
    }
}