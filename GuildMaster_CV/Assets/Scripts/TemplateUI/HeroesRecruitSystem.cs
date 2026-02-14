using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class HeroesRecruitSystem
{
    //[SerializeField] protected List<HeroSlot> _heroSlots;
    [SerializeField] protected List<Hero> _heroSlots;

    //public List<HeroSlot> HeroSlots => _heroSlots;
    public List<Hero> HeroSlots => _heroSlots;

    public HeroesRecruitSystem(int size)
    {
        SetRecruitSize(size);
    }

    protected void SetRecruitSize(int size)
    {
        //_heroSlots = new List<HeroSlot>();
        _heroSlots = new List<Hero>();

        for (int i = 0; i < size; i++)
        {
            //_heroSlots.Add(new HeroSlot());


            //_heroSlots.Add(new Hero());
        }
    }

    public void AddNewRecruit(HeroData hero, int level, QuestParametresSystemFix questParametresSystemFix)
    {
        Hero freeSlot1 = GetFreeSlot();
        freeSlot1.AssignHero(hero, level, questParametresSystemFix);
    }

    protected Hero GetFreeSlot()
    {
        foreach (var slot in _heroSlots)
        {
            if (slot.IsEmpty())
            {
                return slot;
            }
        }

        // ≈сли нет доступных слотов, создаем новый
        Hero newSlot = new Hero();
        _heroSlots.Add(newSlot);
        return newSlot;
    }
}
