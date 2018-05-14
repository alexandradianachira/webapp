
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/13/2018 15:57:31
-- Generated from EDMX file: D:\anul III\WebApplication\WebApplication\UserModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-WebApplication-20171130031735];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ConferenceChair]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Conferences] DROP CONSTRAINT [FK_ConferenceChair];
GO
IF OBJECT_ID(N'[dbo].[FK_PCmembersUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PCmembers] DROP CONSTRAINT [FK_PCmembersUser];
GO
IF OBJECT_ID(N'[dbo].[FK_PCmemberConference]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PCmembers] DROP CONSTRAINT [FK_PCmemberConference];
GO
IF OBJECT_ID(N'[dbo].[FK_UserSubreviewer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subreviewers] DROP CONSTRAINT [FK_UserSubreviewer];
GO
IF OBJECT_ID(N'[dbo].[FK_SubreviewSubreviewer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subreviews1] DROP CONSTRAINT [FK_SubreviewSubreviewer];
GO
IF OBJECT_ID(N'[dbo].[FK_PaperAssignmentPCmember]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaperAssignments] DROP CONSTRAINT [FK_PaperAssignmentPCmember];
GO
IF OBJECT_ID(N'[dbo].[FK_PaperAssignmentSubreviewer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Subreviewers] DROP CONSTRAINT [FK_PaperAssignmentSubreviewer];
GO
IF OBJECT_ID(N'[dbo].[FK_PaperPaperAssignment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaperAssignments] DROP CONSTRAINT [FK_PaperPaperAssignment];
GO
IF OBJECT_ID(N'[dbo].[FK_PaperAssignmentReview]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reviews] DROP CONSTRAINT [FK_PaperAssignmentReview];
GO
IF OBJECT_ID(N'[dbo].[FK_UserAuthor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Authors] DROP CONSTRAINT [FK_UserAuthor];
GO
IF OBJECT_ID(N'[dbo].[FK_AuthorPaper]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Papers] DROP CONSTRAINT [FK_AuthorPaper];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Users]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Users];
GO
IF OBJECT_ID(N'[dbo].[Conferences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Conferences];
GO
IF OBJECT_ID(N'[dbo].[PCmembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PCmembers];
GO
IF OBJECT_ID(N'[dbo].[Papers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Papers];
GO
IF OBJECT_ID(N'[dbo].[Subreviewers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subreviewers];
GO
IF OBJECT_ID(N'[dbo].[Subreviews1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subreviews1];
GO
IF OBJECT_ID(N'[dbo].[PaperAssignments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaperAssignments];
GO
IF OBJECT_ID(N'[dbo].[Reviews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reviews];
GO
IF OBJECT_ID(N'[dbo].[Authors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Authors];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [id_user] int IDENTITY(1,1) NOT NULL,
    [surname] nvarchar(max)  NOT NULL,
    [first_name] nvarchar(max)  NOT NULL,
    [email] nvarchar(max)  NOT NULL,
    [institution] nvarchar(max)  NOT NULL,
    [password] nvarchar(max)  NOT NULL,
    [verified_account] datetime  NOT NULL,
    [date_verification_send] datetime  NOT NULL,
    [date_active] datetime  NOT NULL,
    [verification_key] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Conferences'
CREATE TABLE [dbo].[Conferences] (
    [id_conference] int IDENTITY(1,1) NOT NULL,
    [conference_name] nvarchar(max)  NOT NULL,
    [location] nvarchar(max)  NOT NULL,
    [start_date] datetime  NOT NULL,
    [end_date] datetime  NOT NULL,
    [Chair_id_user] int  NOT NULL
);
GO

-- Creating table 'PCmembers'
CREATE TABLE [dbo].[PCmembers] (
    [id_pcmember] int IDENTITY(1,1) NOT NULL,
    [id_user] int  NOT NULL,
    [id_conference] int  NOT NULL,
    [is_chair] bit  NOT NULL,
    [dateinvitationsent] datetime  NOT NULL,
    [dateinvitationacc] datetime  NOT NULL,
    [is_valid] bit  NOT NULL,
    [User_id_user] int  NOT NULL,
    [Conference_id_conference] int  NOT NULL
);
GO

-- Creating table 'Papers'
CREATE TABLE [dbo].[Papers] (
    [id_paper] int IDENTITY(1,1) NOT NULL,
    [id_conference] nvarchar(max)  NOT NULL,
    [title] nvarchar(max)  NOT NULL,
    [pdf] varbinary(max)  NOT NULL,
    [date_submitted] time  NOT NULL,
    [is_submitted] bit  NOT NULL,
    [decision] nvarchar(max)  NOT NULL,
    [decision_text] nvarchar(max)  NOT NULL,
    [decision_date] time  NOT NULL,
    [email] nvarchar(max)  NOT NULL,
    [au] nvarchar(max)  NOT NULL,
    [Author_id_author] int  NOT NULL
);
GO

-- Creating table 'Subreviewers'
CREATE TABLE [dbo].[Subreviewers] (
    [id_subreviewer] int IDENTITY(1,1) NOT NULL,
    [id_paperassignment] int  NOT NULL,
    [id_user] int  NOT NULL,
    [invitation_send_date] datetime  NOT NULL,
    [invitation_ack] nvarchar(max)  NOT NULL,
    [is_accepted] bit  NOT NULL,
    [User_id_user] int  NOT NULL,
    [PaperAssignment_id_paper_assignment] int  NOT NULL
);
GO

-- Creating table 'Subreviews1'
CREATE TABLE [dbo].[Subreviews1] (
    [id_subreview] int IDENTITY(1,1) NOT NULL,
    [id_subreviewer] int  NOT NULL,
    [grade] int  NOT NULL,
    [confidence] int  NOT NULL,
    [comments] nvarchar(max)  NOT NULL,
    [comment_to_edit] nvarchar(max)  NOT NULL,
    [date_submitted] datetime  NOT NULL,
    [Subreviewer_id_subreviewer] int  NOT NULL
);
GO

-- Creating table 'PaperAssignments'
CREATE TABLE [dbo].[PaperAssignments] (
    [id_paper_assignment] int IDENTITY(1,1) NOT NULL,
    [id_paper] int  NOT NULL,
    [id_pcmember] int  NOT NULL,
    [date_assigned] datetime  NOT NULL,
    [date_due] datetime  NOT NULL,
    [is_delegated] bit  NOT NULL,
    [PCmember_id_pcmember] int  NOT NULL,
    [Paper_id_paper] int  NOT NULL
);
GO

-- Creating table 'Reviews'
CREATE TABLE [dbo].[Reviews] (
    [id_review] int IDENTITY(1,1) NOT NULL,
    [id_paper_assignment] int  NOT NULL,
    [grade] int  NOT NULL,
    [confidence] int  NOT NULL,
    [comment] nvarchar(max)  NOT NULL,
    [comment_to_edit] nvarchar(max)  NOT NULL,
    [date_submitted] nvarchar(max)  NOT NULL,
    [from_subreviewer] nvarchar(max)  NOT NULL,
    [PaperAssignment_id_paper_assignment] int  NOT NULL
);
GO

-- Creating table 'Authors'
CREATE TABLE [dbo].[Authors] (
    [id_author] int IDENTITY(1,1) NOT NULL,
    [id_paper] int  NOT NULL,
    [is_coresponding] bit  NOT NULL,
    [User_id_user] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [id_user] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([id_user] ASC);
GO

-- Creating primary key on [id_conference] in table 'Conferences'
ALTER TABLE [dbo].[Conferences]
ADD CONSTRAINT [PK_Conferences]
    PRIMARY KEY CLUSTERED ([id_conference] ASC);
GO

-- Creating primary key on [id_pcmember] in table 'PCmembers'
ALTER TABLE [dbo].[PCmembers]
ADD CONSTRAINT [PK_PCmembers]
    PRIMARY KEY CLUSTERED ([id_pcmember] ASC);
GO

-- Creating primary key on [id_paper] in table 'Papers'
ALTER TABLE [dbo].[Papers]
ADD CONSTRAINT [PK_Papers]
    PRIMARY KEY CLUSTERED ([id_paper] ASC);
GO

-- Creating primary key on [id_subreviewer] in table 'Subreviewers'
ALTER TABLE [dbo].[Subreviewers]
ADD CONSTRAINT [PK_Subreviewers]
    PRIMARY KEY CLUSTERED ([id_subreviewer] ASC);
GO

-- Creating primary key on [id_subreview] in table 'Subreviews1'
ALTER TABLE [dbo].[Subreviews1]
ADD CONSTRAINT [PK_Subreviews1]
    PRIMARY KEY CLUSTERED ([id_subreview] ASC);
GO

-- Creating primary key on [id_paper_assignment] in table 'PaperAssignments'
ALTER TABLE [dbo].[PaperAssignments]
ADD CONSTRAINT [PK_PaperAssignments]
    PRIMARY KEY CLUSTERED ([id_paper_assignment] ASC);
GO

-- Creating primary key on [id_review] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [PK_Reviews]
    PRIMARY KEY CLUSTERED ([id_review] ASC);
GO

-- Creating primary key on [id_author] in table 'Authors'
ALTER TABLE [dbo].[Authors]
ADD CONSTRAINT [PK_Authors]
    PRIMARY KEY CLUSTERED ([id_author] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Chair_id_user] in table 'Conferences'
ALTER TABLE [dbo].[Conferences]
ADD CONSTRAINT [FK_ConferenceChair]
    FOREIGN KEY ([Chair_id_user])
    REFERENCES [dbo].[Users]
        ([id_user])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ConferenceChair'
CREATE INDEX [IX_FK_ConferenceChair]
ON [dbo].[Conferences]
    ([Chair_id_user]);
GO

-- Creating foreign key on [User_id_user] in table 'PCmembers'
ALTER TABLE [dbo].[PCmembers]
ADD CONSTRAINT [FK_PCmembersUser]
    FOREIGN KEY ([User_id_user])
    REFERENCES [dbo].[Users]
        ([id_user])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PCmembersUser'
CREATE INDEX [IX_FK_PCmembersUser]
ON [dbo].[PCmembers]
    ([User_id_user]);
GO

-- Creating foreign key on [Conference_id_conference] in table 'PCmembers'
ALTER TABLE [dbo].[PCmembers]
ADD CONSTRAINT [FK_PCmemberConference]
    FOREIGN KEY ([Conference_id_conference])
    REFERENCES [dbo].[Conferences]
        ([id_conference])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PCmemberConference'
CREATE INDEX [IX_FK_PCmemberConference]
ON [dbo].[PCmembers]
    ([Conference_id_conference]);
GO

-- Creating foreign key on [User_id_user] in table 'Subreviewers'
ALTER TABLE [dbo].[Subreviewers]
ADD CONSTRAINT [FK_UserSubreviewer]
    FOREIGN KEY ([User_id_user])
    REFERENCES [dbo].[Users]
        ([id_user])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserSubreviewer'
CREATE INDEX [IX_FK_UserSubreviewer]
ON [dbo].[Subreviewers]
    ([User_id_user]);
GO

-- Creating foreign key on [Subreviewer_id_subreviewer] in table 'Subreviews1'
ALTER TABLE [dbo].[Subreviews1]
ADD CONSTRAINT [FK_SubreviewSubreviewer]
    FOREIGN KEY ([Subreviewer_id_subreviewer])
    REFERENCES [dbo].[Subreviewers]
        ([id_subreviewer])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubreviewSubreviewer'
CREATE INDEX [IX_FK_SubreviewSubreviewer]
ON [dbo].[Subreviews1]
    ([Subreviewer_id_subreviewer]);
GO

-- Creating foreign key on [PCmember_id_pcmember] in table 'PaperAssignments'
ALTER TABLE [dbo].[PaperAssignments]
ADD CONSTRAINT [FK_PaperAssignmentPCmember]
    FOREIGN KEY ([PCmember_id_pcmember])
    REFERENCES [dbo].[PCmembers]
        ([id_pcmember])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaperAssignmentPCmember'
CREATE INDEX [IX_FK_PaperAssignmentPCmember]
ON [dbo].[PaperAssignments]
    ([PCmember_id_pcmember]);
GO

-- Creating foreign key on [PaperAssignment_id_paper_assignment] in table 'Subreviewers'
ALTER TABLE [dbo].[Subreviewers]
ADD CONSTRAINT [FK_PaperAssignmentSubreviewer]
    FOREIGN KEY ([PaperAssignment_id_paper_assignment])
    REFERENCES [dbo].[PaperAssignments]
        ([id_paper_assignment])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaperAssignmentSubreviewer'
CREATE INDEX [IX_FK_PaperAssignmentSubreviewer]
ON [dbo].[Subreviewers]
    ([PaperAssignment_id_paper_assignment]);
GO

-- Creating foreign key on [Paper_id_paper] in table 'PaperAssignments'
ALTER TABLE [dbo].[PaperAssignments]
ADD CONSTRAINT [FK_PaperPaperAssignment]
    FOREIGN KEY ([Paper_id_paper])
    REFERENCES [dbo].[Papers]
        ([id_paper])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaperPaperAssignment'
CREATE INDEX [IX_FK_PaperPaperAssignment]
ON [dbo].[PaperAssignments]
    ([Paper_id_paper]);
GO

-- Creating foreign key on [PaperAssignment_id_paper_assignment] in table 'Reviews'
ALTER TABLE [dbo].[Reviews]
ADD CONSTRAINT [FK_PaperAssignmentReview]
    FOREIGN KEY ([PaperAssignment_id_paper_assignment])
    REFERENCES [dbo].[PaperAssignments]
        ([id_paper_assignment])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaperAssignmentReview'
CREATE INDEX [IX_FK_PaperAssignmentReview]
ON [dbo].[Reviews]
    ([PaperAssignment_id_paper_assignment]);
GO

-- Creating foreign key on [User_id_user] in table 'Authors'
ALTER TABLE [dbo].[Authors]
ADD CONSTRAINT [FK_UserAuthor]
    FOREIGN KEY ([User_id_user])
    REFERENCES [dbo].[Users]
        ([id_user])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_UserAuthor'
CREATE INDEX [IX_FK_UserAuthor]
ON [dbo].[Authors]
    ([User_id_user]);
GO

-- Creating foreign key on [Author_id_author] in table 'Papers'
ALTER TABLE [dbo].[Papers]
ADD CONSTRAINT [FK_AuthorPaper]
    FOREIGN KEY ([Author_id_author])
    REFERENCES [dbo].[Authors]
        ([id_author])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AuthorPaper'
CREATE INDEX [IX_FK_AuthorPaper]
ON [dbo].[Papers]
    ([Author_id_author]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------