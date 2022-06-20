using System.ComponentModel.DataAnnotations;

namespace EcAPI.Entities
{
	public class ContentConfig
	{
		[Key]
	  public int? PageId { get; set; }
      public string? PageName { get; set; }
	}
}
