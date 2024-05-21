using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("train_num", "level_train_enginetier", "level_train_maxspeed", "level_train_armor", "level_train_efficient", "train_buy_num", "level_trainnumber_00", "level_trainnumber_10", "level_trainnumber_20", "level_trainnumber_30", "level_trainnumber_40")]
	public class ES3UserType_SA_TrainData : ES3ScriptableObjectType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SA_TrainData() : base(typeof(SA_TrainData)){ Instance = this; priority = 1; }


		protected override void WriteScriptableObject(object obj, ES3Writer writer)
		{
			var instance = (SA_TrainData)obj;
			
			writer.WritePrivateField("train_num", instance);
			writer.WritePrivateField("level_train_enginetier", instance);
			writer.WritePrivateField("level_train_maxspeed", instance);
			writer.WritePrivateField("level_train_armor", instance);
			writer.WritePrivateField("level_train_efficient", instance);
			writer.WritePrivateField("train_buy_num", instance);
			writer.WritePrivateField("level_trainnumber_00", instance);
			writer.WritePrivateField("level_trainnumber_10", instance);
			writer.WritePrivateField("level_trainnumber_20", instance);
			writer.WritePrivateField("level_trainnumber_30", instance);
			writer.WritePrivateField("level_trainnumber_40", instance);
		}

		protected override void ReadScriptableObject<T>(ES3Reader reader, object obj)
		{
			var instance = (SA_TrainData)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "train_num":
					instance = (SA_TrainData)reader.SetPrivateField("train_num", reader.Read<System.Collections.Generic.List<System.Int32>>(), instance);
					break;
					case "level_train_enginetier":
					instance = (SA_TrainData)reader.SetPrivateField("level_train_enginetier", reader.Read<System.Int32>(), instance);
					break;
					case "level_train_maxspeed":
					instance = (SA_TrainData)reader.SetPrivateField("level_train_maxspeed", reader.Read<System.Int32>(), instance);
					break;
					case "level_train_armor":
					instance = (SA_TrainData)reader.SetPrivateField("level_train_armor", reader.Read<System.Int32>(), instance);
					break;
					case "level_train_efficient":
					instance = (SA_TrainData)reader.SetPrivateField("level_train_efficient", reader.Read<System.Int32>(), instance);
					break;
					case "train_buy_num":
					instance = (SA_TrainData)reader.SetPrivateField("train_buy_num", reader.Read<System.Collections.Generic.List<System.Int32>>(), instance);
					break;
					case "level_trainnumber_00":
					instance = (SA_TrainData)reader.SetPrivateField("level_trainnumber_00", reader.Read<System.Int32>(), instance);
					break;
					case "level_trainnumber_10":
					instance = (SA_TrainData)reader.SetPrivateField("level_trainnumber_10", reader.Read<System.Int32>(), instance);
					break;
					case "level_trainnumber_20":
					instance = (SA_TrainData)reader.SetPrivateField("level_trainnumber_20", reader.Read<System.Int32>(), instance);
					break;
					case "level_trainnumber_30":
					instance = (SA_TrainData)reader.SetPrivateField("level_trainnumber_30", reader.Read<System.Int32>(), instance);
					break;
					case "level_trainnumber_40":
					instance = (SA_TrainData)reader.SetPrivateField("level_trainnumber_40", reader.Read<System.Int32>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SA_TrainDataArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SA_TrainDataArray() : base(typeof(SA_TrainData[]), ES3UserType_SA_TrainData.Instance)
		{
			Instance = this;
		}
	}
}