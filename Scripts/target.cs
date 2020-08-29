using UnityEngine;

public class target : MonoBehaviour
{
    public float health = 100f;
    public string deathMethod;

    public Transform head;
    public Transform spine;
    public Transform rightArm;
    public Transform rightForeArm;
    public Transform rightHand;
    public Transform leftArm;
    public Transform leftForeArm;
    public Transform leftHand;
    public Transform torso;
    public Transform rightThigh;
    public Transform rightAnkle;
    public Transform rightFoot;
    public Transform leftThigh;
    public Transform leftAnkle;
    public Transform leftFoot;

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
