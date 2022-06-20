using System.ComponentModel.DataAnnotations;

namespace EcAPI.Entities
{
	public class Contents
	{
		[Key]
		public int ContentId { get; set; }
		public int PageId { get; set; }
		public int SectionId { get; set; }
		public string? Content1 { get; set; }
		public string? Content2 { get; set; }
		public string? Content3 { get; set; }
		public string? Content4 { get; set; }
		public string? Content5 { get; set; }
		public string? List1 { get; set; }
		public string? List2 { get; set; }
		public string? List3 { get; set; }
		public string? List4 { get; set; }
		public string? FilePath1 { get; set; }
		public string? FilePath2 { get; set; }
	}
}
