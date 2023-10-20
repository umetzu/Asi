using Asi.Common.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Asi.Common.Services
{
	public class ContactService : IContactService
	{
		private readonly AsiContext _context;
		private readonly AsiCommonOptions _options;
		public ContactService(AsiContext context, IOptions<AsiCommonOptions> settings) =>
			(_context, _options) = (context, settings.Value);

		public async Task<IEnumerable<Contact>> GetContactsAsync(string? name = null, DateTime? fromDate = null, DateTime? toDate = null)
		{
			var results = await _context.Contacts.Include(x => x.Emails)
				.Where(x => string.IsNullOrWhiteSpace(name) || x.Name.Trim().ToLower().Contains(name.Trim().ToLower()))
				.Where(x => fromDate == null || x.BirthDate >= fromDate)
				.Where(x => toDate == null || x.BirthDate <= fromDate)
				.ToListAsync();

			return results;
		}

		public async Task<Contact?> AddContactAsync(Contact contact)
		{
			if (await _context.Contacts.AnyAsync(x => x.Id == contact.Id))
			{
				return null;
			}

			if (!contact.ValidateAndFix(_options))
			{
				return null;
			}

			_context.Contacts.Add(contact);
			await _context.SaveChangesAsync();

			return contact;
		}

		public async Task<Contact?> UpdateContactAsync(Contact contact)
		{
			if (!await _context.Contacts.AnyAsync(x => x.Id == contact.Id))
			{
				return null;
			}

			if (!contact.ValidateAndFix(_options))
			{
				return null;
			}

			_context.Update(contact);

			DeleteEmails(contact);

			await _context.SaveChangesAsync();

			return contact;
		}

		public async Task<Contact?> GetContactAsync(long id) =>
			await _context.Contacts.Include(x => x.Emails).FirstOrDefaultAsync(x => x.Id == id);

		public async Task<long?> DeleteContactAsync(long id)
		{
			Contact? contact = await _context.Contacts.Include(x => x.Emails).FirstOrDefaultAsync(x => x.Id == id);

			if (contact == null)
			{
				return null;
			}

			_context.Contacts.Remove(contact);

			await _context.SaveChangesAsync();

			return id;
		}

		private void DeleteEmails(Contact contact)
		{
			long[] emailIds = contact.Emails.Select(x => x.Id).ToArray();

			var emailsToRemove = _context.Emails.Where(x => x.ContactId == contact.Id && !emailIds.Contains(x.Id));

			_context.Emails.RemoveRange(emailsToRemove);
		}
	}
}
