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
    }
}
