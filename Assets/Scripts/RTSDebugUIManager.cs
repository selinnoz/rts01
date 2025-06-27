using UnityEngine;

public class RTSDebugUIManager : MonoBehaviour
{
    public RTSEnvironmentController envController;

    public void SpawnVillager() { envController.SpawnUnit(UnitType.Villager); }
    public void SpawnSoldier() { envController.SpawnUnit(UnitType.Soldier); }
    public void SpawnArcher() { envController.SpawnUnit(UnitType.Archer); }
    public void SpawnCavalry() { envController.SpawnUnit(UnitType.Cavalry); }
    public void AssignIdleVillagerToWood() { envController.AssignIdleVillagerToResource(ResourceType.Wood); }
    public void AssignIdleVillagerToFarm() { envController.AssignIdleVillagerToResource(ResourceType.Food); }
    
    
}
