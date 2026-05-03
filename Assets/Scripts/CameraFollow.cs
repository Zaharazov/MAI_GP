using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -7); 
    public float smoothSpeed = 5f; 
    public LayerMask wallLayer;
    private MeshRenderer lastWall;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        transform.LookAt(target);

        HandleWalls();
    }

    void HandleWalls()
    {
        var dir = target.position - transform.position;
        if (Physics.Raycast(transform.position, dir, out var hit, dir.magnitude, wallLayer))
        {
            var currentWall = hit.collider.GetComponent<MeshRenderer>();
            if (currentWall == lastWall) return;

            if (lastWall) lastWall.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            currentWall.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
            lastWall = currentWall;
        }
        else if (lastWall)
        {
            lastWall.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            lastWall = null;
        }
    }
}