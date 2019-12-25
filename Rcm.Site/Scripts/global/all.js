/* ========================================================================
 * Global: global.js
 * /Scripts/global/global.js
 * ========================================================================
 */

Ext.require(['*']);
Ext.setGlyphFontFamily('iconfont');

/*ajax timeout*/
Ext.Ajax.timeout = 300000;
Ext.override(Ext.data.Connection, { timeout: Ext.Ajax.timeout });
Ext.override(Ext.data.JsonP, { timeout: Ext.Ajax.timeout });
Ext.override(Ext.form.Basic, {
    timeout: Ext.Ajax.timeout / 1000,
    afterAction: function (action, success) {
        this.callParent(arguments);

        //prevent the failed window
        var prevent = action.preventWindow || false;
        if (prevent !== true && !success) {
            var message = '';
            if (!Ext.isEmpty(action.result) && !Ext.isEmpty(action.result.message))
                message = action.result.message;

            $$Rcms.ShowFailure(action.response, message);
        }
    },
    getValues: function (asString, dirtyOnly, includeEmptyText, useDataValues) {
        var values = {},
            fields = this.getFields().items,
            f,
            fLen = fields.length,
            isArray = Ext.isArray,
            field, data, val, bucket, name;

        for (f = 0; f < fLen; f++) {
            field = fields[f];

            if (!dirtyOnly || field.isDirty()) {
                data = field[useDataValues ? 'getModelData' : 'getSubmitData'](includeEmptyText);

                if (Ext.isObject(data)) {
                    for (name in data) {
                        if (data.hasOwnProperty(name)) {
                            val = data[name];

                            //dont submit empty values when includeEmptyText is false.
                            if (!includeEmptyText && val === '') {
                                continue;
                            }

                            if (includeEmptyText && val === '') {
                                val = field.emptyText || '';
                            }

                            if (values.hasOwnProperty(name)) {
                                bucket = values[name];

                                if (!isArray(bucket)) {
                                    bucket = values[name] = [bucket];
                                }

                                if (isArray(val)) {
                                    values[name] = bucket.concat(val);
                                } else {
                                    bucket.push(val);
                                }
                            } else {
                                values[name] = val;
                            }
                        }
                    }
                }
            }
        }

        if (asString) {
            values = Ext.Object.toQueryString(values);
        }
        return values;
    }
});

/*global ajax exception handler*/
Ext.override(Ext.Ajax, { unauthorizedCode: 400 });
Ext.Ajax.on('beforerequest', function (conn, options) {
    if (!Ext.isEmpty(options.mask))
        options.mask.show();
});
Ext.Ajax.on('requestcomplete', function (conn, response, options) {
    if (!Ext.isEmpty(options.mask))
        options.mask.hide();
});
Ext.Ajax.on('requestexception', function (conn, response, options) {
    if (!Ext.isEmpty(options.mask))
        options.mask.hide();

    if (response.status === Ext.Ajax.unauthorizedCode) {
        var data = Ext.decode(response.responseText, true);
        if (!Ext.isEmpty(data) && !Ext.isEmpty(data.LoginUrl)) {
            window.location.href = data.LoginUrl;
            return false;
        }
    }

    //prevent the failed window
    var prevent = options.preventWindow || false;
    if (prevent !== true) {
        $$Rcms.ShowFailure(response, response.responseText);
    }
});
Ext.direct.Manager.on('exception', function (event) {
    if (!Ext.isEmpty(event) && !Ext.isEmpty(event.message))
        Ext.Msg.show({ title: '系统错误', msg: event.message, buttons: Ext.Msg.OK, icon: Ext.Msg.ERROR });
});

/*global datetime formart*/
Ext.override(Ext.form.field.Date, {
    format: 'Y-m-d'
});

/*
 修复当Grid分组，然后折叠第一组，其他组无法选中行的问题
*/
Ext.override(Ext.view.Table, {
    getRecord: function (node) {
        node = this.getNode(node);
        if (node) {
            return this.dataSource.data.get(node.getAttribute('data-recordId'));
        }
    },
    indexInStore: function (node) {
        node = this.getNode(node, true);
        if (!node && node !== 0) {
            return -1;
        }

        return this.dataSource.indexOf(this.getRecord(node));
    }
});

/*show failure window*/
window.$$Rcms.ShowFailure = function (response, errorMsg) {
    var bodySize = Ext.getBody().getViewSize(),
        width = (bodySize.width < 500) ? bodySize.width - 50 : 500,
        height = (bodySize.height < 300) ? bodySize.height - 50 : 300,
        win;

    if (Ext.isEmpty(errorMsg))
        errorMsg = response.responseText;

    //prevent the failed window when redirect to an new page.
    if (response.status === 0
        && Ext.isEmpty(response.statusText)
        && Ext.isEmpty(errorMsg))
        return false;

    win = new Ext.window.Window({
        modal: true,
        width: width,
        height: height,
        title: '系统错误',
        layout: "fit",
        maximizable: true,
        items: [{
            xtype: "container",
            layout: {
                type: "vbox",
                align: "stretch"
            },
            items: [
                {
                    xtype: "container",
                    height: 42,
                    layout: "absolute",
                    defaultType: "label",
                    items: [
                        {
                            xtype: "component",
                            x: 5,
                            y: 5,
                            html: '<div class="x-message-box-error" style="width:32px;height:32px"></div>'
                        },
                        {
                            x: 42,
                            y: 5,
                            html: "<b>Status Code: </b>"
                        },
                        {
                            x: 128,
                            y: 5,
                            text: response.status
                        },
                        {
                            x: 42,
                            y: 23,
                            html: "<b>Status Text: </b>"
                        },
                        {
                            x: 128,
                            y: 23,
                            text: response.statusText
                        }
                    ]
                },
                {
                    flex: 1,
                    xtype: "htmleditor",
                    value: errorMsg,
                    readOnly: true,
                    enableAlignments: false,
                    enableColors: false,
                    enableFont: false,
                    enableFontSize: false,
                    enableFormat: false,
                    enableLinks: false,
                    enableLists: false,
                    enableSourceEdit: false
                }
            ]
        }]
    });

    win.show();
}

Ext.override(Ext.panel.Header, {
    initComponent: function () {
        this.callParent(arguments);
        this.insert(0, this.iconCmp);
        this.insert(1, this.titleCmp);
    }
});

/*ajax action*/
window.$$Rcms.Action = {
    Add: 0,
    Edit: 1,
    Delete: 2
};

/*organization*/
window.$$Rcms.Organization = {
    None: -3,
    LSC: -2,
    Area: -1,
    Station: 0,
    Device: 1,
    Dic: 2,
    Aic: 3,
    Doc: 4,
    Aoc: 5,
    Sic: 6,
    Soc: 7,
    Pic: 8,
    Poc: 9,
    Vic: 10,
    Voc: 11,
    ADic: 12,
    ADoc: 13,
    Str: 14,
    Img: 15,
    FS: 16,
    Bus: 17,
    Driver: 18,
    ExObj: 99
};

/*Point Status*/
window.$$Rcms.PointStatus = {
    Normal: 0,
    Level1: 1,
    Level2: 2,
    Level3: 3,
    Level4: 4,
    Opevent: 5,
    Invalid: 6,
    NullValue: 7
};

/*Status Css Class*/
window.$$Rcms.GetPointStatusCls = function (value) {
    switch (value) {
        case $$Rcms.PointStatus.Normal:
            return 'point-status-normal';
        case $$Rcms.PointStatus.Level1:
            return 'point-status-level1';
        case $$Rcms.PointStatus.Level2:
            return 'point-status-level2';
        case $$Rcms.PointStatus.Level3:
            return 'point-status-level3';
        case $$Rcms.PointStatus.Level4:
            return 'point-status-level4';
        case $$Rcms.PointStatus.Opevent:
            return 'point-status-opevent';
        case $$Rcms.PointStatus.Invalid:
            return 'point-status-invalid';
        case $$Rcms.PointStatus.NullValue:
            return 'point-status-nullvalue';
        default:
            return '';
    }
};

/*Alarm Level*/
window.$$Rcms.AlmLevel = {
    Level1: 1,
    Level2: 2,
    Level3: 3,
    Level4: 4
};

window.$$Rcms.SSH = {
    Lsc: -2,
    Area: -1,
    Station: 0,
    Device: 1,
    Dic: 2,
    Aic: 3,
    Doc: 4,
    Aoc: 5
};

/*Alarm Css Class*/
window.$$Rcms.GetAlmLevelCls = function (value) {
    switch (value) {
        case $$Rcms.AlmLevel.Level1:
            return 'alm-level1';
        case $$Rcms.AlmLevel.Level2:
            return 'alm-level2';
        case $$Rcms.AlmLevel.Level3:
            return 'alm-level3';
        case $$Rcms.AlmLevel.Level4:
            return 'alm-level4';
        default:
            return '';
    }
};

window.$$Rcms.UpdateIcons = function (tree, nodes) {
    nodes = nodes || [];

    if (nodes.length === 0) {
        var root = tree.getRootNode();
        nodes.push(root.getId());

        if (root.hasChildNodes()) {
            root.eachChild(function (c) {
                c.cascadeBy(function (n) {
                    nodes.push(n.getId());
                });
            });
        }
    }

    if (nodes.length === 0)
        return;

    Ext.Ajax.request({
        url: '/Component/GetNodeIcons',
        method: 'POST',
        jsonData: nodes,
        success: function (response, options) {
            var data = Ext.decode(response.responseText, true);
            if (data.success) {
                var icons = {},
                    root = tree.getRootNode();

                Ext.each(data.data, function (item, index, allItems) {
                    icons[item.id] = item;
                });

                $$Rcms.SetIcon(root, icons[root.getId()]);
                if (root.hasChildNodes()) {
                    root.eachChild(function (c) {
                        c.cascadeBy(function (n) {
                            $$Rcms.SetIcon(n, icons[n.getId()]);
                        });
                    });
                }
            }
        }
    });
};

window.$$Rcms.SetIcon = function (node, icon) {
    if (Ext.isEmpty(icon))
        return;

    var prefix = 'all';
    if (icon.id !== 'root') {
        var keys = $$Rcms.SplitKeys(icon.id);
        if (keys.length === 2) {
            var type = parseInt(keys[0]);
            if (type === $$Rcms.SSH.Area)
                prefix = 'diqiu';
            else if (type === $$Rcms.SSH.Station)
                prefix = 'juzhan';
            else if (type === $$Rcms.SSH.Device)
                prefix = 'device';
        }
    }

    node.set('iconCls', Ext.String.format('{0}-level-{1}', prefix, icon.level));
};

/*Split Node Keys*/
window.$$Rcms.SplitKeys = function (key) {
    if (Ext.isEmpty(key)) return [];
    return key.split($$Rcms.Separator);
};

/*Icons */
window.$$Rcms.icons = {
    Root: '/content/themes/icons/all.png'
};

/*global delimiter*/
window.$$Rcms.Delimiter = ';';
window.$$Rcms.Separator = '┆';

/*download files via ajax*/
window.$$Rcms.download = function (config) {
    config = config || {};
    var url = config.url,
        method = config.method || 'POST',
        params = config.params || {};

    var form = Ext.create('Ext.form.Panel', {
        standardSubmit: true,
        url: url,
        method: method
    });

    form.submit({
        target: '_blank',
        params: params
    });

    Ext.defer(function () {
        form.close();
    }, 100);
};

/*datetime parse funtion*/
window.$$Rcms.datetimeParse = function (date, format) {
    return Ext.Date.parse(date, format || 'Y-m-d H:i:s', true);
};

/*date parse funtion*/
window.$$Rcms.dateParse = function (date, format) {
    return Ext.Date.parse(date, format || 'Y-m-d', true);
};

/*clone paging toolbar*/
window.$$Rcms.clonePagingToolbar = function (store) {
    return Ext.create('Ext.PagingToolbar', {
        store: store,
        displayInfo: true,
        items: ['-',
            {
                store: new Ext.data.Store({
                    fields: [{ name: 'id', type: 'int' }, { name: 'text', type: 'string' }, { name: 'comment', type: 'string' }],
                    data: [
                        { id: 10, text: '10', comment: '10' },
                        { id: 20, text: '20', comment: '20' },
                        { id: 50, text: '50', comment: '50' },
                        { id: 100, text: '100', comment: '100' },
                        { id: 200, text: '200', comment: '200' }
                    ]
                }),
                xtype: 'combobox',
                fieldLabel: '显示条数',
                displayField: 'text',
                valueField: 'id',
                typeAhead: true,
                queryMode: 'local',
                triggerAction: 'all',
                emptyText: '每页显示的条数',
                selectOnFocus: true,
                forceSelection: true,
                labelWidth: 60,
                width: 200,
                value: store.pageSize,
                listeners: {
                    select: function (combo) {
                        store.pageSize = parseInt(combo.getValue());
                        store.loadPage(1);
                    }
                }
            }
        ]
    });
};

window.$$Rcms.animateNumber = function (target, now) {
    if (Ext.isString(target))
        target = Ext.get(target);

    now = Math.round(now);
    var display = target.getHTML();
    var current = Ext.isEmpty(display) ? 0 : parseInt(display);
    var count = Math.abs(now - current);
    var numUp = now > current;

    var step = 25,
        speed = Math.round(count / step),
        int_speed = 25;

    var interval = setInterval(function () {
        if (numUp) {
            if (speed > 1 && current + speed < now) {
                current += speed;
                target.setHTML(current);
            } else if (current < now) {
                current += 1;
                target.setHTML(current);
            } else {
                clearInterval(interval);
            }
        } else {
            if (speed > 1 && current - speed > now) {
                current -= speed;
                target.setHTML(current);
            } else if (current > now) {
                current -= 1;
                target.setHTML(current);
            } else {
                clearInterval(interval);
            }
        }
    }, int_speed);
}

//global tasks
window.$$Rcms.Tasks = {
    almNoticeTask: Ext.util.TaskManager.newTask({
        run: function () {
        },
        fireOnStart: true,
        interval: 10000,
        repeat: 1
    }),
    actAlarmTask: Ext.util.TaskManager.newTask({
        run: function () {
        },
        fireOnStart: true,
        interval: 10000,
        repeat: 1
    }),
    actPointTask: Ext.util.TaskManager.newTask({
        run: function () {
        },
        fireOnStart: true,
        interval: 10000,
        repeat: 1
    }),
    actOrderTask: Ext.util.TaskManager.newTask({
        run: function () {
        },
        fireOnStart: true,
        interval: 5000,
        repeat: 1
    })
};

/* ========================================================================
 * Global: template.js
 * /Scripts/global/template.js
 * ========================================================================
 */

Ext.onReady(function () {
    Ext.tip.QuickTipManager.init();

    //start tasks
    $$Rcms.Tasks.almNoticeTask.start();

    var _pagebody = Ext.create('Ext.panel.Panel', {
        region: 'north',
        border: false,
        contentEl: 'page-body-content'
    });

    /*home page*/
    var viewport = Ext.create('Ext.container.Viewport', {
        id: 'main-viewport',
        layout: 'border',
        items: [
                {
                    id: 'top-nav-panel-fw',
                    region: 'north',
                    contentEl: 'top-nav-panel',
                    hidden: $$Rcms.__PYLON__4XtnBeCZz97O0PzX__ === '1',
                    height: 51,
                    border: false
                },
                {
                    id: 'center-content-panel-fw',
                    region: 'center',
                    layout: 'border',
                    contentEl: 'center-content-panel',
                    border: false,
                    items: [_pagebody],
                    padding: 5
                }
        ]
    });

    /*title tip*/
    var help = Ext.get('help-btn');
    if (!Ext.isEmpty(help)) {
        help.on('click', function (el, e) {
            var arrow = Ext.get('help-arrow');
            if (Ext.isEmpty(arrow)) {
                return false;
            }

            var tip = Ext.get('help-tip');
            if (Ext.isEmpty(tip)) {
                return false;
            }

            tip.setDisplayed(!tip.isVisible());
            arrow.anchorTo(help, 'tc-bc', [-2, 1]);
            viewport.doLayout();
        });
    }

    /*show page content*/
    Ext.getBody().setDisplayed(true);
});