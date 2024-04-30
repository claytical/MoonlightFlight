using UnityEngine;


public class Vehicle : MonoBehaviour
{
    public int currentHP;
    public int maxHP;

    public float force;
    public float terminalVelocity;
    public float boost = 2f;
    private bool boosting = false;
    public int fuelEfficiency = 20;
    private int fuelCounter = 0;
    public GameObject trail;

    private TrailRenderer trailRenderer;
    private float initialForce;
    private float initialTerminalVelocity;

    public int energyCollectedBeforeLootDrop;
    public Color starColor;

    private bool flying = false;
    private SetInfo set;
    private int energyCollected;
    private ProceduralLevel level;
    private Vector2 driftDirection;

    // Start is called before the first frame update
    void Start()
    {
        initialForce = force;
        initialTerminalVelocity = terminalVelocity;
        GetComponent<SpriteRenderer>().color = starColor;
        GetComponentInParent<Player>().SetVehicleIconColor(starColor);
 
        energyCollected = 0;
    }

    void ClampAngularVelocity()
    {
        float maxAngularVelocity = 540f;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb) {
            if (rb.angularVelocity < -maxAngularVelocity) { rb.angularVelocity = -maxAngularVelocity; }
            if (rb.angularVelocity > maxAngularVelocity) { rb.angularVelocity = maxAngularVelocity; }

        }
    }

    private void FixedUpdate()
    {
        if(boosting)
        {
            fuelCounter++;
            if(fuelCounter >= fuelEfficiency)
            {
                Invoke("TurnOffTrails", 1f);
                TurnOffBoost();
                ResetSpeed();
            }
        }

        Vector2 newForce = Vector2.ClampMagnitude(driftDirection * force, terminalVelocity);
        GetComponent<Rigidbody2D>().AddForce(newForce, ForceMode2D.Impulse);

        if (GetComponent<Rigidbody2D>().velocity.magnitude > terminalVelocity)
        {
            GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity.normalized * terminalVelocity;
        }

        ClampAngularVelocity();
    }
    public void Fly()
    {
        GameObject go = Instantiate(trail, transform);
        trailRenderer = go.GetComponent<TrailRenderer>();
        flying = true;
    }
    public bool isFlying()
    {
        return flying;
    }
    public void TurnOnBoost()
    {
        fuelCounter = 0;
        force *= boost;
        terminalVelocity *= boost;
        boosting = true;
        trailRenderer.emitting = true;
    }

    public void TurnOffBoost()
    {
        boosting = false;
        trailRenderer.emitting = false;
    }

    public void ResetSpeed()
    {
        force = initialForce;
        terminalVelocity = initialTerminalVelocity;
        GetComponent<Rigidbody2D>().drag = 0f;
        boosting = false;
    }

    public bool isBoosting()
    {
        return boosting;
    }

    public void Move(Vector2 direction) {
        driftDirection = direction;
        if(GetComponent<Rigidbody2D>())
        {
            if(direction != Vector2.zero) {
                GetComponent<Rigidbody2D>().AddForce(direction * force);
                // Calculate the angle of the movement direction
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                angle -= 90f;
                // Rotate the vehicle's sprite to face the movement direction
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            }
        }
    }
    
    public void Drift(Vector3 position)
    {
        if(GetComponent<Rigidbody2D>())
        {
            Vector2 direction = (position - transform.position).normalized;
            GetComponent<Rigidbody2D>().AddForce(direction * 5f, ForceMode2D.Impulse); 
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
     
        if(coll.gameObject.GetComponentInParent<Platform>())
        {
            if(!coll.gameObject.GetComponentInParent<Platform>().indestructable)
            {
                if(coll.gameObject.GetComponent<Explode>())
                {
                    coll.gameObject.GetComponent<Explode>().UntilNextSet();
                }
            }
        }

        //POWER UPS
        gameObject.GetComponentInParent<AudioSource>().Play();
    }

    public void CollectEnergy()
    {
        energyCollected++;
        GetComponentInParent<Player>().EnergyCollected();

    }

    public void CollectPart(int amount)
    {
        int parts = PlayerPrefs.GetInt("parts", 0);
        parts+=amount;
        PlayerPrefs.SetInt("parts", parts);
    }   
}

