using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Git.Scripts.Menu
{
    public class QuickMenu : MonoBehaviour
    {
        //[SerializeField] private Canvas canvas;
        [SerializeField] private GameObject PauseMenuContainer;
        public void Update() { if (Input.GetKeyDown(KeyCode.Escape)) { EscapeOnClick(); }}
        private void EscapeOnClick()
        {
            PauseMenuContainer.SetActive(true);
        }
        private void ResumeOnClick() { PauseMenuContainer.SetActive(false); /* time stuff here */ }
    }
}
