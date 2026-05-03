using UnityEngine;

public class LightController : MonoBehaviour
{
    public float speed = 5f;
    public float minZ = 6f;
    public float maxZ = 8f;

    void Update()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.P)) 
        {
            moveInput = -1f; 
        }
        else if (Input.GetKey(KeyCode.O)) 
        {
            moveInput = 1f; 
        }

        float newZ = transform.position.z + (moveInput * speed * Time.deltaTime);
        
        newZ = Mathf.Clamp(newZ, minZ, maxZ);

        transform.position = new Vector3(transform.position.x, transform.position.y, newZ);
    }
}