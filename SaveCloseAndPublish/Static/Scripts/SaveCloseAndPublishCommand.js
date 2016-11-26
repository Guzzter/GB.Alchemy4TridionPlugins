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
        function Save$execute$onSave(event) {

            //console.log(event);
            var selected = $display.getItem().getVersionlessId();
            $display.getItem().checkIn(true);
            publishAndClose(selected);
        }

        function publishAndClose(tcm) {
            var progress = $messages.registerProgress('Publishing ' + tcm, null);

            //console.log("publishAndClose = " + tcm);

            // This is the Promise pattern that the webapi proxy js exposes. Look at another example to
            // see how the callback method can also be used. Your WebAPI controller's route and route prefix
            // attributes controls how the namespace is generated.
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
        }

        var item = $display.getItem();
        var isChanged = !item.isReadOnly() && item.getChanged() && !item.isLoading();

        //console.log("isChanged = " + isChanged);
        if (isChanged) {
            $evt.addEventHandler(item, "save", Save$execute$onSave);
            $commands.getCommand("Save").invoke(selection, pipeline);
        } else {
            item.undoCheckOut(true);
            publishAndClose(item.getVersionlessId());
        }
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
    }
});