using Asi.Common.Models;
using Asi.Common.Services;
using Microsoft.AspNetCore.Mvc;
using static Asi.Web.Records;

namespace Asi.Web.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class ContactController : ControllerBase
	{
		private readonly IContactService _contactService;

		public ContactController(IContactService contactService) => _contactService = contactService;

		[HttpGet]
		public async Task<IEnumerable<ContactRecord>> Get([FromQuery]FilterRecord filter)
			=> (await _contactService.GetContactsAsync(filter.Name, filter.FromDate, filter.ToDate))
			.Select(x => x.ToRecord());

		[HttpPost]
		public async Task<IActionResult> Add(ContactRecord record) =>
			await _contactService.AddContactAsync(record.ToEntity()) is Contact c ? Ok(c.ToRecord()) : BadRequest();

		[HttpPut]
		public async Task<IActionResult> Update(ContactRecord record) =>
			await _contactService.UpdateContactAsync(record.ToEntity()) is Contact c ? Ok(c.ToRecord()) : BadRequest();

		[HttpGet("{id:long}")]
		public async Task<IActionResult> Get(long id) => 
			await _contactService.GetContactAsync(id) is Contact c ? Ok(c.ToRecord()) : NotFound();

		[HttpDelete("{id:long}")]
		public async Task<IActionResult> Delete(long id) =>
			await _contactService.DeleteContactAsync(id) is long c ? Ok(c) : BadRequest();
	}


}