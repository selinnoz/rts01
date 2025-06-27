using UnityEngine;

public class TownCenter : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject villagerPrefab;
    public int maxVillagers = 10;
    public int currentVillagers = 0;

    public Villager ProduceVillager()
    {
        if (currentVillagers >= maxVillagers)
            return null;

        GameObject newVillagerObj = Instantiate(villagerPrefab, spawnPoint.position, Quaternion.identity);
        Villager newVillager = newVillagerObj.GetComponent<Villager>();
        currentVillagers++;
        return newVillager;
    }
}
