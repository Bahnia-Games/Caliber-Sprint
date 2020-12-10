using UnityEngine;

public class DRPTrigger : MonoBehaviour
{
    [SerializeField]
    private DRP DRP;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            MasterTrigger.UpdateDRP("Falling under the map.");
        }
    }
}
