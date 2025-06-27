using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    public ResourceType resourceType;
    public int resourceValue = 400;

    public int GiveResource(int amount)
    {
        int actual = Mathf.Min(amount, resourceValue);
        resourceValue -= actual;
        return actual;
    }

    public bool IsDepleted() => resourceValue <= 0;
}
