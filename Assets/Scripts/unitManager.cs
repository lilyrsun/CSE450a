using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class unitManager : MonoBehaviour
{
    public static unitManager Instance;
    private List<scriptableUnit> _units;

    public basePlayerUnit SelectedBasePlayerUnit;

    private void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<scriptableUnit>("Units").ToList();
    }

    public void SpawnUnit()
    {
        var heroCount = 1;
        for (int i = 0; i <heroCount; ++i)
        {
            var randomPrefab = getRandomUnit<basePlayerUnit>(Faction.Player);
            var spawnedUnit = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetUnitSpawnTile();

            randomSpawnTile.SetUnit(spawnedUnit);
        }

        gameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

    public void SpawnNPCUnits()
    {
        var NPCUnitCount = 1;
        for (int i = 0; i < NPCUnitCount; ++i)
        {
            var randomPrefab = getRandomUnit<baseNPCUnit>(Faction.NPC);
            var spawnedNPCUnit = Instantiate(randomPrefab);
            var randomSpawnTile = GridManager.Instance.GetNPCUnitSpawnTile();

            randomSpawnTile.SetUnit(spawnedNPCUnit);
        }

        gameManager.Instance.ChangeState(GameState.PlayersTurn);

    }

    private T getRandomUnit<T>(Faction faction) where T : baseUnit
    {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().Unitprefab;
    }

    public void setSelectedPlayerUnit(basePlayerUnit playerUnit)
    {
        SelectedBasePlayerUnit = playerUnit;
    }



}
