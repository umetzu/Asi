using Asi.Common.Models;
using CS = System.Console;

namespace Asi.Console
{
	public static class Utils
	{
		public async static Task PrintContacts(this IEnumerable<Contact> contacts)
		{
			if (!contacts.Any())
			{
				await CS.Out.WriteLineAsync("No contacts found");
			}

			foreach (Contact contact in contacts)
			{
				await Print(contact);
			}
		}

		public async static Task Print(this Contact contact)
		{
			await CS.Out.WriteLineAsync($"Contact: ({contact.Id})");
			await CS.Out.WriteLineAsync($"\tName: {contact.Name,10}");
			await CS.Out.WriteLineAsync($"\tBirthDate: {contact.BirthDate,12:yyyy-MM-dd}");

			foreach (var email in contact.Emails)
			{
				await CS.Out.WriteLineAsync($"\tEmail: ({email.Id})");
				await CS.Out.WriteLineAsync($"\t\tAddress: {email.Address}");
				await CS.Out.WriteLineAsync($"\t\tPrimary: {(email.IsPrimary == 1 ? "yes" : "no")}");
			}
		}

		public static void Clear()
		{
			CS.SetCursorPosition(0, 0);
			CS.Clear();
		}

		public async static Task<string> ReadString(string message, string defaultValue = "")
		{
			string messageDefault = !string.IsNullOrEmpty(defaultValue) ? defaultValue : "empty";
			await CS.Out.WriteAsync($"{message} ({messageDefault}): ");

			string? result = await CS.In.ReadLineAsync();

			return string.IsNullOrWhiteSpace(result) ? defaultValue : result;
		}

		public async static Task<bool> Ask(string message, string positive, string negative)
		{
			await CS.Out.WriteAsync($"{message} ({positive}/{negative}): ");

			(int left, int top) = CS.GetCursorPosition();
			string? unparsedString = null;

			do
			{
				CS.SetCursorPosition(left, top);
				await CS.Out.WriteAsync(new string(' ', unparsedString?.Length ?? 0));
				CS.SetCursorPosition(left, top);
				unparsedString = await CS.In.ReadLineAsync();

				if (unparsedString?.ToLower() == positive)
				{
					return true;
				}
				if (unparsedString?.ToLower() == negative)
				{
					return false;
				}

				await CS.Out.WriteLineAsync("Invalid value");

			} while (true);
		}

		public async static Task<long> ReadLong(string message, long defaultValue = 0)
		{
			string messageDefault = defaultValue != 0 ? $"{defaultValue}" : "empty";
			await CS.Out.WriteAsync($"{message} ({messageDefault}): ");

			(int left, int top) = CS.GetCursorPosition();
			long result;
			string? unparsedLong = null;

			do
			{
				CS.SetCursorPosition(left, top);
				await CS.Out.WriteAsync(new string(' ', unparsedLong?.Length ?? 0));
				CS.SetCursorPosition(left, top);
				unparsedLong = await CS.In.ReadLineAsync();
				if (long.TryParse(unparsedLong, out long parsedLong) ||
					string.IsNullOrEmpty(unparsedLong))
				{
					result = string.IsNullOrEmpty(unparsedLong) ? defaultValue : parsedLong;
					break;
				}
				await CS.Out.WriteLineAsync("Invalid value");

			} while (true);

			return result;
		}

		public async static Task<DateTime?> ReadDate(string message, DateTime? defaultValue = null)
		{
			string messageDefault = defaultValue.HasValue ? $"{defaultValue:yyyy-MM-dd}" : "empty";
			await CS.Out.WriteAsync($"{message} ({messageDefault}): ");
			(int left, int top) = CS.GetCursorPosition();
			DateTime? date;
			string? unparsedDate = null;

			do
			{
				CS.SetCursorPosition(left, top);
				await CS.Out.WriteAsync(new string(' ', unparsedDate?.Length ?? 0));
				CS.SetCursorPosition(left, top);
				unparsedDate = await CS.In.ReadLineAsync();
				if (DateTime.TryParse(unparsedDate, out DateTime parsedDate) ||
					string.IsNullOrEmpty(unparsedDate))
				{
					date = string.IsNullOrEmpty(unparsedDate) ? defaultValue : parsedDate;
					break;
				}
				await CS.Out.WriteLineAsync("Date format not recognized");

			} while (true);

			return date;
		}
	}
}
