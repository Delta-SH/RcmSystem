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
                    height: 68,
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