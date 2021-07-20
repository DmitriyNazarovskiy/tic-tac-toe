using System;
using Core;
using UnityEngine;

namespace Configs
{
	[Serializable]
	public struct WinCombination
	{
		public byte Value1, Value2, Value3;
	}

	[CreateAssetMenu(fileName = Constants.GameConfigNameString, menuName = Constants.GameConfigMenuPathString)]
	public class GameConfig : ScriptableObject
	{
		public GameObject MainMenuPrefab, GameViewPrefab, ResultPopupPrefab;
		public Sprite X, O;
		public WinCombination[] WinCombinations;
		public int GameDuration = 60;
		public Color DefaultTimerColor, LowTimerColor;
		public Material HintMaterial;
		[NonSerialized] public Sprite LoadedX, LoadedO, LoadedBg;
	}
}
