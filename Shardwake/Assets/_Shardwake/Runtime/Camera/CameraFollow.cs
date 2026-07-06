using UnityEngine;

namespace Shardwake.Cameras
{
    public sealed class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Vector3 offset = new(0f, 36f, -30f);
        [SerializeField] private float followSpeed = 12f;

        [SerializeField] private Transform target;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            var desiredPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, 1f - Mathf.Exp(-followSpeed * Time.deltaTime));
            transform.rotation = Quaternion.Euler(55f, 0f, 0f);
        }

        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            transform.position = target.position + offset;
        }
    }
}
