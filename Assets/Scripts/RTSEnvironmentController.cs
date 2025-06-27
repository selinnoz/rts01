using UnityEngine;
using System.Collections.Generic;

public class RTSEnvironmentController : MonoBehaviour
{
    public static RTSEnvironmentController Instance { get; private set; }

    [Header("Prefablar (Project panelinden at)")]
    public GameObject townCenterPrefab;
    public GameObject villagerPrefab;
    public GameObject soldierPrefab;
    public GameObject archerPrefab;
    public GameObject cavalryPrefab;
    public GameObject farmPrefab;
    public GameObject woodPrefab;

    [Header("Spawn Alanı (2 boş GameObject, harita köşelerine)")]
    public Transform spawnAreaMin;
    public Transform spawnAreaMax;

    [Header("Sahnedeki Nesneler (Kod dolduracak)")]
    public GameObject currentTownCenter;
    public List<GameObject> villagers = new List<GameObject>();
    public List<GameObject> soldiers = new List<GameObject>();
    public List<GameObject> archers = new List<GameObject>();
    public List<GameObject> cavalries = new List<GameObject>();
    public List<GameObject> farms = new List<GameObject>();
    public List<GameObject> woods = new List<GameObject>();

    public int totalWood = 0;
    public int totalFood = 0;

    [Header("Agent Referansı (Inspector'dan Atanacak)")]
    public CommanderAgent agent;

    void Awake()
    {
        // Singleton. Multi environment için daha sonra Instance kaldırılabilir.
        if (Instance != null && Instance != this)
            Destroy(this.gameObject);
        else
            Instance = this;
    }

    void Start()
    {
        ResetEnvironment();
    }

    public void ResetEnvironment()
    {
        // Eski nesneleri temizle
        if (currentTownCenter) Destroy(currentTownCenter);
        foreach (var v in villagers) if (v) Destroy(v); villagers.Clear();
        foreach (var s in soldiers) if (s) Destroy(s); soldiers.Clear();
        foreach (var a in archers) if (a) Destroy(a); archers.Clear();
        foreach (var c in cavalries) if (c) Destroy(c); cavalries.Clear();
        foreach (var f in farms) if (f) Destroy(f); farms.Clear();
        foreach (var w in woods) if (w) Destroy(w); woods.Clear();

        totalWood = 200; // Başlangıç kaynak
        totalFood = 200;

        // TownCenter spawn
        Vector3 tcPos = GetRandomPosition();
        currentTownCenter = Instantiate(townCenterPrefab, tcPos, Quaternion.identity);

        // Başlangıç birimleri
        for (int i = 0; i < 3; i++)
        {
            var v = Instantiate(villagerPrefab, tcPos + new Vector3(i * 0.7f, 0, 0), Quaternion.identity);
            var villagerComp = v.GetComponent<Villager>();
            if (villagerComp != null)
                villagerComp.envController = this; // Bağlantı!
            villagers.Add(v);
        }

        cavalries.Add(Instantiate(cavalryPrefab, tcPos + new Vector3(2, 0, 1), Quaternion.identity));
        for (int i = 0; i < 2; i++)
            archers.Add(Instantiate(archerPrefab, tcPos + new Vector3(-i * 0.7f, 0, -1), Quaternion.identity));
        for (int i = 0; i < 3; i++)
            soldiers.Add(Instantiate(soldierPrefab, tcPos + new Vector3(i * 0.7f, 0, -2), Quaternion.identity));

        // Kaynak spawn (farm/wood)
        for (int i = 0; i < 20; i++)
        {
            var farm = Instantiate(farmPrefab, GetRandomPosition(), Quaternion.identity);
            var farmObj = farm.GetComponent<ResourceObject>();
            if (farmObj != null)
                farmObj.resourceType = ResourceType.Food;
            farms.Add(farm);

            var wood = Instantiate(woodPrefab, GetRandomPosition(), Quaternion.identity);
            var woodObj = wood.GetComponent<ResourceObject>();
            if (woodObj != null)
                woodObj.resourceType = ResourceType.Wood;
            woods.Add(wood);
        }
    }

    public Vector3 GetRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.position.x, spawnAreaMax.position.x);
        float y = Random.Range(spawnAreaMin.position.y, spawnAreaMax.position.y);
        return new Vector3(x, y, 0f); // 2D düzlemde Z=0
    }

    public bool CanAfford(UnitType type)
    {
        switch (type)
        {
            case UnitType.Villager: return totalFood >= 50;
            case UnitType.Soldier: return totalFood >= 50;
            case UnitType.Archer: return totalFood >= 60 && totalWood >= 20;
            case UnitType.Cavalry: return totalFood >= 100;
        }
        return false;
    }

    public bool PayCost(UnitType type)
    {
        if (!CanAfford(type)) return false;
        switch (type)
        {
            case UnitType.Villager: totalFood -= 50; return true;
            case UnitType.Soldier: totalFood -= 50; return true;
            case UnitType.Archer: totalFood -= 60; totalWood -= 20; return true;
            case UnitType.Cavalry: totalFood -= 100; return true;
        }
        return false;
    }

    public void SpawnUnit(UnitType type)
    {
        if (!PayCost(type)) return;
        Vector3 spawnPos = currentTownCenter.transform.position + (Vector3)Random.insideUnitCircle.normalized * 2f;
        if (type == UnitType.Villager)
        {
            var v = Instantiate(villagerPrefab, spawnPos, Quaternion.identity);
            var villagerComp = v.GetComponent<Villager>();
            if (villagerComp != null)
                villagerComp.envController = this;
            villagers.Add(v);
        }
        else if (type == UnitType.Soldier)
            soldiers.Add(Instantiate(soldierPrefab, spawnPos, Quaternion.identity));
        else if (type == UnitType.Archer)
            archers.Add(Instantiate(archerPrefab, spawnPos, Quaternion.identity));
        else if (type == UnitType.Cavalry)
            cavalries.Add(Instantiate(cavalryPrefab, spawnPos, Quaternion.identity));
    }

    public void AssignIdleVillagerToResource(ResourceType resType)
    {
        List<GameObject> resources = (resType == ResourceType.Food) ? farms : woods;
        GameObject target = null;
        float minDist = Mathf.Infinity;
        foreach (var v in villagers)
        {
            if (!v) continue;
            var villager = v.GetComponent<Villager>();
            if (villager != null && villager.state == VillagerState.Idle)
            {
                foreach (var r in resources)
                {
                    if (!r) continue;
                    var resObj = r.GetComponent<ResourceObject>();
                    if (resObj == null || resObj.IsDepleted()) continue;
                    float d = Vector3.Distance(v.transform.position, r.transform.position);
                    if (d < minDist)
                    {
                        minDist = d;
                        target = r;
                    }
                }
                if (target != null)
                {
                    villager.AssignResource(target);
                    break;
                }
            }
        }
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (type == ResourceType.Wood) totalWood += amount;
        if (type == ResourceType.Food) totalFood += amount;
        if (agent != null) agent.AddReward(0.05f);
    }

    // Sayaç getter'ları
    public int GetWoodCount() => totalWood;
    public int GetFoodCount() => totalFood;
    public int GetVillagerCount() => villagers.Count;
    public int GetMilitaryCount() => soldiers.Count + archers.Count + cavalries.Count;
}
