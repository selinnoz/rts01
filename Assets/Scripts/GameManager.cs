using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int wood = 200;
    public int food = 200;

    public Text woodText;
    public Text foodText;

    public Transform villagerSpawnPoint;
    public GameObject villagerPrefab;
    public GameObject soldierPrefab;
    public GameObject archerPrefab;
    public GameObject cavalryPrefab;

    void Update()
    {
        woodText.text = $"Wood: {wood}";
        foodText.text = $"Food: {food}";
    }

    public void ProduceVillager()
    {
        if (food >= 50)
        {
            food -= 50;
            Instantiate(villagerPrefab, villagerSpawnPoint.position, Quaternion.identity);
        }
    }

    public void ProduceSoldier()
    {
        if (food >= 50)
        {
            food -= 50;
            Instantiate(soldierPrefab, villagerSpawnPoint.position, Quaternion.identity);
        }
    }

    public void ProduceArcher()
    {
        if (food >= 60 && wood >= 20)
        {
            food -= 60;
            wood -= 20;
            Instantiate(archerPrefab, villagerSpawnPoint.position, Quaternion.identity);
        }
    }

    public void ProduceCavalry()
    {
        if (food >= 100)
        {
            food -= 100;
            Instantiate(cavalryPrefab, villagerSpawnPoint.position, Quaternion.identity);
        }
    }
}
