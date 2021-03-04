using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohesionBehaviour : EnemyFlockBehaviour
{
    public override Vector2 CalculateMove(Enemy agent, List<Transform> context, EnemyController flock)
    {
        if (context.Count == 0)
            return Vector2.zero;

        Vector2 coheshionMove = Vector2.zero;
        foreach (Transform item in context)
        {
            coheshionMove += (Vector2)item.position;
        }
        coheshionMove /= context.Count;

        coheshionMove -= (Vector2)agent.transform.position;
        return coheshionMove;
    }

   
}
