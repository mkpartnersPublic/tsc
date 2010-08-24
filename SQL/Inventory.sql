

--sp_help verses


DROP PROC GetVersePrice
GO
CREATE PROC GetVersePrice
	@ProjectId as int,
	@LanguageId as int,
	@IsGroup as bit
--1. Webmethod to return a verse price for a give project is and language id.
--2. If isGroup == true, verse price should be fetched from group_project_price table.
--3. If isGroup == false, verse price should be fetched from verse_groups
--4. groupId for the response should always be fetched from verse_groups.
-- EXEC GetVersePrice 537,7956,1
-- EXEC GetVersePrice 71600,4356,0
AS
IF (@IsGroup=1 )
BEGIN
	SELECT VERSE_PRICE FROM GROUP_PROJECT_PRICE 
	WHERE PROJECT_ID=@ProjectId
	AND LANGUAGE_ID=@LanguageId
END
ELSE
BEGIN
	SELECT VERSE_PRICE FROM VERSE_GROUPS 
	WHERE PROJECT_ID=@ProjectId
	AND LANGUAGE_ID=@LanguageId
END

GO

--select * from VERSES_2_PROJECTS where verse_group_id=50

--update VERSES_2_PROJECTS 
--set sfdc_Donation_Id='aaabbbccc'
--where verse_group_id=50

--update VERSES_2_PROJECTS 
--set AUTH_ID=100100
--where verse_group_id=50


DROP PROC getVersesAssignedByDonationId
GO
CREATE PROC getVersesAssignedByDonationId
	@sfdcDonationId as VARCHAR(50)
--1. Webmethod to return verses assigned for a give sfdcDonationId.
--2. Return all records where verses_2_projects.sfdc_donation_id = sfdcDonationId
--3. Response should include the actual verses from the verses table.
-- EXEC getVersesAssignedByDonationId '1V71600ABS'
AS
BEGIN
SELECT * FROM VERSES WHERE ID IN( 
SELECT VERSE_ID FROM VERSES_2_PROJECTS WHERE sfdc_Donation_Id=@sfdcDonationId 
)
END
GO


DROP PROC getVersesAssignedByAuthId
GO
CREATE PROC getVersesAssignedByAuthId
	@authId as bigint
--1. Webmethod to return verses assigned for a give authId.
--2. Return all records where verses_2_projects.auth_id = authId
--3. Response should include the actual verses from the verses table.
-- EXEC getVersesAssignedByAuthId 2228891830
AS
BEGIN
SELECT * FROM VERSES WHERE ID IN( 
SELECT VERSE_ID FROM VERSES_2_PROJECTS WHERE AUTH_ID=@authId 
)
END
GO



DROP PROC releaseVersesAssignedByDonationId
GO
CREATE PROC releaseVersesAssignedByDonationId
	@sfdcDonationId as VARCHAR(50)
--1. Webmethod to release verses assigned.
--2. In verses_2_projects set auth_id, sfdc_donation_id, sfdc_contact_id, sponsored_time and fund_id to null where sfdc_donation_id = sfdcDonationId
--3. Response should have just the success variable set.
AS
BEGIN
	UPDATE VERSES_2_PROJECTS
	SET AUTH_ID=NULL,
	SPONSORED_TIME=NULL,
	SFDC_DONATION_ID=NULL,
	SFDC_CONTACT_ID=NULL,
	FUND_ID=NULL
	WHERE SFDC_DONATION_ID=@sfdcDonationId
END
GO



DROP PROC releaseVersesAssignedByAuthId
GO
CREATE PROC releaseVersesAssignedByAuthId
	@authId as bigint
--1. Webmethod to release verses assigned.
--2. In verses_2_projects set auth_id, sfdc_donation_id, sfdc_contact_id, sponsored_time and fund_id to null where auth_id = authId
--3. Response should have just the success variable set.
AS
BEGIN
	UPDATE VERSES_2_PROJECTS
	SET AUTH_ID=NULL,
	SPONSORED_TIME=NULL,
	SFDC_DONATION_ID=NULL,
	SFDC_CONTACT_ID=NULL,
	FUND_ID=NULL
	WHERE AUTH_ID=@authId
END
GO


sp_help VERSES_2_PROJECTS 
--alter table VERSES_2_PROJECTS  drop column sfdc_Donation_Id
--alter table VERSES_2_PROJECTS  add sfdc_Donation_Id varchar(50) null

--alter table VERSES_2_PROJECTS  drop column sfdc_contact_id
--alter table VERSES_2_PROJECTS  add sfdc_contact_id varchar(50) null


DROP PROC AssignVerses
GO
CREATE PROC AssignVerses
@projectId int,
@languageId int,
@sfdcDonationId varchar(50),
@sfdcContactId varchar(50),
@fundId varchar(50),
@paymentDate datetime,
@authId int,
@numberOfVerses int

--1. Webmethod to assign the required numbers of verses to a donation
--2. Find the next set of un-assigned verses. To find the set of un-assigned verses: 
--Find the first n records from verses_2_projects where auth_id = null & sfdc_donation_id = null order by verse_id desc. 

--N is equal to numberOfVerses.
--3. Update the records set auth_id = authId, sfdc_opportunity_id = sfdcDonationId, sfdc_contact_id = sfdcContactId, fund_id = fundId, sponsored_time = paymentDate.
--4. Response should include the verses assigned.

AS
BEGIN
	
	
	/* Create a temporary table with all the verseid that needs to be assigned*/
	
	--DROP TABLE #verseid
	--declare @count int 
	--set @count = 3 
	IF OBJECT_ID(N'tempdb..#verseid', N'U') IS NOT NULL 
		DROP TABLE #verseid;
	
	SET ROWCOUNT @numberOfVerses
	SELECT VERSE_ID INTO #verseid
	FROM VERSES_2_PROJECTS 
	WHERE AUTH_ID is null AND FUND_ID is null 
	order by VERSE_ID desc
	SET ROWCOUNT 0
	/* Create a temporary table with all the verseid that needs to be assigned*/

	UPDATE VERSES_2_PROJECTS
	SET auth_id = @authId, 
	sfdc_opportunity_id = @sfdcDonationId, 
	sfdc_Donation_Id = @sfdcContactId, 
	fund_id = @fundId, 
	sponsored_time =CONVERT( BIGINT,@paymentDate)
	WHERE
	VERSE_ID IN (
			SELECT VERSE_ID FROM #verseid
			) 
			
	SELECT * FROM VERSES_2_PROJECTS	
	WHERE VERSE_ID IN (SELECT VERSE_ID FROM #verseid) 
	
	IF OBJECT_ID(N'tempdb..#verseid', N'U') IS NOT NULL 
		DROP TABLE #verseid;
END
GO



----DECLARE @count int 
----SET @count = 20 
 
----SELECT TOP @count * FROM verses
sp_help VERSES_2_PROJECTS	


--declare @sql  nvarchar(200), @count int 
--set @count = 15 
--set @sql = N'select top ' + cast(@count as nvarchar(4)) + ' verse_id INTO #verseid from verses_2_projects' 
--exec (@sql) 
--GO

--DROP TABLE #verseid
--declare @count int 
--set @count = 3 
--SET ROWCOUNT @count
--select VERSE_ID INTO #verseid
--FROM VERSES_2_PROJECTS 
--WHERE AUTH_ID is null AND fund_id is null 
--order by verse_id desc


--select * from #verseid
--SET ROWCOUNT 0


--select * 
--FROM VERSES_2_PROJECTS 
--WHERE AUTH_ID is null AND fund_id is null 


--SELECT VERSE_PRICE FROM GROUP_PROJECT_PRICE 
--WHERE PROJECT_ID=@ProjectId
--AND LANGUAGE_ID=@LanguageId

--update VERSES_2_PROJECTS set AUTH_ID = null , fund_id = null where verse_id in (
--23417,
--23418,
--23419,
--23420,
--23421)

--SELECT * FROM verses_2_projects
--SELECT * FROM verses
--sp_help verses_2_projects
----select top 10 * from verses
--SELECT * FROM VERSE_GROUPS
--SELECT * FROM GROUP_PROJECT_PRICE
----select COUNT(*) from verses
------31087

----alter PROC ReturnTop10Versus
----AS
----SELECT TOP 10 * FROM VERSES FOR XML


----CREATE PROC ReturnVerse
----@id as int
----AS
----SELECT * FROM VERSES WHERE id=@id

------ReturnVerse 2

--ALTER TABLE verses_2_projects
--ADD sfdc_Donation_Id BIGINT

--ALTER TABLE verses_2_projects
--ADD sfdc_contact_id BIGINT

--ALTER TABLE verses_2_projects
--ADD sfdc_Donation_Id BIGINT

--ALTER TABLE verses_2_projects
--ADD sfdc_opportunity_id BIGINT

 
 
 
-- select CONVERT( varchar(10), getdate(),101)
 
--  select CONVERT( BIGINT,getdate())
--  select CONVERT( BIGINT,getdate()-1)
