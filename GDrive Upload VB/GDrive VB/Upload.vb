Imports Google.Apis.Auth
Imports Google.Apis.Download

' Your original code was missing the following "Imports":
Imports Google.Apis.Drive.v2
Imports Google.Apis.Auth.OAuth2
Imports Google.Apis.Services
Imports System.Threading
Imports Google.Apis.Drive.v2.Data

Module Upload

    Sub Main()
        If My.Computer.Network.IsAvailable = True Then
            Dim FilePath As String = "C:\Users\Jay\Downloads\super fast trackers.txt"
            UploadFile(FilePath)
        Else
            MsgBox("Check your Internet Connection! and try again...")
        End If
    End Sub

    Private Service As DriveService = New DriveService

    Private Sub CreateService()
        Dim ClientId = "983365912286-sh8ledv89ichhi44qneojh9fh2bqceej.apps.googleusercontent.com"
        Dim ClientSecret = "5djZp6WaOqb4hJSva0ytRKpZ"
        Dim MyUserCredential As UserCredential = GoogleWebAuthorizationBroker.AuthorizeAsync(New ClientSecrets() With {.ClientId = ClientId, .ClientSecret = ClientSecret}, {DriveService.Scope.Drive}, "user", CancellationToken.None).Result
        Service = New DriveService(New BaseClientService.Initializer() With {.HttpClientInitializer = MyUserCredential, .ApplicationName = "Google Drive VB Dot Net"})
    End Sub

    Private Sub UploadFile(FilePath As String)

        If Service.ApplicationName <> "Google Drive VB Dot Net" Then CreateService()

        Dim TheFile As New File()
        TheFile.Title = "My document"
        TheFile.Description = "Created By RavitechWorld"
        TheFile.MimeType = "text/plain"

        Dim ByteArray As Byte() = System.IO.File.ReadAllBytes(FilePath)
        Dim Stream As New System.IO.MemoryStream(ByteArray)
        Dim FileID As String = "0B0obhE6yM5fbaEpjSTJHU1VST0U"
        Dim UploadRequest As FilesResource.UpdateMediaUpload = Service.Files.Update(TheFile, FileID, Stream, TheFile.MimeType)

        UploadRequest.Upload()
        Dim file As File = UploadRequest.ResponseBody

        MsgBox("Upload Finished")
    End Sub
End Module