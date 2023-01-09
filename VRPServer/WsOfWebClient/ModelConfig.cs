using System;
using System.Collections.Generic;
using System.Text;

namespace WsOfWebClient
{
    public class ModelConfig
    {
        public class directionArrowIcon : interfaceTag.modelForCopy
        {
            public string Tag { get { return ConnectInfo.DirectionArrowIconBase64; } }

            public string imgPath { get { return "model/direcitonarrow/color.jpg"; } }

            public string mtlPath { get { return "model/direcitonarrow/untitled.mtl"; } }

            public string objPath { get { return "model/direcitonarrow/untitled.obj"; } }

            public string Command { get { return "SetDirectionArrowIcon"; } }

            public string GetImg()
            {
                return ConnectInfo.DirectionArrowIconBase64;
            }

            public string GetMtl()
            {
                return ConnectInfo.DirectionArrowMtl;
            }

            public string GetObj()
            {
                return ConnectInfo.DirectionArrowObj;
            }

            public void SetImgBase64(string base64)
            {
                ConnectInfo.DirectionArrowIconBase64 = base64;
            }

            public void SetMtl(string mtl)
            {
                ConnectInfo.DirectionArrowMtl = mtl;
            }

            public void setObj(string obj)
            {
                ConnectInfo.DirectionArrowObj = obj;
            }
        }

        public class opponentIcon : interfaceTag.modelForCopy
        {
            public string Tag { get { return ConnectInfo.OpponentIconBase64; } }

            public string imgPath { get { return "model/opponent/fight.jpg"; } }

            public string mtlPath { get { return "model/opponent/material.mtl"; } }

            public string objPath { get { return "model/opponent/model.obj"; } }

            public string Command { get { return "SetOpponentIcon"; } }

            public string GetImg()
            {
                return ConnectInfo.OpponentIconBase64;
            }

            public string GetMtl()
            {
                return ConnectInfo.OpponentIconMtl;
            }

            public string GetObj()
            {
                return ConnectInfo.OpponentIconObj;
            }

            public void SetImgBase64(string base64)
            {
                ConnectInfo.OpponentIconBase64 = base64;
            }

            public void SetMtl(string mtl)
            {
                ConnectInfo.OpponentIconMtl = mtl;
            }

            public void setObj(string obj)
            {
                ConnectInfo.OpponentIconObj = obj;
            }
        }

        public class teammateIcon : interfaceTag.modelForCopy
        {
            public string Tag { get { return ConnectInfo.TeammateIconBase64; } }

            public string imgPath { get { return "model/teammate/tuanjie1.jpg"; } }

            public string mtlPath { get { return "model/teammate/material.mtl"; } }

            public string objPath { get { return "model/teammate/model.obj"; } }

            public string Command { get { return "SetTeammateIcon"; } }

            public string GetImg()
            {
                return ConnectInfo.TeammateIconBase64;
            }

            public string GetMtl()
            {
                return ConnectInfo.TeammateIconMtl;
            }

            public string GetObj()
            {
                return ConnectInfo.TeammateIconObj;
            }

            public void SetImgBase64(string base64)
            {
                ConnectInfo.TeammateIconBase64 = base64;
            }

            public void SetMtl(string mtl)
            {
                ConnectInfo.TeammateIconMtl = mtl;
            }

            public void setObj(string obj)
            {
                ConnectInfo.TeammateIconObj = obj;
            }
        }
    }
}
