<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="images.aspx.cs" Inherits="GithubImageLite.admin.images" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table>
                <tbody>
                    <tr><td>Image</td><td>Id</td><td>Repo</td><td>Size</td><td>Date</td></tr>
                    <asp:Literal ID="ltlC" runat="server"></asp:Literal>
               
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
