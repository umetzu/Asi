using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asi.Common.Services;
using static Asi.Web.Records;

namespace Asi.Web.Pages
{
    public class CreateModel : PageModel
    {
        private readonly IContactService _contactService;

        public CreateModel(IContactService contactService) => _contactService = contactService;

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ContactRecord Contact { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Contact == null)
            {
                return Page();
            }

            var c = await _contactService.AddContactAsync(Contact.ToEntity());

            if (c != null)
            {
                Contact = c!.ToRecord();
            }
            else
            {
                return BadRequest();
            }

            return RedirectToPage("./Index");
        }
    }
}
