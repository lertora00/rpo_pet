Imports ns_enterprise

Imports System
Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class cls_Encryption
  Public Function EncryptString128Bit(ByVal vstrTextToBeEncrypted As String, ByVal vstrEncryptionKey As String) As String

    Dim bytValue() As Byte
    Dim bytKey() As Byte
    Dim bytEncoded() As Byte
    Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
    Dim intLength As Integer
    Dim intRemaining As Integer
    Dim objMemoryStream As New MemoryStream()
    Dim objCryptoStream As CryptoStream
    Dim objRijndaelManaged As RijndaelManaged

    '**********************************************************************
    '   ******  Strip any null character from string to be encrypted    ******
    '    
    '**********************************************************************

    vstrTextToBeEncrypted = StripNullCharacters(vstrTextToBeEncrypted)

    '    
    '**********************************************************************
    '   ******  Value must be within ASCII range (i.e., no DBCS chars)  ******
    '    
    '**********************************************************************

    bytValue = Encoding.ASCII.GetBytes(vstrTextToBeEncrypted.ToCharArray)

    intLength = Len(vstrEncryptionKey)

    '   ********************************************************************
    '   ******   Encryption Key must be 256 bits long (32 bytes)      ******
    '   ******   If it is longer than 32 bytes it will be truncated.  ******
    '   ******   If it is shorter than 32 bytes it will be padded     ******
    '   ******   with upper-case Xs.                                  ******
    '   ********************************************************************

    If intLength >= 32 Then
      vstrEncryptionKey = Strings.Left(vstrEncryptionKey, 32)
    Else
      intLength = Len(vstrEncryptionKey)
      intRemaining = 32 - intLength
      vstrEncryptionKey = vstrEncryptionKey & Strings.StrDup(intRemaining, "X")
    End If

    bytKey = Encoding.ASCII.GetBytes(vstrEncryptionKey.ToCharArray)

    objRijndaelManaged = New RijndaelManaged()

    '***********************************************************************
    '   ******  Create the encryptor and write value to it after it is   ******
    '   ******  converted into a byte array                              ******
    '    
    '***********************************************************************

    objCryptoStream = New CryptoStream(objMemoryStream, objRijndaelManaged.CreateEncryptor(bytKey, bytIV), CryptoStreamMode.Write)
    objCryptoStream.Write(bytValue, 0, bytValue.Length)

    objCryptoStream.FlushFinalBlock()

    bytEncoded = objMemoryStream.ToArray
    objMemoryStream.Close()
    objCryptoStream.Close()

    '***********************************************************************
    '   ******   Return encryptes value (converted from  byte Array to  ******
    '   ******   a base64 string).  Base64 is MIME encoding)             ******
    '    
    '***********************************************************************

    Return Convert.ToBase64String(bytEncoded)

  End Function

  Public Function DecryptString128Bit(ByVal vstrStringToBeDecrypted As String, ByVal vstrDecryptionKey As String) As String

    Dim bytDataToBeDecrypted() As Byte
    Dim bytTemp() As Byte
    Dim bytIV() As Byte = {121, 241, 10, 1, 132, 74, 11, 39, 255, 91, 45, 78, 14, 211, 22, 62}
    Dim objRijndaelManaged As New RijndaelManaged()
    Dim objMemoryStream As MemoryStream
    Dim objCryptoStream As CryptoStream
    Dim bytDecryptionKey() As Byte

    Dim intLength As Integer
    Dim intRemaining As Integer

    '*****************************************************************
    '******   Convert base64 encrypted value to byte array      ******
    '*****************************************************************

    bytDataToBeDecrypted = Convert.FromBase64String(vstrStringToBeDecrypted)

    '********************************************************************
    '   ******   Encryption Key must be 256 bits long (32 bytes)      ******
    '   ******   If it is longer than 32 bytes it will be truncated.  ******
    '   ******   If it is shorter than 32 bytes it will be padded     ******
    '   ******   with upper-case Xs.                                  ******
    '********************************************************************

    intLength = Len(vstrDecryptionKey)

    If intLength >= 32 Then
      vstrDecryptionKey = Strings.Left(vstrDecryptionKey, 32)
    Else
      intLength = Len(vstrDecryptionKey)
      intRemaining = 32 - intLength
      vstrDecryptionKey = vstrDecryptionKey & Strings.StrDup(intRemaining, "X")
    End If

    bytDecryptionKey = Encoding.ASCII.GetBytes(vstrDecryptionKey.ToCharArray)

    ReDim bytTemp(bytDataToBeDecrypted.Length)

    objMemoryStream = New MemoryStream(bytDataToBeDecrypted)

    '    
    '***********************************************************************
    '   ******  Create the decryptor and write value to it after it is   ******
    '   ******  converted into a byte array                              ******
    '    
    '***********************************************************************


    objCryptoStream = New CryptoStream(objMemoryStream, objRijndaelManaged.CreateDecryptor(bytDecryptionKey, bytIV), CryptoStreamMode.Read)
    objCryptoStream.Read(bytTemp, 0, bytTemp.Length)

    objMemoryStream.Close()
    objCryptoStream.Close()

    '   *****************************************
    '   ******   Return decypted value     ******
    '   *****************************************

    Return StripNullCharacters(Encoding.ASCII.GetString(bytTemp))

  End Function

  Public Function StripNullCharacters(ByVal vstrStringWithNulls As String) As String

    Dim intPosition As Integer
    Dim strStringWithOutNulls As String

    intPosition = 1
    strStringWithOutNulls = vstrStringWithNulls

    Do While intPosition > 0
      intPosition = InStr(intPosition, vstrStringWithNulls, vbNullChar)

      If intPosition > 0 Then
        strStringWithOutNulls = Left$(strStringWithOutNulls, intPosition - 1) & Right$(strStringWithOutNulls, Len(strStringWithOutNulls) - intPosition)
      End If

      If intPosition > strStringWithOutNulls.Length Then
        Exit Do
      End If
    Loop

    Return strStringWithOutNulls

  End Function

End Class