using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asi.Common.Services;
using static Asi.Web.Records;

namespace Asi.Web.Pages
{
    public class EditModel : PageModel
    {
        private readonly IContactService _contactService;

        public EditModel(IContactService contactService) => _contactService = contactService;

        [BindProperty]
        public ContactRecord Contact { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contact = await _contactService.GetContactAsync(id.Value);
            if (contact == null)
            {
                return NotFound();
            }
            Contact = contact.ToRecord();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var c = await _contactService.UpdateContactAsync(Contact.ToEntity());

            if (c != null)
            {
                Contact = c.ToRecord();
            }
            else
            {
                return BadRequest();
            }

            return RedirectToPage("./Index");
        }
    }
}
