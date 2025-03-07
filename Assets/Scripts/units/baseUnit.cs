using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class baseUnit : MonoBehaviour
{
    public Tile OccuppiedTile;
    public Faction Faction;
    public HealthSystem Health;
    public bool hasMoved { get; set; }
    public bool hasAttacked { get; set; }  

    public Image healthBarFill;
    private GameObject moveIndicator;

    public void Initialize(int MaxHealth)
    {
        Health = new HealthSystem(MaxHealth, this);
        hasMoved = false;
        hasAttacked = false;  

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f;
        }
        MoveIndicator(); 
        UpdateMoveIndicator(); 
    }

    public virtual void TakeDamage(int damage)
    {
        Health.damage(damage);
        UpdateHealthBar();

        if (Health.getHealth() <= 0)
        {
            Die();
        }
    }

    public void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            float maxHealth = Health.getMaxHealth();
            float currentHealth = Mathf.Max(0, Health.getHealth());

            healthBarFill.fillAmount = currentHealth / maxHealth;
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public void RegenerateHealth(int amount)
    {
        if (!hasMoved && !hasAttacked)
        {
            Health.heal(amount);
            UpdateHealthBar();
            Debug.Log(name + " healed for " + amount + " HP!");
        }
    }


    private void MoveIndicator()
    {
        if (!(this is playerKnight)) return;
        moveIndicator = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        moveIndicator.transform.SetParent(transform);
        moveIndicator.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
        moveIndicator.transform.localPosition = new Vector3(-.5f, 0.55f, 0);
        Material brightWhiteMaterial = new Material(Shader.Find("Standard"));
        brightWhiteMaterial.color = Color.white;
        brightWhiteMaterial.EnableKeyword("_EMISSION");
        brightWhiteMaterial.SetColor("_EmissionColor", Color.white * 3f);
        moveIndicator.GetComponent<Renderer>().material = brightWhiteMaterial;
        moveIndicator.SetActive(false); 
    }

    public void UpdateMoveIndicator()
    {
        if (moveIndicator != null)
        {
            moveIndicator.SetActive(!hasMoved);
        }
    }

    public void ResetTurnState()
    {
        hasMoved = false;
        hasAttacked = false;
        UpdateMoveIndicator();
        //Debug.Log("AHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHh");
    }
}
