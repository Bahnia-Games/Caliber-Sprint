using Assets.Git.Scripts.Misc;
using UnityEngine;
using System;

namespace Assets.Git.Scripts.Player.Inputs
{
    /// <summary>
    /// To set the controls, do not access the variables directly, instead access the SetControl method
    /// </summary>
    public class InputHandler // controls are saved into playerprefs
    {
        /// <summary>
        /// Controls summary:
        /// ## Movement ##
        /// Jump
        /// Crouch/slide (named crouch)
        /// Fire grapple
        /// 
        /// ## Weapons ## (Equip will always be alpha 1-9)
        /// Fire
        /// Aim
        /// Quick Switch (not yet implimented)
        /// Un Equip
        /// Quick Melee (not yet implimented)
        /// Throw grenade
        /// 
        /// </summary>
        
        public enum Inputs
        {
            Jump,
            Crouch,
            FireGrapple,
            Fire,
            Aim,
            QuickSwitch,
            UnEquip,
            QuickMelee,
            ThrowGrenade,
            // ect...
        }

        public enum Status
        {
            ok,
            notfound
        }

        public static KeyCode JumpKC;
        public static KeyCode CrouchKC;
        public static KeyCode FireGrappleKC;
        public static KeyCode FireKC;
        public static KeyCode AimKC;
        public static KeyCode QuickSwitchKC;
        public static KeyCode UnEquipKC;
        public static KeyCode QuickMeleeKC;
        public static KeyCode ThrowGrenadeKC;


        static InputHandler() => MasterMiscController.ApplicationQuitRequest += Save; // shouldnt need references...

        internal static void Save(object unused, QuitEventArgs e)
        {
            if (e.quitReason == QuitEventArgs.QuitReason.error)
                return;
            SaveManager.AddData(Inputs.Jump.ToString(), JumpKC.ToString());
            SaveManager.AddData(Inputs.Crouch.ToString(), CrouchKC.ToString());
            SaveManager.AddData(Inputs.FireGrapple.ToString(), FireGrappleKC.ToString());
            SaveManager.AddData(Inputs.Fire.ToString(), FireKC.ToString());
            SaveManager.AddData(Inputs.Aim.ToString(), AimKC.ToString());
            SaveManager.AddData(Inputs.QuickSwitch.ToString(), QuickSwitchKC.ToString());
            SaveManager.AddData(Inputs.UnEquip.ToString(), UnEquipKC.ToString());
            SaveManager.AddData(Inputs.QuickMelee.ToString(), QuickMeleeKC.ToString());
            SaveManager.AddData(Inputs.ThrowGrenade.ToString(), ThrowGrenadeKC.ToString());
        }

        internal static Status Load()
        {
            (object data, SaveManager.GetStatus status) jData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.Jump.ToString());
            if (jData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            JumpKC = (KeyCode)Enum.Parse(typeof(KeyCode), (string)jData.data);

            (object data, SaveManager.GetStatus status) cData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.Crouch.ToString());
            if (cData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            CrouchKC = (KeyCode)Enum.Parse(typeof(KeyCode), (string)cData.data);

            (object data, SaveManager.GetStatus status) fgData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.FireGrapple.ToString());
            if (fgData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            FireGrappleKC = (KeyCode)Enum.Parse(typeof(KeyCode), (string)fgData.data);

            (object data, SaveManager.GetStatus status) fData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.Fire.ToString());
            if (fgData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            FireKC = (KeyCode)Enum.Parse(typeof(KeyCode), (string)fData.data);

            (object data, SaveManager.GetStatus status) aData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.Aim.ToString());
            if (aData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            AimKC = (KeyCode)Enum.Parse(typeof(KeyCode), (string)aData.data);

            (object data, SaveManager.GetStatus status) qsData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.QuickSwitch.ToString());
            if (qsData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            QuickSwitchKC = (KeyCode)Enum.Parse(typeof(KeyCode), (string)qsData.data);

            (object data, SaveManager.GetStatus status) uData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.UnEquip.ToString());
            if (uData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            UnEquipKC = (KeyCode)Enum.Parse(typeof(KeyCode), (string)uData.data);

            (object data, SaveManager.GetStatus status) qmData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.QuickMelee.ToString());
            if (qmData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            QuickMeleeKC = (KeyCode)Enum.Parse(typeof(KeyCode), (string)qmData.data);

            (object data, SaveManager.GetStatus status) gData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.ThrowGrenade.ToString());
            if (gData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            ThrowGrenadeKC = (KeyCode)Enum.Parse(typeof(KeyCode), (string)qmData.data);

            return Status.ok;
        }

        /// <summary>
        /// This here should only be called by Master Misc Controller, and nothing else.
        /// </summary>
        internal static void AssignDefaults()
        {
            JumpKC = KeyCode.Space;
            CrouchKC = KeyCode.LeftShift;
            FireGrappleKC = KeyCode.F;
            FireKC = KeyCode.Mouse0;
            AimKC = KeyCode.Mouse1;
            //QuickSwitchKC = KeyCode.; /* (not implimented) */
            UnEquipKC = KeyCode.E;
            //QuickMeleeKC = KeyCode.; /* (not implimented) */
            ThrowGrenadeKC = KeyCode.Q;
        }

        internal static void SetControl(Inputs control, KeyCode key)
        {
            switch (control)
            {
                case Inputs.Jump:
                    JumpKC = key;
                    break;
                case Inputs.Crouch:
                    CrouchKC = key;
                    break;
                case Inputs.FireGrapple:
                    FireGrappleKC = key;
                    break;
                case Inputs.Fire:
                    FireKC = key;
                    break;
                case Inputs.Aim:
                    AimKC = key;
                    break;
                case Inputs.QuickSwitch:
                    QuickSwitchKC = key;
                    break;
                case Inputs.UnEquip:
                    UnEquipKC = key;
                    break;
                case Inputs.QuickMelee:
                    QuickMeleeKC = key;
                    break;
                case Inputs.ThrowGrenade:
                    ThrowGrenadeKC = key;
                    break;
            }
        }
    }
}