(function () {
    var currentNode = null;

    //#region Chart
    var lineChart = null,
        lineOption = {
            tooltip: {
                trigger: 'axis',
                formatter: '{b}: {c} {a}'
            },
            grid: {
                top: 15,
                left: 0,
                right: 5,
                bottom: 0,
                containLabel: true
            },
            xAxis: [{
                type: 'category',
                boundaryGap: false,
                splitLine: { show: false },
                data: ['00′00″']
            }],
            yAxis: [{
                type: 'value'
            }],
            series: [
                {
                    name: '',
                    type: 'line',
                    smooth: true,
                    symbol: 'none',
                    sampling: 'average',
                    itemStyle: {
                        normal: {
                            color: '#0892cd'
                        }
                    },
                    areaStyle: { normal: {} },
                    data: [0]
                }
            ]
        };
    //#endregion

    Ext.define('PointModel', {
        extend: 'Ext.data.Model',
        fields: [
            { name: 'id', type: 'int' },
            { name: 'name', type: 'string' },
            { name: 'type', type: 'int' },
            { name: 'typeDisplay', type: 'string' },
            { name: 'value', type: 'float' },
            { name: 'valueDisplay', type: 'string' },
            { name: 'status', type: 'int' },
            { name: 'statusDisplay', type: 'string' },
            { name: 'timestamp', type: 'string' },
            { name: 'shortTime', type: 'string' }
        ],
        idProperty: 'id'
    });

    var currentStore = Ext.create('Ext.data.Store', {
        autoLoad: false,
        pageSize: 20,
        model: 'PointModel',
        groupField: 'typeDisplay',
        groupDir: 'undefined',
        sortOnLoad: false,
        proxy: {
            type: 'ajax',
            url: '/Home/RequestPoints',
            reader: {
                type: 'json',
                successProperty: 'success',
                messageProperty: 'message',
                totalProperty: 'total',
                root: 'data'
            },
            extraParams: {
                node: 'root',
                types: [$$Rcms.Organization.Dic, $$Rcms.Organization.Aic, $$Rcms.Organization.Aoc, $$Rcms.Organization.Doc]
            },
            simpleSortMode: true
        },
        listeners: {
            load: function (me, records, successful) {
                if (successful) {
                    if (records.length > 0) {
                        var grid = Ext.getCmp('active-grid');
                        if (grid.getSelectionModel().hasSelection()) {
                            var key = grid.getSelectionModel().getSelection()[0].getId();
                            var record = me.getById(key);
                            if (Ext.isEmpty(record) === false) {
                                loadChart(record);
                            }
                        }
                    }

                    $$Rcms.Tasks.actPointTask.fireOnStart = false;
                    $$Rcms.Tasks.actPointTask.restart();
                }
            }
        }
    });

    var selectPath = function (tree, ids, callback) {
        var root = tree.getRootNode(),
            field = 'id',
            separator = '/',
            path = ids.join(separator);

        path = separator + root.get(field) + separator + path;
        tree.selectPath(path, field, separator, callback || Ext.emptyFn);
    };

    var controlWnd = Ext.create('Ext.window.Window', {
        title: '信号遥控参数',
        height: 250,
        width: 400,
        glyph: 0xe61b,
        modal: true,
        border: false,
        hidden: true,
        closeAction: 'hide',
        items: [{
            xtype: 'form',
            itemId: 'controlForm',
            border: false,
            padding: 10,
            items: [
                {
                    itemId: 'point',
                    xtype: 'hiddenfield',
                    name: 'point'
                },
                {
                    itemId: 'type',
                    xtype: 'hiddenfield',
                    name: 'type'
                },
                {
                    itemId: 'controlradio',
                    xtype: 'radiogroup',
                    columns: 1,
                    vertical: true,
                    items: [
                        { boxLabel: '常开控制(0)', name: 'ctrl', inputValue: 0, checked: true },
                        { boxLabel: '常闭控制(1)', name: 'ctrl', inputValue: 1 },
                        { boxLabel: '脉冲控制(2)', name: 'ctrl', inputValue: 2 }
                    ]
                }]
        }],
        buttons: [
          { id: 'controlResult', xtype: 'iconlabel', text: '' },
          { xtype: 'tbfill' },
          {
              xtype: 'button',
              text: '遥控',
              handler: function (el, e) {
                  var form = controlWnd.getComponent('controlForm'),
                      baseForm = form.getForm(),
                      result = Ext.getCmp('controlResult');

                  result.setTextWithIcon('', '');
                  if (baseForm.isValid()) {
                      result.setTextWithIcon('正在处理，请稍后...', 'x-icon-loading');
                      baseForm.submit({
                          submitEmptyText: false,
                          clientValidation: true,
                          preventWindow: true,
                          url: '/Home/ControlPoint',
                          success: function (form, action) {
                              result.setTextWithIcon(action.result.message, 'x-icon-accept');
                          },
                          failure: function (form, action) {
                              var message = 'undefined error.';
                              if (!Ext.isEmpty(action.result) && !Ext.isEmpty(action.result.message))
                                  message = action.result.message;

                              result.setTextWithIcon(message, 'x-icon-error');
                          }
                      });
                  } else {
                      result.setTextWithIcon('表单填写错误', 'x-icon-error');
                  }
              }
          }, {
              xtype: 'button',
              text: '关闭',
              handler: function (el, e) {
                  var form = controlWnd.getComponent('controlForm'),
                      baseForm = form.getForm();

                  baseForm.reset();
                  controlWnd.close();
              }
          }
        ]
    });

    var adjustWnd = Ext.create('Ext.window.Window', {
        title: '信号遥调参数',
        height: 250,
        width: 400,
        glyph: 0xe613,
        modal: true,
        border: false,
        hidden: true,
        closeAction: 'hide',
        items: [{
            xtype: 'form',
            itemId: 'adjustForm',
            border: false,
            padding: 10,
            items: [
                {
                    itemId: 'point',
                    xtype: 'hiddenfield',
                    name: 'point'
                },
                {
                    itemId: 'type',
                    xtype: 'hiddenfield',
                    name: 'type'
                },
                {
                    itemId: 'adjust',
                    xtype: 'numberfield',
                    name: 'adjust',
                    fieldLabel: '模拟量输出值',
                    value: 0,
                    width: 280,
                    allowOnlyWhitespace: false
                }]
        }],
        buttons: [
          { id: 'adjustResult', xtype: 'iconlabel', text: '' },
          { xtype: 'tbfill' },
          {
              xtype: 'button',
              text: '遥调',
              handler: function (el, e) {
                  var form = adjustWnd.getComponent('adjustForm'),
                      baseForm = form.getForm(),
                      result = Ext.getCmp('adjustResult');

                  result.setTextWithIcon('', '');
                  if (baseForm.isValid()) {
                      result.setTextWithIcon('正在处理，请稍后...', 'x-icon-loading');
                      baseForm.submit({
                          submitEmptyText: false,
                          clientValidation: true,
                          preventWindow: true,
                          url: '/Home/AdjustPoint',
                          success: function (form, action) {
                              result.setTextWithIcon(action.result.message, 'x-icon-accept');
                          },
                          failure: function (form, action) {
                              var message = 'undefined error.';
                              if (!Ext.isEmpty(action.result) && !Ext.isEmpty(action.result.message))
                                  message = action.result.message;

                              result.setTextWithIcon(message, 'x-icon-error');
                          }
                      });
                  } else {
                      result.setTextWithIcon('表单填写错误', 'x-icon-error');
                  }
              }
          }, {
              xtype: 'button',
              text: '关闭',
              handler: function (el, e) {
                  var form = adjustWnd.getComponent('adjustForm'),
                      baseForm = form.getForm();

                  baseForm.reset();
                  adjustWnd.close();
              }
          }
        ]
    });

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
            icon: '/Content/themes/icons/all.png'
        },
        viewConfig: {
            loadMask: true
        },
        store: Ext.create('Ext.data.TreeStore', {
            autoLoad: false,
            nodeParam: 'node',
            proxy: {
                type: 'ajax',
                url: '/Component/GetDevices',
                reader: {
                    type: 'json',
                    successProperty: 'success',
                    messageProperty: 'message',
                    totalProperty: 'total',
                    root: 'data'
                }
            },
            listeners: {
                load: function (me, node, records, successful) {
                    if (successful) {
                        var nodes = [];
                        Ext.Array.each(records, function (item, index, allItems) {
                            nodes.push(item.getId());
                        });

                        if (nodes.length > 0) {
                            $$Rcms.UpdateIcons(leftLayout, nodes);
                        }
                    }
                }
            }
        }),
        listeners: {
            select: function (me, record, index) {
                currentNode = record;
                currentStore.proxy.extraParams.node = record.getId();
                currentStore.loadPage(1);
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

                    if (Ext.isEmpty(search._filterData) === false && Ext.isEmpty(search._filterIndex) === false) {
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
                            url: '/Component/FilterDevicePath',
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

    var centerLayout = Ext.create('Ext.panel.Panel', {
        id: 'dashboard',
        region: 'center',
        border: false,
        bodyCls: 'x-border-body-panel',
        layout: {
            type: 'vbox',
            align: 'stretch'
        },
        items: [
            {
                id: 'active-line',
                xtype: 'panel',
                glyph: 0xe61c,
                title: '信号实时图表',
                collapsible: true,
                collapseFirst: false,
                height: 180
            },
            {
                id: 'active-grid',
                xtype: 'grid',
                collapsible: false,
                collapseFirst: false,
                layout: 'fit',
                margin: '5 0 0 0',
                flex: 1,
                header: {
                    glyph: 0xe612,
                    title: '信号实时测值',
                    items: [
                        {
                            xtype: 'checkboxgroup',
                            width: 240,
                            items: [
                                { xtype: 'checkboxfield', boxLabel: '遥信', inputValue: $$Rcms.Organization.Dic, checked: true, boxLabelCls: 'x-label-header x-form-cb-label' },
                                { xtype: 'checkboxfield', boxLabel: '遥测', inputValue: $$Rcms.Organization.Aic, checked: true, boxLabelCls: 'x-label-header x-form-cb-label' },
                                { xtype: 'checkboxfield', boxLabel: '遥调', inputValue: $$Rcms.Organization.Aoc, checked: true, boxLabelCls: 'x-label-header x-form-cb-label' },
                                { xtype: 'checkboxfield', boxLabel: '遥控', inputValue: $$Rcms.Organization.Doc, checked: true, boxLabelCls: 'x-label-header x-form-cb-label' }
                            ],
                            listeners: {
                                change: function (me, newValue, oldValue) {
                                    var types = [];
                                    Ext.Object.each(newValue, function (key, value, myself) {
                                        types.push(value);
                                    });

                                    currentStore.proxy.extraParams.types = types;
                                    currentStore.loadPage(1);
                                }
                            }
                        }

                    ]
                },
                tools: [
                    {
                        type: 'refresh',
                        tooltip: '刷新',
                        handler: function (event, toolEl, panelHeader) {
                            currentStore.loadPage(1);
                        }
                    }
                ],
                store: currentStore,
                viewConfig: {
                    loadMask: false,
                    preserveScrollOnRefresh: true,
                    stripeRows: true,
                    trackOver: true,
                    emptyText: '<h1 style="margin:20px">没有数据记录</h1>',
                    getRowClass: function (record, rowIndex, rowParams, store) {
                        return $$Rcms.GetPointStatusCls(record.get("status"));
                    }
                },
                features: [{
                    ftype: 'grouping',
                    groupHeaderTpl: '{columnName}: {name} ({rows.length}条)',
                    hideGroupedHeader: false,
                    startCollapsed: false
                }],
                columns: [
                    {
                        text: '信号状态',
                        dataIndex: 'statusDisplay',
                        align: 'center',
                        renderer: function (value, meta, record) {
                            var cls = $$Rcms.GetPointStatusCls(record.get("status"));
                            return '<span class="cell-marker ' + cls + '"></span>' + value;
                        }
                    },
                    { text: '信号类型', dataIndex: 'typeDisplay', align: 'center' },
                    { text: '信号名称', dataIndex: 'name', width:150 },
                    { text: '信号测值', dataIndex: 'value' },
                    { text: '单位/描述', dataIndex: 'valueDisplay', align: 'center' },
                    { text: '值变时间', dataIndex: 'timestamp', width: 150, align: 'center' },
                    {
                        xtype: 'actioncolumn',
                        width: 80,
                        align: 'center',
                        menuDisabled: true,
                        menuText: '操作',
                        text: '操作',
                        items: [{
                            tooltip: '遥控',
                            getClass: function (v, metadata, r, rowIndex, colIndex, store) {
                                return (r.get('type') === $$Rcms.Organization.Doc) ? 'x-cell-icon x-icon-remote-control' : 'x-cell-icon x-icon-hidden';
                            },
                            handler: function (view, rowIndex, colIndex, item, event, record) {
                                view.getSelectionModel().select(record);
                                var form = controlWnd.getComponent('controlForm'),
                                    point = form.getComponent('point'),
                                    type = form.getComponent('type'),
                                    result = Ext.getCmp('controlResult');

                                point.setValue(record.getId());
                                type.setValue(record.get('type'));
                                result.setTextWithIcon('', '');
                                controlWnd.show();
                            }
                        }, {
                            tooltip: '遥调',
                            getClass: function (v, metadata, r, rowIndex, colIndex, store) {
                                return (r.get('type') === $$Rcms.Organization.Aoc) ? 'x-cell-icon x-icon-remote-setting' : 'x-cell-icon x-icon-hidden';
                            },
                            handler: function (view, rowIndex, colIndex, item, event, record) {
                                view.getSelectionModel().select(record);
                                var form = adjustWnd.getComponent('adjustForm'),
                                    point = form.getComponent('point'),
                                    type = form.getComponent('type'),
                                    result = Ext.getCmp('adjustResult');

                                point.setValue(record.getId());
                                type.setValue(record.get('type'));
                                result.setTextWithIcon('', '');
                                adjustWnd.show();
                            }
                        }]
                    }
                ],
                listeners: {
                    selectionchange: function (model, selected) {
                        resetChart();
                        if (selected.length > 0) {
                            loadChart(selected[0]);
                        }
                    }
                }
            }
        ]
    });

    var loadChart = function (record) {
        if (!Ext.isEmpty(record)) {
            var maxcount = 90,
                timestamp = record.get('shortTime'),
                point = record.get('point'),
                value = record.get('value'),
                unit = record.get('valueDisplay');

            if (value === 'NULL')
                value = 0;

            if (lineOption.series[0].data.length >= maxcount) {
                lineOption.series[0].data.shift();
                lineOption.xAxis[0].data.shift();
            }

            lineOption.series[0].name = unit;
            lineOption.series[0].data.push(value);
            lineOption.xAxis[0].data.push(timestamp);
            lineChart.setOption(lineOption, true);
        }
    };

    var resetChart = function (fill) {
        fill = fill || false;

        lineOption.series[0].name = '';
        lineOption.series[0].data = fill ? [0] : [];
        lineOption.xAxis[0].data = fill ? ['00′00″'] : [];
        lineChart.setOption(lineOption, true);
    };

    Ext.onReady(function () {
        $$Rcms.Tasks.actPointTask.run = function () {
            currentStore.loadPage(1);
            $$Rcms.UpdateIcons(leftLayout, null);
        };
        $$Rcms.Tasks.actOrderTask.run = function () {
            Ext.Ajax.request({
                url: '/Home/SetOrders',
                params: {
                    node: (currentNode !== null ? currentNode.getId() : '-1')
                },
                success: function (response, options) {
                    $$Rcms.Tasks.actOrderTask.fireOnStart = false;
                    $$Rcms.Tasks.actOrderTask.restart();
                }
            });
        };
        $$Rcms.Tasks.actPointTask.start();
        $$Rcms.Tasks.actOrderTask.start();

        /*add components to viewport panel*/
        var pageContentPanel = Ext.getCmp('center-content-panel-fw');
        if (!Ext.isEmpty(pageContentPanel)) {
            pageContentPanel.add([leftLayout, centerLayout]);
        }
        
        lineChart = echarts.init(Ext.getCmp('active-line').body.dom, 'shine');
        lineChart.setOption(lineOption);
    });
})();