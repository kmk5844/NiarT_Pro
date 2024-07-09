using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("num", "item_id", "item_name", "item_type", "item_information", "box_type", "item_rarity_type", "use_flag", "buy_flag", "sell_flag", "item_buy_pride", "item_sell_pride", "supply_monster", "max_equip", "item_count")]
	public class ES3UserType_ItemDataObject : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_ItemDataObject() : base(typeof(ItemDataObject)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (ItemDataObject)obj;
			
			writer.WritePrivateField("num", instance);
			writer.WritePrivateField("item_id", instance);
			writer.WritePrivateField("item_name", instance);
			writer.WritePrivateField("item_type", instance);
			writer.WritePrivateField("item_information", instance);
			writer.WritePrivateField("box_type", instance);
			writer.WritePrivateField("item_rarity_type", instance);
			writer.WritePrivateField("use_flag", instance);
			writer.WritePrivateField("buy_flag", instance);
			writer.WritePrivateField("sell_flag", instance);
			writer.WritePrivateField("item_buy_pride", instance);
			writer.WritePrivateField("item_sell_pride", instance);
			writer.WritePrivateField("supply_monster", instance);
			writer.WritePrivateField("max_equip", instance);
			writer.WritePrivateField("item_count", instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (ItemDataObject)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "num":
					instance = (ItemDataObject)reader.SetPrivateField("num", reader.Read<System.Int32>(), instance);
					break;
					case "item_id":
					instance = (ItemDataObject)reader.SetPrivateField("item_id", reader.Read<System.String>(), instance);
					break;
					case "item_name":
					instance = (ItemDataObject)reader.SetPrivateField("item_name", reader.Read<System.String>(), instance);
					break;
					case "item_type":
					instance = (ItemDataObject)reader.SetPrivateField("item_type", reader.Read<Information_Item_Type>(), instance);
					break;
					case "item_information":
					instance = (ItemDataObject)reader.SetPrivateField("item_information", reader.Read<System.String>(), instance);
					break;
					case "box_type":
					instance = (ItemDataObject)reader.SetPrivateField("box_type", reader.Read<Information_Item_Box_Type>(), instance);
					break;
					case "item_rarity_type":
					instance = (ItemDataObject)reader.SetPrivateField("item_rarity_type", reader.Read<Information_Item_Rarity_Type>(), instance);
					break;
					case "use_flag":
					instance = (ItemDataObject)reader.SetPrivateField("use_flag", reader.Read<System.Boolean>(), instance);
					break;
					case "buy_flag":
					instance = (ItemDataObject)reader.SetPrivateField("buy_flag", reader.Read<System.Boolean>(), instance);
					break;
					case "sell_flag":
					instance = (ItemDataObject)reader.SetPrivateField("sell_flag", reader.Read<System.Boolean>(), instance);
					break;
					case "item_buy_pride":
					instance = (ItemDataObject)reader.SetPrivateField("item_buy_pride", reader.Read<System.Int32>(), instance);
					break;
					case "item_sell_pride":
					instance = (ItemDataObject)reader.SetPrivateField("item_sell_pride", reader.Read<System.Int32>(), instance);
					break;
					case "supply_monster":
					instance = (ItemDataObject)reader.SetPrivateField("supply_monster", reader.Read<System.Boolean>(), instance);
					break;
					case "max_equip":
					instance = (ItemDataObject)reader.SetPrivateField("max_equip", reader.Read<System.Int32>(), instance);
					break;
					case "item_count":
					instance = (ItemDataObject)reader.SetPrivateField("item_count", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_ItemDataObjectArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_ItemDataObjectArray() : base(typeof(ItemDataObject[]), ES3UserType_ItemDataObject.Instance)
		{
			Instance = this;
		}
	}
}