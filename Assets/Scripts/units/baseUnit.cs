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

    public void Initialize(int MaxHealth)
    {
        Health = new HealthSystem(MaxHealth, this);
        hasMoved = false;
        hasAttacked = false;  

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f;
        }
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

    public void ResetTurnState()
    {
        hasMoved = false;
        hasAttacked = false;
    }
}
