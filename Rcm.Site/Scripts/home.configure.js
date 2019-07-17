(function () {
    Ext.onReady(function () {
        var hcontent = Ext.create('Ext.panel.Panel', {
            region: 'center'
        });

        /*add components to viewport panel*/
        var pageContentPanel = Ext.getCmp('center-content-panel-fw');
        if (!Ext.isEmpty(pageContentPanel)) {
            pageContentPanel.add(hcontent);
        }
    });
})();