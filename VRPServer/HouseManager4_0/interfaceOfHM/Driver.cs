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

        //bool ElectricIsIgnored();
        //void IgnoreElectric(ref RoleInGame role, ref System.Random rm);
        //void ReduceIgnoreElectric(ref RoleInGame player);

        //bool FireIsIgnored();
        //void IgnoreFire(ref RoleInGame role, ref System.Random rm);
        //void ReduceIgnoreFire(ref RoleInGame player);

        //bool WaterIsIgnored();
        //void IgnoreWater(ref RoleInGame role, ref System.Random rm);
        //void ReduceIgnoreWater(ref RoleInGame player);
        //int GetDefensiveValue(CommonClass.driversource.Driver driver);
        //string GetSkillName();
        //Manager_Driver.AmbushMagicType GetAmbushType();
        //Manager_Driver.AmbushMagicType GetAmbushType();


    }
    interface ImproveT : SkillImproveT
    {
        string key { get; }
        string beneficiaryKey { get; }
        int target { get; }
        RoomMainF.RoomMain.commandWithTime.returnning.ChangeType changeType { get; }
        RoomMainF.RoomMain.commandWithTime.ReturningOjb returningOjb { get; }

        // bool ConditionIsEnoughToReleaseAll(long money, long leftVolume, int ImprovedValue);
        void AddValue(int improvedValue, out long costValue, ref List<string> notifyMsg);
        void BalanceAccounts(ref List<string> notifyMsg, long costValue, out string[] msgToSelf, out string[] msgToBeneficiary);
        void AddValueWithMaxV(out long costValue, ref List<string> notifyMsg);
        bool ConditionIsEnoughToReleaseAll(long money, AbilityAndState ability, int improvedValue);
    }
    interface SkillImproveT
    {
        int MagicImprovedProbabilityAndValue(RoleInGame player, ref Random rm);
        // void IgnoreWater(ref RoleInGame role, ref System.Random rm);
        //  void ReduceMagicImprovedValue(RoleInGame player);
    }
    interface AttackT : AttackIgnore, SkillImproveT
    {
        bool isMagic { get; }

        int GetDefensiveValue(CommonClass.driversource.Driver driver);

        int GetDefensiveValue(CommonClass.driversource.Driver driver, bool defened);
        string GetSkillName();
        long leftValue(AbilityAndState ability);
        void setCost(long reduce, RoleInGame player, HouseManager4_0.Car car, ref List<string> notifyMsg);
        Engine_DebtEngine.DebtCondition getCondition();
        // long getVolumeOrBussiness(Manager_Driver.ConfuseManger.AmbushInfomation ambushInfomation);
        bool CheckCarState(HouseManager4_0.Car car);
        public long ImproveAttack(RoleInGame role, long attackMoney, ref List<string> notifyMsgs);
        long DealWithPercentValue(long percentValue, RoleInGame player, RoleInGame victim, RoomMainF.RoomMain that, GetRandomPos grp, ref List<string> notifyMsg);
        void MagicAnimateShow(RoleInGame player, RoleInGame victim, ref List<string> notifyMsgs);
        //bool Ignore(ref RoleInGame role, ref System.Random rm);
        //long DealWithPercentValue(long percentValue, RoleInGame player, RoleInGame victim, Engine_DebtEngine engine_DebtEngine);




    }

    interface ControlExpand : AttackIgnore, SkillDouble
    {
        bool DealWith();

        void SetReturn(bool success, GetRandomPos grp, ref List<string> notifyMsg);
        // void ReduceDefend(ref List<string> notifyMsg);
        void Ignore(ref Random rm);
        void ReduceIgnore();
        void SetHarm(ref long reduceSumInput, ref List<string> notifyMsg);
        int IgnoreValue
        {
            get;
        }
    }
    //interface ControlExpand : AttackIgnore, SkillDouble
    //{

    //}
    interface AttackIgnore
    {
        bool Ignored();
        void Ignore(ref RoleInGame role, ref System.Random rm);
        void ReduceIgnore(ref RoleInGame player);
    }
    interface SkillDouble
    {
        bool MagicDouble(ref Random rm);
        void ReduceDouble();
    }
    //interface AttackTByPhysics : AttackT
    //{
    //    //bool PhysicsIsIgnored();
    //    //void IgnorePhysics(ref RoleInGame role, ref System.Random rm);
    //    //void ReduceIgnorePhysics(ref RoleInGame player);
    //}

    interface ControlMagic
    {
        //int GetDefensiveValue(CommonClass.driversource.Driver driver);
        //string GetSkillName();
        Manager_Driver.ConfuseManger.ControlAttackType GetAttackType();
        string GetName();
    }
}
