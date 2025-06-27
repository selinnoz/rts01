using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CommanderAgent : Agent
{
    public RTSEnvironmentController envController;
    public int CurrentEpisode { get; private set; } = 0;

    void Start()
    {
        Debug.Log("CommanderAgent Start!");
    }


    public override void OnEpisodeBegin()
    {
        CurrentEpisode++;
        if (envController != null)
        {
            envController.ResetEnvironment();
        }
        else
        {
            Debug.LogError("CommanderAgent: envController is NULL in OnEpisodeBegin!");
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (envController == null)
        {
            Debug.LogError("CommanderAgent: envController is NULL in CollectObservations!");
            sensor.AddObservation(0);
            sensor.AddObservation(0);
            sensor.AddObservation(0);
            sensor.AddObservation(0);
            return;
        }
        sensor.AddObservation(envController.GetWoodCount());
        sensor.AddObservation(envController.GetFoodCount());
        sensor.AddObservation(envController.GetVillagerCount());
        sensor.AddObservation(envController.GetMilitaryCount());
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        Debug.Log($"[AGENT] OnActionReceived step: {StepCount}");
        if (envController == null)
        {
            Debug.LogError("CommanderAgent: envController is NULL in OnActionReceived!");
            return;
        }

        int produce = actions.DiscreteActions[0]; // 0=none, 1=Villager, 2=Soldier, 3=Archer, 4=Cavalry
        int assignWood = actions.DiscreteActions[1];
        int assignFarm = actions.DiscreteActions[2];

        if (produce == 1 && envController.CanAfford(UnitType.Villager))
        {
            envController.SpawnUnit(UnitType.Villager);
            AddReward(0.1f);
        }
        else if (produce == 2 && envController.CanAfford(UnitType.Soldier))
        {
            envController.SpawnUnit(UnitType.Soldier);
            AddReward(0.2f);
        }
        else if (produce == 3 && envController.CanAfford(UnitType.Archer))
        {
            envController.SpawnUnit(UnitType.Archer);
            AddReward(0.25f);
        }
        else if (produce == 4 && envController.CanAfford(UnitType.Cavalry))
        {
            envController.SpawnUnit(UnitType.Cavalry);
            AddReward(0.3f);
        }

        if (assignWood == 1)
        {
            envController.AssignIdleVillagerToResource(ResourceType.Wood);
            AddReward(0.05f);
        }
        if (assignFarm == 1)
        {
            envController.AssignIdleVillagerToResource(ResourceType.Food);
            AddReward(0.05f);
        }

        AddReward(-0.001f); // Her adımda küçük ceza
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Debug.Log("===> Heuristic ÇAĞRILDI!");
        var d = actionsOut.DiscreteActions;
        d[0] = 0; // produce
        d[1] = 0; // assignWood
        d[2] = 0; // assignFarm

        if (Input.GetKeyDown(KeyCode.V))
            d[0] = 1; // Villager üret
        if (Input.GetKeyDown(KeyCode.S))
            d[0] = 2; // Soldier üret
        if (Input.GetKeyDown(KeyCode.A))
            d[0] = 3; // Archer üret
        if (Input.GetKeyDown(KeyCode.C))
            d[0] = 4; // Cavalry üret
        if (Input.GetKeyDown(KeyCode.W))
            d[1] = 1; // Wood'a ata
        if (Input.GetKeyDown(KeyCode.F))
            d[2] = 1; // Farm'a ata
    }
}
