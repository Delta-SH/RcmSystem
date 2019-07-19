(function () {
    Ext.onReady(function () {
        var hcontent = Ext.create('Ext.panel.Panel', {
            region: 'center',
            layout: {
                type: 'hbox',
                padding: '5',
                pack: 'center',
                align: 'middle'
            },
            border: false,
            items: [
                {
                    xtype: 'box',
                    cls:'alert alert-info',
                    html: '<span class="iconfont icon-tips" aria-hidden="true"></span> 即将上线，敬请期待！'
                }
            ]
        });

        /*add components to viewport panel*/
        var pageContentPanel = Ext.getCmp('center-content-panel-fw');
        if (!Ext.isEmpty(pageContentPanel)) {
            pageContentPanel.add(hcontent);
        }
    });
})();