﻿using System.Collections; using UnityEngine; using UnityEngine.SceneManagement; namespace Assets.Git.Scripts.Menu { public class IntroSequenceController : MonoBehaviour { [SerializeField] private Animator animator; [SerializeField] private string introClip; private void OnEnable() => SceneManager.sceneLoaded += OnLoadScene; private void OnLoadScene(Scene unused, LoadSceneMode _unused) => StartCoroutine(WaitAndPlay()); private IEnumerator WaitAndPlay() { yield return new WaitForEndOfFrame(); animator.Play(introClip); }} } // lololololololololololololololololol