using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DPM.UI
{
    public class LevelBtn : MonoBehaviour
    {
        [SerializeField] private Button levelBtn;
        [SerializeField] private TextMeshProUGUI text;

        [NonSerialized] public Button.ButtonClickedEvent OnClick = new Button.ButtonClickedEvent();

        private void Start()
        {
            levelBtn.onClick = OnClick;
        }

        public void Construct(string text, Action onClickAction)
        {
            this.text.text = text;
            OnClick.AddListener(onClickAction.Invoke);
        }

        public void SetInteractable(bool interactable)
        {
            levelBtn.interactable = interactable;
        }
    }
}
