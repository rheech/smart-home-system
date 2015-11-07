VERSION 5.00
Object = "{248DD890-BB45-11CF-9ABC-0080C7E7B78D}#1.0#0"; "MSWINSCK.OCX"
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3030
   ClientLeft      =   120
   ClientTop       =   450
   ClientWidth     =   4560
   LinkTopic       =   "Form1"
   ScaleHeight     =   3030
   ScaleWidth      =   4560
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton Command2 
      Caption         =   "Command2"
      Height          =   495
      Left            =   2760
      TabIndex        =   1
      Top             =   600
      Width           =   1215
   End
   Begin MSWinsockLib.Winsock Winsock1 
      Left            =   3840
      Top             =   2280
      _ExtentX        =   741
      _ExtentY        =   741
      _Version        =   393216
   End
   Begin VB.CommandButton Command1 
      Caption         =   "Command1"
      Height          =   495
      Left            =   1680
      TabIndex        =   0
      Top             =   1320
      Width           =   1215
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Private Sub Command1_Click()
    Winsock1.Close
    Winsock1.Connect "127.0.0.1", 5000
End Sub

Private Sub Command2_Click()
    Dim a As Long
    a = 6
    
    Dim s(2) As Variant
    s(0) = a
    s(1) = "Hello"
    
    Dim kk(8) As Byte
    kk(0) = 5
    kk(1) = 0
    kk(2) = 0
    kk(3) = 0
    kk(4) = Asc("A")
    kk(5) = Asc("S")
    kk(6) = Asc("D")
    kk(7) = Asc("F")
    kk(8) = 0

    Winsock1.SendData kk
    
End Sub

Private Sub Winsock1_Error(ByVal Number As Integer, Description As String, ByVal Scode As Long, ByVal Source As String, ByVal HelpFile As String, ByVal HelpContext As Long, CancelDisplay As Boolean)
    MsgBox Description
End Sub
