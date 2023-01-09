using CommonClass;
using System;
using System.Collections.Generic;
using System.Text;
using static HouseManager4_0.Engine;

namespace HouseManager4_0.RoomMainF
{
    public partial class RoomMain
    {
        public string updateMagic(MagicSkill ms, GetRandomPos grp)
        {
            return this.magicE.updateMagic(ms, grp);
            //throw new NotImplementedException();
        }
        internal void speedMagicChanged(RoleInGame role, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    SpeedNotify sn = new SpeedNotify()
                    {
                        c = "SpeedNotify",
                        WebSocketID = player.WebSocketID,
                        Key = role.Key,
                        On = role.improvementRecord.speedValue > 0
                    };

                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        internal void attackMagicChanged(RoleInGame role, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    AttackNotify an = new AttackNotify()
                    {
                        c = "AttackNotify",
                        WebSocketID = player.WebSocketID,
                        Key = role.Key,
                        On = role.improvementRecord.attackValue > 0
                    };

                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        internal void defenceMagicChanged(RoleInGame role, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    DefenceNotify an = new DefenceNotify()
                    {
                        c = "DefenceNotify",
                        WebSocketID = player.WebSocketID,
                        Key = role.Key,
                        On = role.improvementRecord.defenceValue > 0
                    };

                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }


        internal void confusePrepareMagicChanged(RoleInGame role, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    var On = !string.IsNullOrEmpty(role.getCar().isControllingKey);
                    if (this._Players.ContainsKey(role.getCar().isControllingKey))
                    {
                        var victim = this._Players[role.getCar().isControllingKey];
                        if (On)
                        {
                            var carPosition = Program.dt.GetFpByIndex(role.getCar().targetFpIndex);
                            double startX, startY, startZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(carPosition.Longitude, carPosition.Latitde, carPosition.Height, out startX, out startY, out startZ);
                            var targetPosition = Program.dt.GetFpByIndex(victim.StartFPIndex);
                            double endX, endY, endZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(targetPosition.Longitude, targetPosition.Latitde, targetPosition.Height, out endX, out endY, out endZ);

                            ConfusePrepareNotify an = new ConfusePrepareNotify()
                            {
                                c = "ConfusePrepareNotify",
                                WebSocketID = player.WebSocketID,
                                Key = role.Key,
                                On = On,
                                StartX = Convert.ToInt32(startX * 256),
                                StartY = Convert.ToInt32(startY * 256),
                                StartZ = Convert.ToInt32(startZ * 256),
                                EndX = Convert.ToInt32(endX * 256),
                                EndY = Convert.ToInt32(endY * 256),
                                EndZ = Convert.ToInt32(endZ * 256),
                            };

                            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
                            notifyMsgs.Add(url);
                            notifyMsgs.Add(sendMsg);
                        }
                        else
                        {
                        }
                    }

                }
            }
        }
        public void controlPrepareMagicChanged(RoleInGame role, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    var On = false;
                    {
                        ControlPrepareNotify an = new ControlPrepareNotify()
                        {
                            c = "ControlPrepareNotify",
                            WebSocketID = player.WebSocketID,
                            Key = role.Key,
                            On = On
                        };
                        var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
                        notifyMsgs.Add(url);
                        notifyMsgs.Add(sendMsg);
                    }
                }
            }
        }
        internal void confuseMagicChanged(RoleInGame role, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    ConfuseNotify sn = new ConfuseNotify()
                    {
                        c = "ConfuseNotify",
                        WebSocketID = player.WebSocketID,
                        Key = role.Key,
                        On = role.confuseRecord.IsBeingControlledByConfuse()
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(sn);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        internal void loseMagicChanged(RoleInGame role, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    LoseNotify ln = new LoseNotify()
                    {
                        c = "LoseNotify",
                        WebSocketID = player.WebSocketID,
                        Key = role.Key,
                        On = role.confuseRecord.IsBeingControlledByLose()
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        internal void ConfigMagic(RoleInGame role)
        {
            role.confuseRecord = new Manager_Driver.ConfuseManger();
            role.improvementRecord = new Manager_Driver.ImproveManager();
            role.speedMagicChanged = this.speedMagicChanged;
            role.attackMagicChanged = this.attackMagicChanged;
            role.defenceMagicChanged = this.defenceMagicChanged;
            role.confusePrepareMagicChanged = this.confusePrepareMagicChanged;
            role.lostPrepareMagicChanged = this.lostPrepareMagicChanged;
            role.ambushPrepareMagicChanged = this.ambushPrepareMagicChanged;
            role.controlPrepareMagicChanged = this.controlPrepareMagicChanged;

            role.confuseMagicChanged = this.confuseMagicChanged;
            role.loseMagicChanged = this.loseMagicChanged;

            role.fireMagicChanged = this.fireMagicChanged;
            role.waterMagicChanged = this.waterMagicChanged;
            role.electricMagicChanged = this.electricMagicChanged;
        }

        private void electricMagicChanged(RoleInGame actionRole, RoleInGame targetRole, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    ElectricNotify ln = new ElectricNotify()
                    {
                        c = "ElectricNotify",
                        WebSocketID = player.WebSocketID,
                        actionRoleID = actionRole.Key,
                        targetRoleID = targetRole.Key,
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        private void waterMagicChanged(RoleInGame actionRole, RoleInGame targetRole, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    WaterNotify ln = new WaterNotify()
                    {
                        c = "WaterNotify",
                        WebSocketID = player.WebSocketID,
                        actionRoleID = actionRole.Key,
                        targetRoleID = targetRole.Key,
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        private void fireMagicChanged(RoleInGame actionRole, RoleInGame targetRole, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    FireNotify ln = new FireNotify()
                    {
                        c = "FireNotify",
                        WebSocketID = player.WebSocketID,
                        actionRoleID = actionRole.Key,
                        targetRoleID = targetRole.Key,
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        private void lostPrepareMagicChanged(RoleInGame role, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    var On = !string.IsNullOrEmpty(role.getCar().isControllingKey);
                    if (this._Players.ContainsKey(role.getCar().isControllingKey))
                    {
                        var victim = this._Players[role.getCar().isControllingKey];
                        if (On)
                        {
                            var carPosition = Program.dt.GetFpByIndex(role.getCar().targetFpIndex);
                            double startX, startY, startZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(carPosition.Longitude, carPosition.Latitde, carPosition.Height, out startX, out startY, out startZ);
                            var targetPosition = Program.dt.GetFpByIndex(victim.StartFPIndex);
                            double endX, endY, endZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(targetPosition.Longitude, targetPosition.Latitde, targetPosition.Height, out endX, out endY, out endZ);

                            LostPrepareNotify an = new LostPrepareNotify()
                            {
                                c = "LostPrepareNotify",
                                WebSocketID = player.WebSocketID,
                                Key = role.Key,
                                On = On,
                                StartX = Convert.ToInt32(startX * 256),
                                StartY = Convert.ToInt32(startY * 256),
                                StartZ = Convert.ToInt32(startZ * 256),
                                EndX = Convert.ToInt32(endX * 256),
                                EndY = Convert.ToInt32(endY * 256),
                                EndZ = Convert.ToInt32(endZ * 256)
                            };

                            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
                            notifyMsgs.Add(url);
                            notifyMsgs.Add(sendMsg);
                        }
                        else
                        {
                        }
                    }

                }
            }
        }

        private void ambushPrepareMagicChanged(RoleInGame role, ref List<string> notifyMsgs)
        {
            foreach (var item in this._Players)
            {
                if (item.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)item.Value;
                    var url = player.FromUrl;
                    var On = !string.IsNullOrEmpty(role.getCar().isControllingKey);
                    if (this._Players.ContainsKey(role.getCar().isControllingKey))
                    {
                        var victim = this._Players[role.getCar().isControllingKey];
                        if (On)
                        {
                            var carPosition = Program.dt.GetFpByIndex(role.getCar().targetFpIndex);
                            double startX, startY, startZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(carPosition.Longitude, carPosition.Latitde, carPosition.Height, out startX, out startY, out startZ);
                            var targetPosition = Program.dt.GetFpByIndex(victim.StartFPIndex);
                            double endX, endY, endZ;
                            CommonClass.Geography.calculatBaideMercatorIndex.getBaiduPicIndex(targetPosition.Longitude, targetPosition.Latitde, targetPosition.Height, out endX, out endY, out endZ);

                            AmbushPrepareNotify an = new AmbushPrepareNotify()
                            {
                                c = "AmbushPrepareNotify",
                                WebSocketID = player.WebSocketID,
                                Key = role.Key,
                                On = On,
                                StartX = Convert.ToInt32(endX * 256),
                                StartY = Convert.ToInt32(endY * 256),
                                StartZ = Convert.ToInt32(endZ * 256),
                                EndX = Convert.ToInt32(startX * 256),
                                EndY = Convert.ToInt32(startY * 256),
                                EndZ = Convert.ToInt32(startZ * 256)
                            };

                            var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(an);
                            notifyMsgs.Add(url);
                            notifyMsgs.Add(sendMsg);
                        }
                        else
                        {
                        }
                    }

                }
            }
        }

        internal void DrawMagicDoubleLine(double[] lineParameter, ref List<string> notifyMsgs)
        {
            //List<string> notifyMsgs = new List<string>();
            foreach (var role in this._Players)
            {
                if (role.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)role.Value;
                    var url = player.FromUrl;
                    ElectricMarkNotify ln = new ElectricMarkNotify()
                    {
                        c = "ElectricMarkNotify",
                        WebSocketID = player.WebSocketID,
                        lineParameter = lineParameter
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }


        internal void DrawMagicPolyLine(double[] lineParameter, ref List<string> notifyMsgs)
        {
            foreach (var role in this._Players)
            {
                if (role.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)role.Value;
                    var url = player.FromUrl;
                    WaterMarkNotify ln = new WaterMarkNotify()
                    {
                        c = "WaterMarkNotify",
                        WebSocketID = player.WebSocketID,
                        lineParameter = lineParameter
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }

        internal void DrawMagicCircle(double[] lineParameter, ref List<string> notifyMsgs)
        {
            foreach (var role in this._Players)
            {
                if (role.Value.playerType == RoleInGame.PlayerType.player)
                {
                    var player = (Player)role.Value;
                    var url = player.FromUrl;
                    FireMarkNotify ln = new FireMarkNotify()
                    {
                        c = "FireMarkNotify",
                        WebSocketID = player.WebSocketID,
                        lineParameter = lineParameter
                    };
                    var sendMsg = Newtonsoft.Json.JsonConvert.SerializeObject(ln);
                    notifyMsgs.Add(url);
                    notifyMsgs.Add(sendMsg);
                }
            }
        }
    }
}
