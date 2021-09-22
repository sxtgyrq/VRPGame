using System;
using System.Collections.Generic;
using System.Text;

namespace WsOfWebClient.interfaceTag
{
    interface modelForCopy
    {
        string Tag { get; }
        string imgPath { get; }
        string mtlPath { get; }
        string objPath { get; }
        string Command { get; } 

        void SetImgBase64(string base64);
        void SetMtl(string mtl);
        void setObj(string obj);
        string GetObj();
        string GetMtl();
        string GetImg();
    }
}
