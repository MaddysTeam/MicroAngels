namespace MicroAngels.IdentityServer.Providers.MySql
{

	internal static class Sqls
	{
		internal static string Client = @"select * from Clients where ClientId=@client and Enabled=1;
               select t3.* from Clients t1 inner join ClientSecrets t3 on t1.Id=t3.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientGrantTypes t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientScopes t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               select t2.* from Clients t1 inner join ClientRedirectUris t2 on t1.Id=t2.ClientId where t1.ClientId=@client and Enabled=1;
               ";

		internal static string ApiResource = @"select * from ApiResources where Name=@Name and Enabled=1;
                       select * from ApiResources t1 inner join ApiScopes t2 on t1.Id=t2.ApiResourceId where t1.Name=@name and Enabled=1;
                       select * from ApiResources t1 inner join ApiSecrets t2 on t1.Id=t2.ApiResourceId where t1.Name=@name and Enabled=1";
		internal static string ApiResourceWithScopes = @"select distinct t1.* from ApiResources t1 inner join ApiScopes t2 on t1.Id=t2.ApiResourceId where t2.Name in({0}) and Enabled=1;";
		internal static string ApiResourceWithScopesByResourceId = @"select * from ApiScopes where ApiResourceId=@id;
                                                                     select * from ApiSecrets where ApiResourceId=@id";
		internal static string EnabledApiResources = @"select * from ApiResources where Enabled=1";
		internal static string EvanledIdentityResources = @"select * from IdentityResources where Enabled=1";

		internal static string PersistedGrantsBySubjectId = @"select * from PersistedGrants where SubjectId=@subjectId";
		internal static string PersistedGrantsByKey = @"select * from PersistedGrants where `Key`=@key";
		internal static string DeletePersistedGrants = @"delete from PersistedGrants where ClientId=@clientId and SubjectId=@subjectId";
		internal static string DeletePersistedGrants2 = @"delete from PersistedGrants where ClientId=@clientId and SubjectId=@subjectId";
		internal static string DeletePersistedGrantsBykey = @"delete from PersistedGrants where `Key`=@key";
		internal static string InsertPersisedGrants = @"insert into PersistedGrants(`Key`,ClientId,CreationTime,Data,Expiration,SubjectId,Type) values(@Key,@ClientId,@CreationTime,@Data,@Expiration,@SubjectId,@Type)";

	}

}
