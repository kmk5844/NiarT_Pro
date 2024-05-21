using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("mercenary_num", "level_engine_driver", "level_engineer", "level_long_ranged", "level_short_ranged", "level_medic", "engine_driver_type", "mercenary_buy_num", "mercenary_head_image")]
	public class ES3UserType_SA_MercenaryData : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SA_MercenaryData() : base(typeof(SA_MercenaryData)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (SA_MercenaryData)obj;
			
			writer.WritePrivateField("mercenary_num", instance);
			writer.WritePrivateField("level_engine_driver", instance);
			writer.WritePrivateField("level_engineer", instance);
			writer.WritePrivateField("level_long_ranged", instance);
			writer.WritePrivateField("level_short_ranged", instance);
			writer.WritePrivateField("level_medic", instance);
			writer.WritePrivateField("engine_driver_type", instance);
			writer.WritePrivateField("mercenary_buy_num", instance);
			writer.WritePrivateField("mercenary_head_image", instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (SA_MercenaryData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "mercenary_num":
					instance = (SA_MercenaryData)reader.SetPrivateField("mercenary_num", reader.Read<System.Collections.Generic.List<System.Int32>>(), instance);
					break;
					case "level_engine_driver":
					instance = (SA_MercenaryData)reader.SetPrivateField("level_engine_driver", reader.Read<System.Int32>(), instance);
					break;
					case "level_engineer":
					instance = (SA_MercenaryData)reader.SetPrivateField("level_engineer", reader.Read<System.Int32>(), instance);
					break;
					case "level_long_ranged":
					instance = (SA_MercenaryData)reader.SetPrivateField("level_long_ranged", reader.Read<System.Int32>(), instance);
					break;
					case "level_short_ranged":
					instance = (SA_MercenaryData)reader.SetPrivateField("level_short_ranged", reader.Read<System.Int32>(), instance);
					break;
					case "level_medic":
					instance = (SA_MercenaryData)reader.SetPrivateField("level_medic", reader.Read<System.Int32>(), instance);
					break;
					case "engine_driver_type":
					instance = (SA_MercenaryData)reader.SetPrivateField("engine_driver_type", reader.Read<Engine_Driver_Type>(), instance);
					break;
					case "mercenary_buy_num":
					instance = (SA_MercenaryData)reader.SetPrivateField("mercenary_buy_num", reader.Read<System.Collections.Generic.List<System.Int32>>(), instance);
					break;
					case "mercenary_head_image":
					instance = (SA_MercenaryData)reader.SetPrivateField("mercenary_head_image", reader.Read<UnityEngine.Sprite[]>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SA_MercenaryDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SA_MercenaryDataArray() : base(typeof(SA_MercenaryData[]), ES3UserType_SA_MercenaryData.Instance)
		{
			Instance = this;
		}
	}
}