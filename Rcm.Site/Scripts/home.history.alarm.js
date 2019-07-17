(function () {
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
                            id: 'rangePicker',
                            xtype: 'DevicePicker',
                            fieldLabel: '查询范围',
                            emptyText: '默认全部',
                            labelWidth: 60,
                            width: 220
                        },
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
                            xtype: 'button',
                            glyph: 0xe60d,
                            text: '数据查询',
                            handler: function (me, event) {
                                query();
                            }
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    border: false,
                    items: [
                        {
                            id: 'alarm-level-multicombo',
                            xtype: 'AlarmLevelMultiCombo',
                            emptyText: '默认全部'
                        },
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
                            id: 'exportButton',
                            xtype: 'button',
                            glyph: 0xe609,
                            disabled: true,
                            text: '数据导出',
                            handler: function (me, event) {
                                print();
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
                    print();
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

    var query = function () {
        var parent = Ext.getCmp('rangePicker').getValue(),
            stationtypes = Ext.getCmp('station-type-multicombo').getSelectedValues(),
            devicetypes = Ext.getCmp('device-type-multicombo').getSelectedValues(),
            alarmlevels = Ext.getCmp('alarm-level-multicombo').getSelectedValues(),
            startDate = Ext.getCmp('startField').getRawValue(),
            endDate = Ext.getCmp('endField').getRawValue(),
            proxy = currentStore.getProxy();

        proxy.extraParams.parent = parent;
        proxy.extraParams.stationtypes = stationtypes;
        proxy.extraParams.devicetypes = devicetypes;
        proxy.extraParams.alarmlevels = alarmlevels;
        proxy.extraParams.startdate = startDate;
        proxy.extraParams.enddate = endDate;
        currentStore.loadPage(1, {
            callback: function (records, operation, success) {
                Ext.getCmp('exportButton').setDisabled(success === false);
            }
        });
    };

    var print = function () {
        $$Rcms.download({
            url: '/Home/DownloadHisAlarms',
            params: currentStore.getProxy().extraParams
        });
    };

    Ext.onReady(function () {
        /*add components to viewport panel*/
        var pageContentPanel = Ext.getCmp('center-content-panel-fw');
        if (!Ext.isEmpty(pageContentPanel)) {
            pageContentPanel.add([centerLayout]);
        }
    });
})();