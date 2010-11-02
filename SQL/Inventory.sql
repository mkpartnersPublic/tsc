CREATE FUNCTION dbo.DTtoUnixTS 
( 
    @dt DATETIME 
) 
RETURNS BIGINT 
AS 
BEGIN 
    DECLARE @diff BIGINT 
    IF @dt >= '20380119' 
    BEGIN 
        SET @diff = CONVERT(BIGINT, DATEDIFF(S, '19700101', '20380119')) 
            + CONVERT(BIGINT, DATEDIFF(S, '20380119', @dt)) 
    END 
    ELSE 
        SET @diff = DATEDIFF(S, '19700101', @dt) 
    RETURN @diff 
END

GO



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
-- EXEC getVersesAssignedByDonationId 'D-9090'
AS
BEGIN
DECLARE @GROUIPID INT
SELECT TOP 1 @GROUIPID=VERSE_GROUP_ID FROM VERSES_2_PROJECTS WHERE sfdc_Donation_Id=@sfdcDonationId
SELECT *,@GROUIPID AS GROUIPID FROM VERSES WHERE ID IN( 
SELECT VERSE_ID FROM VERSES_2_PROJECTS WHERE sfdc_Donation_Id=@sfdcDonationId 
)
END
GO

--SELECT * FROM VERSES_2_PROJECTS

DROP PROC getVersesAssignedByAuthId
GO
CREATE PROC getVersesAssignedByAuthId
	@authId as bigint
--1. Webmethod to return verses assigned for a give authId.
--2. Return all records where verses_2_projects.auth_id = authId
--3. Response should include the actual verses from the verses table.
-- EXEC getVersesAssignedByAuthId 7777777
--EXEC getVersesAssignedByAuthId 2228891830
AS
BEGIN
DECLARE @GROUIPID INT
SELECT TOP 1 @GROUIPID=VERSE_GROUP_ID FROM VERSES_2_PROJECTS WHERE AUTH_ID=@authId
SELECT *,@GROUIPID AS GROUIPID FROM VERSES WHERE ID IN( 
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
@authId bigint,
@numberOfVerses int

--1. Webmethod to assign the required numbers of verses to a donation
--2. Find the next set of un-assigned verses. To find the set of un-assigned verses: 
--Find the first n records from verses_2_projects where auth_id = null & sfdc_donation_id = null order by verse_id desc. 

--N is equal to numberOfVerses.
--3. Update the records set auth_id = authId, sfdc_opportunity_id = sfdcDonationId, sfdc_contact_id = sfdcContactId, fund_id = fundId, sponsored_time = paymentDate.
--4. Response should include the verses assigned.
--exec AssignVerses 71600,4356,'aaabbbccc','12123',122,'12/10/2010',-1,3
--project_id	language_id
--71600	4356
AS
BEGIN

	--SELECT *,@GROUIPID AS GROUIPID FROM VERSES	
	
	
	/* Create a temporary table with all the verseid that needs to be assigned*/
	
	--DROP TABLE #verseid
	--declare @count int 
	--set @count = 3 
	IF OBJECT_ID(N'tempdb..#verseid', N'U') IS NOT NULL 
		DROP TABLE #verseid;
	DECLARE @GROUIPID INT
	SET @GROUIPID = (SELECT ID FROM VERSE_GROUPS WHERE PROJECT_ID=@projectId AND LANGUAGE_ID=@languageId)
	
	/* Changes made to avoid concurrent user update*/
	BEGIN TRAN
	
		SET ROWCOUNT @numberOfVerses
		SELECT VERSE_ID INTO #verseid
		FROM VERSES_2_PROJECTS 
		WHERE AUTH_ID IS NULL
		AND FUND_ID IS NULL
		AND VERSE_GROUP_ID=@GROUIPID
		ORDER BY VERSE_ID ASC
		SET ROWCOUNT 0
		
		--select * from #verseid
		
		/* Create a temporary table with all the verseid that needs to be assigned*/
		/*if @authId=-1
			set @authId=NULL		*/
		UPDATE VERSES_2_PROJECTS
		SET auth_id = @authId, 
		sfdc_Donation_Id = @sfdcDonationId, 
		sfdc_contact_id = @sfdcContactId, 
		fund_id = @fundId, 
		sponsored_time =dbo.DTtoUnixTS(@paymentDate) 
		--CONVERT( BIGINT,DATEDIFF(day, '1970-01-01 00:00:00.000', @paymentDate))
		--CONVERT( BIGINT,@paymentDate)
		WHERE
		VERSE_GROUP_ID=@GROUIPID
		AND
		VERSE_ID IN (
				SELECT VERSE_ID FROM #verseid
				) 
				
		--SELECT * FROM VERSES_2_PROJECTS	
		SELECT *,@GROUIPID AS GROUPID FROM VERSES
		WHERE ID IN (SELECT VERSE_ID FROM #verseid) 
	
	IF @@error <> 0
		ROLLBACK TRAN
	ELSE
		COMMIT TRAN	
	--SELECT ID FROM VERSE_GROUPS WHERE PROJECT_ID=@projectId AND LANGUAGE_ID=@languageId
	IF OBJECT_ID(N'tempdb..#verseid', N'U') IS NOT NULL 
		DROP TABLE #verseid;
END
GO

--sp_help VERSES_2_PROJECTS

DROP PROC updateVersesAssignedByAuthId
GO
CREATE PROC updateVersesAssignedByAuthId
@authId bigint,
@sfdcDonationId varchar(50),
@sfdcContactId varchar(50),
@fundId varchar(50),
@paymentDate datetime
--1. Webmethod to updated already assigned verses based on the Auth Id.
--2. For all records where verses_2_projects.auth_id = authId, set sfdc_donation_id = sfdcDonationId, sfdc_contact_id = sfdcContactId, fund_id = fundId and sponsored_time = paymentDate.
--3. Include status and error in the response. No need to return anything else.
AS
BEGIN

	UPDATE VERSES_2_PROJECTS
	SET 
	sfdc_Donation_Id = @sfdcDonationId, 
	sfdc_contact_id = @sfdcContactId, 
	fund_id = @fundId, 
	sponsored_time =dbo.DTtoUnixTS(@paymentDate) 
	WHERE
	auth_id = @authId
END
GO


DROP PROC updateVersesAssignedByDonationId
GO
CREATE PROC updateVersesAssignedByDonationId
@authId bigint,
@sfdcDonationId varchar(50),
@sfdcContactId varchar(50),
@fundId varchar(50),
@paymentDate datetime
--1. Webmethod to updated already assigned verses based on the Donation Id.
--2. For all records where verses_2_projects.sfdc_donation_id = sfdcDonationId, set auth_id = authId,  sfdc_contact_id = sfdcContactId, fund_id = fundId, sponsored_time = paymentDate.
--3. Include status and error in the response. No need to return anything else.
AS
BEGIN

	UPDATE VERSES_2_PROJECTS
	SET 
	auth_id = @authId, 
	sfdc_contact_id = @sfdcContactId, 
	fund_id = @fundId, 
	sponsored_time =dbo.DTtoUnixTS(@paymentDate)
	--CONVERT( BIGINT,@paymentDate)
	WHERE
	sfdc_Donation_Id = @sfdcDonationId
END
GO

--declare @paymentDate datetime
--set @paymentDate=getdate()
--select CONVERT( BIGINT,DATEDIFF(day, '1970-01-01 00:00:00.000', @paymentDate))