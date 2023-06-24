IF NOT EXISTS (SELECT * FROM sys.tables t  
				JOIN sys.schemas s ON (t.schema_id = s.schema_id) 
				WHERE s.name = 'dbo' AND t.name = 'SMSurvey')
	CREATE TABLE [dbo].[SMSurvey](
		[Id] BIGINT NOT NULL IDENTITY (1,1),
		[SurveyEntityId] [nvarchar](50) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		[Category] [nvarchar](200) NULL,
		[QuestionCount] [int] NOT NULL,
		[PageCount] [int] NOT NULL,
		[ResponseCount] [int] NOT NULL,
		[CreatedDate] [datetime2](7) NOT NULL,
		[ModifiedDate] [datetime2](7) NOT NULL
		CONSTRAINT PK_SMSurvey_Id PRIMARY KEY (Id)
	)
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.SMSurvey') AND NAME ='UNCI_SurveyEntityId')
	CREATE UNIQUE NONCLUSTERED INDEX UNCI_SurveyEntityId ON [dbo].[SMSurvey] ([SurveyEntityId] ASC) 
GO


IF NOT EXISTS (SELECT * FROM sys.tables t  
				JOIN sys.schemas s ON (t.schema_id = s.schema_id) 
				WHERE s.name = 'dbo' AND t.name = 'SMCollector')
BEGIN
CREATE TABLE [dbo].[SMCollector](
	[Id] BIGINT NOT NULL IDENTITY (1,1),
	[CollectorEntityId] [nvarchar](50) NOT NULL,
	[SMSurveyId] BIGINT NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	CONSTRAINT PK_SMCollector_Id PRIMARY KEY (Id)
)
ALTER TABLE [dbo].[SMCollector]  WITH CHECK ADD FOREIGN KEY([SMSurveyId])
REFERENCES [dbo].[SMSurvey] ([Id]) ON DELETE CASCADE

END
GO

IF EXISTS(SELECT * FROM sys.indexes WHERE object_id = object_id('dbo.SMCollector') AND NAME ='UNCI_CollectorEntityId')
CREATE UNIQUE NONCLUSTERED INDEX UNCI_CollectorEntityId
ON [dbo].[SMCollector] ([CollectorEntityId] ASC)
GO


IF NOT EXISTS (SELECT * FROM sys.tables t  
				JOIN sys.schemas s ON (t.schema_id = s.schema_id) 
				WHERE s.name = 'dbo' AND t.name = 'SMRecipient')
BEGIN
	CREATE TABLE [dbo].[SMRecipient](
		[Id] BIGINT NOT NULL IDENTITY (1,1),
		[RecipientEntityId] [nvarchar](50) NOT NULL,
		[SMCollectorId] BIGINT NOT NULL,
		[Email] [nvarchar](100) NOT NULL,
		[PhoneNumber] [nvarchar](50) NULL,
		CONSTRAINT PK_SMRecipient_Id PRIMARY KEY (Id)
	)

	ALTER TABLE [dbo].[SMRecipient]  WITH CHECK ADD FOREIGN KEY([SMCollectorId])
	REFERENCES [dbo].[SMCollector] ([Id]) ON DELETE CASCADE
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t  
				JOIN sys.schemas s ON (t.schema_id = s.schema_id) 
				WHERE s.name = 'dbo' AND t.name = 'SMPage')
BEGIN
		CREATE TABLE [dbo].[SMPage](
		[Id] BIGINT NOT NULL IDENTITY (1,1),
		[PageEntityId] [nvarchar](50) NOT NULL,
		[SMSurveyId] BIGINT NOT NULL,
		[Title] [nvarchar](200) NOT NULL,
		[Description] [nvarchar](200) NULL,
		[QuestionCount] [int] NOT NULL,
		[Order] [int] NOT NULL,
		CONSTRAINT PK_SMPage_Id PRIMARY KEY (Id)
	)

	ALTER TABLE [dbo].[SMPage]  WITH CHECK ADD FOREIGN KEY([SMSurveyId])
	REFERENCES [dbo].[SMSurvey] ([Id]) ON DELETE CASCADE
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t  
				JOIN sys.schemas s ON (t.schema_id = s.schema_id) 
				WHERE s.name = 'dbo' AND t.name = 'SMQuestion')
BEGIN
	CREATE TABLE [dbo].[SMQuestion](
		[Id] BIGINT NOT NULL IDENTITY (1,1),
		[QuestionEntityId] [nvarchar](50) NOT NULL,
		[SMPageId] BIGINT NOT NULL,
		[QuestionType] [nvarchar](200) NULL,
		[Question] [nvarchar](MAX) NULL,
		[Order] [int] NOT NULL,
		CONSTRAINT PK_SMQuestion_Id PRIMARY KEY (Id)
	)

	ALTER TABLE [dbo].[SMQuestion]  WITH CHECK ADD FOREIGN KEY([SMPageId])
	REFERENCES [dbo].[SMPage] ([Id]) ON DELETE CASCADE
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t  
				JOIN sys.schemas s ON (t.schema_id = s.schema_id) 
				WHERE s.name = 'dbo' AND t.name = 'SMChoice')
BEGIN
	CREATE TABLE [dbo].[SMChoice](
		[Id] BIGINT NOT NULL IDENTITY (1,1),
		[ChoiceEntityId] [nvarchar](50) NOT NULL,
		[SMQuestionId] BIGINT NOT NULL,
		[Name] [nvarchar](200) NULL,
		[Order] [int] NOT NULL,
		CONSTRAINT PK_SMChoice_Id PRIMARY KEY (Id)
	)

	ALTER TABLE [dbo].[SMChoice]  WITH CHECK ADD FOREIGN KEY([SMQuestionId])
	REFERENCES [dbo].[SMQuestion] ([Id])
END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t  
				JOIN sys.schemas s ON (t.schema_id = s.schema_id) 
				WHERE s.name = 'dbo' AND t.name = 'SMResponse')
BEGIN
	  CREATE TABLE [dbo].[SMResponse](
		[Id] BIGINT NOT NULL IDENTITY (1,1),
		[ResponseEntityId] [nvarchar](50) NOT NULL,
		[Status] [nvarchar](20) NOT NULL,
		[IpAddress] [nvarchar](20) NOT NULL,
		[SurveyEntityId] [nvarchar](50) NOT NULL,
		[CollectorEntityId] [nvarchar](50) NOT NULL,
		[RecipientEntityId] [nvarchar](50) NOT NULL,
		[PageEntityId] [nvarchar](50) NOT NULL,
		[QuestionEntityId] [nvarchar](50) NOT NULL,
		[ChoiceEntityId] [nvarchar](50) NULL,
		[Text] [nvarchar](MAX) NULL,
		CONSTRAINT PK_SMResponse_Id PRIMARY KEY (Id)
	)

END
GO

IF NOT EXISTS (SELECT * FROM sys.tables t  
				JOIN sys.schemas s ON (t.schema_id = s.schema_id) 
				WHERE s.name = 'dbo' AND t.name = 'SMMessage')
BEGIN
	  CREATE TABLE [dbo].[SMMessage](
		[Id] BIGINT NOT NULL IDENTITY (1,1),
		[SMCollectorId] BIGINT NOT NULL,
		[MessageEntityId] [nvarchar](50) NOT NULL,
		[Status] [nvarchar](20) NOT NULL,
		[IsScheduled] BIT NOT NULL,
		[EmbedFirstQuestion] BIT NOT NULL,
		[Subject] [nvarchar](500) NOT NULL,
		[Body] [nvarchar](2000) NULL,
		[Type] [nvarchar](50) NOT NULL,
		[ScheduledDate] [datetime2](7) NOT NULL,

	
		CONSTRAINT PK_SMCollectorMessage_Id PRIMARY KEY (Id)
	)

	ALTER TABLE [dbo].[SMMessage]  WITH CHECK ADD FOREIGN KEY([SMCollectorId])
	REFERENCES [dbo].[SMCollector] ([Id])

END
