using HouseManager4_0.RoomMainF;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0
{


    public class Car
    {
        RoleInGame role;
        int _targetFpIndex = -1;
        public Car(RoleInGame roleInGame)
        {
            this.ability = new AbilityAndState(roleInGame);
            this._targetFpIndex = -1;
            this.role = roleInGame;
            this.countStamp = 2;
        }
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
            returning,
            selecting
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
        public int countStamp = 2;

        public void setState(RoleInGame player, ref List<string> notifyMsg, CarState s)
        {

            /*
             * A.将状态发送至前台。
             */
            this._state = s;
            if (player.playerType == RoleInGame.PlayerType.player)
            {
                SendStateAndPurpose((Player)player, this, ref notifyMsg);
                // this.countStamp++;
            }

            /*
             * 这里对应 ，Engine_CollectEngine 回收完毕。
             */
            if (s == CarState.waitOnRoad)
            {
                if (player.playerType == RoleInGame.PlayerType.NPC)
                {
                    ((NPC)player).dealWithWaitedNPC(ref notifyMsg);
                }
            }
            if (this._state != CarState.waitOnRoad)
            {
                if (string.IsNullOrEmpty(player.getCar().isControllingKey)) { }
                else
                {
                    player.getCar().clearControllingObj();
                    player.controlPrepareMagicChanged(player, ref notifyMsg);
                }
            }
        }

        private void clearControllingObj()
        {
            this._isControllingKey = "";

            // throw new NotImplementedException();
        }

        /// <summary>
        /// 汽车的目标地点。
        /// </summary>
        public int targetFpIndex
        {
            get
            {
                return this._targetFpIndex;
            }
        }
        internal void targetFpIndexSet(int to, ref List<string> notifyMsg)
        {
            if (this._targetFpIndex == to) { }
            else
            {
                //this.role
                this._targetFpIndex = to;
                if (this.role.playerType == RoleInGame.PlayerType.player)
                {
                    ((Player)this.role).DrawTarget(this._targetFpIndex, ref notifyMsg);
                }
            }
            //  throw new NotImplementedException();
        }

        /// <summary>
        ///  控制法术的被施对象的Key
        /// </summary>
        public string isControllingKey { get { return this._isControllingKey; } }
        string _isControllingKey = "";

        public class ChangeStateC
        {
            public string md5 { get; set; }
            public int privatekeysLength { get; set; }

            internal void Clear()
            {
                this.md5 = "";
                this.privatekeysLength = -1;
            }
        }

        public ChangeStateC WebSelf = new ChangeStateC()
        {
            md5 = "",
            privatekeysLength = -1,
        };

        /// <summary>
        /// 此函数主要用于动画变化！
        /// </summary>
        //ChangeStateC _changeState = new ChangeStateC()
        //{
        //    md5 = "",
        //    privatekeysLength = -1,
        //};
        public ChangeStateC changeState
        {
            get
            {
                if (this.animateObj == null)
                {
                    var obj = new ChangeStateC()
                    {
                    };
                    obj.Clear();
                    return obj;
                }
                else
                    return new ChangeStateC()
                    {
                        md5 = this.animateObj.Md5,
                        privatekeysLength = animateObj.LengthOfPrivateKeys
                    };
            }
        }
        public SetAnimateChangedF SetAnimateChanged;
        public AnimateObj animateObj { get; private set; }
        //internal void setAnimateData(RoleInGame player, ref List<string> notifyMsg, AnimateData2 data)
        //{
        //    this.animateData = data;
        //    this._changeState++;
        //    SetAnimateChanged(player, this, ref notifyMsg);
        //    //  throw new NotImplementedException();
        //}
        internal void setAnimateData(RoleInGame player, ref List<string> notifyMsg)
        {
            SetAnimateChanged(player, this, ref notifyMsg);
        }
        internal void setAnimateData(RoleInGame player, ref List<string> notifyMsg, List<AnimateDataItem> animations, DateTime recordTime)
        {
            if (animations == null) { }
            else
            {
                AnimateObj obj;
                if (player.playerType == RoleInGame.PlayerType.player)
                {
                    obj = new AnimateObj(animations, recordTime, animateObj, false);
                }
                else
                {
                    obj = new AnimateObj(animations, recordTime, animateObj, true);

                }
                this.animateObj = obj;
                //this._changeState = new ChangeStateC()
                //{
                //    md5 = obj.Md5,
                //    privatekeysLength = obj.LengthOfPrivateKeys
                //};
                SetAnimateChanged(player, this, ref notifyMsg);
            }
        }



        internal string IndexString
        {
            get
            {
                return "car";
            }
            //this._Cars.FindIndex(car);
            //throw new NotImplementedException();
        }

        bool _directAttack = false;
        /// <summary>
        /// 直接从基地进行攻击
        /// </summary>
        public bool DirectAttack
        {
            get { return this._directAttack; }
            set { this._directAttack = value; }
        }

        internal void Refresh(RoleInGame player, ref List<string> notifyMsg)
        {
            this.setState(player, ref notifyMsg, CarState.waitAtBaseStation);
            this._targetFpIndex = -1;
        }

        //private int animateHashCode = 0;
        //public int CurrentHash
        //{
        //    get { return animateHashCode; }
        //}
        //public int PreviousHash
        //{
        //    get;
        //    private set;
        //}

        //internal void SetHashCode(int currentHash)
        //{
        //    this.PreviousHash = animateHashCode;
        //    animateHashCode = currentHash;
        //}

        //   public SendPurposeOfCarF SendPurposeOfCar;
        //internal void setPurpose(RoleInGame role, ref List<string> notifyMsg, Purpose p)
        //{
        //    this.purpose = p;
        //    if (role.playerType == RoleInGame.PlayerType.player)
        //        this.SendPurposeOfCar((Player)role, this, ref notifyMsg);
        //    //throw new NotImplementedException();
        //}

        public class AnimateObj
        {
            int _lengthOfPrivateKeys;
            public int LengthOfPrivateKeys
            {
                get
                {
                    return this._lengthOfPrivateKeys;
                }
                set
                {
                    if (value < animateDataItems.Length)
                    {
                        this._lengthOfPrivateKeys = value + 1;
                    }
                    else
                    {
                        this._lengthOfPrivateKeys = animateDataItems.Length;
                    }
                }
            }

            internal int[] privateKeys;

            public AnimateObj(List<AnimateDataItem> animations, DateTime recordTime, AnimateObj previous, bool fullStep)
            {
                this.animateDataItems = animations.ToArray();
                this.recordTime = recordTime;
                var previousPrivateKeys = new List<int>();
                if (previous != null)
                {
                    this.PreviousMd5 = previous.Md5;
                    // if (fullStep) 
                    for (int i = 0; i < previous.animateDataItems.Length; i++)
                    {
                        previousPrivateKeys.Add(previous.animateDataItems[i].privateKey);
                    }

                }
                //this.PreviousHash = previous.GetHashCode();
                else
                    this.PreviousMd5 = "";
                //this.PreviousHash = 0;
                this.privateKeys = previousPrivateKeys.ToArray();

                if (fullStep)
                {
                    this._lengthOfPrivateKeys = animateDataItems.Length;
                }
                else
                {
                    this._lengthOfPrivateKeys = Math.Min(0, animateDataItems.Length);
                }
            }

            public DateTime recordTime { get; private set; }
            public AnimateDataItem[] animateDataItems { get; private set; }

            public string GetMd5()
            {
                string md5;
                {
                    string Md5Sum = "";
                    for (int i = 0; i < animateDataItems.Length; i++)
                    {
                        Md5Sum = $"{Md5Sum}-{animateDataItems[i].Md5Code}";
                        //  material.Add(animateDataItems[i].hashCode);
                    }
                    md5 = CommonClass.Random.GetMD5HashFromStr(Md5Sum);
                    //  hashCode = material.ToArray().GetHashCode();
                }
                return md5;
            }
            public string Md5
            {
                get { return this.GetMd5(); }
            }
            public string PreviousMd5
            {
                get;
                private set;
            }
        }
        public class AnimateDataItem
        {
            //90151163 is prime! 2^18.25622*256 =313,090.45478*256=   80,151,156.423
            //so get max number 90151163。
            public AnimateDataItem(Data.PathStartPoint3 _start, List<int> _animateData, bool _isParking, int _startT, long privateKeyInput, ref Random rm)
            {
                this.start = _start;
                var animateData = _animateData;
                // this.recordTime = _recordTime;
                this.isParking = _isParking;
                // this.hashCode = this.animateData.GetHashCode();
                this.startT = _startT;
                List<long> data = new List<long>();
                data.Add(this.start.x);
                data.Add(this.start.y);
                data.Add(this.start.z);
                for (int i = 0; i < animateData.Count; i++)
                {
                    data.Add(animateData[i]);
                }
                // for (int i = 0; i < data.Count; i++) { }
                // int privateKey;

                this.dataEncrypted = BitCoin.GamePathEncryption.PathEncryption.MainC.Encrypt(ref rm, data, privateKeyInput);
                this.privateKey = Convert.ToInt32(privateKeyInput);

                var int64Array = data.ToArray();
                byte[] arrayBytes = new byte[int64Array.Length * sizeof(long)];
                Buffer.BlockCopy(int64Array, 0, arrayBytes, 0, arrayBytes.Length);
                this.Md5Code = CommonClass.Random.GetMD5HashFromBytes(arrayBytes);
                //data.ToArray()
            }
            public Data.PathStartPoint3 start { get; private set; }
            //public List<Data.PathResult3> animateData { get; internal set; }
            // public List<int> animateData { get; private set; }
            public List<long> dataEncrypted { get; private set; }
            public bool isParking { get; private set; }
            // public int hashCode { get; private set; }
            public int startT { get; private set; }

            public string Md5Code { get; private set; }
            public int privateKey { get; private set; }
        }

        public void setControllingObj(RoleInGame player, Car car, Manager_Driver.ConfuseManger.ControlAttackType controlAttackType, string key, ref List<string> notifyMsg)
        {
            this._isControllingKey = key;
            if (!string.IsNullOrEmpty(this._isControllingKey))
            {
                switch (controlAttackType)
                {
                    case Manager_Driver.ConfuseManger.ControlAttackType.Confuse:
                        {
                            player.confusePrepareMagicChanged(player, ref notifyMsg);
                        }; break;
                    case Manager_Driver.ConfuseManger.ControlAttackType.Lost:
                        {
                            player.lostPrepareMagicChanged(player, ref notifyMsg);
                        }; break;
                    case Manager_Driver.ConfuseManger.ControlAttackType.Ambush:
                        {
                            player.ambushPrepareMagicChanged(player, ref notifyMsg);
                        }; break;
                }
            }
            //throw new NotImplementedException();
        }

        internal void UpdateSelection()
        {
            if (this.state == CarState.selecting)
            {
                if (this.role.playerType == RoleInGame.PlayerType.player)
                {
                    if (((Player)this.role).ShowCrossAfterWebUpdate != null)
                    {
                        ((Player)this.role).ShowCrossAfterWebUpdate();
                    }
                }
            }
        }
    }

}
