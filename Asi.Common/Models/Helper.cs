using System.Net.Mail;

namespace Asi.Common.Models
{
	public static class Helper
	{		
		public static bool ValidateAndFix(this Contact c, AsiCommonOptions options)
		{
			if (c.BirthDate.HasValue && c.BirthDate >= DateTime.Now)
			{
				return false;
			}

			if (c.Emails.Count > 0)
			{
				if (c.Emails.Any(x => x.IsPrimary > 1 && x.IsPrimary < 0))
				{
					return false;
				}

				if (c.Emails.Count(x => x.IsPrimary == 1) != 1)
				{
					if (!options.AutoSetPrimaryEmail)
					{
						return false;
					}

					for (int i = 0; i < c.Emails.Count; i++)
					{
						c.Emails.ElementAt(i).IsPrimary = i == 0 ? 1 : 0;
					}
				}

				if (options.ValidateEmail && c.Emails.Any(x => !IsValidEmail(x.Address)))
				{
					return false;
				}
			}

			return true;
		}

		public static bool IsValidEmail(string address)
		{
			return MailAddress.TryCreate(address, out _);
		}
	}
}
