using CTCT.Components.Contacts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CCMassEmailNETStandard.Models
{
    public class ContactModel: Component, IEnumerable
    {
        //
        // Summary:
        //     Gets or sets the list of custom fields.
        public IList<CustomField> CustomFields { get; set; }
            //
            // Summary:
            //     Gets or sets the date and time contact's information was last modified
            public DateTime? DateModified { get; set; }
            //
            // Summary:
            //     Gets or sets the date and time the contact was added
            public DateTime? DateCreated { get; set; }
            //
            // Summary:
            //     Gets or sets the fax number.
            public string Fax { get; set; }
            //
            // Summary:
            //     Gets or sets the cell phone.
            public string CellPhone { get; set; }
            //
            // Summary:
            //     Gets or sets the work phone.
            public string WorkPhone { get; set; }
            //
            // Summary:
            //     Gets or sets the home phone.
            public string HomePhone { get; set; }
            //
            // Summary:
            //     Gets or sets the company name.
            public string CompanyName { get; set; }
            //
            // Summary:
            //     Gets or sets the notes.
            public IList<Note> Notes { get; set; }
            //
            // Summary:
            //     Gets or sets the lists.
            public List<ContactList> Lists { get; set; }
            //
            // Summary:
            //     Gets or sets the addresses.
            public IList<Address> Addresses { get; set; }
            //
            // Summary:
            //     Gets or sets the prefix name.
            public string PrefixName { get; set; }
        //
        // Summary:
        //     Gets or sets the email addresses.
        public EmailAddress EmailAddresses { get; set; }
            //
            // Summary:
            //     Gets or sets the source.
            public string Source { get; set; }
            //
            // Summary:
            //     Gets or sets the confirmation flag.
            public bool Confirmed { get; set; }
            //
            // Summary:
            //     Gets or sets the last name.
            [Required]
            public string LastName { get; set; }
            //
            // Summary:
            //     Gets or sets the middle name.

            public string MiddleName { get; set; }
            //
            // Summary:
            //     Gets or sets the first name.
            public string FirstName { get; set; }
            //
            // Summary:
            //     Gets or sets the status.
            public string Status { get; set; }
            //
            // Summary:
            //     Gets or sets the id.
            public string Id { get; set; }
            //
            // Summary:
            //     Gets or sets job title.
            public string JobTitle { get; set; }
            //
            // Summary:
            //     Gets or sets the source details.
            public string SourceDetails { get; set; }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

   
}