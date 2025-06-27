using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class RTSAgent : Agent
{
    public float woodCount = 0f;
    public float foodCount = 0f;
    public int soldierCount = 0;
    public int archerCount = 0;
    public int villagerCount = 0;

    public float woodGatherReward = 0.1f;
    public float farmBuildReward = 0.5f;
    public float soldierProduceReward = 1.0f;
    public float archerProduceReward = 1.2f;
    public float villagerProduceReward = 0.8f;
    public float enemySoldierKillReward = 5.0f; // Düşman askerini öldürme ödülü
    public float enemyBuildingDestroyReward = 10.0f; // Düşman yapısını yıkma ödülü


    public override void OnEpisodeBegin()
    {
        
        woodCount = 0f;
        foodCount = 0f;
        soldierCount = 0;
        archerCount = 0;
        villagerCount = 0;
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Gözlemler:
        sensor.AddObservation(woodCount);
        sensor.AddObservation(foodCount);
        sensor.AddObservation(soldierCount);
        sensor.AddObservation(archerCount);
        sensor.AddObservation(villagerCount);
        // Diğer gözlemler: Oyun süresi, harita bilgisi vb.
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Ajanın aldığı eylemler
        int action = actions.DiscreteActions[0]; // Tek bir discrete action (örn: 0=Hiçbir şey yapma, 1=Odun topla, 2=Tarla kur, 3=Asker üret, 4=Okçu üret, 5=Köylü üret)

        switch (action)
        {
            case 0: // Hiçbir şey yapma (pasif aksiyon)
                AddReward(-0.001f); // Zaman kaybı için küçük bir ceza
                break;
            case 1: // Odun topla
                woodCount += 1f; // Basitçe odun arttır
                AddReward(woodGatherReward);
                break;
            case 2: // Tarla kur
                if (woodCount >= 5 && foodCount < 10) // Örnek: Tarla için 5 odun ve max 10 tarlamız olabilir
                {
                    woodCount -= 5;
                    foodCount += 2; // Tarla kurulunca yiyecek artışını temsil
                    AddReward(farmBuildReward);
                }
                else
                {
                    AddReward(-0.1f); // Geçersiz aksiyon veya kaynak yetersizliği
                }
                break;
            case 3: // Asker üret
                if (woodCount >= 10 && foodCount >= 5) // Örnek: Asker için 10 odun 5 yiyecek
                {
                    woodCount -= 10;
                    foodCount -= 5;
                    soldierCount++;
                    AddReward(soldierProduceReward);
                }
                else
                {
                    AddReward(-0.1f);
                }
                break;
            case 4: // Okçu üret
                if (woodCount >= 12 && foodCount >= 6)
                {
                    woodCount -= 12;
                    foodCount -= 6;
                    archerCount++;
                    AddReward(archerProduceReward);
                }
                else
                {
                    AddReward(-0.1f);
                }
                break;
            case 5: // Köylü üret
                if (woodCount >= 7 && foodCount >= 3)
                {
                    woodCount -= 7;
                    foodCount -= 3;
                    villagerCount++;
                    AddReward(villagerProduceReward);
                }
                else
                {
                    AddReward(-0.1f);
                }
                break;
        }

        // Oyunun bitiş koşulları ve toplam puan (AddReward ile episode sonunda da verilebilir)
        // Eğer belirli bir puan hedefine ulaşılırsa veya zaman biterse EndEpisode() çağrılabilir.
        // Örneğin, 100 saniye sonra veya 50 puan olunca bölümü bitir:
        if (GetCumulativeReward() > 50 || Academy.Instance.TotalStepCount % 1000 == 0) // Örnek
        {
            EndEpisode();
        }
    }

    // Heuristisk mod (Manuel test için)
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionsOut = actionsOut.DiscreteActions;
        discreteActionsOut[0] = 0; // Varsayılan olarak hiçbir şey yapma

        if (Input.GetKey(KeyCode.Alpha1)) discreteActionsOut[0] = 1; // Odun topla
        if (Input.GetKey(KeyCode.Alpha2)) discreteActionsOut[0] = 2; // Tarla kur
        if (Input.GetKey(KeyCode.Alpha3)) discreteActionsOut[0] = 3; // Asker üret
        if (Input.GetKey(KeyCode.Alpha4)) discreteActionsOut[0] = 4; // Okçu üret
        if (Input.GetKey(KeyCode.Alpha5)) discreteActionsOut[0] = 5; // Köylü üret
    }
}