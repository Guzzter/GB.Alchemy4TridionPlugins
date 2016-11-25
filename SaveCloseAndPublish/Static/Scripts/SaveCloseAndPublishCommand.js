/**
 * Creates an anguilla command using a wrapper shorthand.
 *
 * Note the ${PluginName} will get replaced by the actual plugin name.
 */
Alchemy.command("${PluginName}", "SaveCloseAndPublish", {
    /**
     * If an init function is created, this will be called from the command's constructor when a command instance
     * is created.
     */
    init: function () {
    },

    isAvailable: function (selection, pipeline) {
        var isPage = this._getParameterByName("tcm") === "64";
        var saveCloseCmd = $commands.getCommand("SaveClose");
        var isSaveClosePossible = false;
        if (saveCloseCmd) {
            isSaveClosePossible = saveCloseCmd.isAvailable();
        }

        return isPage && isSaveClosePossible;
    },

    isEnabled: function (selection, pipeline) {

        // Always enable when command is available
        return this.isAvailable(selection, pipeline);
    },

    /**
     * Executes your command. You can use _execute or execute as the property name.
     */
    execute: function (selection, pipeline) {
        var progress = $messages.registerProgress("Save Close And Publish", null);
        var item = $models.getItem(selection.getItem(0));
        debugger;
        console.log(opener);

        var saveCloseCmd = $commands.getCommand("SaveClose");
        if (saveCloseCmd) {
            saveCloseCmd.invoke(selection, pipeline);
            var publishCmd = $commands.getCommand("Publish");
            if (publishCmd) {

                //publishCmd.invoke(selection, pipeline);
                _openPublishPopup(item);
            }
        }

        /* TODO: make call and see if there is only one target */
        /* Config: add default target names + publish prio */
        /* Config: open publish queue dialog after publish */
        /*

        // This is the Promise pattern that the webapi proxy js exposes. Look at another example to
        // see how the callback method can also be used. Your WebAPI controller's route and route prefix
        // attributes controls how the namespace is generated.
        Alchemy.Plugins["${PluginName}"].Api.ExportService.userListToCsv()
            .success(function (message) {

                // first arg in success is what's returned by your controller's action
                //$messages.registerGoal(message);
            })
            .error(function (type, error) {

                // first arg is string that shows the type of error ie (500 Internal), 2nd arg is object representing
                // the error.  For BadRequests and Exceptions, the error message will be in the error.message property.
                $messages.registerError("There was an error", error.message);
            })
            .complete(function () {

                // this is called regardless of success or failure.
                progress.finish();
            });*/
    },
    _getParameterByName: function (name, url) {
        if (!url) {
            url = window.location.href;
        }
        name = name.replace(/[\[\]]/g, "\\$&");
        var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, " "));
    },
    /**
     * Opens the publish popup.
     * @param {Array} items Items to publish.
     */
    _openPublishPopup: function (items) {

        // Build params
        var p = this.properties;
        debugger;
        var doRepublish = false;
        for (var i = 0, cnt = items.length; i < cnt; i++) {
            var type = $models.getItemType(items[i]);
            if (type == $const.ItemType.PUBLICATION || type == $const.ItemType.STRUCTURE_GROUP ||
        type == $const.ItemType.PAGE_TEMPLATE || type == $const.ItemType.COMPONENT_TEMPLATE ||
        type == $const.ItemType.VIRTUAL_FOLDER) // For Bundles, which are of item type "Virtual Folder"
            {
                doRepublish = true;
                break;
            }
        }
        var params = { command: "publish", items: items, republish: doRepublish, userWorkflow: false };

        p.popup = $popupManager.createExternalContentPopup(Tridion.Web.UI.Editors.CME.Constants.Popups.PUBLISH.URL, Tridion.Web.UI.Editors.CME.Constants.Popups.PUBLISH.FEATURES, params);

        $evt.addEventHandler(p.popup, "unload",
        function Publish$_execute$_unload(event) {
            if (p.popup) {
                p.popup.dispose();
                p.popup = null;
            }
        });

        $evt.addEventHandler(p.popup, "error",
        function Publish$_execute$_error(event) {
            $messages.registerError(event.data.error.Message, null, null, null, true);

            if (p.popup) {
                p.popup.dispose();
                p.popup = null;
            }
        });

        $evt.addEventHandler(p.popup, "publish",
        function Publish$_execute$_published(event) {
            var item = $models.getItem(event.data.item);
            $messages.registerNotification(Tridion.Utils.Localization.getCmeEditorResource("PublishPopupSentToPublishQueue",
                            item ? item.getStaticTitle() || item.getTitle() || item.getId() : event.data.item));

            if (p.popup) {
                p.popup.dispose();
                p.popup = null;
            }
        });

        $evt.addEventHandler(p.popup, "multipublish", this.getDelegate(this._onMultiPublish));

        p.popup.open();
    }
});