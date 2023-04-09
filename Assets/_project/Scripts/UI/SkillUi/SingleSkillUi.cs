using System.Collections;
using System.Collections.Generic;
using _project.Scripts.Managers;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI
{
    public class SingleSkillUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI skillNameTextMeshProUGUI;
        [SerializeField] private TextMeshProUGUI skillDescriptionTextMeshProUGUI;
        [SerializeField] private Image skillSprite;
        [SerializeField] private List<Image> skillLevelStars;
        [SerializeField] private Button button;

        private SkillPromptInfo skillPromptInfo;

        private WaitForSeconds waitFor;

        private Coroutine myCoroutine;

        private Image newStarImage;

        private bool active;
        private bool initialized;

        public void Initialize(SkillPromptInfo skillPromptInfo)
        {
            // SetWaitTime();
            gameObject.SetActive(true);
            this.skillPromptInfo = skillPromptInfo;

            skillNameTextMeshProUGUI.text = this.skillPromptInfo.SkillName;
            skillDescriptionTextMeshProUGUI.text = this.skillPromptInfo.SkillDescription;
            skillSprite.sprite = this.skillPromptInfo.SkillSprite;

            for (var i = 0; i < skillLevelStars.Count; i++)
            {
                skillLevelStars[i].gameObject.SetActive(i < this.skillPromptInfo.SkillLevel);
            }

            var action = this.skillPromptInfo.Action;
            var skillType = this.skillPromptInfo.SkillType;
            
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate { action(skillType, this); });

            active = true;
            // _myCoroutine = StartCoroutine(AnimateStar());
        }


        // public void Disable()
        // {
        //     _active = false;
        //     if (_myCoroutine != null)
        //     {
        //         StopCoroutine(_myCoroutine);
        //     }
        //
        // }
        //
        //
        // private void SetWaitTime()
        // {
        //     if (_initialized) return;
        //     _initialized = true;
        //
        //     _waitFor = new WaitForSeconds(0.4f);
        // }


        // public IEnumerator AnimateStar()
        // {
        //     var newLevelIcon = skillLevelStars[SkillManager.Instance.GetSkillLevel(_skillPromptInfo.SkillType)];
        //     newLevelIcon.gameObject.SetActive(true);
        //     
        //     while (_active)
        //     {
        //         newLevelIcon.DOFade(1f, 0.3f);
        //         yield return _waitFor;
        //         newLevelIcon.DOFade(0f, 0.3f);
        //         yield return _waitFor;
        //     }
        // }


        public void UpdateDone()
        {
            active = false;
            UpdateLevelUI();
            GameManager.Instance.CloseSkillUnlockPrompt();
            VibrationManager.SoftVibrate();
        }


        private void UpdateLevelUI()
        {
            var newLevelIcon = skillLevelStars[SkillManager.Instance.GetSkillLevel(skillPromptInfo.SkillType) - 1];

            var targetScale = newLevelIcon.transform.localScale;
            newLevelIcon.transform.localScale = Vector3.zero;
            
            newLevelIcon.gameObject.SetActive(true);
            newLevelIcon.transform.DOScale(targetScale, 0.3f).SetEase(Ease.OutBack);
        }
    }
}