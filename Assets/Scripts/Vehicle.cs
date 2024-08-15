using UnityEngine;

public class Vehicle : MonoBehaviour
{
    public int currentHP;
    public int maxHP;

    public float force;
    public float terminalVelocity;
    public float boostMultiplier = 2f;
    public float minimalThrustForce = 0.2f;
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

    public float capacity = 10f;
    public float energyCollected;
    public float energyConsumptionRate = 1f;
    private float energyTimer = 0f;

    private Vector2 driftDirection;

    public Fuel fuelUI;
    public PlayerStatsTracking playerStats; // Reference passed from LocalPlayer

    private Vector3 lastPosition;

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
        energyCollected = capacity;

        UpdateFuelUI();

        if (playerStats == null)
        {
            Debug.LogError("PlayerStatsTracking component is missing.");
            enabled = false;
            return;
        }

        lastPosition = transform.position; // Initialize last position for distance tracking
    }

    void FixedUpdate()
    {
        if (boosting)
        {
            if (energyCollected > 0)
            {
                energyTimer += Time.fixedDeltaTime;

                if (energyTimer >= energyConsumptionRate)
                {
                    ConsumeEnergy(1);
                    playerStats.RecordFuelUsed(1);  // Track fuel usage
                    energyTimer = 0f;
                }

                Vector2 forceDirection = transform.up * force * boostMultiplier;
                rb.AddForce(forceDirection, ForceMode2D.Force);

                if (rb.velocity.magnitude > terminalVelocity)
                {
                    rb.velocity = rb.velocity.normalized * terminalVelocity;
                }
            }
            else
            {
                Vector2 forceDirection = transform.up * minimalThrustForce;
                rb.AddForce(forceDirection, ForceMode2D.Force);

                if (rb.velocity.magnitude > terminalVelocity / 4f)
                {
                    rb.velocity = rb.velocity.normalized * (terminalVelocity / 4f);
                }
            }
        }

        UpdateDistanceCovered(); // Track distance in FixedUpdate

        ClampAngularVelocity();
        audioManager.AdjustPitch(rb.velocity.magnitude * 0.1f);
    }

    private void UpdateDistanceCovered()
    {
     
        // Calculate the distance covered since the last frame
        float distance = Vector3.Distance(transform.position, lastPosition);
        playerStats.distanceCovered += distance;
        playerStats.AddScore((int)distance); // Award points for distance
        
        lastPosition = transform.position;
    }

    public void Fly()
    {
        if (trail != null && trailRenderer == null)
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
            ConsumeEnergy(1);
            playerStats.RecordFuelUsed(1);  // Track fuel usage

            if (trailRenderer != null)
            {
                trailRenderer.emitting = true;
            }
        }
        else if (energyCollected <= 0 && !boosting)
        {
            if (audioManager != null && thrustClip != null)
            {
                audioManager.PlayLoopingSound(thrustClip, 0.5f);
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
        energyTimer = 0f;

        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }
    }

    private void ConsumeEnergy(int amount)
    {
        energyCollected -= amount;
        if (energyCollected < 0) energyCollected = 0;
        UpdateFuelUI();
    }

    public void CollectEnergy()
    {
        energyCollected++;
        if (energyCollected > capacity)
        {
            energyCollected = capacity;
        }

        UpdateFuelUI();
        playerStats.RecordItemCollected();

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

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if (currentHP <= 0)
        {
            Explode();
        }

        playerStats.RecordDamage(damage);
    }

    private void UpdateFuelUI()
    {
        if (fuelUI != null)
        {
            fuelUI.UpdateFuelUI((int)energyCollected);
        }
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
            rb.angularVelocity = 0f;

            float angleRad = Mathf.Atan2(direction.y, direction.x);
            float angleDeg = angleRad * Mathf.Rad2Deg;
            angleDeg -= 90f;

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

    public void CollectPart(int amount)
    {
        int parts = PlayerPrefs.GetInt("parts", 0);
        parts += amount;
        PlayerPrefs.SetInt("parts", parts);
    }
}
