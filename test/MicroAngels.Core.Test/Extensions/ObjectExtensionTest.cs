using MicroAngels.Core;
using Xunit;

namespace MicroAngels.Core.Test
{

	public class ObjectExtensionTest
	{

		public ObjectExtensionTest()
		{
		}

		[Fact]
		public void CheckNullTest()
		{
			Assert.True(TestKeys.NullValue.IsNull());
			Assert.False(TestKeys.AnonymousValue.IsNull());
		}

		[Fact]
		public void EnsureNullTest()
		{
			Assert.Throws<AngleExceptions>(() => {
				TestKeys.AnonymousValue.EnsureNotNull(() => new AngleExceptions("throw"));
			});
		}

		[Fact]
		public void ToJsonTset()
		{
			var json = TestKeys.AnonymousValue.ToJson();
			Assert.NotNull(json);
		}

	}

}
