
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asi.Common.Services;
using static Asi.Web.Records;
using Microsoft.AspNetCore.Mvc;

namespace Asi.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IContactService _contactService;

        public IndexModel(IContactService contactService) => _contactService = contactService;

        public IList<ContactRecord> Contact { get; set; } = default!;

        [BindProperty]
        public FilterRecord Filter { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Contact = (await _contactService.GetContactsAsync()).Select(x => x.ToRecord()).ToList();
        }

        public async Task OnPostAsync()
        {
            Contact = (await _contactService.GetContactsAsync(Filter.Name, Filter.FromDate, Filter.ToDate)).Select(x => x.ToRecord()).ToList();
        }
    }
}
