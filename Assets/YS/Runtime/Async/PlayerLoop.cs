using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace YS.Async {
    public static class YukataPlayerLoop {
        struct YSUpdate {
        }
        static void Init()
        {
            var mySystem = new PlayerLoopSystem
            {
                type = typeof(YSUpdate),
                updateDelegate = CustomUpdate,
            };
            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();

            for (var i = 0; i < playerLoop.subSystemList.Length; i++)
            {
                
                if (playerLoop.subSystemList[i].type == typeof(Update))
                {
                    playerLoop.subSystemList[i] = new PlayerLoopSystem
                    {
                        type = playerLoop.subSystemList[i].type,
                        updateDelegate = playerLoop.subSystemList[i].updateDelegate,
                        subSystemList = playerLoop.subSystemList[i].subSystemList.Prepend(mySystem).ToArray(),    
                        updateFunction = playerLoop.subSystemList[i].updateFunction,
                        loopConditionFunction = playerLoop.subSystemList[i].loopConditionFunction,
                    };
                    break;
                }
            }
            PlayerLoop.SetPlayerLoop(playerLoop);
        }
        public static long Ticks => _runner.Ticks;
        static LoopRunner _runner;
        public static void AddAction(ILoopItem loopItem) {
            if (_runner == null) {
                Init();
                _runner = new LoopRunner();
            }
            _runner.AddAction(loopItem);
        }

        static void CustomUpdate() {
            _runner?.Run();
        }
    }

    
    
    
}