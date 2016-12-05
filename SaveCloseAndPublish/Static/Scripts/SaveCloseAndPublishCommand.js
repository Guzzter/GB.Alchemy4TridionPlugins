/**
 * Creates an anguilla command using a wrapper shorthand.
 *
 * Note the ${PluginName} will get replaced by the actual plugin name.
 */
Alchemy.command("${PluginName}", "SaveCloseAndPublish", {
    /**
     * If an init function is created, this will be called from the command's constructor when a command instance
     * is created.
     * /WebUI/Editors/CME/Views/Page/Page_v8.1.0.194.289_.aspx?mode=js
     */
    init: function () {
    },
    isAvailable: function (selection, pipeline) {
        var type = $models.getItemType(selection.getItem(0));
        var isPage = (type === $const.ItemType.PAGE);
        console.log(type);

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
        function Save$execute$onSave(event) {
            var item = $display.getItem();
            var selected = item.getVersionlessId();
            item.checkIn(true);
            publishAndClose(selected);
        }

        function publishAndClose(tcm) {

            //console.log("publishAndClose = " + tcm);

            // This is the Promise pattern that the webapi proxy js exposes. Look at another example to
            // see how the callback method can also be used. Your WebAPI controller's route and route prefix
            // attributes controls how the namespace is generated.
            Alchemy.Plugins["${PluginName}"].Api.getSettings()
                .success(function (settings) {
                    if (settings.PublishPrio && settings.PublishTargetNamesCsv && settings.PublishTargetNamesCsv !== "") {

                        // Pub target configured: publish directly without showing publish dialog
                        var progress = $messages.registerProgress('Auto publishing ' + tcm + ' with prio ' + settings.PublishPrio, null);
                        Alchemy.Plugins["${PluginName}"].Api.PublishService.saveAndPublish(tcm.replace("tcm:", ""))
                            .success(function (message) {

                                // first arg in success is what's returned by your controller's action
                                $messages.registerGoal(message);
                                window.close();
                            })
                            .error(function (type, error) {

                                // first arg is string that shows the type of error ie (500 Internal), 2nd arg is object representing
                                // the error.  For BadRequests and Exceptions, the error message will be in the error.message property.
                                $messages.registerError("There was an error", error.message);
                            })
                            .complete(function () {

                                // this is called regardless of success or failure.
                                progress.finish();
                            });
                    } else {

                        // Show publish dialog
                        var params = {
                            command: "publish",
                            items: [tcm],
                            republish: false,
                            userWorkflow: false
                        };
                        var popup = $popupManager.createExternalContentPopup(Tridion.Web.UI.Editors.CME.Constants.Popups.PUBLISH.URL, Tridion.Web.UI.Editors.CME.Constants.Popups.PUBLISH.FEATURES, params);

                        var handleCancel = function PopupManager$createDisposingExternalContentPopup$onPopupCanceled(event) {
                            popup.close();
                        };

                        var handleClosed = function PopupManager$createDisposingExternalContentPopup$handleClosed(event) {

                            //$popupManager.properties.popups = {};
                            window.close();
                        };

                        // Maybe not all popups have a cancel event, but this makes it easier in case the popup does.
                        $evt.addEventHandler(popup, "cancel", handleCancel);
                        $evt.addEventHandler(popup, "closed", handleClosed);

                        popup.open();
                    }
                })
                .error(function (error) {
                    console.error("Could not load settings for SaveCloseAndPublish");
                });
        }

        var item = $display.getItem();
        var isChanged = !item.isReadOnly() && item.getChanged() && !item.isLoading();

        //console.log('isChanged = ' + isChanged);

        if (isChanged) {
            $evt.addEventHandler(item, "save", Save$execute$onSave);
            $commands.getCommand("Save").invoke(selection, pipeline);
        } else {
            item.undoCheckOut(true);
            publishAndClose(item.getVersionlessId());
        }
    }
});