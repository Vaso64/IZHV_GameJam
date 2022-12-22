using System.Collections.Generic;
using System.Linq;
using Extensions;
using GameJam.Global;
using UnityEngine.UI;
using GameJam.UI;
using TMPro;
using UnityEngine;

namespace GameJam.UI
{
    public class LevelMenu : MonoBehaviour
    {
        private GameObject loadedLevel;

        [SerializeField] private UIBase levelButtonPrefab;
        [SerializeField] private UIBase playButton;
        [SerializeField] private RawImage levelImage;
        [SerializeField] private TextMeshPro levelName;

        private void Start()
        {
            var levels = GlobalReferences.LevelIndex.levels;
            var gridPivot = new Vector3(0, 0.3f, 0.75f);
            var gridSpace = 0.3f;
            for (var i = 0; i < levels.Count; i++)
            {
                var level = levels[i];
                var levelButton = Instantiate(levelButtonPrefab).GetComponent<Button3D>();
                levelButton.transform.SetParent(transform);
                levelButton.transform.localPosition = gridPivot + new Vector3(0, -Mathf.Ceil(i/3) * gridSpace, -(i%3) * gridSpace);
                levelButton.OnClickUp.AddListener(() => SelectLevel(level));
                levelButton.GetComponentInChildren<TextMeshProUGUI>().text = i.ToString();
            }
            
            SelectLevel(levels.First());           
        }
        
        private void SelectLevel(Level level)
        {
            //levelImage.texture = level.image.mainTexture;
            levelName.text = level.name;
            playButton.OnClickUp.SetListener(() => GlobalReferences.GameLoop.StartLevel(level));
        }
    }
}