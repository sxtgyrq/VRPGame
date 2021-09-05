using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0
{


    public class Car
    {
        public enum CarState
        {
            /// <summary>
            /// 在基地里等待可以执行购买、收税、攻击
            /// </summary>
            waitAtBaseStation,
            /// <summary>
            /// 在路上进行等待
            /// </summary>
            waitOnRoad,
            /// <summary>
            /// 正在工作
            /// </summary>
            working,
            /// <summary>
            /// returning状态，只能在setReturn -ReturnThenSetComeBack是定义。
            /// </summary> 
            returning
        }

       

        //public enum Purpose
        //{
        //    @null,
        //    collect,
        //    tax,
        //    attack
        //}
        public delegate void SendStateAndPurposeF(Player player, Car car, ref List<string> notifyMsg);

        public delegate void SendPurposeOfCarF(Player player, Car car, ref List<string> notifyMsg);

        public delegate void SetAnimateChangedF(RoleInGame player, Car car, ref List<string> notifyMsg);

        //public string name { get; set; }
        public AbilityAndState ability { get; set; }

        CarState _state = CarState.waitAtBaseStation;

        public SendStateAndPurposeF SendStateAndPurpose;


        public CarState state
        {
            get
            {
                return this._state;
            }
        }
        public void setState(RoleInGame player, ref List<string> notifyMsg, CarState s)
        {
            this._state = s;
            if (player.playerType == RoleInGame.PlayerType.player)
                SendStateAndPurpose((Player)player, this, ref notifyMsg);

            if (s == CarState.waitOnRoad)
            {
                if (player.playerType == RoleInGame.PlayerType.NPC)
                {
                    ((NPC)player).dealWithWaitedNPC(ref notifyMsg);
                }
            }
        }
        /// <summary>
        /// 汽车的目标地点。
        /// </summary>
        public int targetFpIndex { get; set; }


        /// <summary>
        /// 此函数主要用于动画变化！
        /// </summary>
        int _changeState = 0;
        public int changeState { get { return this._changeState; } }
        public SetAnimateChangedF SetAnimateChanged;
        public AnimateData2 animateData { get; private set; }
        internal void setAnimateData(RoleInGame player, ref List<string> notifyMsg, AnimateData2 data)
        {
            this.animateData = data;
            this._changeState++;
            SetAnimateChanged(player, this, ref notifyMsg);
            //  throw new NotImplementedException();
        }

        //public int carIndex { get; internal set; }

        internal string IndexString
        {
            get
            {
                return "car";
            }
            //this._Cars.FindIndex(car);
            //throw new NotImplementedException();
        }
        internal void Refresh(RoleInGame player, ref List<string> notifyMsg)
        {
            this.setState(player, ref notifyMsg, CarState.waitAtBaseStation);
            this.targetFpIndex = -1;
        }
        //   public SendPurposeOfCarF SendPurposeOfCar;
        //internal void setPurpose(RoleInGame role, ref List<string> notifyMsg, Purpose p)
        //{
        //    this.purpose = p;
        //    if (role.playerType == RoleInGame.PlayerType.player)
        //        this.SendPurposeOfCar((Player)role, this, ref notifyMsg);
        //    //throw new NotImplementedException();
        //}

        public class AnimateData2
        {
            public AnimateData2(Data.PathStartPoint2 _start, List<int> _animateData, DateTime _recordTime, bool _isParking)
            {
                this.start = _start;
                this.animateData = _animateData;
                this.recordTime = _recordTime;
                this.isParking = _isParking;
            }
            public Data.PathStartPoint2 start { get; private set; }
            //public List<Data.PathResult3> animateData { get; internal set; }
            public List<int> animateData { get; private set; }
            public DateTime recordTime { get; private set; }
            public bool isParking { get; private set; }
        }
    }

}
