var subsidizeSys =
{
    operateAddress: '',
    operateID: 'subsidizePanel',
    html: `<div id="subsidizePanel"  style="position:absolute;z-index:8;top:calc(10% - 1px);width:24em; left:calc(50% - 12em);height:auto;border:solid 1px red;text-align:center;background:rgba(104, 48, 8, 0.85);color:#83ffff;overflow-y: scroll;max-height: calc(90%);  ">
        <table style="width:100%;">
            <tr>
                <th>剩余资助</th>
                <th>现有资助</th>
            </tr>
            <tr>
                <td id="moneyOfSumSubsidizing" >未知</td>
                <td id="moneyOfSumSubsidized">0</td>
            </tr>
        </table>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label  onclick="subsidizeSys.readStr('bitcoinSubsidizeAddressInput');">
                --↓↓↓输入1打头的比特币地址↓↓↓--
            </label>
            <input id="bitcoinSubsidizeAddressInput" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);" />
        </div>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label onclick="subsidizeSys.copyStr();">
                --↓↓↓对以下信息进行签名↓↓↓--
            </label> 
            <input  id="msgNeedToSign" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);" readonly onclick="subsidizeSys.copyStr();" />
        </div>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label onclick="subsidizeSys.readStr('signatureInputTextArea');">
                --↓↓↓输入签名↓↓↓--
            </label>
            <textarea id="signatureInputTextArea" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);height:4em;overflow:hidden;">1111111111111111111111</textarea>

        </div> 

        <table style="width:100%">
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.subsidize(50000)" >
                        资助500
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;"  onclick="subsidizeSys.subsidize(100000)" >
                        资助1000
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.subsidize(200000)" >
                        资助2000
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.subsidize(500000)">
                        资助5000
                    </div>
                </td>
            </tr> 
            <tr>
                <td style="width:50%">
                    <div id="bthNeedToUpdateLevel" style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.updateLevel();" >
                        同步等级
                    </div>
                </td>
                <td style="width: 50%">
                    <div id="btnSignOnLineWhenSubsidize" style="background: yellowgreen; width:90%;margin-left:5%;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.signOnline();">
                        线上私钥签名
                    </div>
                </td>
            </tr>
        </table> 
        <div style="background: orange;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;" onclick="subsidizeSys.add();">
            取消
        </div>
    </div>`,
    add: function () {
        var that = subsidizeSys;
        if (document.getElementById(that.operateID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.html);
            frag.id = that.operateID;

            document.body.appendChild(frag);
            that.updateMoney();
            that.updateSignInfomation();
            that.updateMoneyOfSumSubsidized();
            that.updateMoneyOfSumSubsidizing();

            var el = document.getElementById('moneySubsidize');
            el.classList.remove('msg');
            if (objMain.stateNeedToChange.isLogin) {

            }
            else {
                var bthNeedToUpdateLevel = document.getElementById('bthNeedToUpdateLevel');
                bthNeedToUpdateLevel.classList.add('needToClick');

            }
        }
        else {
            document.getElementById(that.operateID).remove();
            if (objMain.stateNeedToChange.isLogin) { }
            else {
                var el = document.getElementById('moneySubsidize');
                el.classList.add('msg');
            }
        }
    },
    updateMoneyOfSumSubsidizing: function () {
        var that = subsidizeSys;
        if (that.operateAddress != '')
            if (document.getElementById('moneyOfSumSubsidizing') != null) {
                document.getElementById('moneyOfSumSubsidizing').innerText = (that.LeftMoneyInDB[that.operateAddress] / 100).toFixed(2);
            }
    },
    updateMoneyOfSumSubsidized: function () {
        var that = subsidizeSys;
        if (document.getElementById('moneyOfSumSubsidized') != null) {
            document.getElementById('moneyOfSumSubsidized').innerText = (that.SupportMoney / 100).toFixed(2);
        }
    },
    updateMoney: function () {
        document.getElementById('msgNeedToSign').value = JSON.parse(sessionStorage['session']).Key;
    },
    subsidize: function (subsidizeValue) {
        var bitcoinAddress = document.getElementById('bitcoinSubsidizeAddressInput').value;
        if (yrqCheckAddress(bitcoinAddress)) {
            document.getElementById('bitcoinSubsidizeAddressInput').style.background = 'rgba(127, 255, 127, 0.6)';

            var signature = document.getElementById('signatureInputTextArea').value;
            var signMsg = JSON.parse(sessionStorage['session']).Key;
            document.getElementById('msgNeedToSign').value = signMsg;
            if (yrqVerify(bitcoinAddress, signature, signMsg)) {
                document.getElementById('signatureInputTextArea').style.background = 'rgba(127, 255, 127, 0.6)';
                objMain.ws.send(JSON.stringify({ c: 'GetSubsidize', signature: signature, address: bitcoinAddress, value: subsidizeValue }));
                subsidizeSys.operateAddress = bitcoinAddress;
                subsidizeSys.signInfoMatiion = [signature, bitcoinAddress];
                //  nyrqUrl.set(bitcoinAddress);
            }
            else {
                document.getElementById('signatureInputTextArea').style.background = 'rgba(255, 127, 127, 0.9)';
            }
            //var signature=
            //            if (yrqVerify(bitcoinAddress

        }
        else {
            document.getElementById('bitcoinSubsidizeAddressInput').style.background = 'rgba(255, 127, 127, 0.9)';
        }
    },
    signOnline: function () {
        subsidizeSys.add();
        subsidizeSys.add2();
        if (objMain.stateNeedToChange.isLogin) { }
        else {
            var el = document.getElementById('moneySubsidize');
            el.classList.remove('msg');
        }
        PrivateSignPanelObj.subsidizeNotify();
    },
    add2: function () {

        if (document.getElementById(PrivateSignPanelObj.id) == null) {
            PrivateSignPanelObj.show(
                function () {
                    return true;
                },
                function () {
                    if (!objMain.stateNeedToChange.isLogin) {

                    }
                }, function (addr, sign) {
                    var that = subsidizeSys;
                    that.signInfoMatiion = [sign, addr];
                    that.add2();
                    that.add();
                },
                JSON.parse(sessionStorage['session']).Key,
                function () {
                    if (objMain.stateNeedToChange.isLogin) { }
                    else {
                        var el = document.getElementById('moneySubsidize');
                        el.classList.add('msg');

                    }
                }
            );
        }
        else {
            document.getElementById(PrivateSignPanelObj.id).remove();
        }
    },
    signInfoMatiion: null,
    LeftMoneyInDB: {},
    updateSignInfomation: function () {
        var that = subsidizeSys;
        if (that.signInfoMatiion != null) {
            if (document.getElementById('bitcoinSubsidizeAddressInput') != null) {
                document.getElementById('bitcoinSubsidizeAddressInput').value = that.signInfoMatiion[1];
            }
            if (document.getElementById('signatureInputTextArea') != null) {
                document.getElementById('signatureInputTextArea').value = that.signInfoMatiion[0];
            }
        }
    },
    SupportMoney: 0,
    getPrivateKey: function () {
        document.getElementById('subsidizePanelPromptPrivateKeyValue').value = yrqGetRandomPrivateKey();

        $.notify(`私钥是所有游戏权限与收益的基础
务必妥善保管私钥，
私钥如泄露，代表者权限放开，
收益丢失；
私钥丢失，无法找回！`,
            {
                autoHide: true,
                className: 'info',
                autoHideDelay: 60000,
                position: 'top',
            });
        if (objMain.stateNeedToChange.isLogin) { }
        else {
            var el1 = document.getElementById('subsidizeBtnSignMsg');
            el1.classList.add('needToClick');

            var el2 = document.getElementById('subsidizeBtnGetPrivateKey');
            el2.classList.remove('needToClick');

            var el3 = document.getElementById('labelDivNeedToInputPrivateKey');
            el3.classList.remove('needToClick');
        }
    },
    updateLevel: function () {
        var bitcoinAddress = document.getElementById('bitcoinSubsidizeAddressInput').value;
        if (yrqCheckAddress(bitcoinAddress)) {
            document.getElementById('bitcoinSubsidizeAddressInput').style.background = 'rgba(127, 255, 127, 0.6)';

            var signature = document.getElementById('signatureInputTextArea').value;
            var signMsg = JSON.parse(sessionStorage['session']).Key;
            document.getElementById('msgNeedToSign').value = signMsg;
            if (yrqVerify(bitcoinAddress, signature, signMsg)) {
                document.getElementById('signatureInputTextArea').style.background = 'rgba(127, 255, 127, 0.6)';
                objMain.ws.send(JSON.stringify({ c: 'GetSubsidize', signature: signature, address: bitcoinAddress, value: 0 }));
                subsidizeSys.operateAddress = bitcoinAddress;
                subsidizeSys.signInfoMatiion = [signature, bitcoinAddress];
                // nyrqUrl.set(bitcoinAddress);
            }
            else {
                document.getElementById('signatureInputTextArea').style.background = 'rgba(255, 127, 127, 0.9)';
            }
            //var signature=
            //            if (yrqVerify(bitcoinAddress

        }
        else {
            document.getElementById('bitcoinSubsidizeAddressInput').style.background = 'rgba(255, 127, 127, 0.9)';
            if (objMain.stateNeedToChange.isLogin) { }
            else {
                var bthNeedToUpdateLevel = document.getElementById('bthNeedToUpdateLevel');
                bthNeedToUpdateLevel.classList.remove('needToClick');


                var btnSignOnLineWhenSubsidize = document.getElementById('btnSignOnLineWhenSubsidize');
                btnSignOnLineWhenSubsidize.classList.add('needToClick');
            }
        }
        //objMain.ws.send(JSON.stringify({ c: 'UpdateLevel', signature: signature, address: bitcoinAddress, value: subsidizeValue }));
    },
    copyStr: function () {
        if (navigator && navigator.clipboard && navigator.clipboard.writeText) {
            var msg = document.getElementById('msgNeedToSign').value;
            $.notify('"' + msg + '"   已经复制到剪切板', "success");
            //  $(".elem-demo").notify("Hello Box");
            return navigator.clipboard.writeText(msg);
        }
        else {
            alert('浏览器设置不支持访问剪切板！');
        }
    },
    readStr: function (eId) {
        if (navigator && navigator.clipboard && navigator.clipboard.readText) {
            // var msg = document.getElementById('msgNeedToSign').value;
            // $.notify('"' + msg + '"   已经复制到剪切板', "success");
            //  $(".elem-demo").notify("Hello Box");
            // return navigator.clipboard.writeText(msg);
            // document.getElementById('signatureInputTextArea').value = navigator.clipboard.readText();
            navigator.clipboard.readText().then(
                clipText => document.getElementById(eId).value = clipText);
        }
        else {
            alert('浏览器设置不支持访问剪切板！');
        }
    },
    removeBtnsGuid: function () {
        var el = document.getElementById('moneySubsidize');
        if (el)
            el.classList.remove('msg');
        var bthNeedToUpdateLevel = document.getElementById('bthNeedToUpdateLevel');
        if (bthNeedToUpdateLevel)
            bthNeedToUpdateLevel.classList.remove('needToClick');
        var btnSignOnLineWhenSubsidize = document.getElementById('bthNeedToUpdateLevel');
        if (btnSignOnLineWhenSubsidize)
            btnSignOnLineWhenSubsidize.classList.remove('needToClick');
    },
    clearInfo: function () {
        this.signInfoMatiion = null;
    }
}
    ;
var debtInfoSys =
{
    operateID: 'debtPanel',
    html: `<div id="subsidizePanel"  style="position:absolute;z-index:8;top:calc(10% - 1px);width:24em; left:calc(50% - 12em);height:auto;border:solid 1px red;text-align:center;background:rgba(104, 48, 8, 0.85);color:#83ffff;overflow-y: scroll;max-height: calc(90%);  ">
        <table style="width:100%;">
            <tr>
                <th>剩余资助</th>
                <th>现有资助</th>
            </tr>
            <tr>
                <td id="moneyOfSumSubsidizing" >未知</td>
                <td id="moneyOfSumSubsidized">0</td>
            </tr>
        </table>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label>
                --↓↓↓输入1打头的比特币地址↓↓↓--
            </label>
            <input id="bitcoinSubsidizeAddressInput" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);" />
        </div>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label onclick="alert('弹出二维码');">
                --↓↓↓对以下信息进行签名↓↓↓--
            </label> 
            <input  id="msgNeedToSign" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);" readonly />
        </div>
        <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

            <label onclick="alert('弹出扫描二维码');">
                --↓↓↓输入签名↓↓↓--
            </label>
            <textarea id="signatureInputTextArea" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);height:4em;overflow:hidden;">1111111111111111111111</textarea>

        </div> 

        <table style="width:100%">
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;" onclick="subsidizeSys.subsidize(50000)" >
                        资助500
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;"  onclick="subsidizeSys.subsidize(100000)" >
                        资助1000
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width:50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;" onclick="subsidizeSys.subsidize(200000)" >
                        资助2000
                    </div>
                </td>
                <td style="width: 50%">
                    <div style="background: yellowgreen; width:90%;margin-left:5%;" onclick="subsidizeSys.subsidize(500000)">
                        资助5000
                    </div>
                </td>
            </tr> 
        </table>
         <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="subsidizeSys.signOnline();">
            线上私钥签名
        </div>
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="subsidizeSys.add();">
            取消
        </div>
    </div>`,
    add: function () {
        var that = subsidizeSys;
        if (document.getElementById(that.operateID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.html);
            frag.id = that.operateID;

            document.body.appendChild(frag);
            that.updateMoney();
            that.updateSignInfomation();
            that.updateMoneyOfSumSubsidized();
            that.updateMoneyOfSumSubsidizing();
        }
        else {
            document.getElementById(that.operateID).remove();
        }
    },
};