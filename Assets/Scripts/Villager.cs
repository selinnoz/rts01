using UnityEngine;

public class Villager : MonoBehaviour
{
    public RTSEnvironmentController envController;
    public VillagerState state = VillagerState.Idle;
    public GameObject targetResource;
    public float speed = 3f;
    private float workTimer = 0f;
    private float workInterval = 5f;
    void Start()
    {
        if (envController == null)
            Debug.LogWarning($"{gameObject.name} envController atanmadı!");
    }


    void Update()
    {


        // Eğer envController yoksa hiçbir şey yapma (yoksa Idle'a çek)
        if (envController == null)
        {
            state = VillagerState.Idle;
            targetResource = null;
            return;
        }

        // WALKING
        if (state == VillagerState.Walking)
        {
            if (targetResource == null)
            {
                state = VillagerState.Idle;
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetResource.transform.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetResource.transform.position) < 0.1f)
            {
                state = VillagerState.Working;
                workTimer = 0f;
            }
        }
        // WORKING
        else if (state == VillagerState.Working)
        {
            if (targetResource == null)
            {
                state = VillagerState.Idle;
                return;
            }

            workTimer += Time.deltaTime;
            if (workTimer >= workInterval)
            {
                var resource = targetResource.GetComponent<ResourceObject>();
                if (resource != null && !resource.IsDepleted())
                {
                    int gathered = resource.GiveResource(10);
                    envController.AddResource(resource.resourceType, gathered);
                }
                else
                {
                    targetResource = null;
                    state = VillagerState.Idle;
                }
                workTimer = 0f;
            }
        }
    }

    public void AssignResource(GameObject resource)
    {
        Debug.Log($"Villager {gameObject.name} assigned to resource {resource?.name}");
        targetResource = resource;
        state = VillagerState.Walking;
    }

}
