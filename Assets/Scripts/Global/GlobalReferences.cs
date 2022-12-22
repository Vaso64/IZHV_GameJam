using GameJam.Input;
using UnityEngine;
using GameJam.Player;

namespace GameJam.Global
{
    public class GlobalReferences : MonoBehaviour
    {
        public static Player.Player Player { get; private set; }
        public static InputManager InputManager { get; private set; }
        public static GameLoop GameLoop { get; private set; }
        
        
        public static LevelIndex LevelIndex { get; private set; }
        
        public static Transform StartPoint { get; private set; }
        
        private void Awake()
        {
            Player = FindObjectOfType<Player.Player>();
            InputManager = FindObjectOfType<InputManager>();
            GameLoop = FindObjectOfType<GameLoop>();
            LevelIndex = FindObjectOfType<LevelIndex>();
            StartPoint = GameObject.Find("StartPoint").transform;
        }
    }
}