using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Menu
{
	public class MenuModel
	{
		public const string StartLabel = "Start";
		public const string SoonLabel = "Soon";

		public List<string> GetGameModesTitles()
		{
			var modes = Enum.GetNames(typeof(GameMode)).ToList();
			modes.RemoveAt(0); //remove "None" title

			return modes;
		}
	}
}
