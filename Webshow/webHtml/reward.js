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
    'signOfAdministrator': ''
}

//    function () {
//    var tradeAddress = prompt('输入tradeAddress,即建筑物股东', '');
//    var bussinessAddr = prompt('输入bussinessAddr,即建筑物打赏地址', '');
//    var passCoin = prompt('passCoin，即要作为奖励的钱(聪)', '');
//    // var passIndex = prompt('输入要传递的index', '');
//    // var passMsg=`{}`
//};

