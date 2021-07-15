using UnityEngine;

namespace Core.Raycast
{
    public class Raycast2D : IRaycast2D
    {
        RaycastHit2D IRaycast2D.PerformRaycast(Vector3 origin, Vector3 direction, int layerMask)
        {
            return Physics2D.Raycast(origin, direction, Mathf.Infinity, 1 << layerMask);
        }
    }
}
