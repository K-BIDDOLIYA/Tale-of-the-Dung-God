using UnityEngine;

public class CamTrack2D : MonoBehaviour
{
   public Transform target;
   public Vector3 offset = new Vector3(0, 0.72f, -10);
   public float smoothTime = 0.3f;
   private Vector3 _currentVelocity;

   private void LateUpdate()
   {
      transform.position = Vector3.SmoothDamp(transform.position, target.position + offset, ref _currentVelocity, smoothTime);
   }
}
