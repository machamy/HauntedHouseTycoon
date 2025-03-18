using System;
using System.Collections.Generic;

namespace ClassBase.Card
{
    [Serializable]
    public class CardClass
    {
        public long Index;
        public long NameIndex;
        public long ExplainIndex;

        public Type CardType;
        public Rank CardRank;

        public int Cost;
        public long[] KeyWordIndex;
        public string SpritePath;
        public int[] AvailableRoutes;
        public int DestroyPayback;
        public long[] CardEffectIndex;
        public long[] PlaceAnimationIndex;
        public long[] ActionAnimationIndex;
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

    [Serializable]
    public class Effect
    {
        public long Index;
        public ConditonType condition;
        public TargetType target;

        public enum ConditonType
        {
            OnPlayerTurnEnter,
            OnPlayerTurnExit,
            OnNPCTurnEnter,
            OnNPCTurnExit,
            OnCardPlaced,
            OnCardRemoved,
            OnGuestEnter,
            OnGuestExit
        }

        public enum TargetType
        {
            ToCurrentCard,
            ToConnectedCard,
            ToUpCard,
            ToDownCard,
            ToRightCard,
            ToLeftCard,
            ToAllCardInField,
            ToAllCardInHand,
            ToDiagonalCard,
            ToSameCard,
            ToCrossCard,
            _,
            ToCurrentGuest,
            ToNearestGeust,
            ToConnectedGuest,
            To33Guest
        }
    }

    [Serializable]
    public class KeywordData
    {
        public long Index;
        public long NameIndex;
        public long ExplainIndex;
    }

    [Serializable]
    public class EnterCardData
    {
        public long Index;
        public int EffectLastsTurn;
        public long[] SpawningVisitorIndex;
    }

    [Serializable]
    public class CardPackData
    {
        public long Index;
        public long NameIndex;
        public long ExplainIndex;
        public int PackPrice;
        public int NumberOfCardsAppearingUponOpening;
        public long[] AppearingCardIndex;
        public int[] WeightedRatioForEachCards;
        public bool RepurchaseAllowed;
        public bool RepurchaseAllowedForOnce;
        public int MaximumOpenedPackAmount;
        public long PackOpeningAnimation;
        public string IllustFileName;
    }

    [Serializable]
    public class TraumaData
    {
        public long Index;
        public string TraumaName;
    }

    [Serializable]
    public class MarketingData
    {
        public long Index;
    }

    [Serializable]
    public class AnimationData
    {
        public long Index;
        public string EffectSound;
    }

    [Serializable]
    public class TextData
    {
        public long Index;
        public string StringKey;
        public string KOR;
        public string EN;
    }
}

namespace ClassBase.GameObject
{
    [Serializable]
    public class Visitor
    {
        public long Index;

        public Sex sex;
        public enum Sex
        {
            MALE,
            FEMALE,
        }
        public int Age;
        public long[] TraumaIndex;
        public float[] TraumaRatio;
        public float VisualHorrorTolerance;
        public float AuditoryHorrorTolerance;
        public float ScentHorrorTolerance;
        public float TouchHorrorTolerance;
        public int[] RequiredHorrorAmount;
        public int PanicAmount;
        public float AmountOfTiredInTurn;

        public PanicResponse panicResponse;
        public enum PanicResponse
        {
            HeartArrest,
        }
        public int[] PanicWeightedAmount;
        public int ExitGetScreamAmount;
        public long[] EnterCardIndex;
    }
}