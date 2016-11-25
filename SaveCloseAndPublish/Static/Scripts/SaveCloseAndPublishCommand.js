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
        var progress = $messages.registerProgress("Saving and publishing...", null);
        var tcm = selection.getItem(0).replace("tcm:", "");

        // This is the Promise pattern that the webapi proxy js exposes. Look at another example to
        // see how the callback method can also be used. Your WebAPI controller's route and route prefix
        // attributes controls how the namespace is generated.
        Alchemy.Plugins["${PluginName}"].Api.PublishService.saveAndPublish(tcm)
            .success(function (message) {

                // first arg in success is what's returned by your controller's action
                $messages.registerGoal(message);

                debugger;
                $commands.getCommand("CheckOut").invoke(selection, pipeline);
                $commands.getCommand("SaveClose").invoke(selection, pipeline);
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