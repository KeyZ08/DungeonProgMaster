using System;
using UnityEngine;
using UnityEngine.UI;

using DPM.UI;
//не должно использовать UI :(

namespace DPM.App
{
    public class UIController : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Transform WinCanvas;
        [SerializeField] private Transform LoseCanvas;

        [Header("Buttons")]
        [SerializeField] private Button playBtn;
        [SerializeField] private Button resetBtnWin;
        [SerializeField] private Button resetBtnLose;

        [Header("Prefabs")]
        [SerializeField] private Transform levelBtnSpawner;
        [SerializeField] private LevelBtn levelBtnPrefab;

        [NonSerialized] public Button.ButtonClickedEvent OnPlayBtnClick = new Button.ButtonClickedEvent();
        [NonSerialized] public Button.ButtonClickedEvent OnResetBtnClick = new Button.ButtonClickedEvent();

        private void Start()
        {
            playBtn.onClick = OnPlayBtnClick;
            resetBtnWin.onClick = OnResetBtnClick;
            resetBtnLose.onClick = OnResetBtnClick;
        }

        public void SetInteractable(bool value)
        {
            playBtn.interactable = value;
        }

        public void WinShow(bool show)
        {
            WinCanvas.gameObject.SetActive(show);
        }

        public void LoseShow(bool show)
        {
            LoseCanvas.gameObject.SetActive(show);
        }

        public void ConstructLevelBtns(int count, GameController controller)
        {
            for (int i = 0; i < count; i++)
            {
                var index = i;
                var btn = Instantiate(levelBtnPrefab, levelBtnSpawner);
                btn.Construct((index + 1).ToString(), () => controller.LoadLevel(index));
            }
        }
    }

}