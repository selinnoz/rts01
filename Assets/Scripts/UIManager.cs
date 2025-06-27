using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI epText;
    public TextMeshProUGUI rewardText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI villagerText;
    public TextMeshProUGUI militaryText;

    public CommanderAgent agent;
    public RTSEnvironmentController envController;

    void Update()
    {
        epText.text = $"Ep: {agent.CurrentEpisode}";
        rewardText.text = $"Reward: {agent.GetCumulativeReward():F2}";
        woodText.text = $"Wood: {envController.GetWoodCount()}";
        foodText.text = $"Food: {envController.GetFoodCount()}";
        villagerText.text = $"Villager: {envController.GetVillagerCount()}";
        militaryText.text = $"Military: {envController.GetMilitaryCount()}";
    }
}
