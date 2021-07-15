using System;
using System.Collections;
using UnityEngine;

namespace Core
{
	public class ScaleVisualEffect : MonoBehaviour
	{
		private Transform _transform;
		private Action _callback;

		public void Play(Action callback = null)
		{
			_transform = transform;
			_callback = callback;

			StartCoroutine(PlayAnimation());
		}

		private IEnumerator PlayAnimation()
		{
			var model = _transform.transform;
			var startScale = model.localScale;
			var animatedScale = startScale + Vector3.one * 0.1f;
			var animationStep = Vector3.one * Time.deltaTime * 0.2f;
			var isScaleUp = true;

			while (true)
			{
				if (isScaleUp)
				{
					model.localScale += animationStep;

					if (model.localScale.x > animatedScale.x)
						isScaleUp = false;
				}
				else
				{
					model.localScale -= animationStep;

					if (model.localScale.x < startScale.x)
						break;
				}

				yield return null;
			}

			model.localScale = startScale;

			_callback?.Invoke();

			Destroy(GetComponent<ScaleVisualEffect>());
		}
	}
}
