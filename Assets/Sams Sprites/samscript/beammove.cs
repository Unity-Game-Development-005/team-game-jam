using UnityEngine;

public class beammove : MonoBehaviour
{
    public float beamspeed = 10;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position += transform.up * beamspeed * Time.deltaTime;
    }
}
