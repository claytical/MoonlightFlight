using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public int currentHP;
    public int maxHP;

    public float force;
    public float terminalVelocity;
    public float boostMultiplier = 2f;
    private bool boosting = false;

    public GameObject trail;

    private TrailRenderer trailRenderer;
    private Rigidbody2D rb;
    private AudioManager audioManager;

    public AudioClip collisionClip;
    public AudioClip destroyClip;
    public AudioClip collectedClip;
    public AudioClip thrustClip;

    private float initialForce;
    private float initialTerminalVelocity;

    public int energyCollectedBeforeLootDrop;
    public float energyCollected = 5;  // Start with 5 energy units

    private Vector2 driftDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component is missing from the GameObject.");
            enabled = false;
            return;
        }

        audioManager = GetComponent<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("AudioManager component is missing from the GameObject.");
            enabled = false;
            return;
        }

        initialForce = force;
        initialTerminalVelocity = terminalVelocity;
    }

    void FixedUpdate()
    {
        if (boosting)
        {
            if (energyCollected > 0)
            {
                Vector2 forceDirection = transform.up * force * boostMultiplier;
                rb.AddForce(forceDirection, ForceMode2D.Force);

                // Clamp velocity to terminal velocity
                if (rb.velocity.magnitude > terminalVelocity)
                {
                    rb.velocity = rb.velocity.normalized * terminalVelocity;
                }

                // Deplete energy over time
                energyCollected -= Time.fixedDeltaTime;

                if (energyCollected <= 0)
                {
                    energyCollected = 0;
                    TurnOffBoost(); // Automatically turn off boost when energy depletes
                }
            }
            else
            {
                TurnOffBoost(); // Ensure boost is turned off if energy is zero
            }
        }

        ClampAngularVelocity();
        audioManager.AdjustPitch(rb.velocity.magnitude * 0.1f); // Adjust pitch based on speed
    }

    public void Fly()
    {
        if (trail != null)
        {
            GameObject go = Instantiate(trail, transform);
            trailRenderer = go.GetComponent<TrailRenderer>();
        }
    }

    public bool IsFlying()
    {
        return trailRenderer != null && trailRenderer.emitting;
    }

    public void TurnOnBoost()
    {
        if (energyCollected > 0 && !boosting)
        {
            if (audioManager != null && thrustClip != null)
            {
                audioManager.PlayLoopingSound(thrustClip);
            }
            boosting = true;

            if (trailRenderer != null)
            {
                trailRenderer.emitting = true;
            }
        }
    }

    public void TurnOffBoost()
    {
        if (audioManager != null)
        {
            audioManager.StopSound();
        }
        boosting = false;
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }
    }

    public void ResetSpeed()
    {
        force = initialForce;
        terminalVelocity = initialTerminalVelocity;
        boosting = false;
    }

    public bool IsBoosting()
    {
        return boosting;
    }

    public void Move(Vector2 direction)
    {
        driftDirection = direction;

        if (rb != null && direction != Vector2.zero)
        {
            // Reset the existing rotational force (angular velocity)
            rb.angularVelocity = 0f;

            // Calculate the angle of the movement direction
            float angleRad = Mathf.Atan2(direction.y, direction.x);
            float angleDeg = angleRad * Mathf.Rad2Deg;

            // Adjust the angle to account for the sprite's initial orientation (facing up)
            angleDeg -= 90f;

            // Rotate the vehicle to face the movement direction
            rb.rotation = angleDeg;
        }
    }

    private void ClampAngularVelocity()
    {
        float maxAngularVelocity = 540f;
        if (rb != null)
        {
            rb.angularVelocity = Mathf.Clamp(rb.angularVelocity, -maxAngularVelocity, maxAngularVelocity);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        var platform = coll.gameObject.GetComponentInParent<Platform>();
        if (platform != null && !platform.indestructable)
        {
            var explode = coll.gameObject.GetComponent<Explode>();
            if (explode != null)
            {
                explode.UntilNextSet();
            }
        }

        if (audioManager != null)
        {
            switch (coll.gameObject.tag)
            {
                case "Bump":
                    audioManager.PlaySound(collisionClip);
                    break;
                case "Break":
                    audioManager.PlaySound(destroyClip);
                    break;
                case "Collect":
                    audioManager.PlaySound(collectedClip);
                    break;
            }
        }
    }

    public void Explode()
    {
        if (audioManager != null)
        {
            audioManager.PlaySound(destroyClip);
        }
    }

    public void CollectEnergy()
    {
        energyCollected++;
        if (audioManager != null)
        {
            audioManager.PlaySound(collectedClip);
        }
        var localPlayer = GetComponentInParent<LocalPlayer>();
        if (localPlayer != null)
        {
            localPlayer.EnergyCollected((int)energyCollected);
        }
    }

    public void CollectPart(int amount)
    {
        int parts = PlayerPrefs.GetInt("parts", 0);
        parts += amount;
        PlayerPrefs.SetInt("parts", parts);
    }
}
