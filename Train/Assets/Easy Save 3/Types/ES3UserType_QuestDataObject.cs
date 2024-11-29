using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("stage_num", "substage_num", "substage_type", "distance", "emerging_monster", "monster_count", "open_substagenum", "substage_status")]
	public class ES3UserType_QuestDataObject : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_QuestDataObject() : base(typeof(QuestDataObject)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (QuestDataObject)obj;
			
			writer.WritePrivateField("stage_num", instance);
			writer.WritePrivateField("substage_num", instance);
			writer.WritePrivateField("substage_type", instance);
			writer.WritePrivateField("distance", instance);
			writer.WritePrivateField("emerging_monster", instance);
			writer.WritePrivateField("monster_count", instance);
			writer.WritePrivateField("open_substagenum", instance);
			writer.WritePrivateField("substage_status", instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (QuestDataObject)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "stage_num":
					instance = (QuestDataObject)reader.SetPrivateField("stage_num", reader.Read<System.Int32>(), instance);
					break;
					case "substage_num":
					instance = (QuestDataObject)reader.SetPrivateField("substage_num", reader.Read<System.Int32>(), instance);
					break;
					case "substage_type":
					instance = (QuestDataObject)reader.SetPrivateField("substage_type", reader.Read<SubStageType>(), instance);
					break;
					case "distance":
					instance = (QuestDataObject)reader.SetPrivateField("distance", reader.Read<System.Int32>(), instance);
					break;
					case "emerging_monster":
					instance = (QuestDataObject)reader.SetPrivateField("emerging_monster", reader.Read<System.String>(), instance);
					break;
					case "monster_count":
					instance = (QuestDataObject)reader.SetPrivateField("monster_count", reader.Read<System.String>(), instance);
					break;
					case "open_substagenum":
					instance = (QuestDataObject)reader.SetPrivateField("open_substagenum", reader.Read<System.String>(), instance);
					break;
					case "substage_status":
					instance = (QuestDataObject)reader.SetPrivateField("substage_status", reader.Read<System.String>(), instance);
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

		public ES3UserType_QuestDataObjectArray() : base(typeof(QuestDataObject[]), ES3UserType_QuestDataObject.Instance)
		{
			Instance = this;
		}
	}
}