using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stay In Radius")]
public class StayInRadiusBehaviour : FilteredFlockBehaviour
{
    public Vector2 centre;
    public float radius = 15f;

    public override Vector2 CalculateMove(Enemy agent, List<Transform> context, EnemyController flock)
    {
        centre = GameObject.Find("Player").transform.position;
        Vector2 centreOffset = centre - (Vector2)agent.transform.position;
        float t = centreOffset.magnitude / radius;
        if(t < 0.9)
        {
            return Vector2.zero;
        }

        return centreOffset * t * t;
    }
}
