
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
        health -= damageAmount;
        if (health <= 0) owner.Die();
    }
    public int getMaxHealth()
{
    return healthMax;
}


}
