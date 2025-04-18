using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class unitManager : MonoBehaviour
{
    public static unitManager Instance;
    private List<scriptableUnit> _units;

    public baseUnit SpawnedTownHall { get; private set; } 
    public basePlayerUnit SelectedBasePlayerUnit;

    public int spawnUnitCost = 20;

    private Dictionary<string, int> unitCosts = new Dictionary<string, int>
    {
        { "playerKnight", 20 },
        { "playerArcher", 25 }
    };

    private void Awake()
    {
        Instance = this;

        _units = Resources.LoadAll<scriptableUnit>("Units").ToList();
    }

    public void SpawnUnit()
    {
        var heroCount = 1;
        for (int i = 0; i < heroCount; ++i)
        {
            //var randomPrefab = getRandomUnit<basePlayerUnit>(Faction.Player);
            var spawnedUnit = Instantiate(Resources.Load<scriptableUnit>("Units/playerKnight")?.Unitprefab);
            var randomSpawnTile = GridManager.Instance.GetUnitSpawnTile();
            //Debug.Log(randomSpawnTile);
            //Debug.Log(randomPrefab);
            //Debug.Log("THIS IS THE PREFAB TEST");
            spawnedUnit.Initialize(100);
            randomSpawnTile.SetUnit(spawnedUnit);
        }
        var townHallPrefab = Resources.Load<scriptableUnit>("Units/playerTownHall")?.Unitprefab;
        var playerTH = Instantiate(townHallPrefab);
        playerTH.Initialize(272);
        var fixedTile = GridManager.Instance.GetTileAtPosition(3, 10);
        fixedTile.SetUnit(playerTH);
        SpawnedTownHall = playerTH; // Store the town hall for check
        //Debug.Log(fixedUnit);
        //Debug.Log("DID IT WORK");

        gameManager.Instance.RevealTilesAroundUnits();

        gameManager.Instance.ChangeState(GameState.SpawnEnemies);
    }

        public int GetPlayerUnitCount()
    {
        return FindObjectsOfType<basePlayerUnit>().Length;
    }


    public void SpawnNPCUnits()
    {
        var NPCUnitCount = 4;
        for (int i = 0; i < NPCUnitCount; ++i)
        {
            //var randomPrefab = getRandomUnit<baseNPCUnit>(Faction.NPC);
            var spawnedNPCUnit = Instantiate(Resources.Load<scriptableUnit>("Units/NPCKnight")?.Unitprefab);
            /*Debug.Log(randomPrefab);
            Debug.Log("THIS IS THE PREFAB TEST");*/
            var randomSpawnTile = GridManager.Instance.GetNPCUnitSpawnTile();

            spawnedNPCUnit.Initialize(100);
            randomSpawnTile.SetUnit(spawnedNPCUnit);
            //Debug.Log($"Spawned NPC Unit: {spawnedNPCUnit.name}, Health: {spawnedNPCUnit.Health.getHealth()}");
        }
        var NPCtownHallPrefab = Resources.Load<scriptableUnit>("Units/NPCTownHall")?.Unitprefab;
        var NPCTH = Instantiate(NPCtownHallPrefab);
        NPCTH.Initialize(272);
        var fixedTile = GridManager.Instance.GetTileAtPosition(10, 12);
        fixedTile.SetUnit(NPCTH);
        gameManager.Instance.ChangeState(GameState.PlayersTurn);

    }

    public void SpawnNPCWave()
    {
        var spawnedNPCUnit = Instantiate(Resources.Load<scriptableUnit>("Units/NPCKnight")?.Unitprefab);
        var spawnedNPCUnit2 = Instantiate(Resources.Load<scriptableUnit>("Units/NPCKnight")?.Unitprefab);
        
        var randomSpawnTile = GridManager.Instance.GetNPCUnitSpawnTile();
        var randomSpawnTile2 = GridManager.Instance.GetNPCUnitSpawnTile();

        spawnedNPCUnit.Initialize(100);
        randomSpawnTile.SetUnit(spawnedNPCUnit);
        spawnedNPCUnit2.Initialize(100);
        randomSpawnTile2.SetUnit(spawnedNPCUnit2);

    }

    public void SpawnFriendlyUnit() {
        if (gameManager.Instance.gold >= spawnUnitCost && spawnUnitCost <= 2000000)
        {
            gameManager.Instance.gold -= spawnUnitCost;
            spawnUnitCost = Mathf.RoundToInt(spawnUnitCost * 1.5f);
            gameManager.Instance.UpdateGoldUI();
            gameManager.Instance.UpdateUpgradeButtonDisplay();

            Debug.Log(spawnUnitCost);

            var spawnedUnit = Instantiate(Resources.Load<scriptableUnit>("Units/playerKnight")?.Unitprefab);
            var randomSpawnTile = GridManager.Instance.GetUnitSpawnTile();
            spawnedUnit.Initialize(100);
            randomSpawnTile.SetUnit(spawnedUnit);
        }
        else {
            Debug.Log("Not enough gold to spawn a friendly unit!");
        }
    }

    private T getRandomUnit<T>(Faction faction) where T : baseUnit
    {
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().Unitprefab;
    }

    public void setSelectedPlayerUnit(basePlayerUnit playerUnit)
    {
        SelectedBasePlayerUnit = playerUnit;
        //Debug.Log(SelectedBasePlayerUnit.Health.getHealth());
    }

}
