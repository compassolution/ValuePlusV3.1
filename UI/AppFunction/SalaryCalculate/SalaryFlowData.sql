
DELETE FROM TB_SALARY_CALCULATE_FLOW;
--方案一：基础信息的初始化和确认工作分步执行转到下一步骤
INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY01',1,'初始化基本信息','初始化基本信息','../../Archive/Archive.aspx?DOCU=TRSBAS','人事部','人事部','1','../../common/images/fuctionIcon/Function0.png',null,'1','0','SALARY05','0','','');
INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY02',2,'初始化保险信息','初始化保险信息','../../Archive/Archive.aspx?DOCU=TRSINS','人事部','人事部','0','../../common/images/fuctionIcon/Function1.png',null,'0','0','SALARY06','0','','');
INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY03',3,'初始化考勤信息','初始化考勤信息','../../Archive/Archive.aspx?DOCU=TRSATT','人事部','人事部','0','../../common/images/fuctionIcon/Function2.png',null,'0','0','SALARY07','0','','');
INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY04',4,'初始化变更信息','初始化变更信息','../../Archive/Archive.aspx?DOCU=TRSPA','人事部','人事部','0','../../common/images/fuctionIcon/Function3.png',null,'0','0','SALARY08','0','','');
INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY05',5,'核对基本信息','核对基本信息','../../Archive/Archive.aspx?DOCU=TRSBAS','财务部','财务部','0','../../common/images/fuctionIcon/Function4.png',null,'0','0','SALARY09','0','SALARY01','0');
INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY06',6,'核对保险信息','核对保险信息','../../Archive/Archive.aspx?DOCU=TRSINS','财务部','财务部','0','../../common/images/fuctionIcon/Function5.png',null,'0','0','SALARY09','0','SALARY02','0');
INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY07',7,'核对考勤信息','核对考勤信息','../../Archive/Archive.aspx?DOCU=TRSATT','财务部','财务部','0','../../common/images/fuctionIcon/Function6.png',null,'0','0','SALARY09','0','SALARY03','0');
INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY08',8,'核对变更信息','核对变更信息','../../Archive/Archive.aspx?DOCU=TRSPA','财务部','财务部','0','../../common/images/fuctionIcon/Function7.png',null,'0','0','SALARY09','0','SALARY04','0');
INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY09',9,'薪资计算','薪资计算','../../Archive/Archive.aspx?DOCU=PREMPL','财务部','财务部','0','../../common/images/fuctionIcon/Function8.png',null,'0','0','SALARY10','1','','');
INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY10',10,'薪资支付','薪资支付','../../Archive/Archive.aspx?DOCU=PREMPL','财务部','财务部','0','../../common/images/fuctionIcon/Function9.png',null,'0','1','','','','');

--方案一：基础信息的初始化和确认工作在几种过渡数据全部处理完成后转到下一步骤
--INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY01',1,'初始化人事基础信息','初始化人事基础信息','../../FunctionList.aspx?sCode=OS0000','人事部','人事部','0','','','1','../../common/images/fuctionIcon/Function0.png',null);
--INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY02',2,'核对人事基础信息','核对人事基础信息','../../FunctionList.aspx?sCode=OS0000','财务部','财务部','0','','','0','../../common/images/fuctionIcon/Function4.png',null);
--INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY03',3,'薪资项目初始化','薪资项目初始化','../../Archive/Archive.aspx?DOCU=PREMPL','财务部','财务部','0','','','0','../../common/images/fuctionIcon/Function8.png',null);
--INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY04',4,'薪资计算','薪资计算','../../Archive/Archive.aspx?DOCU=PREMPL','财务部','财务部','0','','','0','../../common/images/fuctionIcon/Function9.png',null);
--INSERT INTO TB_SALARY_CALCULATE_FLOW VALUES('SALARY05',5,'薪资支付','薪资支付','../../Archive/Archive.aspx?DOCU=PREMPL','财务部','财务部','0','','','0','../../common/images/fuctionIcon/Function10.png',null);


