using MicroAngels.Core;

namespace Business
{

	public class ChangePasswordViewModel
	{
		public string UserId { get; set; }
		public string UserName { get; set; }
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }

		public bool IsValidate =>!UserId.IsNullOrEmpty()
			&& !UserId.ToGuid().IsEmpty() 
			&& !OldPassword.IsNullOrEmpty() 
			&& !NewPassword.IsNullOrEmpty();
	}

}
