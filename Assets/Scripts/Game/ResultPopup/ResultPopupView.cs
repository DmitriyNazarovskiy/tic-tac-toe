using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.ResultPopup
{
	public class ResultPopupView : MonoBehaviour, IClearable
	{
		[SerializeField] private TMP_Text _result;
		[SerializeField] private Button _restartButton, _menuButton;

		public void SetResult(string resultMessage)
		{
			_result.text = resultMessage;

			gameObject.AddComponent<ScaleVisualEffect>().Play();
		}

		public void Clear() => Destroy(gameObject);

		public void InitButtons(Action restartAction, Action menuAction)
		{
			_restartButton.onClick.RemoveAllListeners();
			_restartButton.onClick.AddListener(() => restartAction?.Invoke());
			
			_menuButton.onClick.RemoveAllListeners();
			_menuButton.onClick.AddListener(() => menuAction?.Invoke());
		}
	}
}
