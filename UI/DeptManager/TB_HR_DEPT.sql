/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2005                    */
/* Created on:     2011/11/14 12:14:17                          */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('TB_HR_DEPT')
            and   type = 'U')
   drop table TB_HR_DEPT
go

/*==============================================================*/
/* Table: TB_HR_DEPT                                            */
/*==============================================================*/
create table TB_HR_DEPT (
   SDEPTID              VARCHAR(200)         not null,
   SDEPTNAME            VARCHAR(200)         null,
   SDEPTNAMECN          VARCHAR(200)         null,
   SPARENTDEPTID        VARCHAR(200)         null,
   SORDER               VARCHAR(20)          null,
   SLEVEL               varchar(10)          null,
   SCOSTCODE            varchar(30)          null,
   BISSTOP              CHAR(1)              null,
   constraint PK_TB_HR_DEPT primary key (SDEPTID)
)
go

