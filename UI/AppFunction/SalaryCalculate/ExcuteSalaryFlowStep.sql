/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2005                    */
/* Created on:     2011/7/5 12:58:27                            */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('TB_SALARY_CALCULATE_FLOW')
            and   type = 'U')
   drop table TB_SALARY_CALCULATE_FLOW
go

/*==============================================================*/
/* Table: TB_SALARY_CALCULATE_FLOW                              */
/*==============================================================*/
create table TB_SALARY_CALCULATE_FLOW (
   SOPERATIONCODE       VARCHAR(40)          not null,
   NORDER               DECIMAL              null,
   SOPNAME              VARCHAR(100)         null,
   SOPNAMECN            VARCHAR(100)         null,
   SOPDETAIL            VARCHAR(100)         null,
   SDEPTNAME            VARCHAR(50)          null,
   SDEPTNAMECN          VARCHAR(50)          null,
   BISCUROP             char(1)              null,
   SIMAGE               VARCHAR(50)          null,
   DTOPDATE             datetime             null,
   BISSTART             char(1)              null,
   BISEND               char(1)              null,
   SNEXTCODE            VARCHAR(40)          null,
   BISBINGXING_NEXT     char(1)              null,
   SBACKCODE            VARCHAR(40)          null,
   BISBINGXING_BACK     char(1)              null,
   constraint PK_TB_SALARY_CALCULATE_FLOW primary key (SOPERATIONCODE)
)
go

declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'guid',
   'user', @CurrentUser, 'table', 'TB_SALARY_CALCULATE_FLOW', 'column', 'SOPERATIONCODE'
go



-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExcuteSalaryFlowStep]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ExcuteSalaryFlowStep]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE ExcuteSalaryFlowStep
	-- Add the parameters for the stored procedure here
	@stepCode  varchar(40),--薪资计算流程当前步骤的ID（即表[TB_SALARY_CALCULATE_FLOW]主键）
	@opFlag    char(1)--操作标志，0表示成功前进，1表示失败回退，2表示前进且只能保持一个BISCUR=1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @nextStep varchar(40)
	DECLARE @backStep varchar(40)
	DECLARE @nextBing char(1)
	DECLARE @backBing char(1)
	DECLARE @isStartStep char(1)
	DECLARE @isEndStep char(1)
	DECLARE @dtCurDatetime datetime

	SELECT 
		@nextStep = [SNEXTCODE],
		@backStep = [SBACKCODE],
		@nextBing = [BISBINGXING_NEXT],
		@backBing = [BISBINGXING_BACK],
		@isStartStep = [BISSTART],
		@isEndStep = [BISEND]
		FROM [dbo].[TB_SALARY_CALCULATE_FLOW] WHERE [SOPERATIONCODE] = @stepCode;
	
	SET @dtCurDatetime = GETDATE();	
	
	if(@opFlag='')
	BEGIN
		--记录操作时间，并取消当前步骤标志
		UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '0', [DTOPDATE] = @dtCurDatetime WHERE [SOPERATIONCODE] = @stepCode;
	END
	ELSE
	BEGIN
		if(@opFlag='0')--0表示成功前进
		BEGIN
			if(@nextBing='1')
			BEGIN
				--如果可以并行，则保留当前记录的[BISCUROP]标志
				UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [DTOPDATE] = @dtCurDatetime WHERE [SOPERATIONCODE] = @stepCode;
			END
			ELSE
			BEGIN
				--如果不可以并行，则将当前记录的[BISCUROP]设置为0，标示取消当前步骤
				UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '0',[DTOPDATE] = @dtCurDatetime WHERE [SOPERATIONCODE] = @stepCode;
			END
			
			--将下一步操作记录的[BISCUROP]设置为1，标示设置为当前步骤
			UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '1' WHERE [SOPERATIONCODE] = @nextStep;
			
			--********特殊处理,“基本信息”初始化后即可初始化“考勤信息”，“保险信息”，“变更信息”
			if(@stepCode='SALARY01')
			BEGIN
				UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '1' WHERE [SOPERATIONCODE]  IN('SALARY02','SALARY03','SALARY04');
			END
	
			--终点步骤，则清除所有当前步骤标志，并将起始步骤置位当前状态
			IF(@isEndStep='1')
			BEGIN
				UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '0' where [BISSTART] = '0';
				UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '1' where [BISSTART] = '1';
			END
		END
		else if(@opFlag='1')--，1表示失败回退，2表示前进且只能保持一个BISCUR=1
		BEGIN
			if(@backBing='1')
			BEGIN
				--如果可以并行，则保留当前记录的[BISCUROP]标志
				UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [DTOPDATE] = @dtCurDatetime WHERE [SOPERATIONCODE] = @stepCode;
			END
			ELSE
			BEGIN
				--如果不可以并行，则将当前记录的[BISCUROP]设置为0，标示取消当前步骤
				UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '0',[DTOPDATE] = @dtCurDatetime WHERE [SOPERATIONCODE] = @stepCode;
			END
			
			--将下一步操作记录的[BISCUROP]设置为1，标示设置为当前步骤
			UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '1' WHERE [SOPERATIONCODE] = @backStep;
		END
		else if(@opFlag='2')--2表示前进且只能保持一个BISCUR=1
		BEGIN
			--将下一步操作记录的[BISCUROP]设置为1，标示设置为当前步骤
			UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '1' WHERE [SOPERATIONCODE] = @nextStep;
			--将下一步操作记录以外的所有的[BISCUROP]设置为0，标示取消当前步骤
			UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '0' WHERE [SOPERATIONCODE] <> @nextStep;
			
			
			--终点步骤，则清除所有当前步骤标志，并将起始步骤置位当前状态
			IF(@isEndStep='1')
			BEGIN
				UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '0' where [BISSTART] = '0';
				UPDATE [dbo].[TB_SALARY_CALCULATE_FLOW] SET [BISCUROP] = '1' where [BISSTART] = '1';
			END
		END
		
	END

END
GO

--**存储过程调用

--**********初始化基本信息
--********************成功
Exec ExcuteSalaryFlowStep 'SALARY01','0';
--********************失败
Exec ExcuteSalaryFlowStep 'SALARY01','1';

--**********初始化保险信息
--********************成功
Exec ExcuteSalaryFlowStep 'SALARY02','0';
--********************失败
Exec ExcuteSalaryFlowStep 'SALARY02','1';

--**********初始化考勤信息
--********************成功
Exec ExcuteSalaryFlowStep 'SALARY03','0';
--********************失败
Exec ExcuteSalaryFlowStep 'SALARY03','1';

--**********初始化变更信息
--********************成功
Exec ExcuteSalaryFlowStep 'SALARY04','0';
--********************失败
Exec ExcuteSalaryFlowStep 'SALARY04','1';

--**********确认基本信息
--********************成功
Exec ExcuteSalaryFlowStep 'SALARY05','';
--********************退回
Exec ExcuteSalaryFlowStep 'SALARY05','1';

--**********确认保险信息
--********************成功
Exec ExcuteSalaryFlowStep 'SALARY06','';
--********************退回
Exec ExcuteSalaryFlowStep 'SALARY06','1';

--**********确认考勤信息
--********************成功
Exec ExcuteSalaryFlowStep 'SALARY07','';
--********************退回
Exec ExcuteSalaryFlowStep 'SALARY07','1';

--**********确认变更信息
--********************成功
Exec ExcuteSalaryFlowStep 'SALARY08','';
--********************退回
Exec ExcuteSalaryFlowStep 'SALARY08','1';

--***********四项全部确认成功
Exec ExcuteSalaryFlowStep 'SALARY05','2';


--**********薪资计算
--********************成功
Exec ExcuteSalaryFlowStep 'SALARY09','0';
--********************失败
Exec ExcuteSalaryFlowStep 'SALARY09','';


--**********薪资支付
--********************成功
Exec ExcuteSalaryFlowStep 'SALARY10','0';
--********************失败
Exec ExcuteSalaryFlowStep 'SALARY10','';