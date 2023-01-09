var reward =
{
    'id': 'rewardPublishPanel',
    'htmlValue': ` <div style="width: 100%;
        height: 100%;
        background-color: rgba(56,60,67,.15);
        position: absolute;
        left: 0px;
        top: 0px;">
            <div style="line-height: 1.5;
       font-family: Gordita,Helvetica Neue,Helvetica,Arial,sans-serif;
       -webkit-font-smoothing: antialiased;
       font-size: calc(15px + (100vw - 320px)/880);
       color: #151922;
       box-sizing: border-box;
       border: 0 solid #151922;
       background-color: #fff;
       border-radius: 3px;
       padding: 1.6rem;
       padding-top: 1.6rem;
       padding-bottom: 1.6rem;
       box-shadow: 0 0 0 1px rgba(56,60,67,.05),0 1px 3px 0 rgba(56,60,67,.15);
       margin-top: 3.2rem; min-height: 300px; width:80%;left:10%;
       position:relative;">
                <div> <label>建筑物地址:</label></div>
                <div>
                    <select id="buidingAddrForAddReward"> 
                    </select>
                </div>
                <div style="text-align:center;"><button id="GetRewardAddrBtn" style="height:2em;">获取奖励地址</button></div>
                <div>
                    <select id="stockAddrForAddReward">
                        <!--<option value="a">c</option>
                        <option value="b">d</option>-->
                    </select>
                </div>
                <div>
                    <label>奖励金额↓↓↓↓</label>
                    <input id="rewardSantoshi" type="number" min="1" max="999" />Santoshi
                </div>
                <div style="text-align:center;"><button id="GenerateRewardAddrBtn" style="height:2em;">生成协议</button></div>

                <div>
                    <label>协议↓↓↓↓</label>
                    <textarea id="setRewardNeedToSignTextArea" style="width:98%;height:10em;"></textarea>
                </div>
                <div>
                    <label>建筑物地址签名↓</label>
                    <textarea id="signForBuidingAddrForAddReward" style="width:98%;height:2em;"></textarea>
                </div>
                <div>
                    <label>奖励地址签名↓</label>
                    <textarea id="signForStockAddrForAddReward" style="width:98%;height:2em;"></textarea>
                </div>
                <div style="text-align:center;"><button id="PublishRewardBtn" style="height:2em;">发布奖励</button></div>
            </div>

        </div>`,
    'tradeAddress': '',
    'bussinessAddr': '',
    'tradeIndex': -1,
    'passCoin': 1,
    'run': function () {
        //var addr = prompt('输入建筑物打赏地址');
        //this.bussinessAddr = addr;
        //this.tradeAddress = prompt('输入奖赏颁布地址');
        //this.tradeIndex = Number.parseInt(prompt('输入交易号'));
        //this.passCoin = Number.parseInt(prompt('输入聪值'));
        //var msg = `${this.tradeIndex}@${this.tradeAddress}@${this.bussinessAddr}->SetAsReward:${this.passCoin}Satoshi`;
        //console.log('msgNeedToSign', msg);
        var that = reward;
        if (document.getElementById(that.id) == null) {


            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.htmlValue);
            frag.id = that.id;

            document.body.appendChild(frag);
            var getFormatDate = function () {
                var dt = new Date();
                var y = dt.getFullYear();
                var m = dt.getMonth() + 1;
                var d = dt.getDate()
                var r = '' + y + (m < 10 ? '0' : '') + m + (d < 10 ? '0' : '') + d;
                return r;

            }
            this.administratorAddr = prompt('输入地址');
            this.signOfAdministrator = prompt('输入对' + getFormatDate() + '的签名', getFormatDate());
            var passObj = { c: "AllBusinessAddr", administratorAddr: that.administratorAddr, signOfAdministrator: that.signOfAdministrator };
            console.log('passStr', JSON.stringify(passObj));
            objMain.ws.send(JSON.stringify(passObj));
            document.getElementById('GetRewardAddrBtn').onclick = function () {
                //  alert('设置名字！');
                //that.ChangeName();
                var bAddr = document.getElementById('buidingAddrForAddReward').value;
                var obj = { 'bAddr': bAddr, 'c': 'AllStockAddr', "administratorAddr": that.administratorAddr, "signOfAdministrator": that.signOfAdministrator };
                objMain.ws.send(JSON.stringify(obj));
                // objMain.ws.send('{"c":"AllStockAddr"}')
            }
            document.getElementById('stockAddrForAddReward').onchange = function () {
                //alert('stockAddrForAddReward！');
                var addrAndValue = document.getElementById('stockAddrForAddReward').value;
                var values = addrAndValue.split(':');
                if (values.length == 2) {
                    document.getElementById('rewardSantoshi').value = Number.parseInt(values[1]);
                    document.getElementById('rewardSantoshi').max = Number.parseInt(values[1]);
                }
            }
            document.getElementById('GenerateRewardAddrBtn').onclick = function () {
                //alert('stockAddrForAddReward！');
                var bAddr = document.getElementById('buidingAddrForAddReward').value;
                var addrAndValue = document.getElementById('stockAddrForAddReward').value.split(':')[0];
                var passCoin = document.getElementById('rewardSantoshi').value;
                var obj = { 'c': 'GenerateRewardAgreement', 'addrFrom': addrAndValue, 'addrBussiness': bAddr, 'tranNum': passCoin, "administratorAddr": that.administratorAddr, "signOfAdministrator": that.signOfAdministrator };
                objMain.ws.send(JSON.stringify(obj));
            }
            document.getElementById('PublishRewardBtn').onclick = function () {

                var msg = document.getElementById('setRewardNeedToSignTextArea').value;
                var signOfAddrBussiness = document.getElementById('signForBuidingAddrForAddReward').value;
                var signOfAddrReward = document.getElementById('signForStockAddrForAddReward').value;
                var obj = {
                    'c': 'RewardPublicSign',
                    'msg': msg,
                    'signOfAddrBussiness': signOfAddrBussiness,
                    'signOfAddrReward': signOfAddrReward,
                    'administratorAddr': that.administratorAddr,
                    'signOfAdministrator': that.signOfAdministrator
                };
                console.log('sendMsg', JSON.stringify(obj));
                objMain.ws.send(JSON.stringify(obj));
                //alert('stockAddrForAddReward！');
                //var bAddr = document.getElementById('buidingAddrForAddReward').value;
                //var addrAndValue = document.getElementById('stockAddrForAddReward').value.split(':')[0];
                //var passCoin = document.getElementById('rewardSantoshi').value;
                //var obj = { 'c': 'GenerateRewardAgreement', 'addrFrom': addrAndValue, 'addrBussiness': bAddr, 'tranNum': passCoin, "administratorAddr": that.administratorAddr, "signOfAdministrator": that.signOfAdministrator };
                //objMain.ws.send(JSON.stringify(obj));
            }
            //  onclick = "selectSingleTeamJoinHtmlF.ChangeName();"
            //frag.onclick = function ()
            //{
            //    alert('提醒你！！！');
            //};
            //that.updateSocialResponsibility();

        }
        else {
            //that.updateSocialResponsibility();
        }
    },
    'showAgreement': function (v) {
        document.getElementById('setRewardNeedToSignTextArea').value = v;
    },
    'administratorAddr': '',
    'signOfAdministrator': '',
    'htmlHasNoData': `<div style="width: 100%;
        height: 100%;
        background-color: rgba(56,60,67,.15);
        position: absolute;
        left: 0px;
        top: 0px;">
            <div class="tableOfReward" style="line-height: 1.5;
       font-family: Gordita,Helvetica Neue,Helvetica,Arial,sans-serif;
       -webkit-font-smoothing: antialiased;
       font-size: calc(15px + (100vw - 320px)/880);
       color: #151922;
       box-sizing: border-box;
       border: 0 solid #151922;
       background-color: #fff;
       border-radius: 3px;
       padding: 1.6rem;
       padding-top: 1.6rem;
       padding-bottom: 1.6rem;
       box-shadow: 0 0 0 1px rgba(56,60,67,.05),0 1px 3px 0 rgba(56,60,67,.15);
       margin-top: 3.2rem; min-height: 300px; width:80%;left:10%;
       position:relative;">
                <div style="text-align:center;"><span id="rewardTimeTitle"></span></div>
                <div>此期无奖品</div>
                <div style="text-align:center;">
                    <button id="previousRewardTimeBtn">上一期</button>
                    <button id="RewardTimeReback">返回</button>
                    <button id="nextRewardTimeBtn">下一期</button>
                </div>
            </div>

        </div>`,
    'htmlHasNoDataDOMID': 'rewardHasNoDataPage',
    'hasNoData': function (title) {
        var that = reward;
        if (document.getElementById(that.id) == null) {
            document.getElementById('rootContainer').innerHTML = '';

            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.htmlHasNoData);
            frag.id = that.htmlHasNoDataDOMID;

            document.getElementById('rootContainer').appendChild(frag);
            document.getElementById('rewardTimeTitle').innerText = title;

            that.navigationAdd();
            that.msgToApply = '';
        }
        else {
        }
    },
    'page': 0,
    'htmlHasDataDOMID': 'rewardHasDataPage',
    'htmlHasData': `  <div id="rewardHasDataPage" class="tableOfReward"  style="width: 100%;
        height: 100%;
        background-color: rgba(56,60,67,.15);
        position: absolute;
        left: 0px;
        top: 0px;">
            <div style="line-height: 1.5;
       font-family: Gordita,Helvetica Neue,Helvetica,Arial,sans-serif;
       -webkit-font-smoothing: antialiased;
       font-size: calc(15px + (100vw - 320px)/880);
       color: #151922;
       box-sizing: border-box;
       border: 0 solid #151922;
       background-color: #fff;
       border-radius: 3px;
       padding: 1.6rem;
       padding-top: 1.6rem;
       padding-bottom: 1.6rem;
       box-shadow: 0 0 0 1px rgba(56,60,67,.05),0 1px 3px 0 rgba(56,60,67,.15);
       margin-top: 3.2rem; min-height: 300px; width:80%;left:10%;
       position:relative;overflow-y:scroll;max-height:calc(100% - 6.2rem);">
                <div style="text-align:center;"><span id="rewardTimeTitle"></span></div>
                <div style="text-align:center;"><span id="rewardTimeMsgToNotify"></span></div>
                <div style="text-align:center;margin-top:2em;">
                    <button id="useLevelToApplyRewardBtn">申请</button>
                </div>
                <div style="text-align:center;margin-top:2em;">
                    <button id="lookRewardBuildingStockDetailBtn">查看</button>
                </div>
                <table border="1" style="text-align:center;">
                    <tr>
                        <th>代签名消息</th>
                        <th>奖励</th>
                        <th>状态</th>
                    </tr>
                    <tr>
                        <td id="rewardMsgNeedToSign">20220928</td>
                        <td id="rewardPublishiMoney">40000Santoshi</td>
                        <td id="rewardPublishState"></td>
                    </tr>
                </table>
                <div id="rewardAppleItemContainer"></div> 
                <div style="text-align:center;">
                    <button id="previousRewardTimeBtn">上一期</button>
                    <button id="RewardTimeReback">返回</button>
                    <button id="nextRewardTimeBtn">下一期</button> 
                </div>
                <table border="1" style="margin-top:4em">
                    <tr>
                        <th>奖励信息</th>
                        <td style="word-break:break-all;word-wrap:anywhere;" id="rewardInfomationMsg">1@3FkXatYUQv81t7mDsQf8igumGnp1mE1gkk@3Kk8VZ4NLAGUgWggEevQtX7xSJEnhstYjV->SetReward:10000sataoshi</td>
                    </tr>
                    <tr>
                        <th>建筑地址签名</th>
                        <td style="word-break:break-all;word-wrap:anywhere;" id="rewardInfomationBuildingAddrSign">IFWLbdEmXP6CNgORu0RNc7IBu7Hg/FYjBY8HRGepgP4zByvD/HIlgJ6lejwSOVcN7iapjtJqKpBzGDsQ71iLIZk=</td>
                    </tr>
                    <tr>
                        <th>奖励地址签名</th>
                        <td style="word-break:break-all;word-wrap:anywhere;" id="rewardInfomationRewardAddrSign">IFWLbdEmXP6CNgORu0RNc7IBu7Hg/FYjBY8HRGepgP4zByvD/HIlgJ6lejwSOVcN7iapjtJqKpBzGDsQ71iLIZk=</td>
                    </tr>
                </table>
            </div>
        </div>`,
    'hasData': function (title, data, list, indexNumber) {
        var that = reward;
        if (document.getElementById(that.id) == null) {
            document.getElementById('rootContainer').innerHTML = '';

            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.htmlHasData);
            frag.id = that.htmlHasDataDOMID;

            document.getElementById('rootContainer').appendChild(frag);
            document.getElementById('rewardTimeTitle').innerText = title;

            document.getElementById('rewardMsgNeedToSign').innerText = data.startDate + '';
            document.getElementById('rewardPublishiMoney').innerText = data.passCoin + '聪';
            document.getElementById('rewardInfomationMsg').innerText = data.orderMessage;
            if (document.getElementById('rewardPublishState') != null) {
                if (data.waitingForAddition == 0) {
                    document.getElementById('rewardHasDataPage').style.backgroundColor = 'orange';
                    document.getElementById('rewardPublishState').innerText = '已颁发';
                }
                else if (data.waitingForAddition == 1) {
                    document.getElementById('rewardHasDataPage').style.backgroundColor = 'green';
                    document.getElementById('rewardPublishState').innerText = '未颁发';
                }

            }
            document.getElementById('rewardInfomationBuildingAddrSign').innerText = data.signOfBussinessAddr;
            document.getElementById('rewardInfomationRewardAddrSign').innerText = data.signOfTradeAddress;

            //document.getElementById('nextRewardTimeBtn').onclick = function () {
            //    that.page++;
            //    objMain.ws.send(JSON.stringify({ 'c': 'RewardInfomation', 'Page': that.page }));
            //}
            that.navigationAdd();
            that.msgToApply = data.orderMessage;
            //list = [];
            for (var i = 0; i < list.length; i++) {
                var itemHtml = `<table border="1" style="margin-top:1em;">

                    <tr>
                        <th>申请地址</th>
                        <th>申请等级</th>
                        <th>获得点数</th>
                        <th>比例</th>
                    </tr>
                    <tr>
                        <td style="word-break:break-all;word-wrap:anywhere;">${list[i].applyAddr}</td>
                        <td style="word-break:break-all;word-wrap:anywhere;">${list[i].applyLevel}级</td>
                        <td style="word-break:break-all;word-wrap:anywhere;">${list[i].satoshiShouldGet}satoshi</td>
                        <td style="word-break:break-all;word-wrap:anywhere;">${list[i].percentStr}</td>
                    </tr>
                    <tr>
                        <th colspan="1" style="word-break:break-all;word-wrap:anywhere;">消息→</th>
                        <td colspan="2" style="word-break:break-all;word-wrap:anywhere;">${list[i].startDate}</td>
                        <th colspan="1" style="word-break:break-all;word-wrap:anywhere;">↓签名↓</th>
                    </tr>
                    <tr>
                        <td colspan="4" style="word-break:break-all;word-wrap:anywhere;">${list[i].applySign}</td>
                    </tr>
                </table>`
                var tableFrag = document.createRange().createContextualFragment(itemHtml);
                document.getElementById('rewardAppleItemContainer').appendChild(tableFrag);
            }

            //useLevelToApplyRewardBtn
            document.getElementById('useLevelToApplyRewardBtn').onclick = function () {
                var domObj = document.createRange().createContextualFragment(that.dialogToApplyRewardHtml);
                domObj.id = that.applyDialogID;
                document.getElementById('rootContainer').appendChild(domObj);
                document.getElementById("msgNeedToSignForRewardApply").innerText = document.getElementById('rewardMsgNeedToSign').innerText;
            };
            document.getElementById('lookRewardBuildingStockDetailBtn').onclick = function () {
                var title = document.getElementById('rewardMsgNeedToSign').innerText;
                QueryReward.draw3D();
                objMain.ws.send(JSON.stringify(
                    {
                        'c': 'RewardBuildingShow',
                        'Title': title
                    }));
                QueryReward.drawToolBar(title);
            };
            that.msgsToTransferSshares = [];
            for (var i = 0; i < list.length; i++) {
                var msg = `${indexNumber + i}@${data.tradeAddress}@${data.bussinessAddr}->${list[i].applyAddr}:${list[i].satoshiShouldGet}Satoshi`;
                that.msgsToTransferSshares.push(msg);
            }
        }
        else {
        }
    },
    'navigationAdd': function () {
        var that = reward;
        document.getElementById('nextRewardTimeBtn').onclick = function () {
            that.page++;
            objMain.ws.send(JSON.stringify({ 'c': 'RewardInfomation', 'Page': that.page }));
        };
        document.getElementById('previousRewardTimeBtn').onclick = function () {
            that.page--;
            objMain.ws.send(JSON.stringify({ 'c': 'RewardInfomation', 'Page': that.page }));
        };
        document.getElementById('RewardTimeReback').onclick = function () {
            // that.page--;
            objMain.ws.send(JSON.stringify({ 'c': 'QueryRewardCancle' }));
        };
        //previousRewardTimeBtn
    },
    'msgToApply': '',
    'dialogToApplyRewardHtml': ` <div id="applyRewardDialog" style="position: absolute;
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
        overflow-y: scroll;
        max-height: calc(90%);
">
            <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

                 <label  onclick="reward.readStr('bitcoinAddressInputForRewardApply');">
                    --↓↓↓输入申请地址↓↓↓--
                </label>
                <input id="bitcoinAddressInputForRewardApply" type="text" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);"/>
            </div>
            <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

                <label onclick="reward.copyStr();">
                    --↓↓↓对以下信息进行签名↓↓↓--
                </label>
                <div id="msgNeedToSignForRewardApply" style="width:calc(90% - 10px);margin-left:calc(5% + 5px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);text-align:center;">
                    1111111111111111111
                </div>

            </div>
            <div style="
        margin-bottom: 0.25em;
        margin-top: 0.25em;border:1px solid gray;">

                <label onclick="reward.readStr('signatureInputTextAreaForRewardApply');">
                    --↓↓↓输入签名↓↓↓--
                </label>
                <textarea id="signatureInputTextAreaForRewardApply" style="width:calc(90% - 10px);margin-bottom:0.25em;background:rgba(127, 255, 127, 0.6);height:4em;overflow:hidden;">您的签名</textarea>

            </div>
            <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.25em 0 0.25em 0;" onclick="reward.apply();">
                申请
            </div>
            <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.25em 0 0.25em 0;" onclick="reward.onlineSign();">
                在线签名
            </div>
            <div style="background: orange;
        margin-bottom: 1.25em;
        margin-top: 0.25em;padding:0.25em 0 0.25em 0;" onclick="reward.applyCancle();">
                取消
            </div>
        </div>`,
    'applyDialogID': 'applyRewardDialog',
    copyStr: function () {
        var msg = document.getElementById('rewardMsgNeedToSign').innerText;
        if (navigator && navigator.clipboard && navigator.clipboard.writeText) {
            $.notify('"' + msg + '"\n   已经复制到剪切板', "success");
            return navigator.clipboard.writeText(msg);
        }
        else {
            alert('浏览器设置不支持访问剪切板！');
        }
    },
    readStr: function (eId) {
        if (navigator && navigator.clipboard && navigator.clipboard.readText) {
            navigator.clipboard.readText().then(
                clipText => document.getElementById(eId).value = clipText);
        }
        else {
            alert('浏览器设置不支持访问剪切板！');
        }
    },
    apply: function () {
        var canPass = true;
        var addr = document.getElementById('bitcoinAddressInputForRewardApply').value;
        if (window.yrqCheckAddress(addr)) {
            document.getElementById('bitcoinAddressInputForRewardApply').style.background = 'rgba(127, 255, 127, 0.6)';
        }
        else {
            document.getElementById('bitcoinAddressInputForRewardApply').style.background = 'rgba(255, 127, 127, 0.9)';

            canPass = canPass & false;
        }
        if (canPass) {
            var msgNeedToSign = document.getElementById('rewardMsgNeedToSign').innerText;
            var signature = document.getElementById('signatureInputTextAreaForRewardApply').value;
            objMain.ws.send(JSON.stringify(
                {
                    'c': 'RewardApply',
                    'addr': addr,
                    'msgNeedToSign': msgNeedToSign,
                    'signature': signature
                }));
            reward.applyCancle();
        }
    },
    applyCancle: function () {
        var that = reward;
        var op = document.getElementById(that.applyDialogID);
        if (op != null) {
            op.remove();
        }
    },
    giveAward:
    {
        run: function () {
            var rewardTime = prompt('输入颁奖日期');

        }

    },
    msgsToTransferSshares: [],
    downloadF: function () {
        const saveTemplateAsFile = (filename, dataObjToWrite) => {
            const blob = new Blob([JSON.stringify(dataObjToWrite)], { type: "text/json" });
            const link = document.createElement("a");

            link.download = filename;
            link.href = window.URL.createObjectURL(blob);
            link.dataset.downloadurl = ["text/json", link.download, link.href].join(":");

            const evt = new MouseEvent("click", {
                view: window,
                bubbles: true,
                cancelable: true,
            });

            link.dispatchEvent(evt);
            link.remove()
        };
        var obj =
        {
            addr: '',
            time: document.getElementById('rewardMsgNeedToSign').innerText,
            list: reward.msgsToTransferSshares
        };
        saveTemplateAsFile('msgsNeedToSign' + document.getElementById('rewardMsgNeedToSign').innerText + '.json', obj);
    },
    notifyMsg(msg) {
        if (document.getElementById('rewardTimeMsgToNotify') == null) {

        }
        else {
            document.getElementById('rewardTimeMsgToNotify').innerText = msg;
            document.getElementById('rewardTimeMsgToNotify').style.color = 'green';
        }
    },
    onlineSign: function () {
        reward.applyCancle();
        PrivateSignPanelObj.show(
            function () {
                return true;// return document.getElementById('agreementText').value != '';
            },
            function () {
                $.notify('协议为空', 'info');
            }, function (addr, sign) {
                var that = reward;
                var domObj = document.createRange().createContextualFragment(that.dialogToApplyRewardHtml);
                domObj.id = that.applyDialogID;
                document.getElementById('rootContainer').appendChild(domObj);
                document.getElementById('bitcoinAddressInputForRewardApply').value = addr;
                document.getElementById('signatureInputTextAreaForRewardApply').value = sign;
                document.getElementById("msgNeedToSignForRewardApply").innerText = document.getElementById('rewardMsgNeedToSign').innerText;
            },
            document.getElementById('rewardMsgNeedToSign').innerText)
        // PrivateSignPanelObj

    }
}

//    function () {
//    var tradeAddress = prompt('输入tradeAddress,即建筑物股东', '');
//    var bussinessAddr = prompt('输入bussinessAddr,即建筑物打赏地址', '');
//    var passCoin = prompt('passCoin，即要作为奖励的钱(聪)', '');
//    // var passIndex = prompt('输入要传递的index', '');
//    // var passMsg=`{}`
//};

