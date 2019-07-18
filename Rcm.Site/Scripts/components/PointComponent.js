/* ========================================================================
 * Components: PointComponent.js
 * /Scripts/components/PointComponent.js
 * ========================================================================
 */

Ext.define("Ext.ux.PointMultiCombo", {
    extend: "Ext.ux.MultiCombo",
    xtype: "PointMultiCombo",
    fieldLabel: '信号名称',
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
        me.storeUrl = '/Component/GetPoints';
        me.callParent(arguments);
        me.store.proxy.extraParams.device = 0;
        me.store.load();
    },
    bind: function (device, _ai, _ao, _di, _do) {
        var me = this;
        if (Ext.isEmpty(device)) return false;

        me.store.proxy.extraParams.device = device;
        me.store.proxy.extraParams._ai = _ai || false;
        me.store.proxy.extraParams._ao = _ao || false;
        me.store.proxy.extraParams._di = _di || false;
        me.store.proxy.extraParams._do = _do || false;
        me.store.load();
    }
});

Ext.define("Ext.ux.PointComboBox", {
    extend: "Ext.ux.SingleCombo",
    xtype: "PointCombo",
    fieldLabel: '信号名称',
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
        me.storeUrl = '/Component/GetPoints';
        me.callParent(arguments);
        me.store.proxy.extraParams.device = 0;
        me.store.load({
            scope: me,
            callback: function (records, operation, success) {
                if (success && records.length > 0)
                    me.select(records[0]);
            }
        });
    },
    bind: function (device, _ai, _ao, _di, _do) {
        var me = this;
        if (Ext.isEmpty(device)) return false;

        me.store.proxy.extraParams.device = device;
        me.store.proxy.extraParams._ai = _ai || false;
        me.store.proxy.extraParams._ao = _ao || false;
        me.store.proxy.extraParams._di = _di || false;
        me.store.proxy.extraParams._do = _do || false;
        me.store.load({
            scope: me,
            callback: function (records, operation, success) {
                if (success && records.length > 0)
                    me.select(records[0]);
            }
        });
    }
});