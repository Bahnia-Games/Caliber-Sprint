using UnityEngine;

namespace Assets.Git.Scripts.Gameplay
{
    public enum CollisionType
    {
        collider,
        trigger
    }
    public interface ITrigger
    {
        void Awake();
        void Update();

        void OnTriggerEnter(Collider c);
        void OnTriggerExit(Collider c);
        void OnTriggerStay(Collider c);
        void OnCollisionEnter(Collision c);
        void OnCollisionExit(Collision c);
        void OnCollisionStay(Collision c);

    }
}
