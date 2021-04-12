using Assets.Git.Scripts.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Git.Scripts.Menu.Gameplay
{
    public class GameplayManager : MonoBehaviour // this class essentially reads gameplay info to save it
    {
        /// <summary>
        /// Structure for saving games:
        /// type | name | key
        /// (what)
        /// int | chapter number | chapterNumber
        /// int | chapter part | chapterPart
        /// (where)
        /// float | x position | xPosition
        /// float | y position | yPosition
        /// float | z position | zPosition
        /// (ok, make sure quaternion => euler and back when reading/writing)
        /// float | x rotation | xRot
        /// float | y rotation | yRot
        /// float | z rotation | zRot
        /// (health)
        /// int | shield chargers | shieldChargers
        /// int | shield health | shieldHealth
        /// (weapons, grenades, gear)
        /// string | weapons | weapons (this needs a structure)
        /// string | grenades and stuff | ordinance (also needs structure)
        /// int | 9mm ammo | nineMilAmmo
        /// int | .45 ammo | fortyFiveAmmo
        /// int | 5mm light ammo | fiveMilLightAmmo
        /// int | 7mm light ammo | sevenMilLightAmmo
        /// int | 7mm heavy ammo | sevenMilHeavyAmmo
        /// int | 12 gauge ammo | twelveGaAmmo
        /// string | 12 gauge ammo type | twelveGaType | buck bird slug
        /// int | 6 gauge ammo | sixGaAmmo
        /// string | 6 gauge ammo type | sixGaType | buck bird thermite he
        /// int | .60 ammo | sixtyAmmo
        /// string | .60 ammo type | sixtyType (also needs structure)
        /// (grenades, note there are 2 ordinance slots, one tactical, one explosive. There is only 1 slot in multiplayer)
        /// int | flashbangs | flashGrenades 
        /// int | frag grenades | timedFrag
        /// int | impact grenades | impactFrag
        /// int | claymores | claymores
        /// bool | jumpkit enabled | jumpKit
        /// bool | grapple enabled | grapple
        /// (scoring)
        /// int | kills | kills
        /// int | deaths | deaths
        /// int | score | score
        /// int | kill streak | killStreak
        /// int | high priority targets killed | highPriorityKills
        /// int | non threatening npc kills | innocentKills
        /// (hidden achievements for endings)
        /// bool | unlabeled criteria | unlabaled criteria
        /// </summary>

        private (object[] data, SaveManager.GetStatus[] getStatus) loadedSaveData;
        private string[] intKeys = new string[21];
        public static PlayerData playerData { get; private set; }
        public void Awake()
        {
            playerData = new PlayerData();
            ReadData();
        }
        public void OnDestroy() => WriteData();
        #region integers
        public static int ChapterNumber { get { return playerData.ChapterNumber; } set { playerData.ChapterNumber = value; } }
        public static int ChapterPart { get { return playerData.ChapterPart; } set { playerData.ChapterPart = value; } }
        public static int ShieldChargers { get { return playerData.ShieldChargers; } set { playerData.ShieldChargers = value; } }
        public static int NineMilAmmo { get { return playerData.NineMilAmmo; } set { playerData.NineMilAmmo = value; } }
        public static int FortyFiveAmmo { get { return playerData.FortyFiveAmmo; } set { playerData.FortyFiveAmmo = value; } }
        public static int FiveMilLightAmmo { get { return playerData.FiveMilLightAmmo; } set { playerData.FiveMilLightAmmo = value; } }
        public static int SevenMilLightAmmo { get { return playerData.SevenMilLightAmmo; } set { playerData.SevenMilLightAmmo = value; } }
        public static int SevenMilHeavyAmmo { get { return playerData.SevenMilHeavyAmmo; } set { playerData.SevenMilHeavyAmmo = value; } }
        public static int TwelveGaugeAmmo { get { return playerData.TwelveGaugeAmmo; } set { playerData.TwelveGaugeAmmo = value; } }
        public static int SixGaugeAmmo { get { return playerData.SixGaugeAmmo; } set { playerData.SixGaugeAmmo = value; } }
        public static int SixtyAmmo { get { return playerData.SixtyAmmo; } set { playerData.SixtyAmmo = value; } }
        public static int FlashBangs { get { return playerData.FlashBangs; } set { playerData.FlashBangs = value; } }
        public static int FragGrenades { get { return playerData.FragGrenades; } set { playerData.FragGrenades = value; } }
        public static int ImpactGrenades { get { return playerData.ImpactGrenades; } set { playerData.ImpactGrenades = value; } }
        public static int Claymores { get { return playerData.Claymores; } set { playerData.Claymores = value; } }
        public static int Kills { get { return playerData.Kills; } set { playerData.Kills = value; } }
        public static int Deaths { get { return playerData.Deaths; } set { playerData.Deaths = value; } }
        public static int Score { get { return playerData.Score; } set { playerData.Score = value; } }
        public static int Killstreak { get { return playerData.Killstreak; } set { playerData.Killstreak = value; } }
        public static int HighPriorityKills { get { return playerData.HighPriorityKills; } set { playerData.HighPriorityKills = value; } }
        public static int InnocentKills { get { return playerData.InnocentKills; } set { playerData.InnocentKills = value; } }
        public static int ShieldHealth { get { return playerData.ShieldHealth; } set { playerData.ShieldHealth = value; } }

        #endregion

        #region floats

        public static float PlayerxPosition { get { return playerData.PlayerxPosition; } set { playerData.PlayerxPosition = value; } }
        public static float PlayeryPosition { get { return playerData.PlayeryPosition; } set { playerData.PlayeryPosition = value; } }
        public static float PlayerzPosition { get { return playerData.PlayerzPosition; } set { playerData.PlayerzPosition = value; } }
        public static float PlayerxRotation { get { return playerData.PlayerxRotation; } set { playerData.PlayerxRotation = value; } }
        public static float PlayeryRotation { get { return playerData.PlayeryRotation; } set { playerData.PlayeryRotation = value; } }
        public static float PlayerzRotation { get { return playerData.PlayerzRotation; } set { playerData.PlayerzRotation = value; } }

        #endregion
        private void ReadData() => playerData = GameplaySaveManager.Load();
        private void WriteData() => GameplaySaveManager.Save(playerData);
    }
}
