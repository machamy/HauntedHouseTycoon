using System;
using System.Collections.Generic;
using Common.Interfaces;
using UnityEngine;

namespace ClassBase.Card
{
    [Serializable]
    public class CardDatabase : IBaseData
    {
        [SerializeField] private long index;
        public long Index
        {
            get => index;
            set => index = value;
        }
        public long NameIndex;
        public long ExplainIndex;

        public Type CardType;
        public Rank CardRank;

        public int Cost;
        public long[] KeyWordIndex;

        public string SpritePath;
        [NonSerialized]
        public Sprite Sprite;

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
    public class Effect : IBaseData
    {
        [SerializeField] private long index;
        public long Index
        {
            get => index;
            set => index = value;
        }
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
    public class KeywordData : IBaseData
    {
        [SerializeField] private long index;
        public long Index
        {
            get => index;
            set => index = value;
        }
        public long NameIndex;
        public long ExplainIndex;
    }

    [Serializable]
    public class EnterCardData : IBaseData
    {
        [SerializeField] private long index;
        public long Index
        {
            get => index;
            set => index = value;
        }
        public int EffectLastsTurn;
        public long[] SpawningVisitorIndex;
    }

    [Serializable]
    public class CardPackData : IBaseData
    {
        [SerializeField] private long index;
        public long Index
        {
            get => index;
            set => index = value;
        }
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
    public class TraumaData : IBaseData
    {
        [SerializeField] private long index;
        public long Index
        {
            get => index;
            set => index = value;
        }
        public string TraumaName;
    }

    [Serializable]
    public class MarketingData : IBaseData
    {
        [SerializeField] private long index;
        public long Index
        {
            get => index;
            set => index = value;
        }
    }

    [Serializable]
    public class AnimationData : IBaseData
    {
        [SerializeField] private long index;
        public long Index
        {
            get => index;
            set => index = value;
        }
    }

    [Serializable]
    public class TextData : IBaseData
    {
        [SerializeField] private long index;
        public long Index
        {
            get => index;
            set => index = value;
        }
        public string StringKey;
        public string KOR;
        public string EN;
    }
}

namespace ClassBase.GameObject
{
    [Serializable]
    public class Visitor : IBaseData
    {
        [SerializeField] private long index;
        public long Index
        {
            get => index;
            set => index = value;
        }

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

namespace ClassBase.ListName
{
    public enum DatabaseType
    {
        animationData,
        cardDataBase,
        cardEffectData,
        cardpackData,
        enterCardData,
        keywordData,
        marketingData,
        textData,
        traumaData,
        visitorData
    }
}