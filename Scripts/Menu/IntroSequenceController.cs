﻿using Assets.Git.Scripts.Misc;namespace Assets.Git.Scripts.Menu{using System.Collections;using UnityEngine;using UnityEngine.SceneManagement;namespace Assets.Git.Scripts.Menu{public class IntroSequenceController:MonoBehaviour{[SerializeField]private Animator animator;[SerializeField]private string introClip;[SerializeField]private float sequenceTime=1.0f;[SerializeField]private MasterMiscController mmc;private void OnEnable()=>SceneManager.sceneLoaded+=OnLoadScene;private void OnLoadScene(Scene unused,LoadSceneMode _unused)=>StartCoroutine(WaitAndPlay());private IEnumerator WaitAndPlay(){yield return new WaitForEndOfFrame();animator.Play(introClip);yield return new WaitForSeconds(sequenceTime);mmc.LoadScene(MasterMiscController.Scenes.MainMenu, true);}}}}//causes nonfatal missing reference exception