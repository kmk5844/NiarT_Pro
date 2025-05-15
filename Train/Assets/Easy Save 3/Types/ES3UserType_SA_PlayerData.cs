using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("EX_GameData", "firstflag", "player_num", "level_atk", "level_atkdelay", "level_hp", "level_armor", "level_speed", "coin", "mission_num", "new_stage", "select_stage", "mission_playing", "before_sub_stage", "select_sub_stage", "simplestation", "story_num", "character_lockoff", "station_tutorial", "eventflag", "food_heal_flag", "food_num", "name")]
	public class ES3UserType_SA_PlayerData : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SA_PlayerData() : base(typeof(SA_PlayerData)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (SA_PlayerData)obj;
			
			writer.WritePrivateFieldByRef("EX_GameData", instance);
			writer.WritePrivateField("firstflag", instance);
			writer.WritePrivateField("player_num", instance);
			writer.WritePrivateField("level_atk", instance);
			writer.WritePrivateField("level_atkdelay", instance);
			writer.WritePrivateField("level_hp", instance);
			writer.WritePrivateField("level_armor", instance);
			writer.WritePrivateField("level_speed", instance);
			writer.WritePrivateField("coin", instance);
			writer.WritePrivateField("mission_num", instance);
			writer.WritePrivateField("new_stage", instance);
			writer.WritePrivateField("select_stage", instance);
			writer.WritePrivateField("mission_playing", instance);
			writer.WritePrivateField("before_sub_stage", instance);
			writer.WritePrivateField("select_sub_stage", instance);
			writer.WritePrivateField("simplestation", instance);
			writer.WritePrivateField("story_num", instance);
			writer.WritePrivateField("character_lockoff", instance);
			writer.WritePrivateField("station_tutorial", instance);
			writer.WritePrivateField("eventflag", instance);
			writer.WritePrivateField("food_heal_flag", instance);
			writer.WritePrivateField("food_num", instance);
			writer.WriteProperty("name", instance.name, ES3Type_string.Instance);
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
					case "firstflag":
					instance = (SA_PlayerData)reader.SetPrivateField("firstflag", reader.Read<System.Boolean>(), instance);
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
					case "mission_num":
					instance = (SA_PlayerData)reader.SetPrivateField("mission_num", reader.Read<System.Int32>(), instance);
					break;
					case "new_stage":
					instance = (SA_PlayerData)reader.SetPrivateField("new_stage", reader.Read<System.Int32>(), instance);
					break;
					case "select_stage":
					instance = (SA_PlayerData)reader.SetPrivateField("select_stage", reader.Read<System.Int32>(), instance);
					break;
					case "mission_playing":
					instance = (SA_PlayerData)reader.SetPrivateField("mission_playing", reader.Read<System.Boolean>(), instance);
					break;
					case "before_sub_stage":
					instance = (SA_PlayerData)reader.SetPrivateField("before_sub_stage", reader.Read<System.Int32>(), instance);
					break;
					case "select_sub_stage":
					instance = (SA_PlayerData)reader.SetPrivateField("select_sub_stage", reader.Read<System.Int32>(), instance);
					break;
					case "simplestation":
					instance = (SA_PlayerData)reader.SetPrivateField("simplestation", reader.Read<System.Boolean>(), instance);
					break;
					case "story_num":
					instance = (SA_PlayerData)reader.SetPrivateField("story_num", reader.Read<System.Int32>(), instance);
					break;
					case "character_lockoff":
					instance = (SA_PlayerData)reader.SetPrivateField("character_lockoff", reader.Read<System.Boolean[]>(), instance);
					break;
					case "station_tutorial":
					instance = (SA_PlayerData)reader.SetPrivateField("station_tutorial", reader.Read<System.Boolean>(), instance);
					break;
					case "eventflag":
					instance = (SA_PlayerData)reader.SetPrivateField("eventflag", reader.Read<System.Boolean>(), instance);
					break;
					case "food_heal_flag":
					instance = (SA_PlayerData)reader.SetPrivateField("food_heal_flag", reader.Read<System.Boolean>(), instance);
					break;
					case "food_num":
					instance = (SA_PlayerData)reader.SetPrivateField("food_num", reader.Read<System.Int32>(), instance);
					break;
					case "name":
						instance.name = reader.Read<System.String>(ES3Type_string.Instance);
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