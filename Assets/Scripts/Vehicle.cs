using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Vehicle : MonoBehaviour
{
    public int currentHP;
    public int maxHP;

    public float force;
    public float terminalVelocity;
    public float boost = 2f;

    public int energyCollectedBeforeLootDrop;
    public Color starColor;

    private bool flying = false;
    private SetInfo set;
    private float glitchAmount = .1f;
    private bool lootAvailable = false;
    private int energyCollected;
    private ProceduralLevel level;
    private Vector2 driftDirection;

    // Start is called before the first frame update
    void Start()
    {       
        GetComponent<SpriteRenderer>().color = starColor;
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
        flying = true;
    }
    public bool isFlying()
    {
        return flying;
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

    public void Glitch(float amount)
    {
        Invoke("ResetGlitch", amount);
    }

    void ResetGlitch()
    {
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift= 0f;
        Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = 0f;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Collision!");

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
        
        if(coll.gameObject.GetComponent<Platform>())
        {
            if(coll.gameObject.GetComponent<Hazard>())
                {
                    //hazard glitch
                    Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift = glitchAmount * 2f;
                    Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().verticalJump = glitchAmount;
                    Invoke("ResetGlitch", .1f);
                }
                else
                {
                    //standard bump glitch
                    Camera.main.gameObject.GetComponent<Kino.AnalogGlitch>().colorDrift = glitchAmount;
                    Invoke("ResetGlitch", .05f);

                }
            }


        //POWER UPS

             if (coll.gameObject.GetComponent<PowerUp>())
             {
                lootAvailable = false;
         //COLLECTION SOUND?
//            GetComponentInParent<AudioSource>().PlayOneShot(coll.gameObject.GetComponent<Breakable>().hit);
                switch (coll.gameObject.GetComponent<PowerUp>().reward)
                {
                    case PowerUp.Reward.Shield:
                        currentHP++;
                        if(GetComponentInParent<Player>())
                        {
                            GetComponentInParent<Player>().playerStats.GetComponent<PlayerStats>().hp.IncreaseHP(1);
                        }                            
                        break;

                    case PowerUp.Reward.Thruster:
//                        EngageThrusters();
                        break;
                                           
                    case PowerUp.Reward.Part:

                        int parts = PlayerPrefs.GetInt("parts", 0);
                        parts++;
                        PlayerPrefs.SetInt("parts", parts);
                        break;
                }
                Destroy(coll.gameObject);
         }
        gameObject.GetComponentInParent<AudioSource>().Play();
    }

    public void CollectEnergy()
    {
        energyCollected++;
        GetComponentInParent<Player>().playerStats.GetComponent<PlayerStats>().EnergyCollected(energyCollected);

    }

    public void CollectPart(int amount)
    {
        int parts = PlayerPrefs.GetInt("parts", 0);
        parts+=amount;
        PlayerPrefs.SetInt("parts", parts);
    }   
}

