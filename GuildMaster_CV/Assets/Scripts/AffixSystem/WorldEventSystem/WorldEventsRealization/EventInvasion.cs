using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventInvasion : WorldEvent
{
    public EventInvasion(WorldEventData worldEventData, WorldEventApplySystem worldEventApplySystem) : base(worldEventData, worldEventApplySystem)
    {
    }

    public override void ApplyEvent(WorldEventData data)
    {
        if (data is EventInvasionData invasionData)
        {
            foreach (EnemyData enemyData in invasionData.EventEnemyList)
                _worldEventApplySystem.WorldEventSystem.AddEventEnemy(enemyData);

            _worldEventApplySystem.WorldEventSystem.SetEventEnemy();
        }
    }

    public override void CancelEvent(WorldEventData data)
    {
        if (data is EventInvasionData invasionData)
        {
            foreach (EnemyData enemyData in invasionData.EventEnemyList)
                _worldEventApplySystem.WorldEventSystem.RemoveEventEnemy(enemyData);

            _worldEventApplySystem.WorldEventSystem.SetEventEnemy();
        }
    }
}
