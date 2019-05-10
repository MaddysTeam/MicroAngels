
/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2018/11/12 22:50:27                          */
/*==============================================================*/


drop table if exists OcelotConfigReRoutes;

drop table if exists OcelotGlobalConfiguration;

drop table if exists OcelotReRoute;

drop table if exists OcelotReRoutesItem;

/*==============================================================*/
/* Table: OcelotConfigReRoutes                                    */
/*==============================================================*/
create table OcelotConfigReRoutes
(
   CtgRouteId           int not null auto_increment comment '配置路由主键',
   OcelotId               int comment '网关主键',
   ReRouteId            int comment '路由主键',
   primary key (CtgRouteId)
);

alter table OcelotConfigReRoutes comment '网关-路由,可以配置多个网关和多个路由';

/*==============================================================*/
/* Table: OcelotGlobalConfiguration                               */
/*==============================================================*/
create table OcelotGlobalConfiguration
(
   OcelotId               int not null auto_increment comment '网关主键',
   GatewayName          varchar(100) not null comment '网关名称',
   RequestIdKey         varchar(100) comment '全局请求默认key',
   BaseUrl              varchar(100) comment '请求路由根地址',
   DownstreamScheme     varchar(50) comment '下游使用架构',
   ServiceDiscoveryProvider varchar(500) comment '服务发现全局配置,值为配置json',
   LoadBalancerOptions  varchar(500) comment '全局负载均衡配置',
   HttpHandlerOptions   varchar(500) comment 'Http请求配置',
   QoSOptions           varchar(200) comment '请求安全配置,超时、重试、熔断',
   IsDefault            int not null default 0 comment '是否默认配置, 1 默认 0 默认',
   InfoStatus           int not null default 1 comment '当前状态, 1 有效 0 无效',
   primary key (OcelotId)
);

alter table OcelotGlobalConfiguration comment '网关全局配置表';

/*==============================================================*/
/* Table: OcelotReRoute                                           */
/*==============================================================*/
create table OcelotReRoute
(
   ReRouteId            int not null auto_increment comment '路由主键',
   ItemId               int comment '分类主键',
   UpstreamPathTemplate varchar(150) not null comment '上游路径模板，支持正则',
   UpstreamHttpMethod   varchar(50) not null comment '上游请求方法数组格式',
   UpstreamHost         varchar(100) comment '上游域名地址',
   DownstreamScheme     varchar(50) not null comment '下游使用架构',
   DownstreamPathTemplate varchar(200) not null comment '下游路径模板,与上游正则对应',
   DownstreamHostAndPorts varchar(500) comment '下游请求地址和端口,静态负载配置',
   AuthenticationOptions varchar(300) comment '授权配置,是否需要认证访问',
   RequestIdKey         varchar(100) comment '全局请求默认key',
   CacheOptions         varchar(200) comment '缓存配置,常用查询和再次配置缓存',
   ServiceName          varchar(100) comment '服务发现名称,启用服务发现时生效',
   LoadBalancerOptions  varchar(500) comment '全局负载均衡配置',
   QoSOptions           varchar(200) comment '请求安全配置,超时、重试、熔断',
   DelegatingHandlers   varchar(200) comment '委托处理方法,特定路由定义委托单独处理',
   Priority             int comment '路由优先级,多个路由匹配上，优先级高的先执行',
   InfoStatus           int not null default 1 comment '当前状态, 1 有效 0 无效',
   primary key (ReRouteId)
);

alter table OcelotReRoute comment '路由配置表';

/*==============================================================*/
/* Table: OcelotReRoutesItem                                      */
/*==============================================================*/
create table OcelotReRoutesItem
(
   ItemId               int not null auto_increment comment '分类主键',
   ItemName             varchar(100) not null comment '分类名称',
   ItemDetail           varchar(500) comment '分类描述',
   ItemParentId         int comment '上级分类,顶级节点为空',
   InfoStatus           int not null default 1 comment '当前状态, 1 有效 0 无效',
   primary key (ItemId)
);

alter table OcelotReRoutesItem comment '路由分类表';

alter table OcelotConfigReRoutes add constraint FK_Relationship_4 foreign key (OcelotId)
      references OcelotGlobalConfiguration (OcelotId) on delete restrict on update restrict;

alter table OcelotConfigReRoutes add constraint FK_Relationship_5 foreign key (ReRouteId)
      references OcelotReRoute (ReRouteId) on delete restrict on update restrict;

alter table OcelotReRoute add constraint FK_分类路由信息 foreign key (ItemId)
      references OcelotReRoutesItem (ItemId) on delete restrict on update restrict;

/*insert demo data */

--插入全局测试信息
insert into OcelotGlobalConfiguration(GatewayName,RequestIdKey,ServiceDiscoveryProvider,IsDefault,InfoStatus)
values('demo网关','OcRequestId','{"Provider":"Consul","Host": "192.168.1.9", "Port": 8500}',1,1);

--插入路由分类测试信息
insert into OcelotReRoutesItem(ItemName,InfoStatus) values('demo_category',1);

--插入路由测试信息 
insert into OcelotReRoute values(1,NULL,'/api/messagecenter/message/{everything}','[ "GET" ]','','http','/api/message/{everything}','[{"Host": "localhost","Port": 5003 }]',
'{ "AuthenticationProviderKey": "MessageServiceKey","AllowedScopes": []}','','','MessageCenter','{"Type": "RoundRobin"}','','',0,1);

--插入网关关联表
insert into OcelotConfigReRoutes values(1,1);