using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Import for UI elements

public class baseUnit : MonoBehaviour
{
    public Tile OccuppiedTile;
    public Faction Faction;
    public HealthSystem Health;
    public bool hasMoved { get; set; }

    public Image healthBarFill;

    public void Initialize(int MaxHealth)
    {
        Health = new HealthSystem(MaxHealth, this);
        hasMoved = false;

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
}
