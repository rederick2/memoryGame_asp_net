<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemoryGame.aspx.cs" Inherits="WebApplicationCartas.MemoryGame" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Juego de Memoria</title>
    <style>
        .grid {
            display: grid;
            grid-template-columns: repeat(4, 100px);
            grid-gap: 10px;
        }
        .card {
            width: 100px;
            height: 100px;
            background-color: lightgray;
            font-size: 24px;
            display: flex;
            justify-content: center;
            align-items: center;
            cursor: pointer;
        }
        
    </style>
    <script>
    function resetCards(firstCard, secondCard) {
        setTimeout(function () {
            document.getElementById(firstCard).innerText = '?';
            document.getElementById(secondCard).innerText = '?';
        }, 1000);

        console.log('primera carta: ' + firstCard)
    }
    </script>
</head>
<body>
   <form id="form1" runat="server">
        <div class="grid" id="gameGrid" runat="server">
        </div>
       <br />
       <asp:Button ID="ResetButton" runat="server" Text="Reiniciar" OnClick="ResetButton_Click" />
       <table>
            <tr>
                <td>Cartas volteadas:</td>
                <td><asp:Label ID="lblFlippedCards" runat="server" Text="0"></asp:Label></td>
            </tr>
            <tr>
                <td>Pares encontrados:</td>
                <td><asp:Label ID="lblFoundPairs" runat="server" Text="0"></asp:Label></td>
            </tr>
            <tr>
                <td>Cartas faltantes:</td>
                <td><asp:Label ID="lblRemainingCards" runat="server" Text="16"></asp:Label></td>
            </tr>
            <tr>
                <td>Pares faltantes:</td>
                <td><asp:Label ID="lblRemainingPairs" runat="server" Text="8"></asp:Label></td>
            </tr>
        </table>
    </form>
</body>
</html>