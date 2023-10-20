using Asi.Common.Models;
using Asi.Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CS = System.Console;

namespace Asi.Console
{
	public class ContactWorker : BackgroundService
	{
		private readonly IHostApplicationLifetime _applicationLifetime;
		private readonly IServiceProvider _serviceProvider;
		private IContactService? _contactService;

		public ContactWorker(IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider) =>
			(_applicationLifetime, _serviceProvider) = (applicationLifetime, serviceProvider);

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using IServiceScope scope = _serviceProvider.CreateScope();
				_contactService = scope.ServiceProvider.GetRequiredService<IContactService>();

				await ShowMenu();
			}

			_applicationLifetime.StopApplication();
		}

		private async Task ShowMenu()
		{
			CS.SetCursorPosition(0, 0);

			await CS.Out.WriteLineAsync("Contacts Options\n");
			await CS.Out.WriteLineAsync("\t1 - Show all");
			await CS.Out.WriteLineAsync("\t2 - Add");
			await CS.Out.WriteLineAsync("\t3 - Update");
			await CS.Out.WriteLineAsync("\t4 - Get");
			await CS.Out.WriteLineAsync("\t5 - Delete");
			await CS.Out.WriteLineAsync("\t6 - Search");

			CS.SetCursorPosition(0, 9);
			await CS.Out.WriteAsync("Select option (1-6): ");
			string selectedOption = await CS.In.ReadLineAsync() ?? "";

			await (selectedOption switch
			{
				"1" => AllContacts(),
				"2" => CreateContact(),
				"3" => UpdateContact(),
				"4" => ContactById(),
				"5" => DeleteContact(),
				"6" => SearchContact(),
				_ => CS.Out.WriteLineAsync("Invalid option"),
			});
		}

		private async Task AllContacts()
		{
			Utils.Clear();

			IEnumerable<Contact> contacts = await _contactService!.GetContactsAsync();

			await contacts.PrintContacts();

			await CS.Out.WriteAsync("\nPress Enter to continue...");
			await CS.In.ReadLineAsync();

			Utils.Clear();
		}

		private async Task ContactById()
		{
			Utils.Clear();

			long id = await Utils.ReadLong("Contact Id to lookup");

			await (await _contactService!.GetContactAsync(id) switch
			{
				Contact c => c.Print(),
				_ => CS.Out.WriteLineAsync("Contact not found")
			});

			await CS.Out.WriteAsync("\nPress Enter to continue...");
			await CS.In.ReadLineAsync();

			Utils.Clear();
		}

		private async Task UpdateContact()
		{
			Utils.Clear();

			long id = await Utils.ReadLong("Contact Id to update");

			await (await _contactService!.GetContactAsync(id) switch
			{
				Contact c => UpdateContact(c),
				_ => CS.Out.WriteLineAsync("Contact not found")
			});

			await CS.Out.WriteAsync("\nPress Enter to continue...");
			await CS.In.ReadLineAsync();

			Utils.Clear();
		}

		private async Task UpdateContact(Contact c)
		{
			Contact contact = new()
			{
				Id = c.Id,
				Name = await Utils.ReadString($"Name", c.Name),
				BirthDate = await Utils.ReadDate("BirthDate", c.BirthDate)
			};

			foreach (Email e in c.Emails)
			{
				await CS.Out.WriteLineAsync($"\t{(e.IsPrimary == 1 ? "Primary " : "")}Email ({e.Id}) - {e.Address}");
				if (await Utils.Ask($"Remove Email {e.Id}?", "y", "n"))
				{
					continue;
				}

				if (!await Utils.Ask($"Update Email {e.Id}?", "y", "n"))
				{
					contact.Emails.Add(new Email { Id = e.Id, Address = e.Address, IsPrimary = e.IsPrimary });
					continue;
				}

				Email email = new()
				{
					Id = e.Id,
					Address = await Utils.ReadString("Address", e.Address),
					IsPrimary = await Utils.Ask("IsPrimary", "y", "n") ? 1 : 0
				};

				contact.Emails.Add(email);
			}

			while (await Utils.Ask("Add Email?", "y", "n"))
			{
				Email email = new()
				{
					Id = await Utils.ReadLong("Id"),
					Address = await Utils.ReadString("Address"),
					IsPrimary = await Utils.Ask("IsPrimary", "y", "n") ? 1 : 0
				};

				contact.Emails.Add(email);
			}

			await (await _contactService!.UpdateContactAsync(contact) switch
			{
				Contact result => result.Print(),
				_ => CS.Out.WriteLineAsync("Contact not created")
			});
		}

		private async Task CreateContact()
		{
			Utils.Clear();

			Contact contact = new()
			{
				Id = await Utils.ReadLong("Id"),
				Name = await Utils.ReadString("Name"),
				BirthDate = await Utils.ReadDate("BirthDate")
			};

			while (await Utils.Ask("Add Email?", "y", "n"))
			{
				Email email = new()
				{
					Id = await Utils.ReadLong("Id"),
					Address = await Utils.ReadString("Address"),
					IsPrimary = await Utils.Ask("IsPrimary", "y", "n") ? 1 : 0
				};

				contact.Emails.Add(email);
			}

			await (await _contactService!.AddContactAsync(contact) switch
			{
				Contact c => c.Print(),
				_ => CS.Out.WriteLineAsync("Contact not created")
			});

			await CS.Out.WriteAsync("\nPress Enter to continue...");
			await CS.In.ReadLineAsync();

			Utils.Clear();
		}

		private async Task SearchContact()
		{
			Utils.Clear();

			string name = await Utils.ReadString("Name");
			DateTime? fromDate = await Utils.ReadDate("BirthDate After");
			DateTime? toDate = await Utils.ReadDate("BirthDate Before");

			IEnumerable<Contact> contacts = await _contactService!.GetContactsAsync(name, fromDate, toDate);

			await contacts.PrintContacts();

			await CS.Out.WriteAsync("\nPress Enter to continue...");
			await CS.In.ReadLineAsync();

			Utils.Clear();
		}

		private async Task DeleteContact()
		{
			Utils.Clear();

			long id = await Utils.ReadLong("Contact Id to delete");

			await (await _contactService!.DeleteContactAsync(id) switch
			{
				long => CS.Out.WriteLineAsync("Contact deleted"),
				_ => CS.Out.WriteLineAsync("Contact not found")
			});

			await CS.Out.WriteAsync("\nPress Enter to continue...");
			await CS.In.ReadLineAsync();

			Utils.Clear();
		}
	}
}
