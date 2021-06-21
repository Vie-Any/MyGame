using UnityEngine;

public class AttackState : EnemyBaseState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.animatorState = 2;
        enemy.targetPoint = enemy.attackList[0];
    }

    public override void OnUpdate(Enemy enemy)
    {
        if (enemy.hasBoom)
        {
            return;
        }
        if(enemy.attackList.Count == 0)
        {
            enemy.TransitionToState(enemy.patrolState);
        }
        if (enemy.attackList.Count > 1)
        {
            for (int i = 0; i < enemy.attackList.Count; i++) 
            {
                if (Mathf.Abs(enemy.transform.position.x - enemy.attackList[i].position.x) < Mathf.Abs(enemy.transform.position.x - enemy.targetPoint.position.x))
                {
                    enemy.targetPoint = enemy.attackList[i];
                }
            }
        }
        if(enemy.attackList.Count == 1)
        {
            enemy.targetPoint = enemy.attackList[0];
        }

        if (enemy.targetPoint.CompareTag("Player"))
        {
            enemy.AttackAction();
        }
        else if (enemy.targetPoint.CompareTag("Boom"))
        {
            enemy.SkillAction();
        }

        enemy.MoveToTarget();
    }
}
