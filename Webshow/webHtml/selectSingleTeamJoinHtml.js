﻿var selectSingleTeamJoinHtmlF =
{
    drawHtml: function () {
        var html = ` <div>
            <div class="animateOfBlockBtn delay3" style="width: calc(100% - 22px); height: calc((100% - 80px - 1em)/3); border: solid 1px green; left: 10px; top: calc(20px); position: absolute; text-align: center;overflow:hidden;" onclick="buttonClick('single')">
                <div class="carRaceLine carLine1MainPage"></div>
                <div class="carRaceLine carLine2MainPage"></div>
                <div class="carRaceLine carLine3MainPage"></div>
                <!--<div class="animateCar"></div>-->
                <div class="animateCarToCorner segmentdelay1"></div>
                <span style="top: calc(50% - 0.5em); left: calc(50% - 1em); position: absolute;">
                    开始
                </span>
            </div>
            <div class="animateOfBlockBtn delay2" style="width: calc((100% - 30px)/2);
        height: calc((100% - 80px - 1em)/3);
        border: solid 1px green;
        left: 10px;
        top: calc((100% + 40px - 1em)/3);
        position: absolute;
        text-align: center;
        line-height: 100%;
        overflow:hidden;
    " onclick="buttonClick('setName')">
                <div class="carRaceLine carLine1MainPage"></div>
                <div class="animateCarToTop segmentdelay2"></div>
                <span style="top: calc(50% - 0.5em); left: calc(50% - 3em); position: absolute;">
                    查看修改昵称
                </span>
            </div>
            <div class="animateOfBlockBtn delay4" style="width: calc((100% - 34px)/2);
        height: calc((100% - 80px - 1em)/3);
        border: solid 1px green;
        left: calc((100% - 30px)/2 + 20px);
        top: calc((100% + 40px - 1em)/3);
        position: absolute;
        text-align: center;
        line-height:100%;
        overflow:hidden;
    " onclick="buttonClick('QueryReward')">
                <div class="carRaceLine carLine3MainPage"></div>
                <div class="animateCarToBottom segmentdelay3"></div>
                <span style="top: calc(50% - 0.5em); left: calc(50% - 1em); position: absolute;">
                    荣誉
                </span>
            </div>
            <div class="animateOfBlockBtn delay1" style="width: calc((100% - 30px)/2); height: calc((100% - 80px - 1em)/3); border: solid 1px green; left: 10px; top: calc((100% + 10px - 1em)/3 * 2); text-align: center; position: absolute; overflow:hidden;" onclick="buttonClick('team')">
                <div class="carRaceLine carLine1MainPage"></div>
                <div class="animateCarToTop segmentdelay1"></div>
                <span style="top: calc(50% - 0.5em); left: calc(50% - 1em); position: absolute;">
                    组队
                </span>
            </div>
            <div class="animateOfBlockBtn delay5" style="width: calc((100% - 30px)/2);
        height: calc((100% - 80px - 1em)/3);
        border: solid 1px green;
        left: calc((100% - 34px)/2 + 20px);
        top: calc((100% + 10px - 1em)/3 * 2);
        position: absolute;
        overflow:hidden;
        " onclick="buttonClick('join')">
                <div class="carRaceLine carLine3MainPage"></div>
                <div class="animateCarToBottom segmentdelay4"></div>
                <span style="top: calc(50% - 0.5em); left: calc(50% - 1em); position: absolute;">
                    加入
                </span>
            </div>
            <div class="animateTextBottom" onclick="buttonClick('help')">
                <span>要瑞卿的粗糙作品，多多指教</span>
            </div>
        </div>`;

//        var html = ` <div>
//            <div style="width:calc(100% - 22px);height:calc((100% - 80px - 1em)/3);border:solid 1px green;left:10px;top:calc(20px);position:absolute;
//        background: url('Pic/mapty.jpg');
//        background-repeat: no-repeat;
//        background-size: auto;
//        text-shadow:0 0.1em 0.1em red;
//        text-align: center;
//        color:wheat;" onclick="buttonClick('single')">
//                <span style="top:calc(50% - 0.5em);position:relative;">
//                    开始
//                </span> </div>
//            <div style="width: calc((100% - 30px)/2);
//         height: calc((100% - 80px - 1em)/3);
//        border: solid 1px green;
//        left: 10px;
//        top: calc((100% + 40px - 1em)/3);
//        position: absolute;
//        text-align: center;
//        line-height: 100%;
//        background: url('Pic/mapty.jpg');
//        background-repeat: no-repeat;
//        background-size: auto;
//        text-shadow:0 0.1em 0.1em red;
//        color:wheat;
//    " onclick="buttonClick('setName')">
//                <span style="top:calc(50% - 0.5em);position:relative;">
//                    查看/修改昵称
//                </span>
//            </div>`+

//            `
//        <div style="width: calc((100% - 34px)/2);
//        height: calc((100% - 80px - 1em)/3);
//        border: solid 1px green;
//        left: calc((100% - 30px)/2 + 20px);
//        top: calc((100% + 40px - 1em)/3);
//        position: absolute;
//        text-align: center;
//        line-height:100%;
//        background: url('Pic/mapty.jpg');
//        background-repeat: no-repeat;
//        background-size: auto;
//        text-shadow:0 0.1em 0.1em red;
//        color:wheat;
//    " onclick="buttonClick('getRewad')">
//                <span style="top:calc(50% - 0.5em);position:relative;">
//                    领取奖励
//                </span>
//            </div>
//`+
//            //        <div style="        width: calc((100% - 34px)/2);
//            //    height: calc((100% - 80px - 1em)/3);
//            //    border: solid 1px green;
//            //    left: calc((100% - 30px)/2 + 20px);
//            //    top: calc((100% + 40px - 1em)/3);
//            //    position: absolute;
//            //    text-align: center;
//            //    line-height:100%;
//            //" onclick="buttonClick('setCarsName')">
//            //            <span style="top:calc(50% - 0.5em);position:relative;">
//            //                查看/修改车名
//            //            </span>
//            //        </div> 
//            `<div style="        width: calc((100% - 30px)/2);
//        height: calc((100% - 80px - 1em)/3);
//        border: solid 1px green;
//        left: 10px;
//        top: calc((100% + 10px - 1em)/3 * 2);
//        position: absolute;
//        background: url('Pic/mapty.jpg');
//        background-repeat: no-repeat;
//        background-size: auto;
//        text-align: center;
//        text-shadow:0 0.1em 0.1em red;
//        color:wheat;" onclick="buttonClick('team')">
//<span style="top:calc(50% - 0.5em);position:relative;">
//                    组队
//                </span>
//        </div>
//            <div style="        width: calc((100% - 30px)/2);
//        height: calc((100% - 80px - 1em)/3);
//        border: solid 1px green;
//        left: calc((100% - 34px)/2 + 20px);
//        top: calc((100% + 10px - 1em)/3 * 2);
//        position: absolute;
//        background: url('Pic/mapty.jpg');
//        background-repeat: no-repeat;
//        background-size: auto;
//        text-align: center;
//        text-shadow:0 0.1em 0.1em red;
//        color:wheat;" onclick="buttonClick('join')">
// <span style="top:calc(50% - 0.5em);position:relative;">
//                    加入
//                </span>
//        </div>
            
//            <div style="width:calc(100% - 22px);height:calc((100% - 80px - 1em)/3);left:10px;top:calc((100% - 20px - 1em)/3 * 3);position:absolute;text-align:right;" onclick="buttonClick('join')">
//                <span>要瑞卿的粗糙作品，多多指教</span> 
//            </div>
//        </div>`;
        return html;
    },
    setNameHtml: `<div style="width: 100%;
        height: 100%;
        background-color: rgba(56,60,67,.15);
        position: absolute;
        left: 0px;
        top: 0px;" id="setNameHtmlPanel">
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
       margin-top: 3.2rem; min-height: 300px;max-height:300px;width:80%;left:10%;
       position:relative;">
                <div> <label>请输入昵称:</label></div>
                <div> <textarea id="playerNameTextArea" style="width:98%;"></textarea></div>
                <div style="text-align:center;"><button id="setNameBtn" style="height:2em;">确认修改</button></div>
            </div>

        </div>`,
    setNameHtmlID: 'setNameHtmlPanel',
    setNameHtmlShow: function () {
        var that = selectSingleTeamJoinHtmlF;
        if (document.getElementById(that.setNameHtmlID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.setNameHtml);
            frag.id = that.setNameHtmlID;

            document.body.appendChild(frag);

            document.getElementById('setNameBtn').onclick = function () {
                //  alert('设置名字！');
                that.ChangeName();
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
    ChangeName: function () {
        var that = selectSingleTeamJoinHtmlF;
        var name = document.getElementById('playerNameTextArea').value;
        objMain.ws.send(JSON.stringify({ c: 'SetPlayerName', 'Name': name }));
        objMain.ws.send(JSON.stringify({ c: 'GetName' }));
        if (document.getElementById(that.setNameHtmlID) != null) {
            document.getElementById(that.setNameHtmlID).remove();
            objMain.receivedState = 'selectSingleTeamJoin';
        }
    },
    setCarsNameHtmlID: 'setCarsNameHtmlPanel',
    setCarsNameHtml: `<div style="width: 100%;
        height: 100%;
        background-color: rgba(56,60,67,.15);
        position: absolute;
        left: 0px;
        top: 0px;" id="setCarsNameHtmlPanel">
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
       margin-top: 3.2rem; min-height: 300px;max-height:300px;width:80%;left:10%;
       position:relative;">
                <div> <label>请输入车名</label></div>
                <div> <textarea id="car1NameTextArea" style="width:98%;max-width:8em;"></textarea></div>
                <div>
                    <textarea id="car2NameTextArea" style="        width: 98%;
        max-width: 8em;
"></textarea>
                </div>
                <div>
                    <textarea id="car3NameTextArea" style="        width: 98%;
        max-width: 8em;
"></textarea>
                </div>
                <div>
                    <textarea id="car4NameTextArea" style="        width: 98%;
        max-width: 8em;
"></textarea>
                </div>
                <div>
                    <textarea id="car5NameTextArea" style="        width: 98%;
        max-width: 8em;
"></textarea>
                </div>
                <div style="text-align:center;"><button  id="setCarsNameBtn"  style="height:2em;">确认修改</button></div>
            </div>

        </div>`,
    setCarsNameHtmlShow: function () {
        var that = selectSingleTeamJoinHtmlF;
        if (document.getElementById(that.setCarsNameHtmlID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.setCarsNameHtml);
            frag.id = that.setCarsNameHtmlID;

            document.body.appendChild(frag);

            document.getElementById('setCarsNameBtn').onclick = function () {
                //  alert('设置名字！');
                that.ChangeCarsName();
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
    ChangeCarsName: function () {
        var that = selectSingleTeamJoinHtmlF;
        var names = [];
        for (var i = 0; i < 5; i++) {
            var iName = 'car' + (i + 1) + 'NameTextArea';
            if (document.getElementById(iName) != undefined) {
                names.push(document.getElementById(iName).value);
            }
        }
        objMain.ws.send(JSON.stringify({ c: 'SetCarsName', 'Names': names }));
        objMain.ws.send(JSON.stringify({ c: 'GetCarsName' }));
        if (document.getElementById(that.setCarsNameHtmlID) != null) {
            document.getElementById(that.setCarsNameHtmlID).remove();
            objMain.receivedState = 'selectSingleTeamJoin';
        }
    }
}