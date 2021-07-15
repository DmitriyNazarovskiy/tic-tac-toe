using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Cell
{
	public class CellView : MonoBehaviour, IPointerDownHandler
	{
		private Action _onClickAction;
		private Transform _transform;

		public void Init(Action onClickAction)
		{
			_onClickAction = onClickAction;
			_transform = transform;
		}

		public void OnPointerDown(PointerEventData eventData) => _onClickAction?.Invoke();
		public byte GetId() => (byte) _transform.GetSiblingIndex();
	}
}
