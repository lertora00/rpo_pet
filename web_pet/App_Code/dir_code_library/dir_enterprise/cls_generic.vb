Imports ns_enterprise

Imports System
Imports System.Collections.Generic
Imports System.Collections
Imports System.Text
Imports System.Reflection

Namespace ns_enterprise
  ''' <summary>
  ''' This class is used to compare any 
  ''' type(property) of a class for sorting.
  ''' This class automatically fetches the 
  ''' type of the property and compares.
  ''' </summary>

  Public NotInheritable Class GenericComparer(Of T)

    Implements IComparer(Of T)

    Public Enum en_sort_order
      ascending
      descending
    End Enum

    Private str__prv_sort_column As String
    Private so__prv_sort_order As en_sort_order

    Public Sub New(ByVal str__prv_sort_column As String, ByVal so__prv_sort_order As en_sort_order)

      Me.str__prv_sort_column = str__prv_sort_column
      Me.so__prv_sort_order = so__prv_sort_order

    End Sub

    ''' <summary>
    ''' Column Name(public property of the class) to be sorted.
    ''' </summary>

    Public ReadOnly Property str_sort_column() As String

      Get
        Return str__prv_sort_column
      End Get

    End Property

    ''' <summary>
    ''' Sorting order.
    ''' </summary>

    Public ReadOnly Property SortingOrder() As en_sort_order

      Get
        Return so__prv_sort_order
      End Get

    End Property

    ''' <summary>
    ''' Compare interface implementation
    ''' </summary>
    ''' <param name="x">First Object</param>
    ''' <param name="y">Second Object</param>
    ''' <returns>Result of comparison</returns>

    Public Function Compare(ByVal x As T, ByVal y As T) As Integer Implements IComparer(Of T).Compare

      Dim propertyInfo As PropertyInfo = GetType(T).GetProperty(str__prv_sort_column)
      Dim obj1 As IComparable = CType(propertyInfo.GetValue(x, Nothing), IComparable)
      Dim obj2 As IComparable = CType(propertyInfo.GetValue(y, Nothing), IComparable)

      If so__prv_sort_order = en_sort_order.Ascending Then
        Return (obj1.CompareTo(obj2))
      Else
        Return (obj2.CompareTo(obj1))
      End If

    End Function

  End Class

End Namespace



