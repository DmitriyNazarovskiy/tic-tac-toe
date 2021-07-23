using TMPro;
using UnityEngine;

namespace Core
{
	/// <summary>
	/// This fix required for exporting .unitypackage
	/// </summary>
	[RequireComponent(typeof(TMP_Text))]
	public class TMP_AlignmentFix : MonoBehaviour
	{
		[SerializeField] private TextAlignmentOptions _alignment;

		private void Awake() => GetComponent<TMP_Text>().alignment = _alignment;
	}
}
