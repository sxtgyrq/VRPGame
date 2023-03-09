var selectSingleTeamJoinHtmlF =
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
            <div class="animateTextBottom" onclick="buttonClick('HelpAndGuide')">
<a style="text-decoration:underline;">攻略与介绍！</a>
            </div>
        </div>`;
        html = `<div style="width:100%;height:100%;background-color:cadetblue;">
            <div style="width:10000px;overflow:hidden;">
                <img src="bg/t2.jpg" />
                <img src="bg/t3.jpg" />
                <img src="bg/t4.jpg" />
                <img src="bg/t5.png" />
                <img src="bg/t3.jpg" />
                <img src="bg/t4.jpg" />
                <img src="bg/t2.jpg" />
                <img src="bg/t3.jpg" />
                <img src="bg/t4.jpg" />
            </div>
            <div style="width:calc(100% + 100px);">
                <img src="Pic/driverimage/105.jpg" />
                <img src="Pic/driverimage/106.jpg" />
                <img src="Pic/driverimage/107.jpg" />
                <img src="Pic/driverimage/108.jpg" />
                <img src="Pic/driverimage/109.jpg" />
                <img src="Pic/driverimage/110.jpg" />
                <img src="Pic/driverimage/111.jpg" />
                <img src="Pic/driverimage/112.jpg" />
                <img src="Pic/driverimage/113.jpg" />
                <img src="Pic/driverimage/114.jpg" />
                <img src="Pic/driverimage/115.jpg" />
                <img src="Pic/driverimage/116.jpg" />
                <img src="Pic/driverimage/117.jpg" />
                <img src="Pic/driverimage/118.jpg" />
                <img src="Pic/driverimage/119.jpg" />
                <img src="Pic/driverimage/120.jpg" />
                <img src="Pic/driverimage/121.jpg" />
                <img src="Pic/driverimage/123.jpg" />
                <img src="Pic/driverimage/124.jpg" />
                <img src="Pic/driverimage/125.jpg" />
                <img src="Pic/driverimage/126.jpg" />
                <img src="Pic/driverimage/127.jpg" />
                <img src="Pic/driverimage/128.jpg" />
                <img src="Pic/driverimage/129.jpg" />
                <img src="Pic/driverimage/130.jpg" />
                <img src="Pic/driverimage/131.jpg" />
                <img src="Pic/driverimage/132.jpg" />
                <img src="Pic/driverimage/133.jpg" />
                <img src="Pic/driverimage/134.jpg" />
                <img src="Pic/driverimage/135.jpg" />
                <img src="Pic/driverimage/136.jpg" />
                <img src="Pic/driverimage/137.jpg" />
                <img src="Pic/driverimage/139.jpg" />
                <img src="Pic/driverimage/140.jpg" />
                <img src="Pic/driverimage/141.jpg" />
                <img src="Pic/driverimage/142.jpg" />
                <img src="Pic/driverimage/144.jpg" />
                <img src="Pic/driverimage/146.jpg" />
                <img src="Pic/driverimage/148.jpg" />
                <img src="Pic/driverimage/149.jpg" />
                <img src="Pic/driverimage/150.jpg" />
                <img src="Pic/driverimage/152.jpg" />
                <img src="Pic/driverimage/153.jpg" />
                <img src="Pic/driverimage/154.jpg" />
                <img src="Pic/driverimage/155.jpg" />
                <img src="Pic/driverimage/156.jpg" />
                <img src="Pic/driverimage/157.jpg" />
                <img src="Pic/driverimage/162.jpg" />
                <img src="Pic/driverimage/163.jpg" />
                <img src="Pic/driverimage/166.jpg" />
                <img src="Pic/driverimage/167.jpg" />
                <img src="Pic/driverimage/168.jpg" />
                <img src="Pic/driverimage/169.jpg" />
                <img src="Pic/driverimage/170.jpg" />
                <img src="Pic/driverimage/171.jpg" />
                <img src="Pic/driverimage/173.jpg" />
                <img src="Pic/driverimage/174.jpg" />
                <img src="Pic/driverimage/175.jpg" />
                <img src="Pic/driverimage/176.jpg" />
                <img src="Pic/driverimage/177.jpg" />
                <img src="Pic/driverimage/178.jpg" />
                <img src="Pic/driverimage/179.jpg" />
                <img src="Pic/driverimage/180.jpg" />
                <img src="Pic/driverimage/180.jpg" />
                <img src="Pic/driverimage/182.jpg" />
                <img src="Pic/driverimage/183.jpg" />
                <img src="Pic/driverimage/184.jpg" />
                <img src="Pic/driverimage/186.jpg" />
                <img src="Pic/driverimage/187.jpg" />
                <img src="Pic/driverimage/189.jpg" />
                <img src="Pic/driverimage/190.jpg" />
                <img src="Pic/driverimage/191.jpg" />
                <img src="Pic/driverimage/192.jpg" />
                <img src="Pic/driverimage/193.jpg" />
                <img src="Pic/driverimage/194.jpg" />
                <img src="Pic/driverimage/195.jpg" />
            </div>
            <div style="width:100%;height:100%;position:absolute;left:0px;top:0px;background-color:#2e418a40;">
                
            </div>
            <div>
                <img src="bg/t1.png" style="max-width:22em; width: min(calc(100% - 18em - 2px),calc(61.8%)); bottom: 2px; position: absolute;" />
            </div>

            <div style="width:18em;max-width:18em;height:100%;left:min(calc(100% - 18em - 2px),calc(61.8%));top:0px;right:2px;position: absolute ;">
                <div>
                    <div class="animateOfBlockBtn delay3" style="width: calc(100% - 22px); height: calc((100% - 80px - 1em)/3); border: solid 1px green; left: 10px; top: calc(20px); position: absolute; text-align: center; overflow: hidden; border-top-left-radius: 2em; border-top-right-radius: 2em;" onclick="buttonClick('single')">
                        <div class="carRaceLine carLine1MainPage"></div>
                        <div class="carRaceLine carLine2MainPage"></div>
                        <div class="carRaceLine carLine3MainPage"></div>
                        <!--<div class="animateCar"></div>-->
                        <div class="animateCarToCorner segmentdelay1"></div>
                        <span style="top: calc(50% - 0.5em); left: calc(50% - 1em); position: absolute;">
                            开始
                        </span>
                    </div>
                    <div class="animateOfBlockBtn delay2" style="width: calc((100% - 30px)/2); height: calc((100% - 80px - 1em)/3); border: solid 1px green; left: 10px; top: calc((100% + 40px - 1em)/3); position: absolute; text-align: center; line-height: 100%; overflow: hidden; " onclick="buttonClick('setName')">
                        <div class="carRaceLine carLine1MainPage"></div>
                        <div class="animateCarToTop segmentdelay2"></div>
                        <span style="top: calc(50% - 0.5em); left: calc(50% - 3em); position: absolute;">
                            查看修改昵称
                        </span>
                    </div>
                    <div class="animateOfBlockBtn delay4" style="width: calc((100% - 34px)/2); height: calc((100% - 80px - 1em)/3); border: solid 1px green; left: calc((100% - 30px)/2 + 20px); top: calc((100% + 40px - 1em)/3); position: absolute; text-align: center; line-height: 100%; overflow: hidden; " onclick="buttonClick('QueryReward')">
                        <div class="carRaceLine carLine3MainPage"></div>
                        <div class="animateCarToBottom segmentdelay3"></div>
                        <span style="top: calc(50% - 0.5em); left: calc(50% - 1em); position: absolute;">
                            荣誉
                        </span>
                    </div>
                    <div class="animateOfBlockBtn delay1" style="width: calc((100% - 30px)/2); height: calc((100% - 80px - 1em)/3); border: solid 1px green; left: 10px; top: calc((100% + 10px - 1em)/3 * 2); text-align: center; position: absolute; overflow: hidden; border-bottom-left-radius: 2em; " onclick="buttonClick('team')">
                        <div class="carRaceLine carLine1MainPage"></div>
                        <div class="animateCarToTop segmentdelay1"></div>
                        <span style="top: calc(50% - 0.5em); left: calc(50% - 1em); position: absolute;">
                            组队
                        </span>
                    </div>
                    <div class="animateOfBlockBtn delay5" style="width: calc((100% - 30px)/2); height: calc((100% - 80px - 1em)/3); border: solid 1px green; left: calc((100% - 34px)/2 + 20px); top: calc((100% + 10px - 1em)/3 * 2); position: absolute; overflow: hidden; border-bottom-right-radius: 2em; " onclick="buttonClick('join')">
                        <div class="carRaceLine carLine3MainPage"></div>
                        <div class="animateCarToBottom segmentdelay4"></div>
                        <span style="top: calc(50% - 0.5em); left: calc(50% - 1em); position: absolute;">
                            加入
                        </span>
                    </div>
                    <div class="animateTextBottom" onclick="buttonClick('HelpAndGuide')">
                        <a style="text-decoration:underline;">攻略与介绍！</a>
                    </div>
                </div>
            </div>
        </div>`;
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
        var reg = /^[\u4e00-\u9fa5]{1}[a-zA-Z0-9\u4e00-\u9fa5]{1,8}$/;
        if (reg.test(name)) {
            objMain.ws.send(JSON.stringify({ c: 'SetPlayerName', 'Name': name }));
            objMain.ws.send(JSON.stringify({ c: 'GetName' }));
            if (document.getElementById(that.setNameHtmlID) != null) {
                document.getElementById(that.setNameHtmlID).remove();
                objMain.receivedState = 'selectSingleTeamJoin';
            }
        }
        else {
            $.notify(`名字需以汉字开头,
连数字与字母
长度不超过9个字符`,
                {
                    autoHide: true,
                    className: 'info',
                    autoHideDelay: 20000,
                    position: 'left',
                });
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