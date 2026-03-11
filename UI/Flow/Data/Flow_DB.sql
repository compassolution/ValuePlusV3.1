/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2005                    */
/* Created on:     2013/4/18 14:02:24                           */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_ENTITY') and o.name = 'FK_Reference_26')
alter table TB_FLOW_ENTITY
   drop constraint FK_Reference_26
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_PATH') and o.name = 'FK_Reference_13')
alter table TB_FLOW_PATH
   drop constraint FK_Reference_13
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_POST_ACTION') and o.name = 'FK_Reference_17')
alter table TB_FLOW_POST_ACTION
   drop constraint FK_Reference_17
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_POST_ACTOR') and o.name = 'FK_Reference_19')
alter table TB_FLOW_POST_ACTOR
   drop constraint FK_Reference_19
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_POST_DEFINE') and o.name = 'FK_Reference_16')
alter table TB_FLOW_POST_DEFINE
   drop constraint FK_Reference_16
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_POST_GROUP_DETAIL') and o.name = 'FK_Reference_30')
alter table TB_FLOW_POST_GROUP_DETAIL
   drop constraint FK_Reference_30
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_POST_GROUP_DETAIL') and o.name = 'FK_Reference_31')
alter table TB_FLOW_POST_GROUP_DETAIL
   drop constraint FK_Reference_31
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_RESERVED_MEMO') and o.name = 'FK_Reference_14')
alter table TB_FLOW_RESERVED_MEMO
   drop constraint FK_Reference_14
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_WORKFLOW_DETAIL') and o.name = 'FK_Reference_18')
alter table TB_FLOW_WORKFLOW_DETAIL
   drop constraint FK_Reference_18
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_WORKFLOW_DETAIL') and o.name = 'FK_Reference_22')
alter table TB_FLOW_WORKFLOW_DETAIL
   drop constraint FK_Reference_22
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_WORKFLOW_DETAIL') and o.name = 'FK_Reference_36')
alter table TB_FLOW_WORKFLOW_DETAIL
   drop constraint FK_Reference_36
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_WORKFLOW_DETAIL') and o.name = 'FK_Reference_37')
alter table TB_FLOW_WORKFLOW_DETAIL
   drop constraint FK_Reference_37
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_WORK_INSTANCE') and o.name = 'FK_Reference_15')
alter table TB_FLOW_WORK_INSTANCE
   drop constraint FK_Reference_15
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_WORK_INSTANCE') and o.name = 'FK_Reference_20')
alter table TB_FLOW_WORK_INSTANCE
   drop constraint FK_Reference_20
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FLOW_WORK_INSTANCE') and o.name = 'FK_Reference_21')
alter table TB_FLOW_WORK_INSTANCE
   drop constraint FK_Reference_21
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FORM_FIELD') and o.name = 'FK_Reference_25')
alter table TB_FORM_FIELD
   drop constraint FK_Reference_25
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FORM_FIELD') and o.name = 'FK_Reference_27')
alter table TB_FORM_FIELD
   drop constraint FK_Reference_27
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FORM_FIELD') and o.name = 'FK_Reference_28')
alter table TB_FORM_FIELD
   drop constraint FK_Reference_28
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FORM_FIELD') and o.name = 'FK_Reference_29')
alter table TB_FORM_FIELD
   drop constraint FK_Reference_29
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FORM_POST_RIGHT') and o.name = 'FK_Reference_32')
alter table TB_FORM_POST_RIGHT
   drop constraint FK_Reference_32
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FORM_POST_RIGHT') and o.name = 'FK_Reference_33')
alter table TB_FORM_POST_RIGHT
   drop constraint FK_Reference_33
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FORM_POST_RIGHT') and o.name = 'FK_Reference_34')
alter table TB_FORM_POST_RIGHT
   drop constraint FK_Reference_34
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_FORM_POST_RIGHT') and o.name = 'FK_Reference_35')
alter table TB_FORM_POST_RIGHT
   drop constraint FK_Reference_35
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_DIC_FLOW_MOVE_FLAG')
            and   type = 'U')
   drop table TB_DIC_FLOW_MOVE_FLAG
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_DIC_FLOW_STATUS')
            and   type = 'U')
   drop table TB_DIC_FLOW_STATUS
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_DIC_FORM_CTRL_RIGHT_TYPE')
            and   type = 'U')
   drop table TB_DIC_FORM_CTRL_RIGHT_TYPE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_DIC_FORM_CTRL_TYPE')
            and   type = 'U')
   drop table TB_DIC_FORM_CTRL_TYPE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_DIC_FORM_FIELD_TYPE')
            and   type = 'U')
   drop table TB_DIC_FORM_FIELD_TYPE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TB_FLOW_DEFINE')
            and   name  = 'index_FLOW_DEFINE'
            and   indid > 0
            and   indid < 255)
   drop index TB_FLOW_DEFINE.index_FLOW_DEFINE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_DEFINE')
            and   type = 'U')
   drop table TB_FLOW_DEFINE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_ENTITY')
            and   type = 'U')
   drop table TB_FLOW_ENTITY
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TB_FLOW_PATH')
            and   name  = 'index_FLOW_WAY_FF'
            and   indid > 0
            and   indid < 255)
   drop index TB_FLOW_PATH.index_FLOW_WAY_FF
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_PATH')
            and   type = 'U')
   drop table TB_FLOW_PATH
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TB_FLOW_POST_ACTION')
            and   name  = 'index_FLOW_ACTION_PB'
            and   indid > 0
            and   indid < 255)
   drop index TB_FLOW_POST_ACTION.index_FLOW_ACTION_PB
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_POST_ACTION')
            and   type = 'U')
   drop table TB_FLOW_POST_ACTION
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TB_FLOW_POST_ACTOR')
            and   name  = 'index_POST_ACTOR_PU'
            and   indid > 0
            and   indid < 255)
   drop index TB_FLOW_POST_ACTOR.index_POST_ACTOR_PU
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TB_FLOW_POST_ACTOR')
            and   name  = 'index_POST_ACTOR_UP'
            and   indid > 0
            and   indid < 255)
   drop index TB_FLOW_POST_ACTOR.index_POST_ACTOR_UP
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_POST_ACTOR')
            and   type = 'U')
   drop table TB_FLOW_POST_ACTOR
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TB_FLOW_POST_DEFINE')
            and   name  = 'index_FLOW_POST_FP'
            and   indid > 0
            and   indid < 255)
   drop index TB_FLOW_POST_DEFINE.index_FLOW_POST_FP
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_POST_DEFINE')
            and   type = 'U')
   drop table TB_FLOW_POST_DEFINE
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TB_FLOW_POST_GROUP')
            and   name  = 'index_FLOW_POST_FP'
            and   indid > 0
            and   indid < 255)
   drop index TB_FLOW_POST_GROUP.index_FLOW_POST_FP
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_POST_GROUP')
            and   type = 'U')
   drop table TB_FLOW_POST_GROUP
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TB_FLOW_POST_GROUP_DETAIL')
            and   name  = 'index_FLOW_POSTGD'
            and   indid > 0
            and   indid < 255)
   drop index TB_FLOW_POST_GROUP_DETAIL.index_FLOW_POSTGD
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_POST_GROUP_DETAIL')
            and   type = 'U')
   drop table TB_FLOW_POST_GROUP_DETAIL
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_RESERVED_MEMO')
            and   type = 'U')
   drop table TB_FLOW_RESERVED_MEMO
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TB_FLOW_WORKFLOW_DETAIL')
            and   name  = 'index_FLOW_DETAIL'
            and   indid > 0
            and   indid < 255)
   drop index TB_FLOW_WORKFLOW_DETAIL.index_FLOW_DETAIL
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_WORKFLOW_DETAIL')
            and   type = 'U')
   drop table TB_FLOW_WORKFLOW_DETAIL
go

if exists (select 1
            from  sysindexes
           where  id    = object_id('TB_FLOW_WORK_INSTANCE')
            and   name  = 'index_FLOW_WORK'
            and   indid > 0
            and   indid < 255)
   drop index TB_FLOW_WORK_INSTANCE.index_FLOW_WORK
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_WORK_INSTANCE')
            and   type = 'U')
   drop table TB_FLOW_WORK_INSTANCE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FLOW_YAER_WORK_DAY')
            and   type = 'U')
   drop table TB_FLOW_YAER_WORK_DAY
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FORM_DEFINE')
            and   type = 'U')
   drop table TB_FORM_DEFINE
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FORM_FIELD')
            and   type = 'U')
   drop table TB_FORM_FIELD
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_FORM_POST_RIGHT')
            and   type = 'U')
   drop table TB_FORM_POST_RIGHT
go

/*==============================================================*/
/* Table: TB_DIC_FLOW_MOVE_FLAG                                 */
/*==============================================================*/
create table TB_DIC_FLOW_MOVE_FLAG (
   SFLOWMOVECODE        CHAR(3)              not null,
   SFLOWMOVENAME        VARCHAR(50)          null,
   SFLOWMOVENAMECN      VARCHAR(50)          null,
   NINDEX               DECIMAL              null,
   BSTOP                CHAR(1)              not null,
   constraint PK_TB_DIC_FLOW_MOVE_FLAG primary key nonclustered (SFLOWMOVECODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：待移交
   002：已经移交但未接收
   003：已经接收',
   'user', @CurrentUser, 'table', 'TB_DIC_FLOW_MOVE_FLAG', 'column', 'SFLOWMOVECODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:在用
   1:停用',
   'user', @CurrentUser, 'table', 'TB_DIC_FLOW_MOVE_FLAG', 'column', 'BSTOP'
go

/*==============================================================*/
/* Table: TB_DIC_FLOW_STATUS                                    */
/*==============================================================*/
create table TB_DIC_FLOW_STATUS (
   SWKSCODE             CHAR(3)              not null,
   SWKSNAME             VARCHAR(50)          null,
   SWKSNAMECN           VARCHAR(50)          null,
   NINDEX               DECIMAL              null,
   BSTOP                CHAR(1)              not null,
   constraint PK_TB_DIC_FLOW_STATUS primary key nonclustered (SWKSCODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：正在处理
   002：等候代理人补办资料
   003：等候子流程处理
   004：流程挂起
   099：流程已经结束
   ',
   'user', @CurrentUser, 'table', 'TB_DIC_FLOW_STATUS', 'column', 'SWKSCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:在用
   1:停用',
   'user', @CurrentUser, 'table', 'TB_DIC_FLOW_STATUS', 'column', 'BSTOP'
go

/*==============================================================*/
/* Table: TB_DIC_FORM_CTRL_RIGHT_TYPE                           */
/*==============================================================*/
create table TB_DIC_FORM_CTRL_RIGHT_TYPE (
   SCTRLRIGHTTYPECODE   VARCHAR(20)          not null,
   SCTRLRIGHTTYPENAME   VARCHAR(50)          null,
   SCTRLRIGHTTYPENAMECN VARCHAR(50)          null,
   NINDEX               DECIMAL              null,
   BSTOP                CHAR(1)              not null,
   constraint PK_TB_DIC_FORM_CTRL_RIGHT_TYPE primary key nonclustered (SCTRLRIGHTTYPECODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：正在处理
   002：等候代理人补办资料
   003：等候子流程处理
   004：流程挂起
   099：流程已经结束
   ',
   'user', @CurrentUser, 'table', 'TB_DIC_FORM_CTRL_RIGHT_TYPE', 'column', 'SCTRLRIGHTTYPECODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:在用
   1:停用',
   'user', @CurrentUser, 'table', 'TB_DIC_FORM_CTRL_RIGHT_TYPE', 'column', 'BSTOP'
go

/*==============================================================*/
/* Table: TB_DIC_FORM_CTRL_TYPE                                 */
/*==============================================================*/
create table TB_DIC_FORM_CTRL_TYPE (
   SCTRLTYPECODE        VARCHAR(20)          not null,
   SCTRLTYPENAME        VARCHAR(50)          null,
   SCTRLTYPENAMECN      VARCHAR(50)          null,
   NINDEX               DECIMAL              null,
   BSTOP                CHAR(1)              not null,
   constraint PK_TB_DIC_FORM_CTRL_TYPE primary key nonclustered (SCTRLTYPECODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：正在处理
   002：等候代理人补办资料
   003：等候子流程处理
   004：流程挂起
   099：流程已经结束
   ',
   'user', @CurrentUser, 'table', 'TB_DIC_FORM_CTRL_TYPE', 'column', 'SCTRLTYPECODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:在用
   1:停用',
   'user', @CurrentUser, 'table', 'TB_DIC_FORM_CTRL_TYPE', 'column', 'BSTOP'
go

/*==============================================================*/
/* Table: TB_DIC_FORM_FIELD_TYPE                                */
/*==============================================================*/
create table TB_DIC_FORM_FIELD_TYPE (
   SFIELDTYPECODE       VARCHAR(20)          not null,
   SFIELDTYPENAME       VARCHAR(50)          null,
   SFIELDTYPENAMECN     VARCHAR(50)          null,
   NINDEX               DECIMAL              null,
   BSTOP                CHAR(1)              not null,
   constraint PK_TB_DIC_FORM_FIELD_TYPE primary key nonclustered (SFIELDTYPECODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：正在处理
   002：等候代理人补办资料
   003：等候子流程处理
   004：流程挂起
   099：流程已经结束
   ',
   'user', @CurrentUser, 'table', 'TB_DIC_FORM_FIELD_TYPE', 'column', 'SFIELDTYPECODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0:在用
   1:停用',
   'user', @CurrentUser, 'table', 'TB_DIC_FORM_FIELD_TYPE', 'column', 'BSTOP'
go

/*==============================================================*/
/* Table: TB_FLOW_DEFINE                                        */
/*==============================================================*/
create table TB_FLOW_DEFINE (
   SFLOWCODE            VARCHAR(40)          not null,
   SFLOWNAME            VARCHAR(100)         null,
   SFLOWNAMECN          VARCHAR(100)         null,
   SFLOWDESC            VARCHAR(500)         null,
   SFLOWDESCCN          VARCHAR(500)         null,
   NWORKCOUNTDAY        DECIMAL              null,
   BISNEEDACCEPT        CHAR(1)              null,
   BISACTIVEWITHSUB     CHAR(1)              null,
   BSTOP                CHAR(1)              null,
   constraint PK_TB_FLOW_DEFINE primary key nonclustered (SFLOWCODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_DEFINE', 'column', 'SFLOWCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '单位为天数
   ',
   'user', @CurrentUser, 'table', 'TB_FLOW_DEFINE', 'column', 'NWORKCOUNTDAY'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0不需要
   1需要
   （需要时，则下岗位需有接收及退回按钮）',
   'user', @CurrentUser, 'table', 'TB_FLOW_DEFINE', 'column', 'BISNEEDACCEPT'
go

/*==============================================================*/
/* Index: index_FLOW_DEFINE                                     */
/*==============================================================*/
create index index_FLOW_DEFINE on TB_FLOW_DEFINE (
SFLOWCODE ASC
)
go

/*==============================================================*/
/* Table: TB_FLOW_ENTITY                                        */
/*==============================================================*/
create table TB_FLOW_ENTITY (
   SENTITYID            VARCHAR(40)          not null,
   SFORMID              varchar(40)          null,
   SFORMKEYVALUE        varchar(40)          null,
   SENTITYNAME          VARCHAR(200)         null,
   SENTITYNAMECN        VARCHAR(200)         null,
   SENTITYDESC          VARCHAR(500)         null,
   SENTITYDESCCN        VARCHAR(500)         null,
   constraint PK_TB_FLOW_ENTITY primary key nonclustered (SENTITYID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '各种流程审批所审批的主体（实体）信息，标示主体的唯一性',
   'user', @CurrentUser, 'table', 'TB_FLOW_ENTITY'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_ENTITY', 'column', 'SENTITYID'
go

/*==============================================================*/
/* Table: TB_FLOW_PATH                                          */
/*==============================================================*/
create table TB_FLOW_PATH (
   SFLOWPATHCODE        VARCHAR(40)          not null,
   SFLOWCODE            VARCHAR(40)          null,
   SPOSTCODE_PRE        VARCHAR(40)          null,
   SPOSTCODE_NEXT       VARCHAR(40)          null,
   NWORKDAY             DECIMAL              null,
   SWAYDESC             VARCHAR(500)         null,
   SWAYDESCCN           VARCHAR(500)         null,
   SBIZSTATUS           VARCHAR(20)          null,
   NINDEX               DECIMAL              null,
   constraint PK_TB_FLOW_PATH primary key nonclustered (SFLOWPATHCODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_PATH', 'column', 'SFLOWPATHCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_PATH', 'column', 'SPOSTCODE_PRE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_PATH', 'column', 'SPOSTCODE_NEXT'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '单位工作日天数',
   'user', @CurrentUser, 'table', 'TB_FLOW_PATH', 'column', 'NWORKDAY'
go

/*==============================================================*/
/* Index: index_FLOW_WAY_FF                                     */
/*==============================================================*/
create index index_FLOW_WAY_FF on TB_FLOW_PATH (
SFLOWCODE ASC,
SPOSTCODE_PRE ASC
)
go

/*==============================================================*/
/* Table: TB_FLOW_POST_ACTION                                   */
/*==============================================================*/
create table TB_FLOW_POST_ACTION (
   SFLOWACTIONCODE      VARCHAR(40)          not null,
   SPOSTCODE            VARCHAR(40)          null,
   SFLOWACTIONNAME      VARCHAR(100)         null,
   SFLOWACTIONNAMECN    VARCHAR(100)         null,
   SACTIONDETAIL        VARCHAR(1000)        null,
   NINDEX               DECIMAL              null,
   SBIZSTATUS           VARCHAR(20)          null,
   SPLUGIN_PRE          VARCHAR(200)         null,
   SPLUGIN_AFTER        VARCHAR(200)         null,
   constraint PK_TB_FLOW_POST_ACTION primary key nonclustered (SFLOWACTIONCODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_POST_ACTION', 'column', 'SFLOWACTIONCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_POST_ACTION', 'column', 'SPOSTCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '参见再用流程',
   'user', @CurrentUser, 'table', 'TB_FLOW_POST_ACTION', 'column', 'SBIZSTATUS'
go

/*==============================================================*/
/* Index: index_FLOW_ACTION_PB                                  */
/*==============================================================*/
create index index_FLOW_ACTION_PB on TB_FLOW_POST_ACTION (
SPOSTCODE ASC,
SBIZSTATUS ASC
)
go

/*==============================================================*/
/* Table: TB_FLOW_POST_ACTOR                                    */
/*==============================================================*/
create table TB_FLOW_POST_ACTOR (
   SACTORID             VARCHAR(40)          not null,
   SPOSTCODE            VARCHAR(40)          null,
   SUSERID              VARCHAR(40)          null,
   SUSERNAME            VARCHAR(40)          null,
   SUSERNAMECN          VARCHAR(40)          null,
   SDEPTID              VARCHAR(40)          null,
   SDEPTNAME            VARCHAR(100)         null,
   SDEPTNAMECN          VARCHAR(100)         null,
   NINDEX               DECIMAL              null,
   constraint PK_TB_FLOW_POST_ACTOR primary key nonclustered (SACTORID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_POST_ACTOR', 'column', 'SACTORID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_POST_ACTOR', 'column', 'SPOSTCODE'
go

/*==============================================================*/
/* Index: index_POST_ACTOR_UP                                   */
/*==============================================================*/
create index index_POST_ACTOR_UP on TB_FLOW_POST_ACTOR (
SPOSTCODE ASC,
SUSERID ASC
)
go

/*==============================================================*/
/* Index: index_POST_ACTOR_PU                                   */
/*==============================================================*/
create index index_POST_ACTOR_PU on TB_FLOW_POST_ACTOR (
SPOSTCODE ASC,
SDEPTID ASC
)
go

/*==============================================================*/
/* Table: TB_FLOW_POST_DEFINE                                   */
/*==============================================================*/
create table TB_FLOW_POST_DEFINE (
   SPOSTCODE            VARCHAR(40)          not null,
   SFLOWCODE            VARCHAR(40)          null,
   SPOSTNAME            VARCHAR(100)         null,
   SPOSTNAMECN          VARCHAR(100)         null,
   NWORKDAY             DECIMAL              null,
   SPLUGIN_PRE          VARCHAR(200)         null,
   SPLUGIN_AFTER        VARCHAR(200)         null,
   BISSTARTPOST         CHAR(1)              null,
   constraint PK_TB_FLOW_POST_DEFINE primary key nonclustered (SPOSTCODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_POST_DEFINE', 'column', 'SPOSTCODE'
go

/*==============================================================*/
/* Index: index_FLOW_POST_FP                                    */
/*==============================================================*/
create index index_FLOW_POST_FP on TB_FLOW_POST_DEFINE (
SPOSTCODE ASC,
SFLOWCODE ASC
)
go

/*==============================================================*/
/* Table: TB_FLOW_POST_GROUP                                    */
/*==============================================================*/
create table TB_FLOW_POST_GROUP (
   SPOSTGROUPCODE       VARCHAR(40)          not null,
   SPOSTGROUPNAME       VARCHAR(100)         null,
   SPOSTGROUPNAMECN     VARCHAR(100)         null,
   constraint PK_TB_FLOW_POST_GROUP primary key nonclustered (SPOSTGROUPCODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_POST_GROUP', 'column', 'SPOSTGROUPCODE'
go

/*==============================================================*/
/* Index: index_FLOW_POST_FP                                    */
/*==============================================================*/
create index index_FLOW_POST_FP on TB_FLOW_POST_GROUP (
SPOSTGROUPCODE ASC
)
go

/*==============================================================*/
/* Table: TB_FLOW_POST_GROUP_DETAIL                             */
/*==============================================================*/
create table TB_FLOW_POST_GROUP_DETAIL (
   SPOSTGROUPCODE       VARCHAR(40)          null,
   SPOSTCODE            VARCHAR(40)          null
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_POST_GROUP_DETAIL', 'column', 'SPOSTGROUPCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_POST_GROUP_DETAIL', 'column', 'SPOSTCODE'
go

/*==============================================================*/
/* Index: index_FLOW_POSTGD                                     */
/*==============================================================*/
create index index_FLOW_POSTGD on TB_FLOW_POST_GROUP_DETAIL (
SPOSTGROUPCODE ASC,
SPOSTCODE ASC
)
go

/*==============================================================*/
/* Table: TB_FLOW_RESERVED_MEMO                                 */
/*==============================================================*/
create table TB_FLOW_RESERVED_MEMO (
   SRESERVEDCODE        VARCHAR(40)          not null,
   SFLOWPATHCODE        VARCHAR(40)          null,
   SRESERVEDMEMO        VARCHAR(200)         null,
   SRESERVEDMEMOCN      VARCHAR(200)         null,
   NINDEX               DECIMAL              null,
   constraint PK_TB_FLOW_RESERVED_MEMO primary key nonclustered (SRESERVEDCODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_RESERVED_MEMO', 'column', 'SRESERVEDCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_RESERVED_MEMO', 'column', 'SFLOWPATHCODE'
go

/*==============================================================*/
/* Table: TB_FLOW_WORKFLOW_DETAIL                               */
/*==============================================================*/
create table TB_FLOW_WORKFLOW_DETAIL (
   SWFDCODE             VARCHAR(40)          not null,
   SWORKFLOWCODE        VARCHAR(40)          null,
   NNUMBER              decimal              null,
   SSOURPOSTCODE        VARCHAR(40)          null,
   SSOURPOSTNAME        VARCHAR(100)         null,
   SSOURPOSTNAMECN      VARCHAR(100)         null,
   SSOURDEPTID          VARCHAR(40)          null,
   SSOURUSERID          VARCHAR(40)          null,
   SSOURUSERNAME        VARCHAR(50)          null,
   SSOURUSERNAMECN      VARCHAR(50)          null,
   DTSOURDATE           datetime             null,
   SDESTPOSTCODE        VARCHAR(40)          null,
   SDESTPOSTNAME        VARCHAR(100)         null,
   SDESTPOSTNAMECN      VARCHAR(100)         null,
   SDESTDEPTID          VARCHAR(40)          null,
   SDESTUSERID          VARCHAR(40)          null,
   SDESTUSERNAME        VARCHAR(50)          null,
   SDESTUSERNAMECN      VARCHAR(50)          null,
   DTDESTDATE           datetime             null,
   SMEMO                VARCHAR(500)         null,
   SFLOWPATHCODE        VARCHAR(40)          null,
   NMOVETOTALDAY        decimal              null,
   NWORKPROCESSDAY      decimal              null,
   BPROMPT              CHAR(1)              null,
   SBIZSTATUS           VARCHAR(20)          null,
   SWORKFLOWNAME        VARCHAR(200)         null,
   SWORKFLOWNAMECN      VARCHAR(200)         null,
   SFLOWCODE            VARCHAR(40)          null,
   SFLOWMOVECODE        CHAR(3)              null,
   SWKSCODE             CHAR(3)              null,
   SENTITYID            VARCHAR(40)          null,
   SENTITYNAME          VARCHAR(200)         null,
   SENTITYNAMECN        VARCHAR(200)         null,
   BISCURSTEP           CHAR(1)              null,
   constraint PK_TB_FLOW_WORKFLOW_DETAIL primary key nonclustered (SWFDCODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SWFDCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SSOURPOSTCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '冗余字段',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SSOURPOSTNAME'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '冗余字段',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SSOURPOSTNAMECN'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '三位一段,如市局001,分局为001001,所为001001001
   
   ',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SSOURDEPTID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '冗余字段',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SSOURUSERNAME'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '冗余字段',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SSOURUSERNAMECN'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SDESTPOSTCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '冗余字段',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SDESTPOSTNAME'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '冗余字段',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SDESTPOSTNAMECN'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '三位一段,如市局001,分局为001001,所为001001001
   
   ',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SDESTDEPTID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '冗余字段',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SDESTUSERNAME'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '冗余字段',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SDESTUSERNAMECN'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SFLOWPATHCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '移交人在系统中移交材料到接收人接收材料之间的总共时间',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'NMOVETOTALDAY'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：待移交
   002：已经移交但未接收
   003：已经接收',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SFLOWMOVECODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：正在处理
   002：等候代理人补办资料
   003：等候子流程处理
   004：流程挂起
   099：流程已经结束
   ',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SWKSCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '对应于流程的主体。
   可以是市场主体或其他对象',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORKFLOW_DETAIL', 'column', 'SENTITYID'
go

/*==============================================================*/
/* Index: index_FLOW_DETAIL                                     */
/*==============================================================*/
create index index_FLOW_DETAIL on TB_FLOW_WORKFLOW_DETAIL (
SWORKFLOWCODE ASC
)
go

/*==============================================================*/
/* Table: TB_FLOW_WORK_INSTANCE                                 */
/*==============================================================*/
create table TB_FLOW_WORK_INSTANCE (
   SWORKFLOWCODE        VARCHAR(40)          not null,
   SFLOWCODE            VARCHAR(40)          null,
   SWORKFLOWNAME        VARCHAR(200)         null,
   SWORKFLOWNAMECN      VARCHAR(200)         null,
   SENTITYID            VARCHAR(40)          null,
   SENTITYNAME          VARCHAR(200)         null,
   SENTITYNAMECN        VARCHAR(200)         null,
   SFLOWACCEPTNO        VARCHAR(40)          null,
   DTSTARTDATE          datetime             null,
   SUSERID              VARCHAR(40)          null,
   SDEPTID              VARCHAR(20)          null,
   SFLOWMOVECODE        CHAR(3)              null,
   SWKSCODE             CHAR(3)              null,
   NCOUNTWORKDAY        decimal              null,
   NSUBNUMBER           decimal              null,
   SBIZSTATUS           VARCHAR(20)          null,
   BISSUBFLOW           CHAR(1)              null,
   SPARENTCODE          VARCHAR(40)          null,
   SPENDINGUSERID       VARCHAR(40)          null,
   constraint PK_TB_FLOW_WORK_INSTANCE primary key nonclustered (SWORKFLOWCODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '冗余字段，同流程定义表的流程名称',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORK_INSTANCE', 'column', 'SWORKFLOWNAME'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '冗余字段，同流程定义表的流程名称',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORK_INSTANCE', 'column', 'SWORKFLOWNAMECN'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '对应于流程的主体。
   可以是市场主体或其他对象',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORK_INSTANCE', 'column', 'SENTITYID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '主体名称',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORK_INSTANCE', 'column', 'SENTITYNAME'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '主体名称',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORK_INSTANCE', 'column', 'SENTITYNAMECN'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '三位一段,如市局001,分局为001001,所为001001001
   
   ',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORK_INSTANCE', 'column', 'SDEPTID'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：待移交
   002：已经移交但未接收
   003：已经接收',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORK_INSTANCE', 'column', 'SFLOWMOVECODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：正在处理
   002：等候代理人补办资料
   003：等候子流程处理
   004：流程挂起
   099：流程已经结束
   ',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORK_INSTANCE', 'column', 'SWKSCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '流程已经处理全部时间（工作日）',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORK_INSTANCE', 'column', 'NCOUNTWORKDAY'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '0,默认为零， 比如
   1 立案
   2  不立案',
   'user', @CurrentUser, 'table', 'TB_FLOW_WORK_INSTANCE', 'column', 'SBIZSTATUS'
go

/*==============================================================*/
/* Index: index_FLOW_WORK                                       */
/*==============================================================*/
create index index_FLOW_WORK on TB_FLOW_WORK_INSTANCE (
SFLOWCODE ASC
)
go

/*==============================================================*/
/* Table: TB_FLOW_YAER_WORK_DAY                                 */
/*==============================================================*/
create table TB_FLOW_YAER_WORK_DAY (
   DTDATE               datetime             not null,
   constraint PK_TB_FLOW_YAER_WORK_DAY primary key nonclustered (DTDATE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '包含每年的工作日',
   'user', @CurrentUser, 'table', 'TB_FLOW_YAER_WORK_DAY'
go

/*==============================================================*/
/* Table: TB_FORM_DEFINE                                        */
/*==============================================================*/
create table TB_FORM_DEFINE (
   SFORMID              varchar(40)          not null,
   SFORMCODE            varchar(40)          null,
   SFORMNAME            varchar(200)         null,
   SFORMAMECN           varchar(200)         null,
   SFORMDESC            varchar(500)         null,
   SFORMDESCCN          varchar(500)         null,
   BISVERSION           char(1)              null,
   SPLUGINAFTERSVAE     varchar(500)         null,
   BISSTOP              char(1)              null,
   constraint PK_TB_FORM_DEFINE primary key (SFORMID)
)
go

/*==============================================================*/
/* Table: TB_FORM_FIELD                                         */
/*==============================================================*/
create table TB_FORM_FIELD (
   SFIELDID             varchar(40)          not null,
   SFORMID              varchar(40)          null,
   SFIELDCODE           varchar(50)          null,
   SFIELDNAME           varchar(50)          null,
   SFIELDNAMECN         varchar(50)          null,
   BISKEY               CHAR(1)              null,
   SFIELDTYPECODE       VARCHAR(20)          null,
   NFIELDLENGTH         decimal              null,
   SFIELDPRECISION      varchar(50)          null,
   BISNULL              CHAR(1)              null,
   SDEFAULTVALUE        varchar(50)          null,
   SCTRLTYPECODE        VARCHAR(20)          null,
   SCTRLDSSQL           varchar(1000)        null,
   NORDER               decimal              null,
   BISMAINVIEW          CHAR(1)              null,
   NCTRLLENGTH          decimal              null,
   BISMUST              CHAR(1)              null,
   STIPDESC             varchar(200)         null,
   STIPDESCCN           varchar(200)         null,
   SCTRLRIGHTTYPECODE   VARCHAR(20)          null,
   BISFKEY              CHAR(1)              null,
   SFKEYTABLE           varchar(50)          null,
   SFKEYFIELD           varchar(50)          null,
   constraint PK_TB_FORM_FIELD primary key (SFIELDID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：正在处理
   002：等候代理人补办资料
   003：等候子流程处理
   004：流程挂起
   099：流程已经结束
   ',
   'user', @CurrentUser, 'table', 'TB_FORM_FIELD', 'column', 'SFIELDTYPECODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：正在处理
   002：等候代理人补办资料
   003：等候子流程处理
   004：流程挂起
   099：流程已经结束
   ',
   'user', @CurrentUser, 'table', 'TB_FORM_FIELD', 'column', 'SCTRLTYPECODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：正在处理
   002：等候代理人补办资料
   003：等候子流程处理
   004：流程挂起
   099：流程已经结束
   ',
   'user', @CurrentUser, 'table', 'TB_FORM_FIELD', 'column', 'SCTRLRIGHTTYPECODE'
go

/*==============================================================*/
/* Table: TB_FORM_POST_RIGHT                                    */
/*==============================================================*/
create table TB_FORM_POST_RIGHT (
   SPFRKEYID            varchar(40)          not null,
   SPOSTCODE            VARCHAR(40)          null,
   SFORMID              varchar(40)          null,
   SFIELDID             varchar(40)          null,
   SCTRLRIGHTTYPECODE   VARCHAR(20)          null,
   constraint PK_TB_FORM_POST_RIGHT primary key (SPFRKEYID)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_FORM_POST_RIGHT', 'column', 'SPOSTCODE'
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '001：正在处理
   002：等候代理人补办资料
   003：等候子流程处理
   004：流程挂起
   099：流程已经结束
   ',
   'user', @CurrentUser, 'table', 'TB_FORM_POST_RIGHT', 'column', 'SCTRLRIGHTTYPECODE'
go

alter table TB_FLOW_ENTITY
   add constraint FK_Reference_26 foreign key (SFORMID)
      references TB_FORM_DEFINE (SFORMID)
go

alter table TB_FLOW_PATH
   add constraint FK_Reference_13 foreign key (SFLOWCODE)
      references TB_FLOW_DEFINE (SFLOWCODE)
go

alter table TB_FLOW_POST_ACTION
   add constraint FK_Reference_17 foreign key (SPOSTCODE)
      references TB_FLOW_POST_DEFINE (SPOSTCODE)
go

alter table TB_FLOW_POST_ACTOR
   add constraint FK_Reference_19 foreign key (SPOSTCODE)
      references TB_FLOW_POST_DEFINE (SPOSTCODE)
go

alter table TB_FLOW_POST_DEFINE
   add constraint FK_Reference_16 foreign key (SFLOWCODE)
      references TB_FLOW_DEFINE (SFLOWCODE)
go

alter table TB_FLOW_POST_GROUP_DETAIL
   add constraint FK_Reference_30 foreign key (SPOSTGROUPCODE)
      references TB_FLOW_POST_GROUP (SPOSTGROUPCODE)
go

alter table TB_FLOW_POST_GROUP_DETAIL
   add constraint FK_Reference_31 foreign key (SPOSTCODE)
      references TB_FLOW_POST_DEFINE (SPOSTCODE)
go

alter table TB_FLOW_RESERVED_MEMO
   add constraint FK_Reference_14 foreign key (SFLOWPATHCODE)
      references TB_FLOW_PATH (SFLOWPATHCODE)
go

alter table TB_FLOW_WORKFLOW_DETAIL
   add constraint FK_Reference_18 foreign key (SWORKFLOWCODE)
      references TB_FLOW_WORK_INSTANCE (SWORKFLOWCODE)
go

alter table TB_FLOW_WORKFLOW_DETAIL
   add constraint FK_Reference_22 foreign key (SFLOWMOVECODE)
      references TB_DIC_FLOW_MOVE_FLAG (SFLOWMOVECODE)
go

alter table TB_FLOW_WORKFLOW_DETAIL
   add constraint FK_Reference_36 foreign key (SWKSCODE)
      references TB_DIC_FLOW_STATUS (SWKSCODE)
go

alter table TB_FLOW_WORKFLOW_DETAIL
   add constraint FK_Reference_37 foreign key (SFLOWPATHCODE)
      references TB_FLOW_PATH (SFLOWPATHCODE)
go

alter table TB_FLOW_WORK_INSTANCE
   add constraint FK_Reference_15 foreign key (SFLOWCODE)
      references TB_FLOW_DEFINE (SFLOWCODE)
go

alter table TB_FLOW_WORK_INSTANCE
   add constraint FK_Reference_20 foreign key (SWKSCODE)
      references TB_DIC_FLOW_STATUS (SWKSCODE)
go

alter table TB_FLOW_WORK_INSTANCE
   add constraint FK_Reference_21 foreign key (SFLOWMOVECODE)
      references TB_DIC_FLOW_MOVE_FLAG (SFLOWMOVECODE)
go

alter table TB_FORM_FIELD
   add constraint FK_Reference_25 foreign key (SFORMID)
      references TB_FORM_DEFINE (SFORMID)
go

alter table TB_FORM_FIELD
   add constraint FK_Reference_27 foreign key (SFIELDTYPECODE)
      references TB_DIC_FORM_FIELD_TYPE (SFIELDTYPECODE)
go

alter table TB_FORM_FIELD
   add constraint FK_Reference_28 foreign key (SCTRLTYPECODE)
      references TB_DIC_FORM_CTRL_TYPE (SCTRLTYPECODE)
go

alter table TB_FORM_FIELD
   add constraint FK_Reference_29 foreign key (SCTRLRIGHTTYPECODE)
      references TB_DIC_FORM_CTRL_RIGHT_TYPE (SCTRLRIGHTTYPECODE)
go

alter table TB_FORM_POST_RIGHT
   add constraint FK_Reference_32 foreign key (SPOSTCODE)
      references TB_FLOW_POST_DEFINE (SPOSTCODE)
go

alter table TB_FORM_POST_RIGHT
   add constraint FK_Reference_33 foreign key (SFORMID)
      references TB_FORM_DEFINE (SFORMID)
go

alter table TB_FORM_POST_RIGHT
   add constraint FK_Reference_34 foreign key (SFIELDID)
      references TB_FORM_FIELD (SFIELDID)
go

alter table TB_FORM_POST_RIGHT
   add constraint FK_Reference_35 foreign key (SCTRLRIGHTTYPECODE)
      references TB_DIC_FORM_CTRL_RIGHT_TYPE (SCTRLRIGHTTYPECODE)
go

