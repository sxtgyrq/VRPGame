var PrivateSignPanelObj =
{
    id: 'WindowToSignMsgByPrivateKey',

    html: `<div id="WindowToSignMsgByPrivateKey" style="position: absolute;
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
        <div  id="labelDivNeedToInputPrivateKey"  style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;"> 
            <label onclick="subsidizeSys.readStr('subsidizePanelPromptPrivateKeyValue');">
                --↓↓↓输入您珍贵的私钥↓↓↓--
            </label> 
            
 <textarea id="subsidizePanelPromptPrivateKeyValue" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);height:4em;overflow:hidden;" onchange="subsidizeSys.privateKeyChanged();"></textarea>
 
       
        <div id="subsidizeBtnSignMsg" style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.25em 0 0.25em 0;" onclick="PrivateSignPanelObj.sign();">
            签名
        </div>
        <div id="subsidizeBtnGetPrivateKey" style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.25em 0 0.25em 0;" onclick="subsidizeSys.getPrivateKey();">
            获取私钥
        </div>
        <div style="background: orange;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.25em 0 0.25em 0;" onclick="PrivateSignPanelObj.cancel();">
            取消
        </div>
    </div>`,
    show: function (checkF, fail, success, msg, cancle) {
        var that = PrivateSignPanelObj;
        if (checkF != undefined && checkF()) {
            if (document.getElementById(that.id) == null) {
                var frag = document.createRange().createContextualFragment(that.html);
                frag.id = that.id;
                document.body.appendChild(frag);
            }
            else {
                document.getElementById(that.id).remove();
            }
        }
        else if (fail != undefined) {
            fail();
        }
        else {
            if (document.getElementById(that.id))
                document.getElementById(that.id).remove();
        }
        if (success != undefined) that.success = success;
        else
            that.success = null;
        if (msg != undefined)
            that.msg = msg;
        else
            that.msg = '';
        if (cancle != undefined) {
            that.cancelF = cancle;
        }
        else that.cancelF = null;
    },
    sign: function () {
        var that = PrivateSignPanelObj;
        var privateKey = document.getElementById('subsidizePanelPromptPrivateKeyValue').value;
        // var signMsg = document.getElementById('bindWordMsg').value;
        if (yrqCheckPrivateKey(privateKey)) {
            if (that.success != null && that.msg != '') {
                var signMsg = that.msg;
                var valuesGet = yrqSign(privateKey, signMsg, document.getElementById('p2wpkhp2sh').checked);
                var addr = valuesGet[1];
                var sign = valuesGet[0];
                that.success(addr, sign);
                that.show();
            }
            // var valuesGet = yrqSign(privateKey, signMsg, document.getElementById('p2wpkhp2sh').checked);
            //GuidObj.charging.signOnLine.show(); 
            //document.getElementById('bindWordAddr').value = valuesGet[1];
            //document.getElementById('bindWordSign').value = valuesGet[0];
        }
        else {
            document.getElementById('subsidizePanelPromptPrivateKeyValue').style.background = 'rgba(255, 127, 127, 0.6)';
        }
    },
    msg: '',
    success: null,
    cancelF: null,
    cancel: function () {
        var that = PrivateSignPanelObj;
        document.getElementById(that.id).remove();
        if (that.cancelF !== undefined && that.cancelF != null) {
            that.cancelF();
            that.cancelF = null;
        }
    },
    subsidizeNotify: function () {
        if (objMain.stateNeedToChange.isLogin) { }
        else {
            var el = document.getElementById('subsidizeBtnGetPrivateKey');
            el.classList.add('needToClick');

            var el = document.getElementById('labelDivNeedToInputPrivateKey');
            el.classList.add('needToClick');
        }
    }
}

