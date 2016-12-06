/**
 * Creates an anguilla command using a wrapper shorthand. Command is responsible for communicating wtih api controller
 * to get the core service api version.
 *
 * Note the ${PluginName} will get replaced by the actual plugin name.
 */
Alchemy.command("${PluginName}", "UsageReport", {
    /**
     * If an init function is created, this will be called from the command's constructor when a command instance
     * is created.
     */
    init: function () {
    },

    /**
     * Whether or not the command is enabled for the user (will usually have extensions displayed but disabled).
     * @returns {boolean}
     */
    isEnabled: function (selection) {
        var items = selection.getItems();
        if (items.length === 1) {
            var itemType = $models.getItemType(selection.getItem(0));
            return itemType === $const.ItemType.FOLDER ||
                itemType === $const.ItemType.STRUCTURE_GROUP;
        }
        else {
            return false;
        }
    },

    /**
     * Whether or not the command is available to the user.This impacts the context menu option but not the
     ribbon bar.
     * @returns {boolean}
     */
    isAvailable: function (selection) {
        return this.isEnabled(selection);
    },

    /**
     * Executes your command. You can use _execute or execute as the property name.
     */
    execute: function (selection) {

        // Gets the item id and its title
        var itemId = selection.getItem(0);

        // Special characters in title are filtered out for making Web Api calls work. Title is used for display to user.
        var title = $models.getItem(itemId).getStaticTitle().replace(/[^0-9A-Z \-_]+/gi, "");

        // Sets the url of a popup window, passing through params for the ID and Title of the selected item
        var url = "${ViewsUrl}UsageReport.aspx?uri=" + itemId + "&title=" + title;

        // Creates a popup with the above URL
        var popup = $popup.create(url, "menubar=no,location=no,resizable=no,scrollbars=no,status=no,width=700,height=450,top=10,left=10", null);
        popup.open();
    }
});