(function () {
    Ext.define('DataModel', {
        extend: 'Ext.data.Model',
        fields: [
            { name: 'index', type: 'int' },
            { name: 'area', type: 'string' },
            { name: 'station', type: 'string' },
            { name: 'device', type: 'string' },
            { name: 'point', type: 'string' },
            { name: 'type', type: 'string' },
            { name: 'value', type: 'float' },
            { name: 'unit', type: 'string' },
            { name: 'time', type: 'string' }
        ],
        idProperty: 'index'
    });

    var currentStore = Ext.create('Ext.data.Store', {
        autoLoad: false,
        pageSize: 20,
        model: 'DataModel',
        proxy: {
            type: 'ajax',
            url: '/Home/RequestHisData',
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
        title: '历时测值信息',
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
                            id: 'startField',
                            xtype: 'datetimepicker',
                            fieldLabel: '开始时间',
                            labelWidth: 60,
                            width: 220,
                            value: Ext.ux.DateTime.addDays(Ext.ux.DateTime.today(), -1),
                            maxDate: '%y-%M-%d',
                            editable: false,
                            allowBlank: false
                        }, {
                            id: 'endField',
                            xtype: 'datetimepicker',
                            fieldLabel: '结束时间',
                            labelWidth: 60,
                            width: 220,
                            value: Ext.ux.DateTime.addSeconds(Ext.ux.DateTime.today(), -1),
                            minDate: '#F{Ext.getCmp(\'startField\').getRawValue()}',
                            maxDate: '#F{$dp.$DV(Ext.getCmp(\'startField\').getRawValue(),{d:1,s:-1})}',
                            editable: false,
                            allowBlank: false
                        },
                        {
                            id: 'keywordField',
                            xtype: 'textfield',
                            fieldLabel: '信号名称',
                            emptyText: '多关键字请以;分隔，例: A;B;C',
                            labelWidth: 60,
                            width: 220
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
            emptyText: '<h1 style="margin:20px">没有数据记录</h1>'
        },
        columns: [
            { text: '序号', dataIndex: 'index', width: 60 },
            { text: '所属区域', dataIndex: 'area', width: 150 },
            { text: '所属站点', dataIndex: 'station', width: 150 },
            { text: '所属设备', dataIndex: 'device', width: 150 },
            { text: '信号名称', dataIndex: 'point', width: 150 },
            { text: '信号测值', dataIndex: 'value', width: 100 },
            { text: '单位/描述', dataIndex: 'unit', width: 100 },
            { text: '测值时间', dataIndex: 'time', align: 'center', width: 150 }
        ],
        bbar: currentPagingToolbar
    });

    var query = function () {
        var parent = Ext.getCmp('rangePicker').getValue(),
            stationtypes = Ext.getCmp('station-type-multicombo').getSelectedValues(),
            devicetypes = Ext.getCmp('device-type-multicombo').getSelectedValues(),
            startDate = Ext.getCmp('startField').getRawValue(),
            endDate = Ext.getCmp('endField').getRawValue(),
            keyword = Ext.getCmp('keywordField').getRawValue(),
            proxy = currentStore.getProxy();

        proxy.extraParams.parent = parent;
        proxy.extraParams.stationtypes = stationtypes;
        proxy.extraParams.devicetypes = devicetypes;
        proxy.extraParams.startdate = startDate;
        proxy.extraParams.enddate = endDate;
        proxy.extraParams.keyword = keyword;
        currentStore.loadPage(1, {
            callback: function (records, operation, success) {
                Ext.getCmp('exportButton').setDisabled(success === false);
            }
        });
    };

    var print = function () {
        $$Rcms.download({
            url: '/Home/DownloadHisData',
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