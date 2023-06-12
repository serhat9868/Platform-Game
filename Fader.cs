using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    public class Fader : MonoBehaviour
    {
        [SerializeField] float startingFadeInTime = 2f;

        CanvasGroup canvasGroup;
        Coroutine currentActiveFade = null;
        void Start()
        {
            canvasGroup = FindObjectOfType<CanvasGroup>();
            StartCoroutine(FadeInAtStart());
        }
        public IEnumerator FadeInAtStart()
        {
            while (canvasGroup.alpha != 0)
            {
                yield return canvasGroup.alpha -= Time.deltaTime / startingFadeInTime;
            }
        }
        public Coroutine FadeOut(float time)
        {
            return Fade(1, time);
        }

        public Coroutine FadeIn(float time)
        {
            return Fade(0, time);
        }

        private Coroutine Fade(float target, float time)
        {
            if (currentActiveFade != null)
            {
                StopCoroutine(currentActiveFade);
            }
            currentActiveFade = StartCoroutine(FadeRoutine(target, time));
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(canvasGroup.alpha, target))
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, time);
                yield return null;
            }
        }
    }