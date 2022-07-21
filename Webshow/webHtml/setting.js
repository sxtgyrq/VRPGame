var settingSys =
{
    operateAddress: '',
    operateID: 'settingPanel',
    html: `<div id="settingPanel" style="position: absolute;
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
        <table  border="1">
            <tr>
                <th style="border:solid 1px white">声音</th>
                <td>
                  开<input name="soundOnOff" type="radio" checked value="on" />
                </td>
                <td>
                  关  <input name="soundOnOff" type="radio" value="off" />
                </td>
            </tr>
            <tr>
                <th style="border:solid 1px white">模型</th>
                <td>
                   显示 <input name="modelOnOff" type="radio" checked value="on" />
                </td>
                <td>
                   隐藏 <input name="modelOnOff" type="radio" value="off" />
                </td>
            </tr>

        </table>

        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="settingSys.apply();settingSys.add();">
            确认
        </div>
        <div style="background: yellowgreen;
        margin-bottom: 0.25em;
        margin-top: 0.25em;" onclick="settingSys.add();">
            取消
        </div>
    </div>`,
    refresh: function () {
    },
    add: function () {
        var that = settingSys;
        if (document.getElementById(that.operateID) == null) {
            // var obj = new DOMParser().parseFromString(that.html, 'text/html');
            var frag = document.createRange().createContextualFragment(that.html);
            frag.id = that.operateID;
            document.body.appendChild(frag);
            that.bindData();
        }
        else {
            document.getElementById(that.operateID).remove();
        }
    },
    getValue: function (objName) {
        for (var i = 0; i < document.getElementsByName(objName).length; i++) {
            if (document.getElementsByName(objName)[i].checked) {
                return document.getElementsByName(objName)[i].value;
            }
        }
        return null;
    },
    setValue: function (objName, value) {
        for (var i = 0; i < document.getElementsByName(objName).length; i++) {
            if (document.getElementsByName(objName)[i].value == value) {
                document.getElementsByName(objName)[i].checked = true;
            }
            else {
                document.getElementsByName(objName)[i].checked = false;
            }
        }
    },
    apply: function () {
        switch (this.getValue('soundOnOff')) {
            case 'on':
                {
                    var x = document.getElementById('backGroudMusick');
                    x.play();
                    objMain.music.on = true;
                }; break;
            case 'off':
                {
                    var x = document.getElementById('backGroudMusick');
                    x.pause();
                    objMain.music.on = false;
                }; break;
        }
        switch (this.getValue('modelOnOff')) {
            case 'on':
                {
                    objMain.buildingGroup.visible = true;
                }; break;
            case 'off':
                {
                    objMain.buildingGroup.visible = false;
                }; break;
        }
    },
    bindData: function () {
        var x = document.getElementById('backGroudMusick');
        if (x.paused)
            this.setValue('soundOnOff', 'off');
        else
            this.setValue('soundOnOff', 'on');
        if (objMain.buildingGroup.visible)
            this.setValue('modelOnOff', 'on');
        else
            this.setValue('modelOnOff', 'off');
        //this.setValue
    }
};