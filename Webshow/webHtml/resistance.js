var resistance =
{
    operateID: 'resistancePanel',

    bindData: function (key) {
        var that = resistance;
        if (document.getElementById(that.operateID) == null) {
            objMain.ws.send(JSON.stringify({ 'c': 'GetResistance', 'KeyLookfor': key, 'RequestType': 0 }));
        }
        else {
            document.getElementById(that.operateID).remove();
        }
        if (document.getElementById(that.operate2ID) == null) { }
        else {
            document.getElementById(that.operate2ID).remove();
        }
    },
    bindData2: function (key) {
        //  alert(key);
        objMain.ws.send(JSON.stringify({ 'c': 'GetResistance', 'KeyLookfor': key, 'RequestType': 1 }));
    },
    display: function (obj) {
        var that = resistance;
        var html = ` <div id="${that.operateID}" style="position: absolute;
        z-index: 8;
        top: calc(10% - 1px);
        width: 24em;
        left: calc(50% - 12em);
        height: auto;
        max-height:calc(90% - 39px);
        border: solid 1px red;
        text-align: center;
        background: rgba(104, 48, 8, 0.85);
        color: #ffffff;
        overflow: hidden;
        overflow-y: scroll;
      /*  max-height: calc(90%);*/
        padding-bottom:20px;
      /* margin-bottom:20px;*/
">


        <table border="0" style="width:100%;padding-bottom:20px;">
            <caption>
                <div style="">
                    详情
                </div>
            </caption>
            <tbody>
                <tr style="border:solid 1px white">
                    <th>${obj.OnLineStr}</th>
                    <td></td>
                    <th>资本</th>
                    <td>${obj.Money}</td>
                </tr>
                <tr style="border:solid 1px white">
                    <td>${obj.Name}</td>
                    <td>(${obj.Level}级)</td>
                    <th></th>
                    <td>${obj.Relation}</td>
                </tr>
                <tr style="border:solid 1px white">

                    <td colspan="4">
                        <div style="float:right;padding-right:20px;">
                            <div>
                                <div>
                                    <img src="${obj.Driver < 1 ? '' : ('Pic/driverimage/' + obj.Driver + '.jpg')}" />
                                </div>
                                <div>
                                    司机:
                                    <span>
                                        ${obj.Driver < 1 ? '无' : obj.DriverName}
                                    </span>
                                </div>
                            </div>
                        </div>
                    </td>

                </tr> 
                <tr style="background:#900000;">
                    <th>红宝石数量</th>
                    <td> ${obj.MileCount[0]}</td>
                    <th>航程</th>
                    <td>${obj.Mile}</td>
                </tr>
                <tr style="background:#009000;">
                    <th>绿宝石数量</th>
                    <td>${obj.BusinessCount[0]}</td>
                    <th>能力</th>
                    <td>${obj.Business}</td>
                </tr>
                <tr style="background:#000090;">
                    <th>蓝宝石数量</th>
                    <td>${obj.VolumeCount[0]}</td>
                    <th>套路</th>
                    <td>${obj.Volume}</td>
                </tr>
                <tr style="background:#000000;">
                    <th>黑宝石数量</th>
                    <td>${obj.SpeedCount[0]}</td>
                    <th>速度</th>
                    <td>${obj.Speed}</td>
                </tr>
                <tr>
                    <th>老大</th>
                    <td colspan="3">${obj.BossName}</td>
                </tr>
                <tr>
                    <th>资助地址</th> 
                </tr>
                <tr> 
                    <td colspan="4" style="word-wrap:anywhere;word-break:break-all;">${obj.BTCAddr}</td>
                </tr>
            </tbody> 
        </table>
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="resistance.cancle();resistance.bindData2('${obj.KeyLookfor}');">
            抗性
        </div>
<div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="">
            认作老大
        </div>
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="resistance.cancle();">
            取消
        </div>
    </div>`;
        if (document.getElementById(that.operateID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(html);
            frag.id = that.operateID;
            document.body.appendChild(frag);
            //  that.bindData();
        }
        else {
            document.getElementById(that.operateID).remove();
            that.display(obj);
        }

    },
    operate2ID: 'resistancePanel2',
    display2: function (obj) {
        var that = resistance;

        var raceContent = '';
        switch (obj.race) {
            case 0: { raceContent = ''; }; break;
            case 1:
                {
                    raceContent = ` <tr style="border:solid 1px white">
                    <th colspan="4">效果</th>
                    <th colspan="4">几率</th>
                    <th colspan="4"></th>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">招募成功</th>
                    <td colspan="4">${obj.recruit}%</td>
                    <td colspan="4">(${obj.buildingReward[0]})</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">忽视抗卷</th>
                    <td colspan="4">${obj.ignorePhysics}%</td>
                    <td colspan="4">(${obj.buildingReward[1]}%)</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">强化速法几率</th>
                    <td colspan="4">${obj.SpeedImprove}%</td>
                    <td colspan="4">(${obj.buildingReward[2]}%)</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">强化防法几率</th>
                    <td colspan="4">${obj.DefenseImprove}%</td>
                    <td colspan="4">(${obj.buildingReward[3]}%)</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">强化攻发几率</th>
                    <td colspan="4">${obj.AttackImprove}%</td>
                    <td colspan="4">(${obj.buildingReward[4]}%)</td>
                </tr>`;
                }; break;
            case 2: {
                raceContent = ` <tr style="border:solid 1px white">
                    <th colspan="4">效果</th>
                    <th colspan="4">几率</th>
                    <th colspan="4"></th>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">招募成功</th>
                    <td colspan="4">${obj.recruit}%</td>
                    <td colspan="4">(${obj.buildingReward[0]})</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">故技重施几率</th>
                    <td colspan="4">${obj.controlImprove}%</td>
                    <td colspan="4">(${obj.buildingReward[1]}%)</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">忽视混乱20点</th>
                    <td colspan="4">${obj.ignoreConfuse}%</td>
                    <td colspan="4">(${obj.buildingReward[2]}%)</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">忽视迷失20点</th>
                    <td colspan="4">${obj.ignoreLose}%</td>
                    <td colspan="4">(${obj.buildingReward[3]}%)</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">忽视潜伏20点</th>
                    <td colspan="4">${obj.ignoreAmbush}%</td>
                    <td colspan="4">(${obj.buildingReward[4]}%)</td>
                </tr>`;
            }; break;
            case 3:
                {
                    raceContent = ` <tr style="border:solid 1px white">
                    <th colspan="4">效果</th>
                    <th colspan="4">几率</th>
                    <th colspan="4"></th>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">招募成功</th>
                    <td colspan="4">${obj.recruit}%</td>
                    <td colspan="4">(${obj.buildingReward[0]})</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">法术狂暴</th>
                    <td colspan="4">${obj.magicViolent}%</td>
                    <td colspan="4">(${obj.buildingReward[1]}%)</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">忽视雷抗20点</th>
                    <td colspan="4">${obj.ignoreElectic}%</td>
                    <td colspan="4">(${obj.buildingReward[2]}%)</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">忽视火抗20点</th>
                    <td colspan="4">${obj.ignoreFire}%</td>
                    <td colspan="4">(${obj.buildingReward[3]}%)</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="4">忽视水抗20点</th>
                    <td colspan="4">${obj.ignoreOfWater}%</td>
                    <td colspan="4">(${obj.buildingReward[4]}%)</td>
                </tr>`;
                }; break;
        }

        var html = `  <div id="${that.operate2ID}" style="position: absolute;
        z-index: 8;
        top: calc(10% - 1px);
        width: 24em;
        left: calc(50% - 12em);
        height: auto;
        max-height:calc(90% - 39px);
        border: solid 1px red;
        text-align: center;
        background: rgba(104, 48, 8, 0.85);
        color: #83ffff;
        overflow: hidden;
        overflow-y: scroll;
      /*  max-height: calc(90%);*/
        padding-bottom:20px;
      /* margin-bottom:20px;*/
">


        <table style="width:100%;">
            <caption>
                <div style="">
                    属性
                </div>
            </caption>
            <tbody> 
                <tr style="border:solid 1px white">
                    <th colspan="3">抗雷</th>
                    <td colspan="3">${obj.defensiveOfElectic}</td>
                    <th colspan="3">抗混</th>
                    <td colspan="3">${obj.defensiveOfConfuse}</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="3">抗水</th>
                    <td colspan="3">${obj.defensiveOfWater}</td>
                    <th colspan="3">抗迷</th>
                    <td colspan="3">${obj.defensiveOfLose}</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="3">抗火</th>
                    <td colspan="3">${obj.defensiveOfFire}</td>
                    <th colspan="3">抗潜</th>
                    <td colspan="3">${obj.defensiveOfAmbush}</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="3">抗内卷</th>
                    <td colspan="3">${obj.defensiveOfPhysics}</td>
                    <th colspan="3"></th>
                    <td colspan="3"></td>
                </tr>
                 ${raceContent}
            </tbody> 
        </table> 
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="resistance.cancle2();resistance.bindData('${obj.KeyLookfor}');">
            确认
        </div>
    </div>`;
        if (document.getElementById(that.operate2ID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(html);
            frag.id = that.operate2ID;
            document.body.appendChild(frag);
            // that.bindData();
        }
        else {
            document.getElementById(that.operate2ID).remove();
            that.display(obj);
        }

    },
    cancle2: function () {
        var that = resistance;
        if (document.getElementById(that.operate2ID) == null) {
        }
        else {
            document.getElementById(that.operate2ID).remove();
        }
    },
    cancle: function () {
        var that = resistance;
        if (document.getElementById(that.operateID) == null) {
        }
        else {
            document.getElementById(that.operateID).remove();
        }
    }
};