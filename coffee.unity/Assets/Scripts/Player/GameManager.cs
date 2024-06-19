using System;
using UnityEngine;

namespace POLYGONWARE.Coffee.Player
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField][Range(1, 3600*24)] private int _fastForward = 1;
        
        public int FastForward => _fastForward;
        
        public static GameManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
            
            DontDestroyOnLoad(gameObject);
        }
    }
}
