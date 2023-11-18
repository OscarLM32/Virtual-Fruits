using Player.StateMachine;
using TMPro;
using UnityEngine;

namespace EditorSystems
{
    //[RequireComponent(typeof(RectTransform))]
    //[RequireComponent(typeof(CanvasRenderer))]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class PlayerStateMachineDebugger : MonoBehaviour
    {
        public bool display = true;

        private static PlayerStateMachineDebugger _instance;

        private PlayerStateMachine playerStateMachine;
        private TextMeshProUGUI _text;

        //This is a pseudo Singleton since I want to ensure that there only is one instance of this class in scene
        //but do not want any other script to be able to access the instance reference
        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            if (playerStateMachine == null)
            {
                playerStateMachine = FindObjectOfType<PlayerStateMachine>();
                if (playerStateMachine == null) Destroy(gameObject);
            }

            SetUpDisplay();
        }

        private void OnEnable()
        {
            if (display)
            {
                playerStateMachine.OnStateChange += UpdateDisplay;
            }
        }

        private void OnDisable()
        {
            if (display)
            {
                playerStateMachine.OnStateChange -= UpdateDisplay;
            }
        }

        private void SetUpDisplay()
        {
            _text = GetComponent<TextMeshProUGUI>();

            if (!display)
            {
                _text.enabled = false;
                return;
            }

            RectTransform rTransform = GetComponent<RectTransform>();
            rTransform.anchorMax = Vector3.zero;
            rTransform.anchorMin = Vector3.zero;
            rTransform.sizeDelta = new Vector2(500, 65);
            rTransform.anchoredPosition = new Vector2(250, 32.5f);

            _text.enableAutoSizing = true;
        }

        private void UpdateDisplay(PlayerState newState)
        {
            _text.text = $"State: {newState}";
        }

    }
}