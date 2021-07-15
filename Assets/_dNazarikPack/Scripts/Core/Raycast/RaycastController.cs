using UnityEngine;

namespace Core.Raycast
{
	public class RaycastController
	{
		private readonly Camera _camera;

		public RaycastController(Camera camera) => _camera = camera;

		public Transform PerformRaycast(Vector3 screenPos, out Vector3 point)
		{
			Transform hitTransform = null;

			var ray = _camera.ScreenPointToRay(screenPos);

			if (Physics.Raycast(ray, out var hit))
			{
				hitTransform = hit.transform;
				point = hit.point;
			}
			else
				point = Vector3.zero;

			return hitTransform;
		}

		public Transform PerformRaycast(Vector3 screenPos)
		{
			Transform hitTransform = null;

			var ray = _camera.ScreenPointToRay(screenPos);

			if (Physics.Raycast(ray, out var hit))
				hitTransform = hit.transform;

			return hitTransform;
		}
	}
}
