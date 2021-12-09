using System;
using UnityEngine;
using Assets.Git.Scripts.Misc;
using Discord;

namespace Assets.Git.Scripts.Gameplay
{
    [RequireComponent(typeof(MasterMiscController))]
    class DRPTrigger : MonoBehaviour, ITrigger
    {
        [SerializeField] private MasterMiscController mmc;
        private MasterDRPController mrp;

        #region trigger properties

        [Header("Trigger Properties")]

        [Header("OnTriggerEnter")]
        public string triggerEnterState,
            triggerEnterDetail,
            triggerEnterLargeImage,
            triggerEnterSmallImage,
            triggerEnterLargeText,
            triggerEnterSmallText;
        public bool triggerEnterUseTimestamp;

        [Header("OnTriggerExit")]
        public string triggerExitState,
            triggerExitDetail,
            triggerExitLargeImage,
            triggerExitSmallImage,
            triggerExitLargeText,
            triggerExitSmallText;
        public bool triggerExitUseTimestamp;

        [Header("OnTriggerStay")]
        public string triggerStayState,
            triggerStayDetail,
            triggerStayLargeImage,
            triggerStaySmallImage,
            triggerStayLargeText,
            triggerStaySmallText;
        public bool triggerStayUseTimestamp;

        [Header("OnCollisionEnter")]
        public string collisionEnterState,
            collisionEnterDetail,
            collisionEnterLargeImage,
            collisionEnterSmallImage,
            collisionEnterLargeText,
            collisionEnterSmallText;
        public bool collisionEnterUseTimestamp;

        [Header("OnCollisionExit")]
        public string collisionExitState,
            collisionExitDetail,
            collisionExitLargeImage,
            collisionExitSmallImage,
            collisionExitLargeText,
            collisionExitSmallText;
        public bool collisionExitUseTimestamp;

        [Header("OnCollisionStay")]
        public string collisionStayState,
            collisionStayDetail,
            collisionStayLargeImage,
            collisionStaySmallImage,
            collisionStayLargeText,
            collisionStaySmallText;
        public bool collisionStayUseTimestamp;

        #endregion

        public void Awake()
        {
            if (mmc.TryGetComponent<MasterDRPController>(out MasterDRPController tmrp))
                mrp = tmrp;
            else
                throw new Exception("No Matser DRP controller found on this scene's Master Misc Controller. @DRPTrigger.cs Awake()");
        }

        public void Update()
        {
            //throw new NotImplementedException();
        }

        public void OnTriggerEnter(Collider c)
        {
            ActivityTimestamps? ts;
            if (triggerEnterUseTimestamp)
                ts = new ActivityTimestamps
                { Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() };
            else
                ts = null;
            mrp.UpdatePresence(new Activity {
                State = triggerEnterState,
                Details = triggerEnterDetail,
                Assets = new ActivityAssets
                {
                    LargeImage = triggerEnterLargeImage,
                    LargeText = triggerEnterLargeText,
                    SmallImage = triggerEnterSmallImage,
                    SmallText = triggerEnterSmallText
                },
                Timestamps = (ActivityTimestamps)ts,
            });
        }

        public void OnTriggerExit(Collider c)
        {
            ActivityTimestamps? ts;
            if (triggerExitUseTimestamp)
                ts = new ActivityTimestamps
                { Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() };
            else
                ts = null;
            mrp.UpdatePresence(new Activity {
                State = triggerExitState,
                Details = triggerExitDetail,
                Assets = new ActivityAssets
                {
                    LargeImage = triggerExitLargeImage,
                    LargeText = triggerExitLargeText,
                    SmallImage = triggerExitSmallImage,
                    SmallText = triggerExitSmallText
                },
                Timestamps = (ActivityTimestamps)ts                
            });
        }

        public void OnTriggerStay(Collider c)
        {
            ActivityTimestamps? ts;
            if (triggerStayUseTimestamp)
                ts = new ActivityTimestamps
                { Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() };
            else
                ts = null;
            mrp.UpdatePresence(new Activity { 
                State = triggerStayState,
                Details = triggerStayDetail,
                Assets = new ActivityAssets
                {
                    LargeImage = triggerStayLargeImage,
                    LargeText = triggerStayLargeText,
                    SmallImage = triggerStaySmallImage,
                    SmallText = triggerStaySmallText
                },
                Timestamps = (ActivityTimestamps)ts
            });
        }

        public void OnCollisionEnter(Collision c)
        {
            ActivityTimestamps? ts;
            if (collisionEnterUseTimestamp)
                ts = new ActivityTimestamps { Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() };
            else
                ts = null;
            mrp.UpdatePresence(new Activity
            {
                State = collisionEnterState, 
                Details = collisionEnterDetail, 
                Assets = new ActivityAssets
                { 
                    LargeImage = collisionEnterLargeImage,
                    LargeText = collisionEnterLargeText,
                    SmallImage = collisionEnterSmallImage,
                    SmallText = collisionEnterSmallText
                },
                Timestamps = (ActivityTimestamps)ts
            });
        }

        public void OnCollisionExit(Collision c)
        {
            ActivityTimestamps? ts;
            if (collisionExitUseTimestamp)
                ts = new ActivityTimestamps { Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() };
            else
                ts = null;
            mrp.UpdatePresence(new Activity
            {
                State = collisionExitState,
                Details = collisionExitDetail,
                Assets = new ActivityAssets
                {
                    LargeImage = collisionExitLargeImage,
                    LargeText = collisionExitLargeText,
                    SmallImage = collisionExitSmallImage,
                    SmallText = collisionExitSmallText
                },
                Timestamps = (ActivityTimestamps)ts
            });
        }

        public void OnCollisionStay(Collision c)
        {
            ActivityTimestamps? ts;
            if (collisionStayUseTimestamp)
                ts = new ActivityTimestamps { Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds() };
            else
                ts = null;
            mrp.UpdatePresence(new Activity
            {
                State = collisionStayState,
                Details = collisionStayDetail,
                Assets = new ActivityAssets
                {
                    LargeImage = collisionStayLargeImage,
                    LargeText = collisionStayLargeText,
                    SmallImage = collisionStaySmallImage,
                    SmallText = collisionStaySmallText
                },
                Timestamps = (ActivityTimestamps)ts
            });
        }
    }
}