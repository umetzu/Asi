using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asi.Common.Services;
using static Asi.Web.Records;
using System.Net;

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

        [BindProperty]
        public int EmailIndex { get; set; } = 0;

        public async Task<IActionResult> OnPostRow()
        {
            List<EmailRecord> emails = Contact.Emails != null ? Contact.Emails.ToList() : new List<EmailRecord>();
            emails.Add(new EmailRecord());

            Contact = new ContactRecord(Contact.Id, Contact.Name, Contact.BirthDate, emails.ToArray());

            await Task.Delay(0);
            return Page();
        }

        public async Task<IActionResult> OnPostLess()
        {
            if (Contact.Emails != null)
            {
                EmailRecord emailRecord = Contact.Emails[EmailIndex];
                List<EmailRecord> emails = Contact.Emails.ToList();
                emails.Remove(emailRecord);

                Contact = new ContactRecord(Contact.Id, Contact.Name, Contact.BirthDate, emails.ToArray());
            }

            await Task.Delay(0);
            return Page();
        }

        public async Task<IActionResult> OnPostAllAsync()
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
