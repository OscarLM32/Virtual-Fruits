using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace Menus.LevelSelection
{
    public class LevelSelection : MonoBehaviour
    {
        public SOLevelsDatabase database;

        public string levelId;
        private AssetReference levelRef;
        private Button _playButton;

        private void Start()
        {
            _playButton = GameObject.Find("Play").GetComponent<Button>();

            var level = database[levelId];
            if (level == null) return;
            if (level.unlocked)
            {
                levelRef = level.reference;
                GetComponent<Button>().interactable = true;
            }
        }

        public void OnClick()
        {
            SOLevelData.instance.levelRef = levelRef;
            _playButton.interactable = true;
        }
    }
}