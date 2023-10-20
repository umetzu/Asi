using Asi.Common.Models;

namespace Asi.Web
{
    public static class Records
    {
        public record EmailRecord(long Id = 0, string Address = "", long IsPrimary = 0);
        public record ContactRecord(long Id = 0, string Name = "", DateTime? BirthDate = null, EmailRecord[]? Emails = null);
        public record FilterRecord(string? Name = "", DateTime? FromDate = null, DateTime? ToDate = null);

        public static Contact ToEntity(this ContactRecord contactRecord)
        {
            Contact contact = new()
            {
                Id = contactRecord.Id,
                Name = contactRecord.Name,
                BirthDate = contactRecord.BirthDate
            };

            if (contactRecord.Emails != null)
            {
                contact.Emails = contactRecord.Emails.Select(x => new Email
                {
                    Id = x.Id,
                    Address = x.Address,
                    IsPrimary = x.IsPrimary,
                    ContactId = contactRecord.Id
                }).ToList();
            }

            return contact;
        }

        public static ContactRecord ToRecord(this Contact contact)
        {
            ContactRecord record = new(contact.Id,
                contact.Name,
                contact.BirthDate,
                contact.Emails.Select(x => new EmailRecord(x.Id, x.Address, x.IsPrimary)).ToArray());

            return record;
        }
    }


}
