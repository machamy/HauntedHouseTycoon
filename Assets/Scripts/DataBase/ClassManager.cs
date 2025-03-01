using System;
using System.Collections.Generic;

namespace ClassManager.Card
{
    [Serializable]
    public class CardClass
    {
        public long Index;
        public string Name;

        public Type CardType;
        public Rank CardRank;

        public int Cost;
        public long[] KeyWordIndex;
        public string SpritePath;
        public int[] AvailableRoutes;
        public int DestroyPayback;
        public long CardEffectIndex;
        public long PlaceAnimationIndex;
        public long ActionAnimationIndex;
        public int[] input1;
        public int[] input2;
        public int[] input3;
        public int[] input4;

        public enum Type
        {
            ENTER,
            EXIT,
            HORROR,
            SCREAM,
            ASSISTANCE
        }

        public enum Rank
        {
            NORMAL,
            RARE,
            LEGEND
        }
    }
}
