using System;
using UnityEngine;

namespace Core
{
	public class AudioEffect : MonoBehaviour
	{
		private bool _shouldTriggerAtTheEnd;
		private float _triggerDelay;
		private float _elapsedTime;
		private Action _onFinishCallback;
		private AudioSource _audioSource;

		public int Id { get; set; }

		public bool IsPlaying => _audioSource != null && _audioSource.isPlaying;
		public void SetStereoPan(float pan) => _audioSource.panStereo = pan;
		public float GetCurrentVolume => _audioSource != null ? _audioSource.volume : -1;
		public AudioClip GetCurrentAudioClip => _audioSource != null ? _audioSource.clip : null;

		private void Update()
		{
			if (_shouldTriggerAtTheEnd)
			{
				_elapsedTime += Time.deltaTime;

				if (_elapsedTime < _triggerDelay)
					return;

				if (_onFinishCallback == null)
					return;

				_onFinishCallback();

				Stop();
			}
		}

		public void PlaySound(AudioClip clip, float volume, bool loop = false, Action onFinishCallback = null)
		{
			_audioSource = InitAudioSource(clip, volume, loop, onFinishCallback);

			if (onFinishCallback != null && clip != null)
				InitOnFinishCallback(onFinishCallback, clip.length);
		}

		public void Stop(float clipLength = 0.0f)
		{
			Destroy(this, clipLength);
			Destroy(gameObject, clipLength);
		}

		public float SetPitch(float newPitchValue)
		{
			var currentPitch = 0.0f;

			if (_audioSource != null)
				currentPitch = _audioSource.pitch = newPitchValue;

			return currentPitch;
		}

		public float SetVolume(float newVolume)
		{
			var resVol = 0.0f;

			if (_audioSource != null)
				resVol = _audioSource.volume = newVolume;

			return resVol;
		}

		private AudioSource InitAudioSource(AudioClip clip, float volume, bool loop, Action actionToTrigger)
		{
			var audioSource = gameObject.AddComponent<AudioSource>();

			if (clip == null)
				return audioSource;

			audioSource.clip = clip;
			audioSource.loop = loop;
			audioSource.volume = volume;
			audioSource.spatialBlend = 0; // 0 makes it full 2D
			audioSource.Play();

			if (!loop && actionToTrigger == null)
				Stop(clip.length);

			return audioSource;
		}

		private void InitOnFinishCallback(Action actionToTrigger, float clipLength)
		{
			_onFinishCallback = actionToTrigger;
			_shouldTriggerAtTheEnd = true;
			_triggerDelay = clipLength;
		}
	}
}