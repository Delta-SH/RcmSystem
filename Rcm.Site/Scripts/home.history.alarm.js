(function () {
    var currentNode = null;

    Ext.define('AlarmModel', {
        extend: 'Ext.data.Model',
        fields: [
            { name: 'index', type: 'int' },
			{ name: 'id', type: 'int' },
            { name: 'level', type: 'int' },
            { name: 'levelDisplay', type: 'string' },
            { name: 'area', type: 'string' },
            { name: 'station', type: 'string' },
            { name: 'device', type: 'string' },
            { name: 'point', type: 'string' },
            { name: 'comment', type: 'string' },
            { name: 'alarmValue', type: 'float' },
            { name: 'endValue', type: 'float' },
            { name: 'startTime', type: 'string' },
            { name: 'endTime', type: 'string' },
            { name: 'confirmer', type: 'string' },
            { name: 'confirmedTime', type: 'string' },
            { name: 'interval', type: 'string' },
            { name: 'endType', type: 'string' }
        ],
        idProperty: 'index'
    });

    var currentStore = Ext.create('Ext.data.Store', {
        autoLoad: false,
        pageSize: 20,
        model: 'AlarmModel',
        proxy: {
            type: 'ajax',
            url: '/Home/RequestHisAlarms',
            reader: {
                type: 'json',
                successProperty: 'success',
                messageProperty: 'message',
                totalProperty: 'total',
                root: 'data'
            },
            extraParams: {
                node: 'root'
            },
            simpleSortMode: true
        }
    });

    var currentPagingToolbar = $$Rcms.clonePagingToolbar(currentStore);

    var selectPath = function (tree, ids, callback) {
        var root = tree.getRootNode(),
            field = 'id',
            separator = '/',
            path = ids.join(separator);

        path = separator + root.get(field) + separator + path;
        tree.selectPath(path, field, separator, callback || Ext.emptyFn);
    };

    var change = function (node, pagingtoolbar) {
        var me = pagingtoolbar.store;
        me.proxy.extraParams.node = node.getId();
        me.loadPage(1);
    };

    var filter = function (pagingtoolbar) {
        var me = pagingtoolbar.store,
            stationtypes = Ext.getCmp('station-type-multicombo'),
            devicetypes = Ext.getCmp('device-type-multicombo'),
            alarmlevels = Ext.getCmp('alarm-level-multicombo'),
            startdate = Ext.getCmp('startField'),
            enddate = Ext.getCmp('endField'),
            interval = Ext.getCmp('intervalField');

        if (!startdate.isValid()) return;
        if (!enddate.isValid()) return;
        if (!interval.isValid()) return;

        me.proxy.extraParams.stationtypes = stationtypes.getSelectedValues();
        me.proxy.extraParams.devicetypes = devicetypes.getSelectedValues();
        me.proxy.extraParams.alarmlevels = alarmlevels.getSelectedValues();
        me.proxy.extraParams.startdate = startdate.getRawValue();
        me.proxy.extraParams.enddate = enddate.getRawValue();
        me.proxy.extraParams.interval = interval.getValue();
        me.loadPage(1);
    };

    var print = function (store) {
        $$Rcms.download({
            url: '/Home/DownloadHisAlarms',
            params: store.proxy.extraParams
        });
    };

    var leftLayout = Ext.create('Ext.tree.Panel', {
        id: 'system-organization',
        region: 'west',
        title: '系统设备列表',
        glyph: 0xe619,
        width: 220,
        split: true,
        collapsible: true,
        collapsed: false,
        autoScroll: true,
        useArrows: false,
        rootVisible: true,
        root: {
            id: 'root',
            text: '监控中心',
            expanded: true,
            icon: '/Content/themes/icons/home.png'
        },
        viewConfig: {
            loadMask: true
        },
        store: Ext.create('Ext.data.TreeStore', {
            autoLoad: false,
            nodeParam: 'node',
            proxy: {
                type: 'ajax',
                url: '/Home/GetOrganization',
                reader: {
                    type: 'json',
                    successProperty: 'success',
                    messageProperty: 'message',
                    totalProperty: 'total',
                    root: 'data'
                }
            }
        }),
        listeners: {
            select: function (me, record, index) {
                currentNode = record;
                change(record, currentPagingToolbar);
            }
        },
        tbar: [
            {
                id: 'search-field',
                xtype: 'textfield',
                emptyText: '请输入筛选条件...',
                flex: 1,
                listeners: {
                    change: function (me, newValue, oldValue, eOpts) {
                        delete me._filterData;
                        delete me._filterIndex;
                    }
                }
            },
            {
                id: 'search-button',
                xtype: 'button',
                glyph: 0xe60d,
                handler: function () {
                    var tree = Ext.getCmp('system-organization'),
                        search = Ext.getCmp('search-field'),
                        text = search.getRawValue();

                    if (Ext.isEmpty(text, false)) {
                        return;
                    }

                    if (text.length < 2) {
                        return;
                    }

                    if (search._filterData != null && search._filterIndex != null) {
                        var index = search._filterIndex + 1;
                        var paths = search._filterData;
                        if (index >= paths.length) {
                            index = 0;
                            Ext.Msg.show({ title: '系统提示', msg: '搜索完毕', buttons: Ext.Msg.OK, icon: Ext.Msg.INFO });
                        }

                        selectPath(tree, paths[index]);
                        search._filterIndex = index;
                    } else {
                        Ext.Ajax.request({
                            url: '/Home/SearchOrganization',
                            params: { text: text },
                            mask: new Ext.LoadMask({ target: tree, msg: '正在处理，请稍后...' }),
                            success: function (response, options) {
                                var data = Ext.decode(response.responseText, true);
                                if (data.success) {
                                    var len = data.data.length;
                                    if (len > 0) {
                                        selectPath(tree, data.data[0]);
                                        search._filterData = data.data;
                                        search._filterIndex = 0;
                                    } else {
                                        Ext.Msg.show({ title: '系统提示', msg: Ext.String.format('未找到指定内容:<br/>{0}', text), buttons: Ext.Msg.OK, icon: Ext.Msg.INFO });
                                    }
                                } else {
                                    Ext.Msg.show({ title: '系统错误', msg: data.message, buttons: Ext.Msg.OK, icon: Ext.Msg.ERROR });
                                }
                            }
                        });
                    }
                }
            }
        ]
    });

    var centerLayout = Ext.create('Ext.grid.Panel', {
        id: 'active-grid',
        glyph: 0xe612,
        title: '历时告警信息',
        region: 'center',
        store: currentStore,
        dockedItems: [{
            xtype: 'panel',
            glyph: 0xe61a,
            dock: 'top',
            items: [
                {
                    xtype: 'toolbar',
                    border: false,
                    items: [
                        {
                            id: 'station-type-multicombo',
                            xtype: 'StationTypeMultiCombo',
                            emptyText: '默认全部'
                        },
                        {
                            id: 'device-type-multicombo',
                            xtype: 'DeviceTypeMultiCombo',
                            emptyText: '默认全部'
                        },
                        {
                            id: 'alarm-level-multicombo',
                            xtype: 'AlarmLevelMultiCombo',
                            emptyText: '默认全部'
                        },
                        {
                            xtype: 'button',
                            glyph: 0xe60d,
                            text: '数据查询',
                            handler: function (me, event) {
                                filter(currentPagingToolbar);
                            }
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    border: false,
                    items: [
                        {
                            id: 'startField',
                            xtype: 'datefield',
                            fieldLabel: '开始时间',
                            labelWidth: 60,
                            width: 220,
                            value: Ext.Date.add(new Date(), Ext.Date.DAY, -1),
                            editable: false,
                            allowBlank: false
                        }, {
                            id: 'endField',
                            xtype: 'datefield',
                            fieldLabel: '结束时间',
                            labelWidth: 60,
                            width: 220,
                            value: Ext.Date.add(new Date(), Ext.Date.DAY, -1),
                            editable: false,
                            allowBlank: false
                        },
                        {
                            id: 'intervalField',
                            xtype: 'numberfield',
                            fieldLabel: '告警历时',
                            emptyText: '筛选超时告警(分钟)',
                            labelWidth: 60,
                            width: 220,
                            value: 0,
                            minValue: 0,
                            allowBlank: false
                        },
                        {
                            xtype: 'button',
                            glyph: 0xe609,
                            text: '数据导出',
                            handler: function (me, event) {
                                print(currentStore);
                            }
                        }
                    ]
                }
            ]
        }],
        tools: [
            {
                type: 'print',
                tooltip: '数据导出',
                handler: function (event, toolEl, panelHeader) {
                    print(currentStore);
                }
            }
        ],
        viewConfig: {
            loadMask: false,
            preserveScrollOnRefresh: true,
            stripeRows: true,
            trackOver: true,
            emptyText: '<h1 style="margin:20px">没有数据记录</h1>',
            getRowClass: function (record, rowIndex, rowParams, store) {
                return $$Rcms.GetAlmLevelCls(record.get("level"));
            }
        },
        columns: [
            {
                text: '序号',
                dataIndex: 'index',
                width: 60
            },
            {
                text: '告警级别',
                dataIndex: 'levelDisplay',
                align: 'center',
                tdCls: 'x-level-cell'
            },
            {
                text: '所属区域',
                dataIndex: 'area'
            },
            {
                text: '所属站点',
                dataIndex: 'station'
            },
            {
                text: '设备名称',
                dataIndex: 'device'
            },
            {
                text: '信号名称',
                dataIndex: 'point'
            },
            {
                text: '告警描述',
                dataIndex: 'comment'
            },
            {
                text: '触发值',
                dataIndex: 'alarmValue'
            },
            {
                text: '恢复值',
                dataIndex: 'endValue'
            },
            {
                text: '告警时间',
                dataIndex: 'startTime',
                align: 'center',
                width: 150
            },
            {
                text: '结束时间',
                dataIndex: 'endTime',
                align: 'center',
                width: 150
            },
            {
                text: '确认人员',
                dataIndex: 'confirmer'
            },
            {
                text: '确认时间',
                dataIndex: 'confirmedTime',
                width: 150
            },
            {
                text: '告警时长',
                dataIndex: 'interval',
                width: 150
            },
            {
                text: '结束类型',
                dataIndex: 'endType'
            }
        ],
        bbar: currentPagingToolbar
    });

    Ext.onReady(function () {
        /*add components to viewport panel*/
        var pageContentPanel = Ext.getCmp('center-content-panel-fw');
        if (!Ext.isEmpty(pageContentPanel)) {
            pageContentPanel.add([leftLayout, centerLayout]);
            filter(currentPagingToolbar);
        }
    });
})();