using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/SteeredCohesion")]
public class SteeredCohesion : FilteredFlockBehaviour
{
    Vector2 currentVelocity;
    public float agentSmoothTime = 0.5f;

    public override Vector2 CalculateMove(Enemy agent, List<Transform> context, EnemyController flock)
    {
        if (context.Count == 0)
            return Vector2.zero;

        Vector2 coheshionMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            coheshionMove += (Vector2)item.position;
        }
        coheshionMove /= context.Count;

        coheshionMove -= (Vector2)agent.transform.position;
        coheshionMove = Vector2.SmoothDamp(agent.transform.up, coheshionMove, ref currentVelocity, agentSmoothTime);
        return coheshionMove;
    }


}

