using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Menu
{
	public class MenuView : MonoBehaviour, IClearable
	{
		[SerializeField] private TMP_Dropdown _modesDropdown;
		[SerializeField] private Button _startButton;

		public void InitStartButton(Action<int> startButtonClickedAction)
		{
			_startButton.onClick.AddListener(() =>
			{
				startButtonClickedAction?.Invoke(_modesDropdown.value + 1); //+1 because we need to skip "None" mode which is 0
			});
		}

		public void InitModesDropdown(List<string> names)
		{
			_modesDropdown.options.Clear();
			_modesDropdown.AddOptions(names.ToList());
		}

		public void Clear()
		{
			_startButton.onClick.RemoveAllListeners();

			Destroy(gameObject);
		}
	}
}
