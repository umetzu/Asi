namespace Asi.Common.Models
{
	public class Email
	{
		public long Id { get; set; }
		public long IsPrimary { get; set; } = 0;
		public string Address { get; set; } = "";
		public long ContactId { get; set; }
		public virtual Contact? Contact { get; set; }
    }
}
