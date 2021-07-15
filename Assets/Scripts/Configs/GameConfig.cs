using Core;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = Constants.GameConfigNameString, menuName = Constants.GameConfigMenuPathString)]
	public class GameConfig : ScriptableObject
	{
		public GameObject MainMenuPrefab, GameViewPrefab;
	}
}
