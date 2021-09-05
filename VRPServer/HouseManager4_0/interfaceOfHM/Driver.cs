using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.interfaceOfHM
{
    interface Driver
    {
    }
    interface AttackMagic : AttackT
    {
        //int GetDefensiveValue(CommonClass.driversource.Driver driver);
        //string GetSkillName();
        //Manager_Driver.AmbushMagicType GetAmbushType();
        //Manager_Driver.AmbushMagicType GetAmbushType();
    }
    interface AttackT
    {
        int GetDefensiveValue(CommonClass.driversource.Driver driver);
        string GetSkillName();
        long leftValue(AbilityAndState ability);
        void setCost(long reduce, RoleInGame player, HouseManager4_0.Car car, ref List<string> notifyMsg);
        Engine_DebtEngine.DebtCondition getCondition();
        long getVolumeOrBussiness(Manager_Driver.ConfuseManger.AmbushInfomation ambushInfomation);
        bool CheckCarState(HouseManager4_0.Car car);
    }

    interface ControlMagic
    {
        //int GetDefensiveValue(CommonClass.driversource.Driver driver);
        //string GetSkillName();
        Manager_Driver.ConfuseManger.ControlAttackType GetAttackType();
        string GetName();
    }
}
