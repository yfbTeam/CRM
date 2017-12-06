Imports Owin
Imports Microsoft.Owin.Security.Cookies
Imports Microsoft.Owin

Partial Public Class Startup
    ' 有关配置身份验证的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=301883
    Public Sub ConfigureAuth(app As IAppBuilder)
        ' 使应用程序可以使用 Cookie 来存储已登录用户的信息
        ' 并且还存储使用第三方登录提供程序登录的用户的相关信息。
        ' 这是必填字段(如果你的应用程序允许用户登录)
        app.UseCookieAuthentication(New CookieAuthenticationOptions() With {
        .AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
        .LoginPath = New PathString("/Account/Login")})
        app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie)

        ' 取消注释以下行即可允许使用第三方登录提供程序登录
        'app.UseMicrosoftAccountAuthentication(
        '    clientId:= "",
        '    clientSecret:= "")

        'app.UseTwitterAuthentication(
        '   consumerKey:= "",
        '   consumerSecret:= "")

        'app.UseFacebookAuthentication(
        '   appId:= "",
        '   appSecret:= "")

        'app.UseGoogleAuthentication()
    End Sub
End Class
