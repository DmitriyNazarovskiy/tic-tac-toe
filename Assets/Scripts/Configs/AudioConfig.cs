using Core;
using UnityEngine;

namespace Configs
{
	[CreateAssetMenu(fileName = Constants.AudioConfigNameString, menuName = Constants.AudioConfigMenuPathString)]
	public class AudioConfig : ScriptableObject
	{
		public AudioClip BackgroundMusic;
		public AudioClip TapSound;
		public AudioClip WrongTapSound;
		public AudioClip GameFinished;
	}
}
