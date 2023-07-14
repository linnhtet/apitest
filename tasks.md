# Task-1

### Pagination Function
- [Controller->GetSentFolderAsync](https://github.com/linnhtet/apitest/blob/main/Controllers/V1/MailsController.cs#L115)
- [MailService->GetPagedMailsByFolderIDAsync](https://github.com/linnhtet/apitest/blob/main/Services/MailService.cs#L132)
- [Repository->GetPagedMailsByFolderAsync](https://github.com/linnhtet/apitest/blob/main/Repositories/MailRepository.cs#L36)
# Task-2

### Frontend Suggestions

 1. Frontend url with # seems odd , will be better if it's normal url structure that most of the user experienced
 2. Pagination only added in bottom of the page, user need to scroll down to see the pagination
 will be better if user can also paginate from top of the page like in Gmail
 3. if want to use bottom pagination only, should scroll up to the top of the page when page change to feel like data is refreshed.
 4. Using api url directly in service layer will hard to maintain if we have lot of api to use and if url structure need to be changed, and to avoid unintentional changes, better to separate them into Constant file 
*this._httpClient.get(`${environment.apiUrl}/v1/mails/folderData/${folderId}/${pageNumber}/${rowsOfPage}`)* 
5. Project structure is complex and not easy to find the file if developer is not familiar , should have standard scaffold project structure to make every project same structure and every developer familiar and walk in to check

### Backend Suggestions

 1. Having Data context, Database Activities and hardcoded **Raw SQL Queries** in Controller and Service layer will make the codes hard to maintain and test , if we want to use another database provider or if we want to test business logic, Controller should only be responsible for Accepting user requests and Response result to user, Service or Business Layer should only be responsible to perform business logics and calculation, should add Data layer to communicate to Database and perform database activities.
 2. Token Generation functionalities and Reading functions are unnecessarily bounded to UserService class , token reading methods are static and used frequently, it's better to separate them into a static helper class
 UserController response a lots of plain text user information to UI and information are stored in browser local storage, instead we can add more user info into Token and should only use Token to get user info.
 3. Every time we want to get User info from Token we have to call a token reading function UserService, instead of doing that we can read token and pass user info to every controller since the request comes in
 4. Frequently use database activities like add to the context and commit the changes are implemented in Service layer, they should be in Repository layer and make them base for every repository.
 5. Interfaces and interface implementation should be separated into relevant folders , Enum type should also be in its own folder, to make file structure clean and manageable and easy to find
 6. having 2 classes in a file is also make the file messy and hard to find
 7. API should be versioned, separating apis with it own version of api and responses will make us easy to maintain in long run, it's much suitable if we are also using api in Mobile native app that hard to update
 8. Entity class like mail should only be used to entry and store data from database, functions like SendMail should not be in entity class, it will be better if we implement Mail sending function in MailModel class, it also has necessary information such us **SendingStaffEmail, ReceivingStaffEmail** to create and send a mail.
 9. should Upgrade .netcore 3.1 to .net6 , .net6 brings more performance improvement and especially .netcore 3.1 is **out of support**
 
 

# Task-3

### Suggestion Implementation

#### Frontend Updates

 1. Changed Location Strategy to be normal url structure without #, saw a comment about getting 404 when page load, we could find a way to fix it by enabling url rewrite module and adding url rewrite rules to web server config 
[Url structure update](https://github.com/linnhtet/uitest/commit/9431fda7ccb1e4f2105b4d7e2b82277ab4da1fb9#diff-502d295b435b10ba99f5377504ca51149312e3cbebbbaa78ac5d925cb04cbfb0)
 2.  Add pagination control at the top of the mail list.
 [Pagination added](https://github.com/linnhtet/uitest/commit/9431fda7ccb1e4f2105b4d7e2b82277ab4da1fb9#diff-2d7001fc79ffc9e882444edb9448739ecfdfeeb35c481eb36ebb874b523b448b)
#### Backend Updates
1.  New Database Layer to separate DB activities from Controller and Service Layer,To avoid hardcoded SQL and run query at db level, Created Store procedures 
     - [New DB Layer](https://github.com/linnhtet/apitest/tree/main/Repositories)
      - [Stored Procedure Scripts](https://github.com/linnhtet/apitest/tree/main/StoredProcedures)
      - [Stored Procedure Usage](https://github.com/linnhtet/apitest/blob/main/Repositories/MailRepository.cs#L101)
      - [Service layer update for stored procedure](https://github.com/linnhtet/apitest/blob/main/Services/MailService.cs#L181)
2. Extract token related codes form UserService class to HelperClass
    - [TokenHelper](https://github.com/linnhtet/apitest/blob/main/Helpers/TokenHelper.cs)
    - [Adding new claims](https://github.com/linnhtet/apitest/blob/main/Helpers/TokenHelper.cs#L70)
    - [New custom claim types](https://github.com/linnhtet/apitest/blob/main/Helpers/CustomClaimTypes.cs#L9)
3. To use token info without calling token reading function again and again and to get more info from token
    - [New way of token info retrieving](https://github.com/linnhtet/apitest/blob/main/Controllers/V1/BaseController.cs#L23)
    - [New way of token info usage](https://github.com/linnhtet/apitest/blob/main/Controllers/V1/MailsController.cs#L120)
4. New functions to achieve db entry and save changes for every repo
    - [BaseRepository](https://github.com/linnhtet/apitest/blob/main/Repositories/Abstract/BaseRepository.cs)
    - [Implementation of Base Repository functions](https://github.com/linnhtet/apitest/blob/main/Repositories/MailRepository.cs#L119)
    - [Base Repository Method usage](https://github.com/linnhtet/apitest/blob/main/Services/MailService.cs#L102)
5. Interfaces and Enum in separate folders
    - [Repository Interfaces](https://github.com/linnhtet/apitest/tree/main/Repositories/Interfaces)
    - [Service Interfaces](https://github.com/linnhtet/apitest/tree/main/Services/Interfaces)
    - [Enum](https://github.com/linnhtet/apitest/tree/main/Helpers/Enum)
6. having 2 classes in a file is also make the file messy and hard to find
    - [Mail and Paged Mail Models](https://github.com/linnhtet/apitest/tree/main/Models/Messaging)
 7. Api should be versioned to be easy to maintain and mange, should use asp.net versioning package and configuration and attribute later to version api automatically
    -  [Versioned API and Response](https://github.com/linnhtet/apitest/tree/main/Controllers/V1)
