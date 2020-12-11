using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    GameObject projectile;

    // Start is called before the first frame update
    void Start()
    {
        projectile = Resources.Load("magic projectile") as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
