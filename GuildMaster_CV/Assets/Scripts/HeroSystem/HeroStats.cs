using System;
using UnityEngine;

[System.Serializable]
public class HeroStats
{
    [Header("Main Power Stats Multi")]
    public int StrengthMulti;
    public int DexterityMulti;
    public int IntelligenceMulti;
    [Space]
    [Header("Main Defence Stats Multi")]
    public int EnduranceMulti;
    public int FlexibilityMulti;
    public int SanityMulti;
    [Header("Secondary Power Stats Multi")]
    public int CritMulti;
    public int HasteMulti;
    public int AccuracyMulti;
    [Space]
    [Header("Secondary Defence Stats Multi")]
    public int DurabilityMulti;
    public int AdaptabilityMulti;
    public int SuppressionMulti;
    [Space]
    public int Health;



    [Space(50)]
    [Header("Main Power Stats Value")]
    public int StrengthValue;
    public int DexterityValue;
    public int IntelligenceValue;
    [Space]
    [Header("Main Defence Stats Value")]
    public int EnduranceValue;
    public int FlexibilityValue;
    public int SanityValue;
    [Space]
    [Header("Secondary Power Stats Value")]
    public int CritValue;
    public int HasteValue;
    public int AccuracyValue;
    [Space]
    [Header("Secondary Defence Stats Value")]
    public int DurabilityValue;
    public int AdaptabilityValue;
    public int SuppressionValue;

    public HeroStats(HeroStats copyStats)
    {
        StrengthMulti = copyStats.StrengthMulti;
        StrengthValue = copyStats.StrengthValue;

        DexterityMulti = copyStats.DexterityMulti;
        DexterityValue = copyStats.DexterityValue;

        IntelligenceMulti = copyStats.IntelligenceMulti;
        IntelligenceValue = copyStats.IntelligenceValue;

        EnduranceMulti = copyStats.EnduranceMulti;
        EnduranceValue = copyStats.EnduranceValue;

        FlexibilityMulti = copyStats.FlexibilityMulti;
        FlexibilityValue = copyStats.FlexibilityValue;

        SanityMulti = copyStats.SanityMulti;
        SanityValue = copyStats.SanityValue;

        CritMulti = copyStats.CritMulti;
        CritValue = copyStats.CritValue;

        HasteMulti = copyStats.HasteMulti;
        HasteValue = copyStats.HasteValue;

        AccuracyMulti = copyStats.AccuracyMulti;
        AccuracyValue = copyStats.AccuracyValue;

        DurabilityMulti = copyStats.DurabilityMulti;
        DurabilityValue = copyStats.DurabilityValue;

        AdaptabilityMulti = copyStats.AdaptabilityMulti;
        AdaptabilityValue = copyStats.AdaptabilityValue;

        SuppressionMulti = copyStats.SuppressionMulti;
        SuppressionValue = copyStats.SuppressionValue;

        Health = copyStats.Health;
    }

    public HeroStats() { }

    public void ChangeHealth(int health)
    {
        Health += health;
    }

    // »«Ã≈Õ≈Õ»≈ «Õ¿◊≈Õ»È

    #region —»À¿_«Õ¿◊≈Õ»ﬂ_œ≈–¬»◊Õ€≈ 
    public void ChangeStrengthValue(int value)
    {
        StrengthValue += value;
    }

    public void ChangeDexterityValue(int value)
    {
        DexterityValue += value;
    }

    public void ChangeIntelligenceValue(int value)
    {
        IntelligenceValue += value;
    }
    #endregion



    #region «¿Ÿ»“¿_«Õ¿◊≈Õ»ﬂ_œ≈–¬»◊Õ€≈ 
    public void ChangeEnduranceValue(int value)
    {
        EnduranceValue += value;
    }

    public void ChangeFlexibilityValue(int value)
    {
        FlexibilityValue += value;
    }

    public void ChangeSanityValue(int value)
    {
        SanityValue += value;
    }
    #endregion



    #region —»À¿_«Õ¿◊≈Õ»ﬂ_¬“Œ–»◊Õ€≈ 
    public void ChangeHasteValue(int value)
    {
        HasteValue += value;
    }

    public void ChangeCritValue(int value)
    {
        CritValue += value;
    }

    public void ChangeAccuracyValue(int value)
    {
        AccuracyValue += value;
    }
    #endregion



    #region «¿Ÿ»“¿_«Õ¿◊≈Õ»ﬂ_¬“Œ–»◊Õ€≈ 
    public void ChangeDurabilityValue(int value)
    {
        DurabilityValue += value;
    }

    public void ChangeAdaptabilityValue(int value)
    {
        AdaptabilityValue += value;
    }

    public void ChangeSuppressionValue(int value)
    {
        SuppressionValue += value;
    }
    #endregion



    // »«Ã≈Õ≈Õ»≈ Ã”À‹“»


    #region —»À¿_Ã”À‹“»_¬“Œ–»◊Õ€≈ 
    public void ChangeCritMulti(int value)
    {
        CritMulti += value;
    }

    public void ChangeHasteMulti(int value)
    {
        HasteMulti += value;
    }

    public void ChangeAccuracyMulti(int value)
    {
        AccuracyMulti += value;
    }
    #endregion



    #region «¿Ÿ»“¿_Ã”À‹“»_¬“Œ–»◊Õ€≈ 
    public void ChangeDurabilityMulti(int value)
    {
        DurabilityMulti += value;
    }

    public void ChangeAdaptabilityMulti(int value)
    {
        AdaptabilityMulti += value;
    }

    public void ChangeSuppressionMulti(int value)
    {
        SuppressionMulti += value;
    }
    #endregion


    public void ClearAllStats()
    {
        StrengthMulti = 0;
        DexterityMulti = 0;
        IntelligenceMulti = 0;

        EnduranceMulti = 0;
        FlexibilityMulti = 0;
        SanityMulti = 0;

        CritMulti = 0;
        HasteMulti = 0;
        AccuracyMulti = 0;

        DurabilityMulti = 0;
        AdaptabilityMulti = 0;
        SuppressionMulti = 0;

        StrengthValue = 0;
        DexterityValue = 0;
        IntelligenceValue = 0;

        EnduranceValue = 0;
        FlexibilityValue = 0;
        SanityValue = 0;

        CritValue = 0;
        HasteValue = 0;
        AccuracyValue = 0;

        DurabilityValue = 0;
        AdaptabilityValue = 0;
        SuppressionValue = 0;
    }
}
