using UnityEngine;

public class AgentDebugTrigger : MonoBehaviour
{
    public CommanderAgent agent;

    void Update()
    {
        // Space tuşuna basınca agent'a karar verdir.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("=== SPACE BASILDI, RequestDecision çağrılıyor! ===");
            agent.RequestDecision();
        }
    }
}
