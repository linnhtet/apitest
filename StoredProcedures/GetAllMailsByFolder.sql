USE [test_db]
GO
/****** Object:  StoredProcedure [dbo].[GetAllMailsByFolder]    Script Date: 7/14/2023 2:25:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[GetAllMailsByFolder] 
(
@FolderID int ,
@userID int
)
as
set nocount on;
IF @FolderID = 0
                    select m.Id, su.StaffName as SendingStaffName, su.StaffEmail as SendingStaffEmail, ru.StaffName as ReceivingStaffName,
                    ru.StaffEmail as ReceivingStaffEmail, m.Subject, m.Message, m.SentTime, m.SentSuccessToSMTPServer, m.[Read], m.Starred, m.Important, m.HasAttachments, m.[Label], @FolderID as 'Folder'
                    from Mail m
                    left join Users su on m.SendingUserID = su.UserID
                    left join Users ru on m.ReceivingUserID = ru.UserID
                    where m.SendingUserID = @UserID
                    order by m.SentTime desc

                ELSE
                    select m.Id,
                    su.StaffName as SendingStaffName, su.StaffEmail as SendingStaffEmail, ru.StaffName as ReceivingStaffName, ru.StaffEmail as ReceivingStaffEmail,	m.Subject, m.Message, m.SentTime, m.SentSuccessToSMTPServer, m.[Read],	m.Starred, m.Important,	m.HasAttachments, m.[Label], @FolderID as 'Folder'
                    from Mail m
                    left join Users su on su.userID = m.SendingUserID
                    left join Users ru on ru.userID = m.ReceivingUserID
                    where m.ReceivingUserID = @UserID