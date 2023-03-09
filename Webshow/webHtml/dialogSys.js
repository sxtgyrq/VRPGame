var dialogSys =
{
    /*
     * 显示通信录；
     */
    showFriendsList: function () {

        while (document.getElementById('msgDialog') != null) {
            document.getElementById('msgDialog').remove();
        }
        if (document.getElementById('friendsList') != null) {
            document.getElementById('friendsList').remove();
        }
        if (document.getElementById('roleStatePanel') != null) {
            document.getElementById('roleStatePanel').remove();
        }
        {
            var friendsList = document.createElement('friendsList');

            friendsList.id = 'friendsList';
            friendsList.style.position = 'absolute';
            friendsList.style.zIndex = '11';
            friendsList.style.top = '5px';
            friendsList.style.left = '5px';
            friendsList.style.width = '12em';
            friendsList.style.height = 'calc(100% - 65px)';
            friendsList.style.border = 'solid 1px red';
            friendsList.style.textAlign = 'center';
            friendsList.style.background = 'rgba(104, 48, 8, 0.85)';
            friendsList.style.color = '#83ffff';
            friendsList.style.overflowY = 'scroll';

            for (var key in objMain.othersBasePoint) {
                var divItem = document.createElement('div');
                divItem.style.marginBottom = '0.25em';
                divItem.style.marginTop = '0.25em';
                divItem.innerText = objMain.othersBasePoint[key].playerName;


                divItem.CustomTag = objMain.othersBasePoint[key];
                divItem.indexKey = key;
                divItem.id = 'contact_' + key;
                if (this.msgs[key] != undefined) {
                    if (this.msgs[key].read.length > this.msgs[key].readLength) {
                        divItem.classList.add('friendMsg');
                    }
                }
                var that = this;
                divItem.onclick = function () {
                    //   alert(this.CustomTag);
                    that.showDialog(this.CustomTag);
                }

                friendsList.appendChild(divItem);
                //  console.log(key, objMain.othersBasePoint[key])
            }
            document.body.appendChild(friendsList);

            this.iconAlart();
            this.updatePanel();
        }
        this.dealWithRoleState();

        var obj = { 'c': 'GetOnLineState' };
        objMain.ws.send(JSON.stringify(obj));
        //document.getElementById('friendsList')
    },
    show: function () {
        if (document.getElementById('friendsList') != null) {
            document.getElementById('friendsList').remove();
            if (document.getElementById('roleStatePanel') != null) {
                document.getElementById('roleStatePanel').remove();
            }
            this.AlertNewTask();
            return;
        }
        this.cancleTask();
        var count = 0;
        var selectKey = '';
        for (var item in this.msgs) {
            if (this.msgs[item].read.length > this.msgs[item].readLength) {
                count++;
                if (this.msgs[item].read[0].Key == objMain.indexKey) {
                    selectKey = this.msgs[item].read[0].To;
                }
                else {
                    selectKey = this.msgs[item].read[0].Key;
                }
            }
        }
        if (count == 0) {
            //看情况打开!
            if (document.getElementById('friendsList') == null) {
                this.showFriendsList();
            }
        }
        else if (count == 1) {
            if (selectKey != '')
                if (objMain.othersBasePoint[selectKey] != undefined) {

                    this.showDialog(objMain.othersBasePoint[selectKey]);
                }
                else {
                    this.showFriendsList();
                }
            else this.showFriendsList();
            //alert(selectData.notRead[0].Key);
            //if (objMain.othersBasePoint[selectData.notRead[0].Key] != undefined) {

            //    this.showDialog(objMain.othersBasePoint[selectData.notRead[0].Key]);
            //}
            //else {
            //    this.showFriendsList();

        }
        else if (count > 1) {
            this.showFriendsList();
        }

    },
    showDialog: function (data) {

        if (document.getElementById('friendsList') != null) {
            document.getElementById('friendsList').remove();
        }
        while (document.getElementById('msgDialog') != null) {
            document.getElementById('msgDialog').remove();
        }
        if (document.getElementById('roleStatePanel') != null) {
            document.getElementById('roleStatePanel').remove();
        }
        var dialog = document.createElement('div');
        dialog.id = 'msgDialog';
        dialog.indexKey = data.indexKey;

        resistance.positionF(data.indexKey);
        //  dialog.classList.add
        dialog.classList.add('dialog');
        dialog.classList.add('show');

        var dialogPanel1 = document.createElement('div');
        dialogPanel1.className = 'dialogPanel1';

        {
            var header = document.createElement('header');
            header.id = 'panelHeader-5';
            header.className = 'dialogPanelheader';

            var btn = document.createElement('button');
            btn.id = 'panelLeftButtonGoBack';
            btn.className = 'dialogPanelLeftButton';

            var span = document.createElement('span');
            span.className = 'dialogBtn_text';
            span.innerText = '返回';
            btn.appendChild(span);
            var that = this;
            btn.onclick = function () {
                while (document.getElementById('msgDialog') != null) {
                    document.getElementById('msgDialog').remove();
                }
                that.iconAlart();
            };

            var h1 = document.createElement('h1');
            h1.id = 'panelTitleDialogTo';
            h1.className = 'dialogPanelTitleName';
            h1.style.paddingLeft = '2em';
            h1.innerText = data.playerName;

            header.appendChild(btn);
            header.appendChild(h1);

            dialogPanel1.appendChild(header);
        }

        //for (var i = 0; i < this.msgs[data.indexKey].read.length; i++) {
        //    this.msgs[data.indexKey].read.push(this.msgs[data.indexKey].notRead[i]);
        //}
        // this.msgs[data.indexKey].notRead = [];
        {
            var panelBodyShowMsg = document.createElement('div');
            panelBodyShowMsg.className = 'dialogPanel_body';
            panelBodyShowMsg.id = 'panelBodyShowMsg';
            dialogPanel1.appendChild(panelBodyShowMsg);
        }

        {
            var panelFooter = document.createElement('footer');
            panelFooter.className = 'dialogChat_toolbar_footer';

            var ul = document.createElement('ul');
            {
                var li1 = document.createElement('li');
                li1.classList.add('dialogSession');
                li1.classList.add('left');
                li1.id = 'btnOfContactList';
                var a = document.createElement('a');
                a.style.marginTop = '5px';
                var span = document.createElement('span');
                span.style.display = 'block';
                span.style.bottom = '4px';
                span.style.width = '100%';
                span.style.fontSize = '28px';
                span.innerText = '✚';
                a.appendChild(span);
                li1.append(a);
                ul.appendChild(li1);
                var that = this;
                li1.onclick = function () {
                    that.showFriendsList();
                }
            }
            {
                var li2 = document.createElement('li');
                li2.classList.add('dialogSession');
                li2.classList.add('input');

                var textarea = document.createElement('textarea');
                textarea.id = 'dialogChatInput';
                textarea.className = 'dialogChat_textareaStyle';
                textarea.maxLength = '120';
                li2.append(textarea);
                ul.append(li2);
            }

            {
                var li3 = document.createElement('li');
                li3.CustomTag = data;
                li3.classList.add('dialogSession');
                li3.classList.add('right');

                var a = document.createElement('a');
                a.style.marginTop = '5px';

                var span = document.createElement('span');
                span.style.display = 'block';
                span.style.bottom = '4px';
                span.style.width = '100%';
                span.style.fontSize = '28px';
                span.innerText = '发送';
                a.appendChild(span);
                li3.appendChild(a);
                li3.Tag = data;
                li3.onclick = function () {
                    document.getElementById('dialogChatInput');
                    if (document.getElementById('dialogChatInput').value == '') {
                        alert('不要发送空信息！');
                    }
                    else {
                        var msg = document.getElementById('dialogChatInput').value;
                        //  alert(msg);
                        document.getElementById('dialogChatInput').value = '';
                        var obj = { 'c': 'Msg', 'MsgPass': msg, 'to': this.Tag.indexKey };

                        objMain.ws.send(JSON.stringify(obj));
                        //console.log('obj', obj);
                        //console.log('obj', this.Tag);
                    }
                }
                ul.appendChild(li3);
            }

            panelFooter.appendChild(ul);
            dialogPanel1.appendChild(panelFooter);
        }

        dialog.appendChild(dialogPanel1);
        console.log('tag', data);
        dialog.indexKey = data.indexKey;
        document.body.appendChild(dialog);


        if (this.msgs[data.indexKey] != undefined) {
            for (var i = 0; i < this.msgs[data.indexKey].read.length; i++) {
                if (this.msgs[data.indexKey].read[i].Key == objMain.indexKey) {
                    this.addMsgOfSelf(this.msgs[data.indexKey].read[i]);
                }
                else if (this.msgs[data.indexKey].read[i].To == objMain.indexKey) {
                    this.addMsgOfBuddy(this.msgs[data.indexKey].read[i]);
                }
                // this.msgs[data.indexKey].readLength = i + 1;
            }
            this.msgs[data.indexKey].readLength = this.msgs[data.indexKey].read.length;
        }

        this.iconAlart();
        //for (var i = 0; i < this.msgs[data.indexKey].read.length; i++) {

        //}
    },
    dealWithMsg: function (msg) {
        /*
         * 对应后台DialogMsg
         */
        console.log('msg', msg);
        if (objMain.indexKey == msg.Key) {
            if (this.msgs[msg.To] == undefined) {
                this.msgs[msg.To] = { 'read': [], 'readLength': 0 };
            }
            this.msgs[msg.To].read.push(msg);
        }
        else if (objMain.indexKey == msg.To) {

            if (this.msgs[msg.Key] == undefined) {
                this.msgs[msg.Key] = { 'read': [], 'readLength': 0 };
            }
            this.msgs[msg.Key].read.push(msg);
        }
        this.iconAlart();
        this.updatePanel();
    },
    msgs: {},
    addMsgOfSelf: function (msg) {
        var selfMsgDiv = document.createElement('div');
        selfMsgDiv.classList.add('dialogChat_content_group');
        selfMsgDiv.classList.add('self');

        var p1 = document.createElement('p');
        p1.classList.add('dialogChat_nick');
        var a = document.createElement('a');
        a.innerText = objMain.displayName;
        p1.appendChild(a);

        var p2 = document.createElement('p');
        p2.classList.add('chat_content');
        p2.innerText = msg.Msg;

        //  a.innerText = objMain.basePoint.;
        selfMsgDiv.appendChild(p1);
        selfMsgDiv.appendChild(p2);

        var panelBodyShowMsg = document.getElementById('panelBodyShowMsg');
        panelBodyShowMsg.appendChild(selfMsgDiv);
        panelBodyShowMsg.scrollTo(0, panelBodyShowMsg.scrollHeight);
    },
    addMsgOfBuddy: function (msg) {
        var buddyMsgDiv = document.createElement('div');
        buddyMsgDiv.classList.add('dialogChat_content_group');
        buddyMsgDiv.classList.add('buddy');

        var p1 = document.createElement('p');
        p1.classList.add('dialogChat_nick');
        var a = document.createElement('a');
        a.innerText = objMain.othersBasePoint[msg.Key].playerName;// objMain.displayName;
        p1.appendChild(a);

        var p2 = document.createElement('p');
        p2.classList.add('chat_content');
        p2.innerText = msg.Msg;

        //  a.innerText = objMain.basePoint.;
        buddyMsgDiv.appendChild(p1);
        buddyMsgDiv.appendChild(p2);

        var panelBodyShowMsg = document.getElementById('panelBodyShowMsg');
        panelBodyShowMsg.appendChild(buddyMsgDiv);

        panelBodyShowMsg.scrollTo(0, panelBodyShowMsg.scrollHeight);
        // var panelBodyShowMsg = document.getElementById('panelBodyShowMsg')
    },
    iconAlart: function () {
        if (document.getElementById('msgDialog') != null) {
            var keyOperate = document.getElementById('msgDialog').indexKey;

            for (var key in objMain.othersBasePoint) {
                if (key == keyOperate) {
                    continue;
                }
                else {
                    if (this.msgs[key] != undefined) {
                        if (this.msgs[key].read.length > this.msgs[key].readLength) {
                            if (!document.getElementById('btnOfContactList').classList.contains('friendMsg')) {
                                document.getElementById('btnOfContactList').classList.add('friendMsg');
                                break;
                            }
                        }
                    }
                }
            }
            // var panelBodyShowMsg = document.getElementById('panelBodyShowMsg');
            //if (this.msgs[keyOperate] != undefined)
            //    for (var i = panelBodyShowMsg.children.length; i < this.msgs[keyOperate].read.length; i++) {
            //        if (this.msgs[keyOperate].read[i].Key == objMain.indexKey) {
            //            this.addMsgOfSelf(this.msgs[keyOperate].read[i]);
            //        }
            //        else if (this.msgs[keyOperate].read[i].To == objMain.indexKey) {
            //            this.addMsgOfBuddy(this.msgs[keyOperate].read[i]);
            //        }
            //        this.msgs[keyOperate].readLength = panelBodyShowMsg.children.length;
            //    }
        }
        else {
            var needToNotifyFunctionBtn = false;
            for (var key in objMain.othersBasePoint) {
                if (this.msgs[key] != undefined) {

                    if (this.msgs[key].read.length > this.msgs[key].readLength) {
                        // divItem.classList.add('friendMsg');
                        needToNotifyFunctionBtn = true;
                        var idNeedToUpdate = 'contact_' + key;
                        if (document.getElementById(idNeedToUpdate) != null) {
                            if (!document.getElementById(idNeedToUpdate).classList.contains('friendMsg')) {
                                document.getElementById(idNeedToUpdate).classList.add('friendMsg');
                            }
                        }

                    }
                    else {
                        var idNeedToUpdate = 'contact_' + key;
                        if (document.getElementById(idNeedToUpdate) != null) {
                            if (document.getElementById(idNeedToUpdate).classList.contains('friendMsg')) {
                                document.getElementById(idNeedToUpdate).classList.remove('friendMsg');
                            }
                        }
                    }

                }
            }
            if (needToNotifyFunctionBtn) {
                SysOperatePanel.notifyMsg();
            }
            else {
                SysOperatePanel.cancelNotifyMsg();
            }

        }
    },
    updatePanel: function () {
        if (document.getElementById('msgDialog') != null) {
            var keyOperate = document.getElementById('msgDialog').indexKey;
            var panelBodyShowMsg = document.getElementById('panelBodyShowMsg');
            if (this.msgs[keyOperate] != undefined)
                for (var i = panelBodyShowMsg.children.length; i < this.msgs[keyOperate].read.length; i++) {
                    if (this.msgs[keyOperate].read[i].Key == objMain.indexKey) {
                        this.addMsgOfSelf(this.msgs[keyOperate].read[i]);
                    }
                    else if (this.msgs[keyOperate].read[i].To == objMain.indexKey) {
                        this.addMsgOfBuddy(this.msgs[keyOperate].read[i]);
                    }
                    this.msgs[keyOperate].readLength = panelBodyShowMsg.children.length;
                }
        }
        else if (document.getElementById('friendsList') != null) {
            /*
             * 这里好核对数据条数。
             */
        }

        // document.body.appendChild(friendsList);
    },
    toBeMyBoss: function (key) {
        var obj = { 'c': 'Msg', 'MsgPass': '认你做老大', 'to': key };
        objMain.ws.send(JSON.stringify(obj));
    },
    ShowFightSituation: function (data) {
        var Opponents = data.Opponents;
        var Parters = data.Parters;
        for (var i = 0; i < Parters.length; i++) {
            var model = objMain.ModelInput.Teammate.obj.clone();
            model.rotation.x = Math.PI;
            var flag = objMain.playerGroup.getObjectByName('flag_' + Parters[i]);
            model.position.set(flag.position.x, flag.position.y - 0.15, flag.position.z);
            objMain.fightSituationGroup.add(model);
        }
        for (var i = 0; i < Opponents.length; i++) {
            var model = objMain.ModelInput.Opponent.obj.clone();
            model.rotation.x = Math.PI;
            var flag = objMain.playerGroup.getObjectByName('flag_' + Opponents[i]);
            model.position.set(flag.position.x, flag.position.y - 0.15, flag.position.z);
            objMain.fightSituationGroup.add(model);
        }
        this.dealWithRoleState();
    },
    dealWithRoleState: function () {
        if (document.getElementById('friendsList') == null) {
            document.getElementById('roleStatePanel').remove();
            return;
        }
        var fightSituationBtnName = '';
        if (objMain.fightSituationGroup.children.length > 0) {
            fightSituationBtnName = '取消查看';
        }
        else {
            fightSituationBtnName = '查看态势';
        }
        var html = `<div id="roleStatePanel" style="position: absolute; z-index: 8; top: calc(100% - 60px - 5.25em); left: calc(12em + 20px); width: 6em; height: 5.25em; border: solid 1px red; text-align: center; background: rgba(104, 48, 8, 0.85); color: #83ffff;">
        <div id="taskShowBtnToDisplayPanel" style="background: yellowgreen; 
        margin-top: 0.25em;
        padding:0.5em 0 0.5em 0;" onclick="dialogSys.RefreshTaskCopy();">
            任务
        </div>
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;
        padding:0.5em 0 0.5em 0;" onclick="dialogSys.RefreshRoleState();">
            ${fightSituationBtnName}
        </div>
    </div>`;
        var frag = document.createRange().createContextualFragment(html);
        frag.id = 'roleStatePanel';
        document.body.appendChild(frag);
        var that = dialogSys;
        that.AlertNewTask();
    },
    RefreshRoleState: function () {
        while (document.getElementById('roleStatePanel') != null) {
            document.getElementById('roleStatePanel').remove();
        }
        if (objMain.fightSituationGroup.children.length > 0) {
            objMain.mainF.removeF.clearGroup(objMain.fightSituationGroup);
            this.dealWithRoleState();
        }
        else {
            objMain.ws.send(JSON.stringify({ 'c': 'GetFightSituation', 'Page': 0 }));
        }
    },
    RefreshTaskCopy: function () {
        objMain.stateNeedToChange.HasNewTask = false;
        this.AlertNewTask();
        objMain.ws.send(JSON.stringify({ 'c': 'GetTaskCopy' }));
        this.show();
    },
    drawPanelOfTaskCoyp: function (datas) {
        var s1 = '';
        if (datas.length == 0) {
            s1 = `<div style="border-bottom:dashed 1px #edcb8b45;margin-bottom: 2em;">
                <h1 style="font-size:x-large;">当前没有任务</h1> 
            </div>`;
        }
        else {
            for (var i = 0; i < datas.length; i += 3) {
                s1 += `<div style="border-bottom: dashed 1px #edcb8b45;margin-bottom: 2em;">
                <h1 style="font-size:x-large;">${datas[i + 1]}</h1>
                <b style="word-break:break-all;word-wrap:anywhere">${datas[i + 2]}</b>
                <button onclick="dialogSys.TaskCopyCancle('${datas[i]}',this);">取消</button>
            </div>`;
            }
        }
        var html = `<div id="taskCoypPanel" style="overflow-y: scroll; width: 80%; height: 80%; max-width: 30em; max-height: calc(100% - 10em); margin-left: auto; margin-right: auto; margin-top: 5em; border: dotted 2px blue; border-top-left-radius: 1em; color: greenyellow; background-color: #722732; opacity: 0.85; background-size: 74px 74px; background-image: repeating-linear-gradient(0deg, #852732, #852732 3.7px, #722732 3.7px, #722732);z-index:9;position:relative;">
<div style="float:right;margin-right:2em;margin-top:-2em;">
                <button style="position: fixed;" onclick="dialogSys.cancleTask();">×</button>
            </div>
             ${s1}
        </div>`;
        var frag = document.createRange().createContextualFragment(html);
        frag.id = 'taskCoypPanel';
        document.body.appendChild(frag);

    },
    cancleTask: function () {
        while (document.getElementById('taskCoypPanel') != null) {
            document.getElementById('taskCoypPanel').remove();
        }
    },
    TaskCopyCancle: function (code, ele) {
        if (confirm('确认取消任务？')) {
            ele.parentElement.remove();
            objMain.ws.send(JSON.stringify({ 'c': 'RemoveTaskCopy', 'Code': code }));
        }
        //while (document.getElementById('taskCoypPanel') != null) {
        //    document.getElementById('taskCoypPanel').remove();
        //}
    },
    AlertNewTask: function () {
        if (objMain.stateNeedToChange.HasNewTask) {
            var taskShowBtnToDisplayPanel = document.getElementById('taskShowBtnToDisplayPanel');
            if (taskShowBtnToDisplayPanel) {
                document.getElementById('taskShowBtnToDisplayPanel').classList.add('needToClick');
            }
            else {
                SysOperatePanel.notifyMsg();
            }
        }
    },
    clear: function () {
        while (document.getElementById('msgDialog') != null) {
            document.getElementById('msgDialog').remove();
        }
        if (document.getElementById('friendsList') != null) {
            document.getElementById('friendsList').remove();
        }
        if (document.getElementById('roleStatePanel') != null) {
            document.getElementById('roleStatePanel').remove();
        }
    },
    dealWithOnLine: function (parameter) {
        var id = 'contact_' + parameter.Key;
        var operate = document.getElementById(id);
        if (operate != null) {
            if (parameter.IsNPC) {
                if (parameter.IsEnemy) {
                    operate.style.color = '#FF0000';
                }
                else {
                    operate.style.color = '#FF8800';
                }
            }
            else {
                if (parameter.IsPartner) {
                    operate.style.color = '#00FF00';
                }
                else {
                    operate.style.color = '#00FF88';
                }
                if (parameter.OnLine) { }
                else { operate.style.color = '#7F7F7F'; }
            }

        }
    }
};

/*
 * 测试文档
 * 有玩家A，玩家B，玩家C
 * 测试需要玩家判断前端UI状态
 * 有三种状态，这三种状态，分别是Null,FriendsList,MsgDialog
 * 测试001: A.State=Null, B给A发送消息。A点击按钮，弹出对话框。B继续给A发送消息。增加消息。
 *          C给A发送消息。提示。
 */