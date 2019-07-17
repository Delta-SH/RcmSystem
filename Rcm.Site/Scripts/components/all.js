Ext.define("Ext.ux.AlarmLevelMultiCombo", {
    extend: "Ext.ux.MultiCombo",
    xtype: "AlarmLevelMultiCombo",
    fieldLabel: '告警级别',
    valueField: 'id',
    displayField: 'text',
    delimiter: $$Rcms.Delimiter,
    queryMode: 'local',
    triggerAction: 'all',
    selectionMode: 'all',
    forceSelection: true,
    labelWidth: 60,
    width: 220,
    initComponent: function () {
        var me = this;
        me.storeUrl = '/Component/GetAlarmLevels';
        me.callParent(arguments);
        me.store.load();
    }
});

Ext.define("Ext.ux.AlarmLevelComboBox", {
    extend: "Ext.ux.SingleCombo",
    xtype: "AlarmLevelCombo",
    fieldLabel: '告警级别',
    displayField: 'text',
    valueField: 'id',
    typeAhead: true,
    queryMode: 'local',
    triggerAction: 'all',
    selectOnFocus: true,
    forceSelection: true,
    labelWidth: 60,
    width: 220,
    initComponent: function () {
        var me = this;
        me.storeUrl = '/Component/GetAlarmLevels';
        me.callParent(arguments);
        me.store.load();
    }
});

/* ========================================================================
 * Components: AreaComponent.js
 * /Scripts/components/AreaComponent.js
 * ========================================================================
 */

Ext.define("Ext.ux.AreaMultiTreePanel", {
    extend: "Ext.ux.TreePicker",
    xtype: "AreaMultiPicker",
    fieldLabel: '所属区域',
    displayField: 'text',
    labelWidth: 60,
    width: 280,
    multiSelect: true,
    searchVisible: true,
    //fastSelectVisible: false,
    initComponent: function () {
        var me = this;

        me.storeUrl = '/Component/GetAreas';
        me.searchUrl = '/Component/FilterAreaPath';
        me.queryUrl = '/Component/GetAreaPath';

        me.callParent(arguments);
        me.store.load();
    }
});

Ext.define("Ext.ux.AreaTreePanel", {
    extend: "Ext.ux.TreePicker",
    xtype: "AreaPicker",
    fieldLabel: '所属区域',
    displayField: 'text',
    labelWidth: 60,
    width: 280,
    selectAll: true,
    multiSelect: false,
    searchVisible: true,
    initComponent: function () {
        var me = this;

        me.storeUrl = '/Component/GetAreas';
        me.searchUrl = '/Component/FilterAreaPath';
        me.queryUrl = '/Component/GetAreaPath';
        me.rootVisible = me.selectAll;

        me.callParent(arguments);
        me.store.load({
            scope: me,
            callback: function (records, operation, success) {
                if (!me.selectAll) return;
                me.setValue('root');
                var root = me.findRoot();
                if (root) root.expand();
            }
        });
    }
});

/* ========================================================================
 * Components: DeviceComponent.js
 * /Scripts/components/DeviceComponent.js
 * ========================================================================
 */

Ext.define("Ext.ux.DeviceMultiTreePanel", {
    extend: "Ext.ux.TreePicker",
    xtype: "DeviceMultiPicker",
    fieldLabel: '设备名称',
    displayField: 'text',
    labelWidth: 60,
    width: 280,
    multiSelect: true,
    searchVisible: true,
    //fastSelectVisible: true,
    initComponent: function () {
        var me = this;

        me.storeUrl = '/Component/GetDevices';
        me.searchUrl = '/Component/FilterDevicePath';
        me.queryUrl = '/Component/GetDevicePath';

        me.callParent(arguments);
        me.store.load();
    }
});

Ext.define("Ext.ux.DeviceTreePanel", {
    extend: "Ext.ux.TreePicker",
    xtype: "DevicePicker",
    fieldLabel: '设备名称',
    displayField: 'text',
    labelWidth: 60,
    width: 280,
    selectAll: true,
    multiSelect: false,
    searchVisible: true,
    initComponent: function () {
        var me = this;

        me.storeUrl = '/Component/GetDevices';
        me.searchUrl = '/Component/FilterDevicePath';
        me.queryUrl = '/Component/GetDevicePath';
        me.rootVisible = me.selectAll;

        me.callParent(arguments);
        me.store.load({
            scope: me,
            callback: function (records, operation, success) {
                if (!me.selectAll) return;
                me.setValue('root');
                var root = me.findRoot();
                if (root) root.expand();
            }
        });
    }
});

Ext.define("Ext.ux.DeviceTypeMultiCombo", {
    extend: "Ext.ux.MultiCombo",
    xtype: "DeviceTypeMultiCombo",
    fieldLabel: '设备类型',
    valueField: 'id',
    displayField: 'text',
    delimiter: $$Rcms.Delimiter,
    queryMode: 'local',
    triggerAction: 'all',
    selectionMode: 'all',
    forceSelection: true,
    labelWidth: 60,
    width: 220,
    initComponent: function () {
        var me = this;
        me.storeUrl = '/Component/GetDeviceTypes';
        me.callParent(arguments);
        me.store.load();
    }
});

Ext.define("Ext.ux.DeviceTypeComboBox", {
    extend: "Ext.ux.SingleCombo",
    xtype: "DeviceTypeCombo",
    fieldLabel: '设备类型',
    displayField: 'text',
    valueField: 'id',
    typeAhead: true,
    queryMode: 'local',
    triggerAction: 'all',
    selectOnFocus: true,
    forceSelection: true,
    labelWidth: 60,
    width: 220,
    initComponent: function () {
        var me = this;
        me.storeUrl = '/Component/GetDeviceTypes';
        me.callParent(arguments);
        me.store.load();
    }
});

/* ========================================================================
 * Components: StationComponent.js
 * /Scripts/components/StationComponent.js
 * ========================================================================
 */

Ext.define("Ext.ux.StationMultiTreePanel", {
    extend: "Ext.ux.TreePicker",
    xtype: "StationMultiPicker",
    fieldLabel: '所属站点',
    displayField: 'text',
    labelWidth: 60,
    width: 280,
    multiSelect: true,
    searchVisible: true,
    //fastSelectVisible: true,
    initComponent: function () {
        var me = this;

        me.storeUrl = '/Component/GetStations';
        me.searchUrl = '/Component/FilterStationPath';
        me.queryUrl = '/Component/GetStationPath';

        me.callParent(arguments);
        me.store.load();
    }
});

Ext.define("Ext.ux.StationTreePanel", {
    extend: "Ext.ux.TreePicker",
    xtype: "StationPicker",
    fieldLabel: '所属站点',
    displayField: 'text',
    labelWidth: 60,
    width: 280,
    selectAll: true,
    multiSelect: false,
    searchVisible: true,
    initComponent: function () {
        var me = this;

        me.storeUrl = '/Component/GetStations';
        me.searchUrl = '/Component/FilterStationPath';
        me.queryUrl = '/Component/GetStationPath';
        me.rootVisible = me.selectAll;

        me.callParent(arguments);
        me.store.load({
            scope: me,
            callback: function (records, operation, success) {
                if (!me.selectAll) return;
                me.setValue('root');
                var root = me.findRoot();
                if (root) root.expand();
            }
        });
    }
});

Ext.define("Ext.ux.StationTypeMultiCombo", {
    extend: "Ext.ux.MultiCombo",
    xtype: "StationTypeMultiCombo",
    fieldLabel: '站点类型',
    valueField: 'id',
    displayField: 'text',
    delimiter: $$Rcms.Delimiter,
    queryMode: 'local',
    triggerAction: 'all',
    selectionMode: 'all',
    forceSelection: true,
    labelWidth: 60,
    width: 220,
    initComponent: function () {
        var me = this;
        me.storeUrl = '/Component/GetStationTypes';
        me.callParent(arguments);
        me.store.load();
    }
});

Ext.define("Ext.ux.StationTypeComboBox", {
    extend: "Ext.ux.SingleCombo",
    xtype: "StationTypeCombo",
    fieldLabel: '站点类型',
    displayField: 'text',
    valueField: 'id',
    typeAhead: true,
    queryMode: 'local',
    triggerAction: 'all',
    selectOnFocus: true,
    forceSelection: true,
    labelWidth: 60,
    width: 220,
    initComponent: function () {
        var me = this;
        me.storeUrl = '/Component/GetStationTypes';
        me.callParent(arguments);
        me.store.load();
    }
});