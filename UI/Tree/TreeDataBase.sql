/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2005                    */
/* Created on:     2011/9/28 16:12:30                           */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.TB_HRTREED')
            and   type = 'U')
   drop table dbo.TB_HRTREED
go

if exists (select 1
            from  sysobjects
           where  id = object_id('dbo.TB_HRTREEH')
            and   type = 'U')
   drop table dbo.TB_HRTREEH
go

/*==============================================================*/
/* Table: TB_HRTREED                                            */
/*==============================================================*/
create table dbo.TB_HRTREED (
   TID                  varchar(20)          collate Chinese_PRC_CI_AS not null,
   PID                  varchar(20)          collate Chinese_PRC_CI_AS not null,
   PDESC                varchar(100)         collate Chinese_PRC_CI_AS null,
   PDESCCHS             varchar(100)         collate Chinese_PRC_CI_AS null,
   PTYPE                varchar(16)          collate Chinese_PRC_CI_AS null,
   PLEN                 int                  not null,
   PPREC                int                  null,
   PNULL                int                  null constraint DF__SMTMPD__PNULL__25869641 default (1),
   PDEFAULT             varchar(50)          collate Chinese_PRC_CI_AS null,
   PISKEY               int                  null constraint DF__SMTMPD__PISKEY__267ABA7A default (0),
   PCTRL                int                  null constraint DF__SMTMPD__PCTRL__276EDEB3 default (0),
   PCTRLID              varchar(1000)        collate Chinese_PRC_CI_AS null,
   PCTRLD               varchar(3000)        collate Chinese_PRC_CI_AS null,
   PORDER               int                  null,
   PRIGHT               int                  null constraint DF_SMTMPD_PRIGHT default (0),
   PSYS                 int                  null constraint DF_SMTMPD_PSYS default (0),
   PLIST                int                  null constraint DF_SMTMPD_PLIST default (0),
   PWIDTH               int                  null constraint DF_SMTMPD_PWIDTH default (80),
   PMAST                varchar(20)          collate Chinese_PRC_CI_AS null,
   constraint PK_TB_HRTREED primary key (TID, PID)
         on "PRIMARY"
)
on "PRIMARY"
go

execute sp_addextendedproperty 'MS_Description', 
   '表名字段表',
   'user', 'dbo', 'table', 'TB_HRTREED'
go

/*==============================================================*/
/* Table: TB_HRTREEH                                            */
/*==============================================================*/
create table dbo.TB_HRTREEH (
   TID                  varchar(20)          collate Chinese_PRC_CI_AS not null,
   TDESC                varchar(50)          collate Chinese_PRC_CI_AS null,
   TDESCCHS             varchar(50)          collate Chinese_PRC_CI_AS null,
   TCRTDATE             datetime             null,
   TREC                 int                  null default 0,
   BISSTOP              char(1)              null,
   constraint PK_TB_HRTREEH primary key (TID)
         on "PRIMARY"
)
ON [PRIMARY]
go

execute sp_addextendedproperty 'MS_Description', 
   '树形定义表',
   'user', 'dbo', 'table', 'TB_HRTREEH'
go

