using System;
using Assets.Git.Scripts.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Git.Scripts.Player.Inputs
{
    /// <summary>
    /// To set the controls, access the SetControl method
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
            noparse,
            notfound
        }

        public static KeyCode JumpKC { get; private set; }
        public static KeyCode CrouchKC { get; private set; }
        public static KeyCode FireGrappleKC { get; private set; }
        public static KeyCode FireKC { get; private set; }
        public static KeyCode AimKC { get; private set; }
        public static KeyCode QuickSwitchKC { get; private set; }
        public static KeyCode UnEquipKC { get; private set; }
        public static KeyCode QuickMeleeKC { get; private set; }
        public static KeyCode ThrowGrenadeKC { get; private set; }

        static bool IHFO = true;

        static InputHandler() 
        {
            if (IHFO) // subscription
            {
                MasterMiscController.ApplicationQuitRequest += Save;
                SceneManager.activeSceneChanged += SceneChanged;
                IHFO = false;
            }
        }

        private static void SceneChanged(Scene c, Scene n) => IHFO = true;
        internal static void Save(object unused = null, QuitEventArgs e = null)
        {
            if (e != null)
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
            SaveManager.Save();
        }

        /// <summary>
        /// This should really only be called once... i think...
        /// VERY IMPORTANT: keybinds only load at the end of a frame.
        /// </summary>
        /// <returns></returns>
        internal static Status Load()
        {
            (object data, SaveManager.GetStatus status) jData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.Jump.ToString()); // serves as a model
            Debug.Log("Loading keybinds... "/*jData: " + jData.data.ToString()*/);
            //JumpKC = (KeyCode)Enum.Parse(typeof(KeyCode), (string)jData.data);
            if (jData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            if (Enum.TryParse<KeyCode>((string)jData.data, out KeyCode jr)) // ignore cases
                JumpKC = jr;
            else
            { LoadFail("Jump"); return Status.noparse; }
            // :(
                
            (object data, SaveManager.GetStatus status) cData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.Crouch.ToString());
            if (cData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            if (Enum.TryParse<KeyCode>((string)cData.data, true, out KeyCode cr))
                CrouchKC = cr;
            else
            { LoadFail("Crouch"); return Status.noparse; }

            (object data, SaveManager.GetStatus status) fgData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.FireGrapple.ToString());
            if (fgData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            if (Enum.TryParse<KeyCode>((string)fgData.data, true, out KeyCode fgr))
                FireGrappleKC = fgr;
            else
            { LoadFail("Fire Grapple"); return Status.noparse; }

            (object data, SaveManager.GetStatus status) fData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.Fire.ToString());
            if (fgData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            if (Enum.TryParse<KeyCode>((string)fData.data, true, out KeyCode fr))
                FireKC = fr;
            else
            { LoadFail("Fire"); return Status.noparse; }

            (object data, SaveManager.GetStatus status) aData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.Aim.ToString());
            if (aData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            if (Enum.TryParse<KeyCode>((string)aData.data, true, out KeyCode ar))
                AimKC = ar;
            else
            { LoadFail("Aim"); return Status.noparse; }

            (object data, SaveManager.GetStatus status) qsData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.QuickSwitch.ToString());
            if (qsData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            if (Enum.TryParse<KeyCode>((string)qsData.data, true, out KeyCode qsr))
                QuickSwitchKC = qsr;
            else
            { LoadFail("Quick Switch"); return Status.noparse; }

            (object data, SaveManager.GetStatus status) uData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.UnEquip.ToString());
            if (uData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            if (Enum.TryParse<KeyCode>((string)uData.data, true, out KeyCode ur))
                UnEquipKC = ur;
            else
            { LoadFail("Unequip"); return Status.noparse; }

            (object data, SaveManager.GetStatus status) qmData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.QuickMelee.ToString());
            if (qmData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            if (Enum.TryParse<KeyCode>((string)qmData.data, true, out KeyCode qmr))
                QuickMeleeKC = qmr;
            else
            { LoadFail("QuickMelee"); return Status.noparse; }

            (object data, SaveManager.GetStatus status) gData = SaveManager.Load(SaveManager.DataType.dstring, Inputs.ThrowGrenade.ToString());
            if (gData.status == SaveManager.GetStatus.notfound)
                return Status.notfound;
            if (Enum.TryParse<KeyCode>((string)gData.data, true, out KeyCode gr))
                ThrowGrenadeKC = gr;
            else
            { LoadFail("Grenade"); return Status.noparse; }

            return Status.ok;
        }

        private static void LoadFail(string KeyName) => Debug.LogError("Unable to find control for key " + KeyName);

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
            Save();
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