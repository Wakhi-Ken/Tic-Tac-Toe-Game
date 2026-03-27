using UnityEngine;

namespace AI
{
    public interface IAIStrategy
    {
        int GetAIMove(int[] boardState);
    }
}