var GuidObj =
{

    //        style="width: 100%;
    //        height: 100 %;

    //position: absolute;
    //left: 0px;
    //top: 0px; " 
    gameIntroHtml: `<div id="GameIntrolPanel" style="background-color:rgba(56, 60, 67, .15);width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;">
            <p style="text-align:left;">
                <h1 style="text-align:left">游戏剧情</h1>
                &emsp;&emsp;在游戏中，模拟城市交通，通过<a href="javascript:void(null);" onclick="GuidObj.selectDriver.show();">选取司机</a>、收集奖励、挑战NPC、收集宝石，获得虚拟股份及比特币。
            </p>
            <p style="text-align:left;">
                <h1 style="text-align:left">开发团队</h1>
                &emsp;&emsp;此游戏由<a href="javascript:void(null);" onclick="GuidObj.developTeam.show();">要瑞卿及其团队</a>进行开发并维护。
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.Exit()" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`,
    gameIntroHtmlID: 'GameIntrolPanel',
    gameIntroShow: function () {
        var that = GuidObj;
        if (document.getElementById(that.gameIntroHtmlID) == null) {
            document.getElementById('rootContainer').innerHTML = '';
            var frag = document.createRange().createContextualFragment(that.gameIntroHtml);
            frag.id = that.gameIntroHtmlID;
            document.getElementById('rootContainer').appendChild(frag);


        }
        else {
            //that.updateSocialResponsibility();
        }
    },
    Exit: function () {
        //alert('退出');
        objMain.ws.send(JSON.stringify({ 'c': 'QueryGuid' }));
    },
    selectDriver:
    {
        html: `<div id="selectDriverPanel" style="background-color:rgba(56, 60, 67, .15);width: 100%;height: 100%;overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;">
            <p style="text-align:left;">
                <h1 style="text-align:left">选取司机</h1>
                &emsp;&emsp;当你在大本营之时，调整视角，将旗帜调制值中间。旗帜会快速旋转，然后点击右侧招募。完成选取。<br />
                &emsp;&emsp;司机总类，分为输出、控制、辅助三种角色。角色之间可以项目配合。
            </p>
            <p>
                <img style="max-width:80%;width:80%;margin-left:10%" src="Pic/gameintro/guid_selectrole_01.png" />
                <div style="text-align:center;">
                    <span>1,调整视角</span>
                </div>
            </p>
            <p>
                <img style="max-width: 80%; width: 80%; margin-left: 10%" src="Pic/gameintro/guid_selectrole_02.png" />
                <div style="text-align:center;">
                    <span>2,选择自己的旗帜</span>
                </div>
            </p>
            <p>
                <img style="max-width: 80%; width: 80%; margin-left: 10%" src="Pic/gameintro/guid_selectrole_03.png" />
                <div style="text-align:center;">
                    <span>3,选取角色</span>
                </div>
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`,
        id: 'selectDriverPanel',
        show: function () {
            var that = GuidObj.selectDriver;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html);
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
                //that.updateSocialResponsibility();
            }
        }
    },
    developTeam:
    {
        html: `<div id="developTeamPanel" style="width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;">
            <p style="text-align:left;">
                <h1 style="text-align:left">团队介绍</h1>
                &emsp;&emsp;此程序由要瑞卿开发并维护。你可以通过<a href="javascript:void(null);" onclick="GuidObj.charging.show();">打赏</a>，来支持开发者，并在游戏过程中，使您实力增强。
            </p>
        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`,
        id: 'developTeamPanel',
        show: function () {
            var that = GuidObj.developTeam;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html);
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
            }
            else {
                //that.updateSocialResponsibility();
            }
        }
    },
    charging:
    {
        html: `<div id="guidChargingPanel" style="width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left">打赏</h1>
            <p style="text-align:left;">

                &emsp;&emsp;一，你要有一个地址，如<span style="background-color:aqua;color:forestgreen;">356irRFazab63B3m95oyiYeR5SDKJRFa99</span>。
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;二，地址<span style="background-color:aqua;color:forestgreen;">356irRFazab63B3m95oyiYeR5SDKJRFa99</span>在游戏里，有5000以上的余额。
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;三，你掌握地址<span style="background-color:aqua;color:forestgreen;">356irRFazab63B3m95oyiYeR5SDKJRFa99</span>的私钥。
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;四，你选择一个你能熟记的二到十字的汉语短语如“<span style="background-color:aqua;color:forestgreen;">欢迎来到我的游戏</span>”作为绑定词。
            </p>
            <p style="text-align:left;word-break:break-all;word-wrap:anywhere;">
                &emsp;&emsp;五，用你的私钥，对“<span style="background-color:aqua;color:forestgreen;">欢迎来到我的游戏</span>”进行签名，得到结果：<span style="background-color:aqua;color:forestgreen;">IDsBRU37kmlF+NAEJZEUz12bxI2ter02Ga5jQNI6SqYbeekPBaYZuMr03C+xZQzrHtfCSCAHvzrHf8j1kOYE3mQ=</span>。
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;六，将地址、绑定词、签名发送，待条件无误，即完成绑定。
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;七，绑定成功后。你可以依靠绑定词进行<a href="javascript:void(null);" onclick="GuidObj.wechat.show('wechat');">微信扫码</a>或<a href="javascript:void(null);" onclick="GuidObj.wechat.show('alipay');">支付宝扫码</a>。
            </p>
        </div>
        <div style=" width:100%;">
            <table style=" width:100%;">
                <tr>
                    <th style="width:4em;"><label>地址</label> </th>
                    <td> <textarea style="min-width:20em;" id="bindWordAddr">356irRFazab63B3m95oyiYeR5SDKJRFa99</textarea></td>
                </tr>
                <tr>
                    <th><label>绑定词</label> </th>
                    <td> <textarea style="min-width: 20em;" id="bindWordMsg">欢迎来到我的游戏</textarea></td>
                </tr>
                <tr>
                    <th><label>签名</label> </th>
                    <td> <textarea style="min-width: 20em;" id="bindWordSign">IDsBRU37kmlF+NAEJZEUz12bxI2ter02Ga5jQNI6SqYbeekPBaYZuMr03C+xZQzrHtfCSCAHvzrHf8j1kOYE3mQ=</textarea></td>
                </tr>
                <tr>
                    <th><label>验证码</label> </th>
                    <td><textarea style="min-width: 4em;" id="verifyCodeValue"></textarea>
                    <img id="verifyCodeImg" />
                    </td>
                </tr>
                 <tr>
                    <td  colspan="2">
                       <div id="bindVerifyCodeNotifyMsg" style="text-align:center;"></div>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <div>
                            <button onclick="GuidObj.charging.sendBindWordInfo();" style="width: 5em;
        height: 3em;
        margin-top: 1em;
        background-color:aqua;
        ">
                                发送
                            </button>
                            <button onclick="GuidObj.charging.clearBindWordInfo();" style="width: 5em; height: 3em; margin-top: 1em; background-color:transparent;">
                                清空
                            </button>
                            <button onclick="GuidObj.charging.signOnLine.show();" style="width: 5em; height: 3em; margin-top: 1em; background-color: red;">
                                签名
                            </button>
                        </div>
                    </td>
                </tr>
            </table>

        </div>
        <div style="text-align:center;">
            <button  onclick="GuidObj.gameIntroShow();"  style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`,
        id: 'guidChargingPanel',
        show: function () {
            var that = GuidObj.charging;
            if (document.getElementById(that.id) == null) {
                document.getElementById('rootContainer').innerHTML = '';
                var frag = document.createRange().createContextualFragment(that.html);
                frag.id = that.id;
                document.getElementById('rootContainer').appendChild(frag);
                document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + localStorage['nyrqVerifyImg'];
            }
            else {
                //that.updateSocialResponsibility();
            }
        },
        sendBindWordInfo: function () {
            var bindWordAddr = document.getElementById('bindWordAddr').value;
            var bindWordMsg = document.getElementById('bindWordMsg').value;
            var bindWordSign = document.getElementById('bindWordSign').value;
            var verifyCodeValue = document.getElementById('verifyCodeValue').value;
            var obj = {
                c: 'BindWordInfo',
                bindWordAddr: bindWordAddr,
                bindWordMsg: bindWordMsg,
                bindWordSign: bindWordSign,
                verifyCodeValue: verifyCodeValue
            };
            objMain.ws.send(JSON.stringify(obj));
        },
        clearBindWordInfo: function () {
            document.getElementById('bindWordAddr').value = '';
            document.getElementById('bindWordMsg').value = '';
            document.getElementById('bindWordSign').value = '';
        },
        SetImage: function (base64) {
            if (document.getElementById('verifyCodeImg') == null) {

            }
            else {
                document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + base64;
            }
        },
        showNotifyMsg(msg) {
            if (document.getElementById('bindVerifyCodeNotifyMsg') == null) {

            }
            else {
                document.getElementById('bindVerifyCodeNotifyMsg').innerText = msg;
            }
        },
        signOnLine: {
            html: `<div id="guidChargingPrivateKeyPanel" style="position: absolute;
        z-index: 8;
        top: calc(10% - 1px);
        width: 24em;
        left: calc(50% - 12em);
        height: auto;
        border: solid 1px red;
        text-align: center;
        background: rgba(104, 48, 8, 0.85);
        color: #83ffff;
        overflow: hidden;
        max-height: calc(90%);
">
<div>
            <label>
                p2wpkh-p2sh:
            </label>

            <input type="checkbox" id="p2wpkhp2sh" />
        </div>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label>
                --↓↓↓输入您珍贵的私钥↓↓↓--
            </label>
          
            
 <textarea id="subsidizePanelPromptPrivateKeyValue" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);height:4em;overflow:hidden;" onchange="subsidizeSys.privateKeyChanged();"></textarea>
 
       
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="GuidObj.charging.signOnLine.sign();">
            签名
        </div>
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="subsidizeSys.getPrivateKey();">
            获取私钥
        </div>
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="GuidObj.charging.signOnLine.show();">
            取消
        </div>
    </div>`,
            id: 'guidChargingPrivateKeyPanel',
            show: function () {
                var rexx = /^[\u4e00-\u9fa5]{2,10}$/;
                if (rexx.test(document.getElementById('bindWordMsg').value)) {
                    var that = GuidObj.charging.signOnLine;
                    if (document.getElementById(that.id) == null) {
                        var frag = document.createRange().createContextualFragment(that.html);
                        frag.id = that.id;
                        document.body.appendChild(frag);
                    }
                    else {
                        document.getElementById(that.id).remove();
                    }
                }
                else {
                    GuidObj.charging.showNotifyMsg('绑定词需要至少两汉字，至多十汉字');
                }

            },
            sign: function () {
                var privateKey = document.getElementById('subsidizePanelPromptPrivateKeyValue').value;
                var signMsg = document.getElementById('bindWordMsg').value;
                if (yrqCheckPrivateKey(privateKey)) {
                    var valuesGet = yrqSign(privateKey, signMsg, document.getElementById('p2wpkhp2sh').checked);
                    GuidObj.charging.signOnLine.show();

                    document.getElementById('bindWordAddr').value = valuesGet[1];
                    document.getElementById('bindWordSign').value = valuesGet[0];
                }
                else {
                    document.getElementById('subsidizePanelPromptPrivateKeyValue').style.background = 'rgba(255, 127, 127, 0.6)';
                }
            }
        },
        html2: '',
        add2: function () {

            //var that = GuidObj.charging;
            //if (document.getElementById(that.operateID2) == null) {
            //    // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            //    var frag = document.createRange().createContextualFragment(that.html2);
            //    frag.id = that.operateID2;

            //    document.body.appendChild(frag);
            //    //that.updateMoney();
            //}
            //else {
            //    document.getElementById(that.operateID2).remove();
            //}
        },
    },
    wechat:
    {
        html: function (id, payType, payName, img1, img2) {
            var temple = ` <div id="${id}" style="width: 100%; height: 100%; overflow-x:hidden;overflow-y:scroll;position:absolute;left:0px;top:0px;">
        <div style="text-align:left;word-wrap:anywhere;word-break:break-all;">
            <h1 style="text-align:left">${payType}</h1>
            <p style="text-align:left;">

                &emsp;&emsp;一，你要有一个地址，如<span style="background-color:aqua;color:forestgreen;">356irRFazab63B3m95oyiYeR5SDKJRFa99</span>。
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;二，地址已经与某个绑定词关联。
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;三，你掌握地址的私钥。
            </p>
            <p style="text-align:left;">
                &emsp;&emsp;四，用${payName}扫描。<br />
                <img style="max-width:calc(80%);margin-left:2em;" src="Pic/gameintro/${img1}" />
            </p>
            <p style="text-align:left;word-break:break-all;word-wrap:anywhere;">
                &emsp;&emsp;五，在扫码支付的备注中，务必填写上您的由两至十个汉字组成的绑定词。
                <img style="max-width:calc(80%);margin-left:2em;" src="Pic/gameintro/${img2}" />
            </p>
        </div>
        <div style=" width:100%;">
            <p style="text-align:left;word-break:break-all;word-wrap:anywhere;">
                &emsp;&emsp;六，以下对话框用于地址与绑定词之间的绑定。查询内容，可以输入地址或绑定词。
            </p>
            <table style=" width:100%;">
                <tr>
                    <th style="width:4em;"><label>查询内容</label> </th>
                    <td> <textarea style="min-width:20em;" id="bindWordOrAddr">356irRFazab63B3m95oyiYeR5SDKJRFa99</textarea></td>
                </tr>
                <tr>
                    <th><label>验证码</label> </th>
                    <td>
                        <textarea style="min-width: 4em;" id="verifyCodeValue"></textarea>
                        <img id="verifyCodeImg" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div id="bindVerifyCodeNotifyMsg" style="text-align:center;"></div>
                    </td>
                </tr>
                <tr>
                    <th></th>
                    <td>
                        <div>
                            <button onclick="GuidObj.wechat.lookFor();" style="width: 5em;
        height: 3em;
        margin-top: 1em;
        background-color:aqua;
        ">
                                查询
                            </button>
                            <button onclick="GuidObj.wechat.clearInfo();" style="width: 5em; height: 3em; margin-top: 1em; background-color:transparent;">
                                清空
                            </button>
                        </div>
                    </td>
                </tr>
            </table>

        </div>
        <div style="text-align:center;">
            <button onclick="GuidObj.gameIntroShow();" style="width: 5em;
        height: 3em;
        margin-top: 1em;">
                返回
            </button>
        </div>
    </div>`
            return temple;
        },
        id: 'guidWechatPayPanel',
        show: function (config) {
            if (config == 'wechat') {

                var that = GuidObj.wechat;
                if (document.getElementById(that.id) == null) {
                    document.getElementById('rootContainer').innerHTML = '';
                    var frag = document.createRange().createContextualFragment(that.html(that.id, '微信扫码打赏', '微信', 'wechatbindcode.jpg', 'wechatbindcode2.jpg'));
                    frag.id = that.id;
                    document.getElementById('rootContainer').appendChild(frag);
                    document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + localStorage['nyrqVerifyImg'];
                }
                else {
                    //that.updateSocialResponsibility();
                }
            }
            else if (config == 'alipay') {
                var that = GuidObj.wechat;
                if (document.getElementById(that.id) == null) {
                    document.getElementById('rootContainer').innerHTML = '';
                    var frag = document.createRange().createContextualFragment(that.html(that.id, '支付宝转账打赏', '支付宝', 'alipay1.jpg', 'alipay2.jpg'));
                    frag.id = that.id;
                    document.getElementById('rootContainer').appendChild(frag);
                    document.getElementById('verifyCodeImg').src = 'data:image/gif;base64,' + localStorage['nyrqVerifyImg'];
                }
                else {
                    //that.updateSocialResponsibility();
                }
            }
        },
        lookFor: function () {
            // GuidObj.wechat.lookFor();
            var infomation = document.getElementById('bindWordOrAddr').value;
            var verifyCodeValue = document.getElementById('verifyCodeValue').value;
            var obj = {
                c: 'LookForBindInfo',
                infomation: infomation,
                verifyCodeValue: verifyCodeValue
            };
            objMain.ws.send(JSON.stringify(obj));
        },
        clearInfo: function () {
            document.getElementById('bindWordOrAddr').value = '';
        },
    }, 
};