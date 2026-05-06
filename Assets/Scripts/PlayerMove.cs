using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; 

public class PlayerMove : MonoBehaviour
{
    public float speed = 4f;
    public float acceleration = 5f;
    public float deceleration = 3f;
    public float jumpForce = 10f;
    public float gravity = 25f;
    public Light[] spotlights; 
    public ParticleSystem deathParticles; 
    public GameObject playerVisual;

    private Vector3 currentVelocity = Vector3.zero;
    private float verticalVelocity = 0f;
    private CharacterController controller;
    private bool isDead = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isDead) return;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        Vector3 inputDir = new Vector3(moveX, 0, moveZ).normalized;

        if (inputDir.magnitude > 0.1f)
            currentVelocity = Vector3.Lerp(currentVelocity, inputDir * speed, acceleration * Time.deltaTime);
        else
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);

        if (controller.isGrounded)
        {
            verticalVelocity = -gravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump")) verticalVelocity = jumpForce;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        Vector3 finalMove = currentVelocity;
        finalMove.y = verticalVelocity;
        controller.Move(finalMove * Time.deltaTime);

        if (!CheckIfSafe())
        {
            Die();
        }
    }

    bool CheckIfSafe()
    {
        foreach (Light spot in spotlights)
        {
            if (spot == null || !spot.enabled) continue;

            Vector3 dirToPlayer = transform.position - spot.transform.position;
            float distance = dirToPlayer.magnitude;

            if (distance > spot.range) continue;

            if (spot.type == LightType.Spot)
            {
                float angle = Vector3.Angle(spot.transform.forward, dirToPlayer);
                if (angle > (spot.spotAngle / 2f)) continue;
            }

            RaycastHit hit;
            if (Physics.Raycast(spot.transform.position, dirToPlayer.normalized, out hit, distance + 0.1f))
            {
                if (hit.collider.gameObject == gameObject) return true;
            }
        }
        return false; 
    }

    void Die()
    {
        if (!isDead) 
        {
            StartCoroutine(DeathRoutine());
        }
    }

    IEnumerator DeathRoutine()
    {
        isDead = true;

        MeshRenderer mr = playerVisual.GetComponent<MeshRenderer>();
        Material mat = mr.material; 
        
        if (deathParticles != null)
        {
            deathParticles.transform.position = transform.position;
            deathParticles.Play();
        }

        float duration = 3.0f;
        float currentTime = 0f;
        Color startColor = mat.color;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, currentTime / duration);
            
            mat.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            
            mat.SetColor("_EmissionColor", mat.GetColor("_EmissionColor") * alpha); 

            yield return null;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}