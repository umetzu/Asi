using Asi.Common.Models;

namespace Asi.Common.Services
{
	public interface IContactService
	{
		public Task<Contact?> AddContactAsync(Contact contact);
		public Task<Contact?> GetContactAsync(long Id);
		public Task<Contact?> UpdateContactAsync(Contact contact);
		public Task<long?> DeleteContactAsync(long Id);
		public Task<IEnumerable<Contact>> GetContactsAsync(string? name = null, DateTime? fromDate = null, DateTime? toDate = null);
	}
}
