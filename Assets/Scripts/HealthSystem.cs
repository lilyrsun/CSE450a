using UnityEngine;

public class HealthSystem 
{
    private int health;
    private int healthMax;
    private baseUnit owner;
    
    public HealthSystem(int healthMax, baseUnit owner)
    {
        this.health = healthMax;
        this.healthMax = healthMax;
        this.owner = owner;
    }

    public int getHealth()
    {
        return health;
    }

    public void damage(int damageAmount)
    {
        health = Mathf.Max(0, health - damageAmount);
        if (health <= 0) owner.Die();
    }

    public void heal(int amount)
    {
        health = Mathf.Min(healthMax, health + amount); // Fixed incorrect variable names
    }

    public int getMaxHealth()
    {
        return healthMax;
    }
}
