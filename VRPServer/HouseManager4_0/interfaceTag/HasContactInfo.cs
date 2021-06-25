using System;
using System.Collections.Generic;
using System.Text;

namespace HouseManager4_0.interfaceTag
{
    public interface HasContactInfo
    {
        void GetUrlAndWebsocket(out string url, out int websocketID);
    }
}
