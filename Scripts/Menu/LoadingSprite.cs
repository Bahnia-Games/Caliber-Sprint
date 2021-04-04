using Assets.Git.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Git.Scripts.Menu
{
    [RequireComponent(typeof(Image))]
    public class LoadingSprite : MonoBehaviour // thanks AQaddora2020
    {
        public float duration;
        [SerializeField] private Sprite[] sprites;
        private Image image;
        private int index = 0;
        private float timer = 0;
        void Start() => image = GetComponent<Image>();
        private void Update()
        {
            if ((timer += Time.deltaTime) >= (duration / sprites.Length))
            {
                timer = 0;
                image.sprite = sprites[index];
                index = (index + 1) % sprites.Length;
            }
        }
    }
}
