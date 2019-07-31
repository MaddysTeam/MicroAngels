﻿namespace MicroAngels.Cache
{
    using System;

    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class CachingAttribute : Attribute
    {
        public int AbsoluteExpiration { get; set; } = 30;

		public ActionType ActionType { get; set; } = ActionType.search;

		//add other settings ...
	}

	public enum ActionType
	{
		edit,search
	}
}
