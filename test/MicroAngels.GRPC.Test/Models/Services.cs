using MagicOnion;

namespace MicroAngels.Core.Test.Models
{

	public interface IGrpcService: IService<IGrpcService>
	{
		UnaryResult<string> Execute();
	}

}
