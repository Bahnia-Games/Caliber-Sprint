using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Git.Scripts.Gameplay
{
    [Serializable]
    ///<summary>
    /// DO NOT ACCESS THIS CLASS DIRECTLY
    /// </summary>
    public class PlayerData
    {
        /// <summary>
        /// DO NOT WRITE TO THIS CLASS DIRECTLY OR I WILL SHOOT YOU
        /// </summary>
        public int ChapterNumber;
        public int ChapterPart;
        public int ShieldChargers;
        public int NineMilAmmo;
        public int FortyFiveAmmo;
        public int FiveMilLightAmmo;
        public int SevenMilLightAmmo;
        public int SevenMilHeavyAmmo;
        public int TwelveGaugeAmmo;
        public int SixGaugeAmmo;
        public int SixtyAmmo;
        public int FlashBangs;
        public int FragGrenades;
        public int ImpactGrenades;
        public int Claymores;
        public int Kills;
        public int Deaths;
        public int Score;
        public int Killstreak;
        public int HighPriorityKills;
        public int InnocentKills;
        public int ShieldHealth;
        public float PlayerxPosition;
        public float PlayeryPosition;
        public float PlayerzPosition;
        public float PlayerxRotation;
        public float PlayeryRotation;
        public float PlayerzRotation;
    }
}
