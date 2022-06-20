using System.ComponentModel.DataAnnotations;

namespace EcAPI.Entities
{
	public class Account
	{
		[Key]
		public int UserId { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? Password { get; set; }
		public string? Role { get; set; }
	}
}
