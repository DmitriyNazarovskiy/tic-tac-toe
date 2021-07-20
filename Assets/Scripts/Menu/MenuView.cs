using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Menu
{
	public class MenuView : MonoBehaviour, IClearable
	{
		[SerializeField] private TMP_Dropdown _modesDropdown;
		[SerializeField] private Button _startButton, _updateArtButton;
		[SerializeField] private TMP_Text _startButtonText;

		public void InitButtons(Action<int> startButtonClickedAction, Action updateArt)
		{
			_startButton.onClick.AddListener(() => startButtonClickedAction?.Invoke(_modesDropdown.value + 1)); //+1 because we need to skip "None" mode which is 0
			_updateArtButton.onClick.AddListener(() => updateArt?.Invoke());
		}

		public void InitModesDropdown(List<string> names, Action<int> dropDownValueChanged)
		{
			_modesDropdown.options.Clear();
			_modesDropdown.AddOptions(names.ToList());
			_modesDropdown.onValueChanged.AddListener(new UnityAction<int>(dropDownValueChanged));
		}

		public void SetStartButtonState(bool isEnabled, string label)
		{
			_startButton.interactable = isEnabled;
			_startButtonText.text = label;
		}

		public void HideUpdateArtButton() => _updateArtButton.gameObject.SetActive(false);

		public void Clear()
		{
			_startButton.onClick.RemoveAllListeners();
			_updateArtButton.onClick.RemoveAllListeners();

			_modesDropdown.onValueChanged.RemoveAllListeners();

			Destroy(gameObject);
		}
	}
}
