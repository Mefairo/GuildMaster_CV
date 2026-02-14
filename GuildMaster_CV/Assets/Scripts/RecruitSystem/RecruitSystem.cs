using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RecruitSystem
{
    //[SerializeField] private List<RecruitSlot> _recruitSlots;

    //public List<RecruitSlot> RecruitSlots => _recruitSlots;

    //public RecruitSystem(int size)
    //{
    //    SetRecruitSize(size);
    //}

    //private void SetRecruitSize(int size)
    //{
    //    _recruitSlots = new List<RecruitSlot>();

    //    for (int i = 0; i < size; i++)
    //    {
    //        _recruitSlots.Add(new RecruitSlot());
    //    }
    //}

    //public void AddNewRecruit(HeroData data, int level)
    //{
    //    RecruitSlot freeSlot = GetFreeSlot();
    //    freeSlot.AssignHero(data, level);
    //}

    //private RecruitSlot GetFreeSlot()
    //{
    //    foreach (var slot in _recruitSlots)
    //    {
    //        if (slot.IsEmpty())
    //        {
    //            return slot;
    //        }
    //    }

    //    // ≈сли нет доступных слотов, создаем новый
    //    RecruitSlot newSlot = new RecruitSlot();
    //    _recruitSlots.Add(newSlot);
    //    return newSlot;
    //}
}
