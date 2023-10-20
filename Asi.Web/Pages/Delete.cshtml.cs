﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Asi.Common.Services;
using static Asi.Web.Records;

namespace Asi.Web.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IContactService _contactService;

        public DeleteModel(IContactService contactService) => _contactService = contactService;

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
            else
            {
                Contact = contact.ToRecord();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var contactId = await _contactService.DeleteContactAsync(id.Value);

            if (contactId == null)
            {
                return NotFound();
            }

            return RedirectToPage("./Index");
        }
    }
}
