using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("stage_num", "substage_num", "substage_type", "distance", "emerging_monster", "monster_count", "open_substagenum", "substage_status")]
	public class ES3UserType_QuestDataObject : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_QuestDataObject() : base(typeof(MissionDataObject)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (MissionDataObject)obj;
			
			writer.WritePrivateField("stage_num", instance.Stage_Num);
			writer.WritePrivateField("substage_num", instance.SubStage_Num);
			writer.WritePrivateField("substage_type", instance.SubStage_Type);
			writer.WritePrivateField("distance", instance.Distance);
			writer.WritePrivateField("emerging_monster", instance.Emerging_Monster);
			writer.WritePrivateField("monster_count", instance.Monster_Count);
			writer.WritePrivateField("open_substagenum", instance.Open_SubStageNum);
			writer.WritePrivateField("substage_status", instance.SubStage_Status);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (MissionDataObject)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "stage_num":
					instance = (MissionDataObject)reader.SetPrivateField("stage_num", reader.Read<System.Int32>(), instance);
					break;
					case "substage_num":
					instance = (MissionDataObject)reader.SetPrivateField("substage_num", reader.Read<System.Int32>(), instance);
					break;
					case "substage_type":
					instance = (MissionDataObject)reader.SetPrivateField("substage_type", reader.Read<SubStageType>(), instance);
					break;
					case "distance":
					instance = (MissionDataObject)reader.SetPrivateField("distance", reader.Read<System.Int32>(), instance);
					break;
					case "emerging_monster":
					instance = (MissionDataObject)reader.SetPrivateField("emerging_monster", reader.Read<System.String>(), instance);
					break;
					case "monster_count":
					instance = (MissionDataObject)reader.SetPrivateField("monster_count", reader.Read<System.String>(), instance);
					break;
					case "open_substagenum":
					instance = (MissionDataObject)reader.SetPrivateField("open_substagenum", reader.Read<System.String>(), instance);
					break;
					case "substage_status":
					instance = (MissionDataObject)reader.SetPrivateField("substage_status", reader.Read<System.String>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_QuestDataObjectArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_QuestDataObjectArray() : base(typeof(MissionDataObject[]), ES3UserType_QuestDataObject.Instance)
		{
			Instance = this;
		}
	}
}