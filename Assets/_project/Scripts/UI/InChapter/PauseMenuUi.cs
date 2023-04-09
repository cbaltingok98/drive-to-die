using System;
using System.Collections.Generic;
using _project.Scripts.Managers;
using _project.Scripts.Skills.Skill_Logic;
using _project.Scripts.Skills.Skill_Logic.BOMB;
using UnityEngine;
using UnityEngine.UI;

namespace _project.Scripts.UI.InChapter
{
    public class PauseMenuUi : MonoBehaviour
    {
        [SerializeField] private Button pauseBtn;
        [SerializeField] private Button continueBtn;
        [SerializeField] private Button returnHomeBtn;
        [SerializeField] private List<PauseMenuSingleItem> items;


        public void Init()
        {
            pauseBtn.onClick.RemoveAllListeners();
            pauseBtn.onClick.AddListener(EnablePauseMenu);
            
            continueBtn.onClick.RemoveAllListeners();
            continueBtn.onClick.AddListener(DisablePauseMenu);
            
            returnHomeBtn.onClick.RemoveAllListeners();
            returnHomeBtn.onClick.AddListener(LevelManager.Instance.ReturnHomeFromPauseMenu);

            ResetItems();
        }


        public void ResetItems()
        {
            for (var i = 0; i < items.Count; i++)
            {
                items[i].ResetItem();
            }
        }


        private void EnablePauseMenu()
        {
            GameManager.Instance.PauseGame();
            UpdateItems();
            pauseBtn.gameObject.SetActive(false);
            gameObject.SetActive(true);
        }


        private void DisablePauseMenu()
        {
            gameObject.SetActive(false);
            pauseBtn.gameObject.SetActive(true);
            GameManager.Instance.ContinueGame();
        }


        private void UpdateItems()
        {
            var activeSkills = SkillManager.Instance.GetActiveSkills();

            var index = 0;
            foreach (var skillsKey in activeSkills.Keys)
            {
                Skill skill = activeSkills[skillsKey];
                var sprite = skill.GetSkillSprite();
                var level = skill.GetCurrentLevel();
                
                items[index++].Init(sprite, level);
            }
        }
    }
}