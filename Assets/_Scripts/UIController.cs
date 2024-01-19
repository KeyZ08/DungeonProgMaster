using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DPM.UI;
//не должно использовать UI :(

namespace DPM.App
{
    public class UIController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform WinCanvas;
        [SerializeField] private Transform LoseCanvas;
        [SerializeField] private Transform ErrorCanvas;

        [Header("Buttons")]
        [SerializeField] private Button playBtn;
        [SerializeField] private Button stopBtn;
        [SerializeField] private Button nextLevelBtn;

        [SerializeField] private Button resetBtnWin;
        [SerializeField] private Button resetBtnLose;
        [SerializeField] private Button resetBtnError;

        [Header("Prefabs")]
        [SerializeField] private Transform levelBtnSpawner;
        [SerializeField] private LevelBtn levelBtnPrefab;

        [NonSerialized] public Button.ButtonClickedEvent OnPlayBtnClick = new Button.ButtonClickedEvent();
        [NonSerialized] public Button.ButtonClickedEvent OnStopBtnClick = new Button.ButtonClickedEvent();
        [NonSerialized] public Button.ButtonClickedEvent OnNextLevelBtnClick = new Button.ButtonClickedEvent();
        [NonSerialized] public Button.ButtonClickedEvent OnResetBtnClick = new Button.ButtonClickedEvent();

        private List<LevelBtn> levels;

        private void Start()
        {
            playBtn.onClick = OnPlayBtnClick;
            stopBtn.onClick = OnStopBtnClick;
            nextLevelBtn.onClick = OnNextLevelBtnClick;
            resetBtnWin.onClick = OnResetBtnClick;
            resetBtnLose.onClick = OnResetBtnClick;
            resetBtnError.onClick.AddListener(() => { ErrorShow(false); });
        }

        public void SetInteractable(bool value)
        {
            playBtn.interactable = value;
            for (int i = 0; i < levels.Count; i++)
                levels[i].SetInteractable(value);
        }

        public void WinShow(bool show)
        {
            WinCanvas.gameObject.SetActive(show);
        }

        public void LoseShow(bool show)
        {
            LoseCanvas.gameObject.SetActive(show);
        }

        public void ErrorShow(bool show)
        {
            ErrorCanvas.gameObject.SetActive(show);
        }

        public void ConstructLevelBtns(int count, GameController controller)
        {
            levels = new List<LevelBtn>(count);
            for (int i = 0; i < count; i++)
            {
                var index = i;
                var btn = Instantiate(levelBtnPrefab, levelBtnSpawner);
                btn.Construct((index + 1).ToString(), () => controller.LoadLevel(index));
                levels.Add(btn);
            }
        }
    }
}
