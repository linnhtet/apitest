USE [test_db]
GO
/****** Object:  StoredProcedure [dbo].[GetMailsByLabel]    Script Date: 7/14/2023 2:25:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create procedure [dbo].[GetMailsByLabel] 
( @UserID int ,
@LabelID int
)
as
set nocount on;
 select m.Id, su.StaffName as SendingStaffName, su.StaffEmail as SendingStaffEmail, ru.StaffName as ReceivingStaffName, ru.StaffEmail as ReceivingStaffEmail, m.Subject, m.Message, m.SentTime, m.SentSuccessToSMTPServer, m.[Read], m.Starred, m.Important, m.HasAttachments, m.[Label], m.Folder 
                from Mail m
                left join Users su on su.UserID = m.SendingUserID
                left join Users ru on ru.UserID = m.ReceivingUserID
                where (m.SendingUserID = @UserID OR m.ReceivingUserID = @UserID)
                and m.Label = @LabelID
                order by m.SentTime desc