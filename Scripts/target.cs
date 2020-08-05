using UnityEngine;

public class target : MonoBehaviour
{
    public float health = 100f;
    public string deathMethod;

    public void takeDamage(float amount)
    {
        health -= amount;
        if(health <= 0.0f)
        {
            if (deathMethod == "destroy")
            {
                dieDestroy();
            }
        }
    }

    void dieDestroy()
    {
        Destroy(gameObject);
    }

}
