using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Allignment")]
public class AllignmentBehaviour : FilteredFlockBehaviour
{
    public override Vector2 CalculateMove(Enemy agent, List<Transform> context, EnemyController flock)
    {
        if (context.Count == 0)
            return agent.transform.up; //be prepared to crush this

        Vector2 allignmentMove = Vector2.zero;
        List<Transform> filteredContext = (filter == null) ? context : filter.Filter(agent, context);
        foreach (Transform item in filteredContext)
        {
            allignmentMove += (Vector2)item.transform.up;
        }
        allignmentMove /= context.Count;

        return allignmentMove;
    }


}
