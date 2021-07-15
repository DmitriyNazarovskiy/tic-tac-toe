using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Core
{
	public static class SaveManager
	{
		private const string PlayerDataFileName = "player_data.dat";
		private const string ErrorMessage = " is null or of incorrect type, assigning it's default value";

		private static string DataPath => Path.Combine(Application.persistentDataPath, "PlayerData", PlayerDataFileName);

		public static void SaveDataAsJson<T>(T objectToSave, bool isEncrypt = false)
		{
			var json = JsonUtility.ToJson(objectToSave);

			if (isEncrypt)
				json = EncryptionController.Base64Encode(json);

			File.WriteAllText(DataPath, json);
		}

		private static T GetDataAsJson<T>(bool isEncrypted = false)
		{
			try
			{
				var json = File.ReadAllText(DataPath);

				if (isEncrypted)
					json = EncryptionController.Base64Encode(json);

				var data = JsonUtility.FromJson<T>(json);

				CheckDataConsistency(data, Activator.CreateInstance<T>());

				return data;
			}
			catch (Exception e)
			{
				Debug.Log(e.Message);
			}

			return default;
		}

		private static void CheckDataConsistency(object target, object reference)
		{
			foreach (var field in target.GetType().GetFields())
			{
				var value = field.GetValue(target);
				var fieldInDefaultData = reference.GetType().GetFields().FirstOrDefault(f => f.Name == field.Name);

				if (value != null && fieldInDefaultData != null && field.FieldType == fieldInDefaultData.FieldType)
					continue;

				Debug.Log(field.Name + ErrorMessage);

				field.SetValue(target, field.GetValue(reference));
			}
		}
	}
}