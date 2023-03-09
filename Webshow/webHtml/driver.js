var driverSys = {
    data:
    {
        assistant:
        {
            m: [
                { name: '孙策', index: 510 },
                { name: '明世隐', index: 501 },
                { name: '百里约', index: 196 },
                { name: '哪吒', index: 180 },
                { name: '李元芳', index: 173 },
                { name: '刘备', index: 170 },
                { name: '后羿', index: 169 },
                { name: '亚瑟', index: 166 },
                { name: '张良', index: 156 },
                { name: '孙膑', index: 118 },
                { name: '刘禅', index: 114 },
                { name: '廉颇', index: 105 }
            ],
            wm: [
                { name: '艾琳', index: 155 },
                { name: '阿古朵', index: 533 },
                { name: '西施', index: 523 },
                { name: '阿瑶', index: 505 },
                { name: '元歌', index: 125 },
                { name: '杨玉环', index: 176 },
                { name: '大乔', index: 191 },
                { name: '蔡文姬', index: 184 },
                { name: '雅典娜', index: 183 },
                { name: '芈月', index: 121 },
                { name: '阿轲', index: 116 },
                { name: '小乔', index: 106 },
            ]
        },
        controller:
        {
            m: [
                { name: '司马懿', index: 137 },
                { name: '诸葛亮', index: 190 },
                { name: '韩信', index: 150 },
                { name: '刘邦', index: 149 },
                { name: '达摩', index: 134 },
                { name: '狄仁杰', index: 133 },
                { name: '马可波', index: 132 },
                { name: '李白', index: 131 },
                { name: '曹操', index: 128 },
                { name: '周瑜', index: 124 },
                { name: '高渐离', index: 115 },
                { name: '嬴政', index: 110 }
            ],
            wm: [
                { name: '夏洛特', index: 536 },
                { name: '云中君', index: 506 },
                { name: '嫦娥', index: 515 },
                { name: '上官婉', index: 513 },
                { name: '沈梦溪', index: 312 },
                { name: '米莱狄', index: 504 },
                { name: '女娲', index: 179 },
                { name: '娜露露', index: 162 },
                { name: '不火舞', index: 157 },
                { name: '露娜', index: 146 },
                { name: '貂蝉', index: 141 },
                { name: '武则天', index: 136 },
            ]
        },
        exporter:
        {
            m: [
                { name: '马超', index: 518 },
                { name: '苏烈', index: 194 },
                { name: '百里玄', index: 195 },
                { name: '阿铠', index: 193 },
                { name: '黄忠', index: 192 },
                { name: '杨戬', index: 178 },
                { name: '张飞', index: 171 },
                { name: '程咬金', index: 144 },
                { name: '关羽', index: 140 },
                { name: '项羽', index: 135 },
                { name: '吕布', index: 123 },
                { name: '赵云', index: 107 }
            ],
            wm: [
                { name: '云缨', index: 538 },
                { name: '伽罗', index: 508 },
                { name: '公孙离', index: 199 },
                { name: '孟琪', index: 198 },
                { name: '虞姬', index: 174 },
                { name: '花木兰', index: 154 },
                { name: '王昭君', index: 152 },
                { name: '安琪拉', index: 142 },
                { name: '甄姬', index: 127 },
                { name: '钟无艳', index: 117 },
                { name: '孙尚香', index: 111 },
                { name: '妲己', index: 109 },
            ]
        }
    },
    draw: function (sendF) {
        if (document.getElementById('diverDialogMainPanel') == null) { }
        else return;

        var dialog = document.createElement('div');
        dialog.id = 'diverDialogMainPanel';

        dialog.classList.add('diverDialog');

        var divParent = document.createElement('div');
        var titleSpan = document.createElement('span');
        titleSpan.innerText = '选择司机';
        divParent.appendChild(titleSpan);

        var drawTable = function (title, fromData, tableStyle) {
            var tableAssistant = document.createElement('table');
            tableAssistant.classList.add('driversTable');
            tableAssistant.classList.add(tableStyle);

            var tr1 = document.createElement('tr');
            {
                var th = document.createElement('th');
                th.colSpan = 6;
                th.innerText = title;
                tr1.appendChild(th);
            }

            var tr2 = document.createElement('tr');
            {
                var th = document.createElement('th');
                th.colSpan = 3;
                th.innerText = "男";
                tr2.appendChild(th);

                var th = document.createElement('th');
                th.colSpan = 3;
                th.innerText = "女";
                tr2.appendChild(th);
            }
            tableAssistant.appendChild(tr1);
            tableAssistant.appendChild(tr2);


            {
                for (var i = 0; i < 4; i++) {
                    var tr3 = document.createElement('tr');

                    for (var j = 0; j < 3; j++) {
                        var dataItem = fromData.m[i * 3 + j];
                        var td = document.createElement('td');

                        var divItem = document.createElement('div');
                        divItem.dataItem = dataItem;
                        divItem.onclick = function () {
                            sendF(this.dataItem.index);
                            document.getElementById('diverDialogMainPanel').remove();
                        }
                        divItem.classList.add('diverDataItem');
                        divItem.classList.add('man');

                        var divImage = document.createElement('div');
                        var image = document.createElement('img');
                        image.classList.add('diverImg')
                        image.src = 'Pic/driverimage/' + dataItem.index.toString() + '.jpg';
                        divImage.appendChild(image);
                        divItem.appendChild(divImage);

                        var divName = document.createElement('div');
                        var nameSpan = document.createElement('span');
                        nameSpan.innerText = dataItem.name;
                        divName.classList.add('diverName');
                        divName.appendChild(nameSpan);
                        divItem.appendChild(divName);

                        td.appendChild(divItem);
                        tr3.appendChild(td);
                    }
                    for (var j = 0; j < 3; j++) {
                        var dataItem = fromData.wm[i * 3 + j];
                        var td = document.createElement('td');
                        var divItem = document.createElement('div');
                        divItem.dataItem = dataItem;
                        divItem.onclick = function () {
                            //alert(this.dataItem.name);
                            //objMain.ws.send(
                            sendF(this.dataItem.index);
                            document.getElementById('diverDialogMainPanel').remove();
                        }
                        divItem.classList.add('diverDataItem');
                        divItem.classList.add('woman');
                        var divImage = document.createElement('div');

                        var divImage = document.createElement('div');
                        var image = document.createElement('img');
                        image.classList.add('diverImg')
                        image.src = 'Pic/driverimage/' + dataItem.index.toString() + '.jpg';
                        divImage.appendChild(image);
                        divItem.appendChild(divImage);

                        var divName = document.createElement('div');
                        var nameSpan = document.createElement('span');
                        nameSpan.innerText = dataItem.name;
                        divName.classList.add('diverName');
                        divName.appendChild(nameSpan);
                        divItem.appendChild(divName);

                        td.appendChild(divItem);
                        tr3.appendChild(td);
                    }
                    tableAssistant.appendChild(tr3);
                }
            }



            divParent.appendChild(tableAssistant);
        }

        drawTable("辅助", this.data.assistant, "Assistant");
        drawTable("控制", this.data.controller, "Controller");
        drawTable("输出", this.data.exporter, "Exporter");

        var cancleBtn = document.createElement('button');
        cancleBtn.innerText = '取消';
        cancleBtn.classList.add('diverCancle');
        cancleBtn.onclick = function () {
            document.getElementById('diverDialogMainPanel').remove();
        };
        divParent.appendChild(cancleBtn);

        dialog.appendChild(divParent);
        document.body.appendChild(dialog);
    },
    drawIcon: function (data) {
        if (document.getElementById('driverIconPanel') == null) { }
        else {
            document.getElementById('driverIconPanel').remove();
        }
        var div1 = document.createElement('div');
        div1.id = 'driverIconPanel';
        div1.classList.add('driverIcon');
        var div2 = document.createElement('div');
        div2.classList.add('driverIconBorder');
        var div3 = document.createElement('div');
        var div4 = document.createElement('div');
        div2.classList.add('driverIconSpan');
        var div5 = document.createElement('div');
        div5.classList.add('driverLevetValue');

        var image = document.createElement('img');
        image.classList.add('driverIconImage');
        image.src = 'Pic/driverimage/'+data.driverIndex.toString()+'.jpg';

        var span1 = document.createElement('span');
        span1.innerText = data.name;
        var span2 = document.createElement('span');
        span2.innerText = '';
        span2.id = 'driverLevetValue';

        div3.appendChild(image);
        div4.appendChild(span1);
        div5.appendChild(span2);

        div2.append(div3);
        div2.append(div4);
        div2.append(div5);
        div1.appendChild(div2);
        document.body.appendChild(div1);
        //<div class="driverIcon">
        //    <div class="driverIconBorder">
        //        <div>
        //            <img class="driverIconImage" src="Pic/driverimage/510.jpg" />
        //        </div>
        //        <div class="driverIconSpan">
        //            <span>孙策</span>
        //        </div>
        //        <div class="driverLevetValue">
        //            <span id="driverLevetValue">5级</span>
        //        </div>
        //    </div>
        //</div>
    },
    clearIcon: function ()
    {
        if (document.getElementById('driverIconPanel') == null) { }
        else {
            document.getElementById('driverIconPanel').remove();
        }
    }
};