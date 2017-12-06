Imports System.Web
Imports System.Web.Routing
Imports Microsoft.AspNet.FriendlyUrls
Imports Microsoft.AspNet.FriendlyUrls.Resolvers

Public Partial Class ViewSwitcher
    Inherits System.Web.UI.UserControl
    Protected Property CurrentView As String
    Protected Property AlternateView As String
    Protected Property SwitchUrl As String

    Protected Sub Page_Load(sender As Object, e As EventArgs)
        ' 确定当前视图
        Dim isMobile = WebFormsFriendlyUrlResolver.IsMobileView(New HttpContextWrapper(Context))
        CurrentView = If(isMobile, "Mobile", "Desktop")

        ' 确定备用视图
        AlternateView = If(isMobile, "Desktop", "Mobile")

        ' 从路由创建交换机 URL，例如 ~/__FriendlyUrls_SwitchView/Mobile?ReturnUrl=/Page
        Dim switchViewRouteName = "AspNet.FriendlyUrls.SwitchView"
        Dim switchViewRoute = RouteTable.Routes(switchViewRouteName)
        If switchViewRoute Is Nothing Then
            ' 友好 URL 未启用，或者交换机视图路由的名称不同步
            Me.Visible = False
            Return
        End If
        Dim url = GetRouteUrl(switchViewRouteName, New With {
            .view = AlternateView,
            .__FriendlyUrls_SwitchViews = True
        })
        url += "?ReturnUrl=" & HttpUtility.UrlEncode(Request.RawUrl)
        SwitchUrl = url
    End Sub
End Class
