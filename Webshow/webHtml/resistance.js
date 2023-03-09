var resistance =
{
    operateID: 'resistancePanel',

    bindData: function (key) {
        var that = resistance;
        that.positionF(key);
        //objMain.othersBasePoint['0703abae622a0ec9cff4b3f85f8de7ff'].basePoint

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
        var bgmStr = 'rgba(104, 48, 8, 0.85)';
        switch (obj.Relation) {
            case '自己':
                {
                    bgmStr = 'rgba(48, 104, 8, 0.85)';
                }; break;
            case 'NPC':
                {
                    bgmStr = 'rgba(104, 48, 8, 0.85)';
                }; break;
            case '玩家':
                {
                    bgmStr = 'rgba(8, 48, 104, 0.85)';
                }; break;
            case '老大':
                {
                    bgmStr = 'rgba(76, 76, 8, 0.85)';
                }; break;
            case '队友':
                {
                    bgmStr = 'rgba(8, 76, 76, 0.85)';
                }; break;
            default:
                {
                    bgmStr = 'rgba(104, 48, 8, 0.85)';
                }
        };

        var html = ` <div id="${that.operateID}" style="position: absolute;
        z-index: 8;
        top: calc(10% - 1px);
        width: 24em;
        left: calc(50% - 12em);
        height: auto;
        max-height:calc(90% - 39px);
        border: solid 1px red;
        text-align: center;
        background: ${bgmStr};
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
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;" onclick="resistance.cancle();resistance.bindData2('${obj.KeyLookfor}');">
            抗性
        </div>
<div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;" onclick="dialogSys.toBeMyBoss('${obj.KeyLookfor}');resistance.cancle();">
            认作老大
        </div>  
        <div style="background: orange;
        margin-bottom: 0.25em;
        margin-top: 0.25em;padding:0.5em 0 0.5em 0;" onclick="resistance.cancle();">
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
            case 0: {
                raceContent = `<tr style="border:solid 1px white">
                    <th colspan="6">效果</th>
                    <th colspan="6">几率</th> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">招募成功率</th>
                    <td colspan="6">${obj.recruit}%</td> 
                </tr>`;
            }; break;
            case 1:
                {
                    raceContent = `<tr style="border:solid 1px white">
                    <th colspan="6">效果</th>
                    <th colspan="6">几率</th> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">招募成功率</th>
                    <td colspan="6">${obj.recruit}%</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">忽视抗物理${obj.ignorePhysicsValue}点</th>
                    <td colspan="6">${obj.ignorePhysics}%</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">提速法术提升${100 + obj.SpeedImproveValue}%</th>
                    <td colspan="6">${obj.SpeedImproveProbability}%</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">加防法术提升${100 + obj.DefenseImproveValue}%</th>
                    <td colspan="6">${obj.DefenseImproveProbability}%</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">红牛法术提升${100 + obj.AttackImproveValue}%</th>
                    <td colspan="6">${obj.AttackImproveProbability}%</td> 
                </tr>`;
                }; break;
            case 2: {
                raceContent = ` <tr style="border:solid 1px white">
                    <th colspan="6">效果</th>
                    <th colspan="6">几率</th> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">招募成功率</th>
                    <td colspan="6">${obj.recruit}%</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">故技重施</th>
                    <td colspan="6">${obj.controlImprove}%</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">忽视抗混乱</th>
                    <td colspan="6">${obj.ignoreConfuse}点</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">忽视抗迷失</th>
                    <td colspan="6">${obj.ignoreLose}点</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">忽视抗潜伏</th>
                    <td colspan="6">${obj.ignoreAmbush}点</td> 
                </tr>`;
            }; break;
            case 3:
                {
                    raceContent = ` <tr style="border:solid 1px white">
                     <th colspan="6">效果</th>
                    <th colspan="6">几率</th> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">招募成功率</th>
                    <td colspan="6">${obj.recruit}%</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">法术狂暴${100 + obj.magicViolentValue}%</th>
                    <td colspan="6">${obj.magicViolentProbability}%</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">忽视雷抗</th>
                    <td colspan="6">${obj.IgnoreElectricMagicValue}点</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">忽视火抗</th>
                    <td colspan="6">${obj.IgnoreFireMagicValue}点</td> 
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="6">忽视水抗</th>
                    <td colspan="6">${obj.IgnoreWaterMagicValue}点</td> 
                </tr>`;
                }; break;
        }
        var constrolState = '否';
        var controlsColor = 'green';
        var controlValue = '';
        if (obj.LoseValue > 0) {
            constrolState = '被迷惑';
            controlsColor = '#FFB5B5';
            controlValue = obj.LoseValue + '';
        }
        else if (obj.ConfuseValue > 0) {
            constrolState = '处于混乱';
            controlsColor = '#D9B3B3';
            controlValue = obj.ConfuseValue + '';
        }
        else {
            constrolState = '否';
            controlsColor = 'green';
        }
        var plus = '';
        if (obj.DefenceValue > 0) {
            plus = '+';
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
                    <td colspan="3">${obj.defensiveOfElectic}${plus}${plus == '' ? '' : obj.DefenceAttackMagicAdd}</td>
                    <th colspan="3">抗混</th>
                    <td colspan="3">${obj.defensiveOfConfuse}${plus}${plus == '' ? '' : obj.ConfusePropertyByDefendMagic}</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="3">抗水</th>
                    <td colspan="3">${obj.defensiveOfWater}${plus}${plus == '' ? '' : obj.DefenceAttackMagicAdd}</td>
                    <th colspan="3">抗迷</th>
                    <td colspan="3">${obj.defensiveOfLose}${plus}${plus == '' ? '' : obj.LostPropertyByDefendMagic}</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="3">抗火</th>
                    <td colspan="3">${obj.defensiveOfFire}${plus}${plus == '' ? '' : obj.DefenceAttackMagicAdd}</td>
                    <th colspan="3">抗潜伏</th>
                    <td colspan="3">${obj.defensiveOfAmbush}${plus}${plus == '' ? '' : obj.AmbushPropertyByDefendMagic}</td>
                </tr>
                <tr style="border:solid 1px white">
                    <th colspan="3">抗物理</th>
                    <td colspan="3">${obj.defensiveOfPhysics}${plus}${plus == '' ? '' : obj.DefencePhysicsAdd}</td>
                    <th colspan="3"></th>
                    <td colspan="3"></td>
                </tr>
                 ${raceContent}
                <tr style="border:solid 1px white;color:yellow;">
                    <th colspan="3">加防</th>
                    <td colspan="3">${obj.DefenceValue}</td>
                    <th colspan="3">提速</th>
                    <td colspan="3">${obj.SpeedValue}</td>
                </tr> 
                <tr style="border:solid 1px white;color:yellow;">
                    <th colspan="3">红牛</th>
                    <td colspan="3">${obj.AttackValue}</td>
                    <th colspan="3"></th>
                    <td colspan="3"></td>
                </tr>
                <tr style="border:solid 1px white;color:${controlsColor};">
                    <th colspan="3">被控制</th>
                    <td colspan="3">${constrolState}</td>
                    <th colspan="3">${controlValue}</th>
                    <td colspan="3"></td>
                </tr>
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
    },
    positionF: function (key) {
        var selectObj = objMain.playerGroup.getChildByName('flag_' + key);
        if (selectObj != null && selectObj != undefined) {
            var animationData =
            {
                old: {
                    x: objMain.controls.target.x,
                    y: objMain.controls.target.y,
                    z: objMain.controls.target.z,
                    t: Date.now()
                },
                newT:
                {
                    x: selectObj.position.x,
                    y: selectObj.position.y,
                    z: selectObj.position.z,
                    t: Date.now() + 3000
                }
            };
            objMain.heightLevel = selectObj.position.y;
            objMain.camaraAnimateData = animationData;
        }
    }
}