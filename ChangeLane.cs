using UnityEngine;

public class ChangeLane : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PositionLane()
    {
        int randomLane = Random.Range(-1, 2);
        transform.position = new Vector3(randomLane, transform.position.y, transform.position.z);
    }
}
