using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("EX_GameData", "player_num", "level_atk", "level_atkdelay", "level_hp", "level_armor", "level_speed", "coin", "point", "select_stage")]
	public class ES3UserType_SA_PlayerData : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SA_PlayerData() : base(typeof(SA_PlayerData)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (SA_PlayerData)obj;
			
			writer.WritePrivateFieldByRef("EX_GameData", instance);
			writer.WritePrivateField("player_num", instance);
			writer.WritePrivateField("level_atk", instance);
			writer.WritePrivateField("level_atkdelay", instance);
			writer.WritePrivateField("level_hp", instance);
			writer.WritePrivateField("level_armor", instance);
			writer.WritePrivateField("level_speed", instance);
			writer.WritePrivateField("coin", instance);
			writer.WritePrivateField("point", instance);
			writer.WritePrivateField("select_stage", instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (SA_PlayerData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "EX_GameData":
					instance = (SA_PlayerData)reader.SetPrivateField("EX_GameData", reader.Read<Game_DataTable>(), instance);
					break;
					case "player_num":
					instance = (SA_PlayerData)reader.SetPrivateField("player_num", reader.Read<System.Int32>(), instance);
					break;
					case "level_atk":
					instance = (SA_PlayerData)reader.SetPrivateField("level_atk", reader.Read<System.Int32>(), instance);
					break;
					case "level_atkdelay":
					instance = (SA_PlayerData)reader.SetPrivateField("level_atkdelay", reader.Read<System.Int32>(), instance);
					break;
					case "level_hp":
					instance = (SA_PlayerData)reader.SetPrivateField("level_hp", reader.Read<System.Int32>(), instance);
					break;
					case "level_armor":
					instance = (SA_PlayerData)reader.SetPrivateField("level_armor", reader.Read<System.Int32>(), instance);
					break;
					case "level_speed":
					instance = (SA_PlayerData)reader.SetPrivateField("level_speed", reader.Read<System.Int32>(), instance);
					break;
					case "coin":
					instance = (SA_PlayerData)reader.SetPrivateField("coin", reader.Read<System.Int32>(), instance);
					break;
					case "point":
					instance = (SA_PlayerData)reader.SetPrivateField("point", reader.Read<System.Int32>(), instance);
					break;
					case "select_stage":
					instance = (SA_PlayerData)reader.SetPrivateField("select_stage", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SA_PlayerDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SA_PlayerDataArray() : base(typeof(SA_PlayerData[]), ES3UserType_SA_PlayerData.Instance)
		{
			Instance = this;
		}
	}
}