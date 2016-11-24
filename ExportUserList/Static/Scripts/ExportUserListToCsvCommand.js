/**
 * Creates an anguilla command using a wrapper shorthand.
 *
 * Note the ${PluginName} will get replaced by the actual plugin name.
 */
Alchemy.command("${PluginName}", "ExportUserListToCsv", {
    /**
     * If an init function is created, this will be called from the command's constructor when a command instance
     * is created.
     */
    init: function () {
    },

    isAvailable: function (selection, pipeline) {

        // Check if newUser is enabled: then you have rights to export users
        return $commands.getCommand("NewUser").isAvailable(selection, pipeline);
    },

    isEnabled: function (selection, pipeline) {

        // Check if newUser is enabled: then you have rights to export users
        return this.isAvailable();
    },

    /**
     * Executes your command. You can use _execute or execute as the property name.
     */
    execute: function () {
        var progress = $messages.registerProgress("Getting ready to export users...", null);
        var d = new Date();
        var n = d.getTime();
        window.open('/Alchemy/Plugins/Export_User_List/api/ExportService/UserListToCsv?__t=' + n, '_blank');
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
    }
});