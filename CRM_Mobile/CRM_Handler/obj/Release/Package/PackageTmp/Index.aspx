<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CRM_Handler.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
   
<body>
    <form id="form1" runat="server">
        <div>
            <asp:FileUpload ID="FileUpload1" runat="server" Width="400px" />
            <label>请输入负责人名称</label>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <asp:Button ID="btnChange" runat="server" Text="提交信息" OnClick="btnChange_Click" />

            <asp:Label ForeColor="Red" ID="Label1" runat="server"></asp:Label>
        </div>
      
        <asp:Panel Width="100%" ID="Panel1" runat="server"></asp:Panel>
    </form>
</body>
</html>
