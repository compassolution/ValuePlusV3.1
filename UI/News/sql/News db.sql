/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2005                    */
/* Created on:     2012/5/8 10:52:54                            */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_NEWS_CONTENT') and o.name = 'FK_Reference_24')
alter table TB_NEWS_CONTENT
   drop constraint FK_Reference_24
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('TB_NEWS_CONTENT') and o.name = 'FK_Reference_38')
alter table TB_NEWS_CONTENT
   drop constraint FK_Reference_38
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_NEWS_CONTENT')
            and   type = 'U')
   drop table TB_NEWS_CONTENT
go

if exists (select 1
            from  sysobjects
           where  id = object_id('TB_NEWS_PLATE')
            and   type = 'U')
   drop table TB_NEWS_PLATE
go

/*==============================================================*/
/* Table: TB_NEWS_CONTENT                                       */
/*==============================================================*/
create table TB_NEWS_CONTENT (
   SNEWSID              varchar(40)          not null,
   SPLATEID             varchar(40)          null,
   SUSERID              varchar(40)          null,
   STITLE               varchar(100)         null,
   STITLECHS            varchar(100)         null,
   SCONTENT             text                 null,
   SCONTENTCHS          text                 null,
   DTTIME               datetime             null,
   BISSTOP              char(1)              null,
   constraint PK_TB_NEWS_CONTENT primary key (SNEWSID)
)
go

/*==============================================================*/
/* Table: TB_NEWS_PLATE                                         */
/*==============================================================*/
create table TB_NEWS_PLATE (
   SPLATEID             varchar(40)          not null,
   SPLATENAME           varchar(100)         null,
   SPLATENAMECHS        varchar(100)         null,
   SDESC                varchar(1000)        null,
   SDESCCHS             varchar(1000)        null,
   SPARENTID            varchar(40)          null,
   SLEVEL               varchar(10)          null,
   SIMAGENAME           varchar(50)          null,
   SIMAGE               image                null,
   SORDER               varchar(20)          null,
   BISSTOP              char(1)              null,
   constraint PK_TB_NEWS_PLATE primary key (SPLATEID)
)
go

alter table TB_NEWS_CONTENT
   add constraint FK_Reference_24 foreign key (SPLATEID)
      references TB_NEWS_PLATE (SPLATEID)
go

alter table TB_NEWS_CONTENT
   add constraint FK_Reference_38 foreign key (SUSERID)
      references dbo.TB_HR_USER (SUSERID)
go

