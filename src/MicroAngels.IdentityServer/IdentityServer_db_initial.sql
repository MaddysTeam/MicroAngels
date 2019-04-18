CREATE TABLE ApiResources (
    Id int NOT NULL auto_increment,
    `Description` varcharacter(1000) NULL,
    DisplayName varcharacter(200) NULL,
    Enabled bit NOT NULL,
    `Name` varcharacter(200) NOT NULL,
    CONSTRAINT PK_ApiResources PRIMARY KEY (Id)
);


CREATE TABLE Clients (
    Id int NOT NULL auto_increment,
    AbsoluteRefreshTokenLifetime int NOT NULL,
    AccessTokenLifetime int NOT NULL,
    AccessTokenType int NOT NULL,
    AllowAccessTokensViaBrowser bit NOT NULL,
    AllowOfflineAccess bit NOT NULL,
    AllowPlainTextPkce bit NOT NULL,
    AllowRememberConsent bit NOT NULL,
    AlwaysIncludeUserClaimsInIdToken bit NOT NULL,
    AlwaysSendClientClaims bit NOT NULL,
    AuthorizationCodeLifetime int NOT NULL,
    BackChannelLogoutSessionRequired bit NOT NULL,
    BackChannelLogoutUri varcharacter(2000) NULL,
    ClientClaimsPrefix varcharacter(200) NULL,
    ClientId varcharacter(200) NOT NULL,
    ClientName varcharacter(200) NULL,
    ClientUri varcharacter(2000) NULL,
    ConsentLifetime int NULL,
    `Description` varcharacter(1000) NULL,
    EnableLocalLogin bit NOT NULL,
    Enabled bit NOT NULL,
    FrontChannelLogoutSessionRequired bit NOT NULL,
    FrontChannelLogoutUri varcharacter(2000) NULL,
    IdentityTokenLifetime int NOT NULL,
    IncludeJwtId bit NOT NULL,
    LogoUri varcharacter(2000) NULL,
    PairWiseSubjectSalt varcharacter(200) NULL,
    ProtocolType varcharacter(200) NOT NULL,
    RefreshTokenExpiration int NOT NULL,
    RefreshTokenUsage int NOT NULL,
    RequireClientSecret bit NOT NULL,
    RequireConsent bit NOT NULL,
    RequirePkce bit NOT NULL,
    SlidingRefreshTokenLifetime int NOT NULL,
    UpdateAccessTokenClaimsOnRefresh bit NOT NULL,
    CONSTRAINT PK_Clients PRIMARY KEY (Id)
);

CREATE TABLE IdentityResources (
    Id int NOT NULL auto_increment,
    `Description` varcharacter(1000) NULL,
    DisplayName varcharacter(200) NULL,
    Emphasize bit NOT NULL,
    Enabled bit NOT NULL,
    `Name` varcharacter(200) NOT NULL,
    Required bit NOT NULL,
    ShowInDiscoveryDocument bit NOT NULL,
    CONSTRAINT PK_IdentityResources PRIMARY KEY (Id)
);

CREATE TABLE ApiClaims (
    Id int NOT NULL auto_increment,
    ApiResourceId int NOT NULL,
    `Type` varcharacter(200) NOT NULL,
    CONSTRAINT PK_ApiClaims PRIMARY KEY (Id)
);

CREATE TABLE ApiScopes (
    Id int NOT NULL auto_increment,
    ApiResourceId int NOT NULL,
    `Description` varcharacter(1000) NULL,
    DisplayName varcharacter(200) NULL,
    Emphasize bit NOT NULL,
    `Name` varcharacter(200) NOT NULL,
    Required bit NOT NULL,
    ShowInDiscoveryDocument bit NOT NULL,
    CONSTRAINT PK_ApiScopes PRIMARY KEY (Id)
);

CREATE TABLE ApiSecrets (
    Id int NOT NULL auto_increment,
    ApiResourceId int NOT NULL,
    `Description` varcharacter(1000) NULL,
    Expiration timestamp NULL,
    `Type` varcharacter(250) NULL,
    `Value` varcharacter(2000) NULL,
    CONSTRAINT PK_ApiSecrets PRIMARY KEY (Id)
);

CREATE TABLE ClientClaims (
    Id int NOT NULL auto_increment,
    ClientId int NOT NULL,
    Type varcharacter(250) NOT NULL,
    Value varcharacter(250) NOT NULL,
    CONSTRAINT PK_ClientClaims PRIMARY KEY (Id)
);


CREATE TABLE ClientCorsOrigins (
    Id int NOT NULL auto_increment,
    ClientId int NOT NULL,
    Origin varcharacter(150) NOT NULL,
    CONSTRAINT PK_ClientCorsOrigins PRIMARY KEY (Id)
);



CREATE TABLE ClientGrantTypes (
    Id int NOT NULL auto_increment,
    ClientId int NOT NULL,
    GrantType varcharacter(250) NOT NULL,
    CONSTRAINT PK_ClientGrantTypes PRIMARY KEY (Id)
);



CREATE TABLE ClientIdPRestrictions (
    Id int NOT NULL auto_increment,
    ClientId int NOT NULL,
    Provider varcharacter(200) NOT NULL,
    CONSTRAINT PK_ClientIdPRestrictions PRIMARY KEY (Id)
);


CREATE TABLE ClientPostLogoutRedirectUris (
    Id int NOT NULL auto_increment,
    ClientId int NOT NULL,
    PostLogoutRedirectUri varcharacter(2000) NOT NULL,
    CONSTRAINT PK_ClientPostLoutRedirectUris PRIMARY KEY (Id)
);



CREATE TABLE ClientProperties (
    Id int NOT NULL auto_increment,
    ClientId int NOT NULL,
    `Key` varcharacter(250) NOT NULL,
    `Value` varcharacter(2000) NOT NULL,
    CONSTRAINT PK_ClientProperties PRIMARY KEY (Id)
);



CREATE TABLE ClientRedirectUris (
    Id int NOT NULL auto_increment,
    ClientId int NOT NULL,
    RedirectUri varcharacter(2000) NOT NULL,
    CONSTRAINT PK_ClientRedirectUris PRIMARY KEY (Id)
);



CREATE TABLE ClientScopes (
    Id int NOT NULL auto_increment,
    ClientId int NOT NULL,
    Scope varcharacter(200) NOT NULL,
    CONSTRAINT PK_ClientScopes PRIMARY KEY (Id)
);



CREATE TABLE ClientSecrets (
    Id int NOT NULL auto_increment,
    ClientId int NOT NULL,
    `Description` varcharacter(2000) NULL,
    Expiration timestamp NULL,
    `Type` varcharacter(250) NULL,
    `Value` varcharacter(2000) NOT NULL,
    CONSTRAINT PK_ClientSecrets PRIMARY KEY (Id)
);



CREATE TABLE IdentityClaims (
    Id int NOT NULL auto_increment,
    IdentityResourceId int NOT NULL,
    `Type` varcharacter(200) NOT NULL,
    CONSTRAINT PK_auto_incrementClaims PRIMARY KEY (Id)
);



CREATE TABLE ApiScopeClaims (
    Id int NOT NULL auto_increment,
    ApiScopeId int NOT NULL,
    `Type` varcharacter(200) NOT NULL,
    CONSTRAINT PK_ApiScopeClaims PRIMARY KEY (Id)
);

CREATE TABLE PersistedGrant(
 Id int NOT NULL auto_increment,
 ClientId int NOT NULL,
 `Type` varcharacter(250) NULL,
 SubjectId int NOT NULL,
 Expiration timestamp NULL,
 CreationTime timestamp NOT NULL,
 `Data` varcharacter(2000) NOT NULL,
);