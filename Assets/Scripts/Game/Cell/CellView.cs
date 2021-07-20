using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game.Cell
{
	public class CellView : MonoBehaviour, IPointerDownHandler
	{
		[SerializeField] private Image _image;

		private Action _onClickAction;
		private Transform _transform;

		public void Init(Action onClickAction)
		{
			_onClickAction = onClickAction;
			_transform = transform;

			_image.color = Color.clear;
		}

		public void OnPointerDown(PointerEventData eventData) => _onClickAction?.Invoke();
		public byte GetId() => (byte) _transform.GetSiblingIndex();
		public void SetHint(bool isShow, Material hintMaterial = null) => _image.material = !isShow ? null : hintMaterial;

		public void SetSprite(Sprite sprite)
		{
			_image.sprite = sprite;
			_image.color = Color.white;
		}
	}
}
