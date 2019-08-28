namespace MicroAngels.Cache
{
	using System;

	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class CachingAttribute : Attribute
	{
		public int AbsoluteExpiration { get; set; } = 30;

		public ActionType ActionType { get; set; } = ActionType.search;

		public string[] DeleteKeys { get; set; }

		public bool IsAsync { get; set; }

		public Type TargetType { get; set; }
	}

	public enum ActionType
	{
		edit, search
	}
}
