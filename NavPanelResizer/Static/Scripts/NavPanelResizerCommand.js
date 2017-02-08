/**
 * Creates an anguilla command using a wrapper shorthand.
 *
 * Note the ${PluginName} will get replaced by the actual plugin name.
 */
Alchemy.command("${PluginName}", "NavPanelResizer", {
    /**
     * If an init function is created, this will be called from the command's constructor when a command instance
     * is created.
     * /WebUI/Editors/CME/Views/Page/Page_v8.1.0.194.289_.aspx?mode=js
     */
    init: function () {
        Alchemy.Plugins["${PluginName}"].Api.getSettings()
                .success(function (settings) {
                    var size = settings.InitialSize;
                    if (size) {

                        // console.log("NavPanelResizer - Set NavPanel to: " + size);
                        if (size.endsWith("%")) {
                            var sizeWidthPercentage = parseInt(size[size.length - 1]);
                            if (!isNaN(sizeWidthPercentage)) {
                                sizeWidthPercentage = sizeWidthPercentage / 100;
                                var newWidth = (self.innerWidth * sizeWidthPercentage);
                                console.log("NavPanelResizer - Set NavPanel to: " + newWidth);
                                $("div#NavigationPanel").style.width = (newWidth + 'px');
                            } else {
                                console.log("NavPanelResizer - Could not parse percentage");
                            }
                        } else if (size.endsWith("px")) {
                            $("div#NavigationPanel").style.width = '400px';
                        } else {
                            var sizeWidth = parseInt(size);
                            if (!isNaN(sizeWidth)) {
                                $("div#NavigationPanel").style.width = sizeWidth + 'px';
                            }
                        }
                    } else {
                        $messages.registerError("There was an error", "No valid size found in NavPanelResizer settings. (Examples: 20%, 400px or 300)");
                    }
                })
                .error(function (error) {
                    console.log("Could not load settings for NavPanelResizer");
                });
    },
    isAvailable: function (selection, pipeline) {

        // No GUI option
        return false;
    },

    isEnabled: function (selection, pipeline) {

        // No GUI option
        return false;
    },

    /**
     * Executes your command. You can use _execute or execute as the property name.
     */
    execute: function (selection, pipeline) {
    }
});