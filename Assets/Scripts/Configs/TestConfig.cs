using Core;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = Constants.TestConfigNameString, menuName = Constants.TestConfigMenuPathString)]
	public class TestConfig : ScriptableObject
	{
		public GameConfig Config;
	}
}
