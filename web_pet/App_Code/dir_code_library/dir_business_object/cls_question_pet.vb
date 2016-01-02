Imports ns_enterprise.cls_utility
Imports System.Data

Namespace ns_enterprise

	Public Class cls_question_pet

		Public Shared Function fnc_get_question__initial() As DataRow

			Return cls_data_access_layer.fnc_get_datarow("select top 1 pk_question_pet, question from tbl_question_pet order by sort_order")

		End Function

	End Class

End Namespace