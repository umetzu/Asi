namespace Asi.Common.Models
{
	public class Contact
	{
        public long Id { get; set; }
        public string Name { get; set; } = "";
        public DateTime? BirthDate { get; set; }
        public virtual ICollection<Email> Emails { get; set; } = new List<Email>();
    }
}
