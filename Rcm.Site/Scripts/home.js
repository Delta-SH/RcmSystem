(function () {
    var currentLayout = Ext.create('Ext.panel.Panel', {
        header: false,
        border: false,
        region: 'center',
        contentEl: 'welcome'
    });

    Ext.onReady(function () {
        /*add components to viewport panel*/
        var pageContentPanel = Ext.getCmp('center-content-panel-fw');
        if (!Ext.isEmpty(pageContentPanel)) {
            pageContentPanel.add(currentLayout);
        }
    });
})();