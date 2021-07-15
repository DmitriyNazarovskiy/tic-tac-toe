using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core
{
	public interface IAudioController
	{
		(int, GameObject) PlayEffect(AudioClip sound, float volume, float stereoPan = 0.0f, float pitch = 1.0f,
			bool isLoop = false,
			Action onFinishCallback = null);
	}

	public static class AudioPlayer
	{
		private static IAudioController _audioController;
		public static void Init(IAudioController audioController) => _audioController = audioController;

		public static (int, GameObject) PlayEffect(AudioClip sound, float volume, float stereoPan = 0.0f,
			float pitch = 1.0f,
			bool isLoop = false,
			Action onFinishCallback = null)
			=> _audioController.PlayEffect(sound, volume, stereoPan, pitch, isLoop, onFinishCallback);
	}

	public class AudioController : IAudioController
	{
		private const string AudioObjectName = "AudioEffect_";

		private List<AudioEffect> _effects;

		public void Init() => _effects = new List<AudioEffect>();
		private void RemoveFromList(AudioEffect effect) => _effects.Remove(effect);
		private AudioEffect GetEffectFromList(int id) => _effects.Find(e => e.Id == id);

		public void Stop(int id)
		{
			var effect = GetEffectFromList(id);

			if (effect == null)
				return;

			effect.Stop();

			RemoveFromList(effect);
		}

		public (int, GameObject) PlayEffect(AudioClip sound, float volume, float stereoPan, float pitch, bool isLoop,
			Action onFinishCallback)
		{
			var id = _effects.Count;
			var effect = new GameObject(AudioObjectName + sound).AddComponent<AudioEffect>();
			effect.Id = id;
			effect.PlaySound(sound, volume, isLoop, () =>
			{
				onFinishCallback?.Invoke();

				RemoveFromList(effect);
			});
			effect.SetStereoPan(stereoPan);
			effect.SetPitch(pitch);

			_effects.Add(effect);

			return (id, effect.gameObject);
		}
	}
}
