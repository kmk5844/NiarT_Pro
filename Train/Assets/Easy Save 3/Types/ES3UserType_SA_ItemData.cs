using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("EmptyObject", "equiped_item", "equiped_item_count")]
	public class ES3UserType_SA_ItemData : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SA_ItemData() : base(typeof(SA_ItemData)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (SA_ItemData)obj;
			
			writer.WritePropertyByRef("EmptyObject", instance.EmptyObject);
			writer.WritePrivateField("equiped_item", instance);
			writer.WritePrivateField("equiped_item_count", instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (SA_ItemData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "EmptyObject":
						instance.EmptyObject = reader.Read<ItemDataObject>(ES3UserType_ItemDataObject.Instance);
						break;
					case "equiped_item":
					instance = (SA_ItemData)reader.SetPrivateField("equiped_item", reader.Read<System.Collections.Generic.List<System.Int32>>(), instance);
					break;
					case "equiped_item_count":
					instance = (SA_ItemData)reader.SetPrivateField("equiped_item_count", reader.Read<System.Collections.Generic.List<System.Int32>>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SA_ItemDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SA_ItemDataArray() : base(typeof(SA_ItemData[]), ES3UserType_SA_ItemData.Instance)
		{
			Instance = this;
		}
	}
}