using Dapper;
using IdentityServer4.Models;
using MicroAngels.IdentityServer.Models;
using MicroAngels.Logger;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroAngels.IdentityServer.Providers.MySql
{

	public class MySqlGrantStoreProvider : IGrantStoreProvider
    {

        public MySqlGrantStoreProvider(ILogger logger, MySqlStoreOptions storeOptions)
        {
            _logger = logger;
            _storeOptions = storeOptions;
        }

        /// <summary>
        /// 根据用户标识获取所有的授权信息
        /// </summary>
        /// <param name="subjectId">用户标识</param>
        /// <returns></returns>
        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
            {
				string sql = Sqls.PersistedGrantsBySubjectId;// "select * from PersistedGrants where SubjectId=@subjectId";
                var data = (await connection.QueryAsync<IdentityPersistedGrant>(sql, new { subjectId }))?.AsList();
                var model = data.Select(x => x.Map());

                _logger.Info($"{data.Count} persisted grants found for {subjectId}");
                return model;
            }
        }

        /// <summary>
        /// 根据key获取授权信息
        /// </summary>
        /// <param name="key">认证信息</param>
        /// <returns></returns>
        public async Task<PersistedGrant> GetAsync(string key)
        {
            using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
            {
				string sql = Sqls.PersistedGrantsByKey;// "select * from PersistedGrants where `Key`=@key";
                var result = await connection.QueryFirstOrDefaultAsync<IdentityPersistedGrant>(sql, new { key });
                var model = result.Map();

                _logger.Info($"{key} found in database: {model != null}");
                return model;
            }
        }

        /// <summary>
        /// 根据用户标识和客户端ID移除所有的授权信息
        /// </summary>
        /// <param name="subjectId">用户标识</param>
        /// <param name="clientId">客户端ID</param>
        /// <returns></returns>
        public async Task RemoveAllAsync(string subjectId, string clientId)
        {
            using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
            {
				string sql = Sqls.DeletePersistedGrants;// "delete from PersistedGrants where ClientId=@clientId and SubjectId=@subjectId";
                await connection.ExecuteAsync(sql, new { subjectId, clientId });
                _logger.Info($"remove {subjectId} {clientId} from database success");
            }
        }

        /// <summary>
        /// 移除指定的标识、客户端、类型等授权信息
        /// </summary>
        /// <param name="subjectId">标识</param>
        /// <param name="clientId">客户端ID</param>
        /// <param name="type">授权类型</param>
        /// <returns></returns>
        public async Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
            {
				string sql = Sqls.DeletePersistedGrants2;// "delete from PersistedGrants where ClientId=@clientId and SubjectId=@subjectId and Type=@type";
                await connection.ExecuteAsync(sql, new { subjectId, clientId });
                _logger.Info($"remove {subjectId} {clientId} {type} from database success");
            }
        }

        /// <summary>
        /// 移除指定KEY的授权信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public async Task RemoveAsync(string key)
        {
            using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
            {
				string sql = Sqls.DeletePersistedGrantsBykey;// "delete from PersistedGrants where `Key`=@key";
                await connection.ExecuteAsync(sql, new { key });
                _logger.Info($"remove {key} from database success");
            }
        }

        /// <summary>
        /// 存储授权信息
        /// </summary>
        /// <param name="grant">实体</param>
        /// <returns></returns>
        public async Task StoreAsync(PersistedGrant grant)
        {
			using (var connection = new MySqlConnection(_storeOptions.ConnectionStrings))
			{
				//移除防止重复
				await RemoveAsync(grant.Key);
				string sql = Sqls.InsertPersisedGrants;// "insert into PersistedGrants(`Key`,ClientId,CreationTime,Data,Expiration,SubjectId,Type) values(@Key,@ClientId,@CreationTime,@Data,@Expiration,@SubjectId,@Type)";
				await connection.ExecuteAsync(sql, grant);
			}
		}

        private readonly ILogger _logger;

        private readonly MySqlStoreOptions _storeOptions;

    }

}
