using EcAPI.Entity;

namespace EcAPI.Model
{
	public class ResponseModel
	{
		public dynamic Response { get; set; }
		public int StatusCode { get; set; }
		public string Message { get; set; }
		public string Token { get; set; }

	}
}
