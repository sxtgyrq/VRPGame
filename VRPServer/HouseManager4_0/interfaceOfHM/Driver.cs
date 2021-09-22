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
        bool isMagic { get; }

        int GetDefensiveValue(CommonClass.driversource.Driver driver);

        int GetDefensiveValue(CommonClass.driversource.Driver driver, bool v);
        string GetSkillName();
        long leftValue(AbilityAndState ability);
        void setCost(long reduce, RoleInGame player, HouseManager4_0.Car car, ref List<string> notifyMsg);
        Engine_DebtEngine.DebtCondition getCondition();
        // long getVolumeOrBussiness(Manager_Driver.ConfuseManger.AmbushInfomation ambushInfomation);
        bool CheckCarState(HouseManager4_0.Car car);
        public long ImproveAttack(RoleInGame role, long attackMoney, ref List<string> notifyMsgs);
        long DealWithPercentValue(long percentValue, RoleInGame player, RoleInGame victim, RoomMainF.RoomMain that);
        void MagicAnimateShow(RoleInGame player, RoleInGame victim, ref List<string> notifyMsgs);
        //long DealWithPercentValue(long percentValue, RoleInGame player, RoleInGame victim, Engine_DebtEngine engine_DebtEngine);
    }

    interface ControlMagic
    {
        //int GetDefensiveValue(CommonClass.driversource.Driver driver);
        //string GetSkillName();
        Manager_Driver.ConfuseManger.ControlAttackType GetAttackType();
        string GetName();
    }
}
