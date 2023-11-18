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
        private static PlayerStateMachineDebugger _instance;

        [SerializeField] private PlayerStateMachine playerStateMachine;
        [SerializeField] private TextMeshProUGUI _text;
        
        //This is a pseudo Singleton since I want to ensure that there only is one instance of this class in scene
        //but do not want any other script to be able to access the instance reference
        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            else{
                Destroy(gameObject);
            }

            if(playerStateMachine == null)
            {
                playerStateMachine = FindObjectOfType<PlayerStateMachine>();
                if (playerStateMachine == null) Destroy(gameObject);
            }

            _text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            playerStateMachine.OnStateChange += UpdateDisplay;
        }

        private void OnDisable()
        {
            playerStateMachine.OnStateChange -= UpdateDisplay;
        }

        private void UpdateDisplay(PlayerState newState)
        {
            _text.text = $"State: {newState}";
        }
    }
}