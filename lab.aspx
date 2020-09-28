<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lab.aspx.cs" Inherits="GithubImageLite.lab" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">

        <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="true" />
        <br />
        <div>
            <asp:Button ID="Button3" runat="server" Text="File Upload" OnClick="Button3_Click" />
        </div>

    </form>
</body>
</html>
