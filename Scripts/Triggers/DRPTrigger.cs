using UnityEngine;

public class DRPTrigger : MonoBehaviour
{
    [SerializeField]
    private DRP DRP;
    public string message;
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            MasterTrigger.UpdateDRP(message);
        }
    }
}
