/**
 * Handles all functionality of my popup window, including retrieving the where used and using functionality
 * and manipulating the tabs.
 *
 * Note the self executing function wrapping the JS. This is to limit the scope of my variables and avoid
 * conflicts with other scripts.
 */
!(function () {
    debugger;

    // Alchemy comes with jQuery and several other libraries already pre-installed so assigning
    // it a variable here eliminates the redundancy of loading my own copy, and avoids any conflicts over
    // the $ character
    $j = Alchemy.library("JQuery");

    //Alchemy.Resources.Libs.JQuery
    // Grabs the URL of the popup and gets the TCM of the item selected in Tridion from the querystring
    var url = location.href;
    var tcm = location.href.substring(url.indexOf("uri=tcm%3A") + 10, url.indexOf("&"));

    // From the page url we want to get the title so we can update our first tab with the title
    var title = location.href.substring(url.indexOf("title=") + 6, url.indexOf("#"));

    // My plugin only has one setting, used to determine how many items we should look through when searching
    // for pages using an item. This eliminates the risk of infinite loops with component links creating loops
    // Here I set a variable that I will later populate with the settings
    var pluginSettings = "";

    // On page load I display the items using and being used by the selected item by calling this function
    // I will later be calling this function recursively, for this go the isRecursive value is false
    updateUsingAndUsedItems(tcm, title, false);

    /**
     * Takes a TCM ID for a Tridion item and retrieves the used and using items list, updating the display with
     * a new tab. Also takes a boolean "isRecursive". This is because it will be calling itself when
     * a new item is selected and and we want to see its used and using items. The first time we run the function
     * we need to do some special manipulations that don't need to be done on subsequent calls of the funtion.
     */
    function updateUsingAndUsedItems(tcmInput, title, isRecursive) {

        // This is the call to my controller where the core service code is used to gather the
        // where used and using information. It is returned as a string of HTML
        Alchemy.Plugins["${PluginName}"].Api.UsageReportService.getUseCountList(tcmInput, title)
            .success(function (items) {

                // First arg in success is what's returned by your controller's action
                // I get the settings for my plugin right away so I have them for later in this function
                //getSettings();

                // Add my returned HTML string to the active tab
                if (!$j(".tab-body.active .results").length) {
                    $j(".tab-body.active").append(items);
                }

                // These are the steps we need to run only on the first time through the function.
                if (!isRecursive) {

                    // Replace encoded spaces with an actual spac
                    title = title.replace(/\%20/g, ' ');

                    // Find the first tab and set its data-tcm value to be the tcm of the input item.
                    // This attribute is used to pair the tab at the top of the window with the contents
                    // to be displayed when they are selected.
                    $j(".tabs>div.active").attr("data-tcm", tcmInput);

                    // Update the tab title
                    $j(".tabs>div.active").html(title);

                    // Update the first tab body with the same data-tcm as its tab
                    $j(".tab-body").attr("data-tcm", tcmInput);
                }

                // Set the click action for the active tab, since we need to add the function each time
                // a new tab is added and a new tab will always be active
                $j(".tabs>div.active").click(function (e) {

                    // When there are multiple tabs the active tab will have a close button.
                    // We don't want our action to trigger when we click the close button since it has its
                    // own action, so we first find the close button element.
                    var close = $j(this).find(".close");

                    if (!close.is(e.target) // if the target of the click isn't the close...
                    && close.has(e.target).length === 0) // ... nor a descendant of the container
                    {
                        // When we click on a tab we want to first deactivate the currently active tab
                        $j(".tabs>div.active").removeClass("active");

                        // Then we want to activate the clicked tab
                        $j(this).addClass("active");

                        // Next we deactivate the currently active tab contents
                        $j(".tab-body.active").removeClass("active");

                        // Get the data-tcm attribute from the clicked tab
                        var id = $j(this).attr("data-tcm");

                        // And then find the matching tab contents to activate them
                        $j(".tab-body[data-tcm=" + id + "]").addClass("active");
                    }
                });

                // Similar to above, we want to have an action when we click anywhere on the tab body
                // that isn't a used or using item
                $j(".tab-body").mouseup(function (e) {

                    // To do this we first find the results item containing the used and using items
                    var results = $j(".results");
                    if (!results.is(e.target) // if the target of the click isn't the results...
                    && results.has(e.target).length === 0) // ... nor a descendant of the results
                    {
                        // Call a function to deselect the current item
                        deselectItems();
                    }
                });

                // An item is a Tridion item that is being used or is using the current item.
                // This is the click function for the items.
                $j(".item").click(function () {

                    // When you click on an item we deselect any currently selected item
                    $j(".item.selected").removeClass("selected");

                    // And select the item you clicked on
                    $j(this).addClass("selected");

                    // We then use this function to enable the buttons since they are only enabled
                    // when an item is selected
                    enableButtons();

                    // This is another set of functions we only want to run the first time through the
                    // function since we don't need to set the button functions each time.
                    // These are all the click functions for the buttons at the bottom of the plugin.
                    // They get set when we click on an item because we only want them to happen when
                    // the buttons are enabled and the buttons only get enabled when an item is selected.
                    if (!isRecursive) {

                        // Where used and using button click function
                        $j("#where_used_using.enabled").click(function () {

                            // Gets the selected item's TCM and title
                            var selectedItemId = $j(".item.selected .id").html();
                            selectedItemId = selectedItemId.substring(selectedItemId.indexOf(":") + 1);
                            var selectedItemTitle = $j(".item.selected .name").html();

                            // Calls a function to create a new tab and set it to active
                            createNewTab(selectedItemId, selectedItemTitle, "a");

                            // Recursively call the updateUsingAndUsedItems function, this time setting
                            // isRecursive as true to avoid the redundant portions
                            updateUsingAndUsedItems(selectedItemId, selectedItemTitle, true);

                            // Calls a function to deselect the selected item
                            deselectItems();
                        });

                        // Pages where used button click function
                        $j("#pages_where_used.enabled").click(function () {

                            // reads the plugin settings to determine how many item connections should be
                            // checked for pages using the selected item
                            var depth = pluginSettings.substring(pluginSettings.indexOf(":") + 2, pluginSettings.length - 2);

                            // Gets the selected item TCM
                            var selectedItemId = $j(".item.selected .id").html();
                            selectedItemId = selectedItemId.substring(selectedItemId.indexOf(":") + 1);

                            // Calls a function to create a new tab and set it to active

                            var selectedItemTitle = $j(".item.selected .name").html();
                            createNewTab(selectedItemId, selectedItemTitle, "b");

                            // Calls a function to populate this new tab with the pages where the selected
                            // item is used
                            updatePagesWhereUsedAndUsing(selectedItemId, selectedItemTitle, depth);

                            // Calls a function to deselect the selected item
                            deselectItems();
                        });

                        // Open item button function
                        $j("#open_item.enabled").click(function () {

                            // Gets the selected item TCM
                            var selectedItemId = $j(".item.selected .id").html();

                            // Checks if the selected item is a container and sets an appropriate command, either
                            // "Properties" for containers or "Open" for other items
                            var command = $models.isContainerItemType(selectedItemId) ? "Properties" : "Open";

                            // Runs the Tridion command to open the selected item in the original CM window
                            // Note that because this uses a $ rather than the $j assigned to JQuery this is actually
                            // using the Sizzler library from the Tridion CME
                            $cme.executeCommand(command, new Tridion.Cme.Selection(new Tridion.Core.Selection([selectedItemId])));
                        });

                        // Go to item location button function
                        $j("#go_to_item_location.enabled").click(function () {

                            // Gets the selected item TCM
                            var selectedItemId = $j(".item.selected .id").html();

                            // Runs the Tridion command to go to the location of the selected item in the original CM window
                            // Note that because this uses a $ rather than the $j assigned to JQuery this is actually
                            // using the Sizzler library from the Tridion CME
                            $cme.executeCommand("Goto", new Tridion.Cme.Selection(new Tridion.Core.Selection([selectedItemId])));
                        });
                    }
                });
            })
            .error(function (type, error) {

                // first arg is string that shows the type of error ie (500 Internal), 2nd arg is object representing
                // the error.  For BadRequests and Exceptions, the error message will be in the error.message property.
                console.log("There was an error", error.message);
            })
            .complete(function () {

                // this is called regardless of success or failure.
            });
        /**
        ** This function updates the popup window with the where used and using information for the
        ** currently selected item, retrieving the information from the controller containing the
        ** core service code and appending it to the new tab
        **/
        function updatePagesWhereUsedAndUsing(tcmInput, title, depth) {

            // Calls my controller to retrieve the used and using items, which come back as an HTML string
            Alchemy.Plugins["${PluginName}"].Api.UsageReportService.getPagesWhereUsed(tcmInput, title, depth)
                .success(function (items) {

                    // first arg in success is what's returned by your controller's action
                    // If we already have a tab with the results we don't need to populate a new one,
                    // we can just reuse it
                    if (!$j(".tab-body.active .results").length) {

                        // Append the results to the active tab
                        $j(".tab-body.active").append(items);
                    }

                    // On clicking an item in the popup we need to deselect any other selected items and select
                    // this one
                    $j(".item").click(function () {
                        $j(".item.selected").removeClass("selected")
                        $j(this).addClass("selected");

                        // We also need to make sure the buttons are enabled since they are enabled
                        // whenever an item is selected
                        enableButtons();
                    });

                    // When clicking on a tab we need to deselect the currently selected tab and select the clicked one
                    $j(".tabs>div.active").click(function () {

                        // Deactivate any active tabs
                        $j(".tabs>div.active").removeClass("active");

                        // Activate the clicked tab
                        $j(this).addClass("active");

                        // Deselect any active tab content divs
                        $j(".tab-body.active").removeClass("active");

                        // Find the tab content div that corresponds to the clicked tab and activate it
                        var id = $j(this).attr("data-tcm");
                        $j(".tab-body[data-tcm=" + id + "]").addClass("active");

                        // Deselect any items since we're now on a new tab
                        deselectItems();
                    });
                })
                .error(function (type, error) {

                    // first arg is string that shows the type of error ie (500 Internal), 2nd arg is object representing
                    // the error.  For BadRequests and Exceptions, the error message will be in the error.message property.
                    console.log("There was an error", error.message);
                })
                .complete(function () {

                    // this is called regardless of success or failure.
                });
        }
        /**
        ** Function which will call the Alchemy api to retrieve the settings for this plugin
        **/
        function getSettings() {
            Alchemy.Plugins["${PluginName}"].Api.getSettings()
                    .success(function (settings) {

                        // Set a variable with scope in this JS file to the settings as a string
                        pluginSettings = JSON.stringify(settings);
                    })
                    .error(function () {
                        debugger;
                        console.log("There was an error", error.message);
                    })
                    .complete(function () {
                    });
        }
        /**
        ** Whenever we deactivate the current item we need to remove the selected class from the item
        ** and disable all buttons since they are dependent on an item being selected to have a meaning
        **/
        function deselectItems() {
            debugger;
            $j("#where_used_using").addClass("disabled");
            $j("#pages_where_used").addClass("disabled");
            $j("#open_item").addClass("disabled");
            $j("#go_to_item_location").addClass("disabled");
            $j(".item.selected").removeClass("selected");
        }
        /**
        ** Enables all buttons by removing the disabled class and adding an enabled class.
        **/
        function enableButtons() {
            debugger;
            $j("#where_used_using").removeClass("disabled");
            $j("#where_used_using").addClass("enabled");
            $j("#pages_where_used").removeClass("disabled");
            $j("#pages_where_used").addClass("enabled");
            $j("#open_item").removeClass("disabled");
            $j("#open_item").addClass("enabled");
            $j("#go_to_item_location").removeClass("disabled");
            $j("#go_to_item_location").addClass("enabled");
        }
        /**
        ** Function to close the current tab and activate another tab.
        ** Takes a tab which should be closed.
        **/
        function closeTab(tab) {

            // Get the ID of the tab being closed.
            var id = tab.attr("data-tcm");

            // If the tab is the active tab then we need to activate another tab to replace it.
            if (tab.hasClass("active")) {

                // If the tab being closed is the furthest tab to the right we want to activate the tab to its
                // left, otherwise we want to activate the next tab to the right.
                if (tab.is(':last-child')) {

                    // Remove the tab contents for the tab being closed.
                    $j(".tab-body[data-tcm=" + id + "]").remove();

                    // Removes the tab.
                    tab.remove();

                    // Finds the last tab in the popup and adds the active class to it.
                    $j(".tabs>div").last().addClass("active");

                    // Gets the id of the last tab in the popup
                    id = $j(".tabs>div").last().attr("data-tcm");

                    // Finds the corresponding content and activates it as well
                    $j(".tab-body[data-tcm=" + id + "]").addClass("active");
                }
                else {

                    // If the tab being closed isn't the right most we find the next tab on the page,
                    // which must be done before removing the old tab so we don't lose our place in the order.
                    var nextTab = tab.next();

                    // Then we remove the tab contents for the tab to be removed.
                    $j(".tab-body[data-tcm=" + id + "]").remove();

                    // Removes the tab to be removed
                    tab.remove();

                    // Activates the next tab to the right.
                    nextTab.addClass("active");

                    // Finds the ID of the tab we just activated
                    id = nextTab.attr("data-tcm");

                    // Activates the contents to go with this tab.
                    $j(".tab-body[data-tcm=" + id + "]").addClass("active");
                }
            }

            // If there is only one tab left on the page we remove the close icon, since you can't close the
            // last tab.
            if ($j(".tabs>div").length == 1) {
                $j(".tabs>div").find(".close").remove();
            }
        }
        /**
        ** Creates a new empty tab.
        ** Takes the ID and Title of the item for which this tab will display information.
        ** Also takes a suffix to be added to the ID in order to differentiate between tabs showing
        ** where used/using info and pages where used info
        **/
        function createNewTab(selectedItemId, selectedItemTitle, suffix) {

            // First we deactivate the current tab.
            $j(".tabs>div.active").removeClass("active");

            // If we're creating the second tab on the popup the first tab will not have the html for
            // a close button with a click function. We add this here.
            if (!$j(".tabs>div").first().find(".close").length) {

                // Finds the first tab and adds the close button div.
                $j(".tabs>div").first().append("<div class=\"close\"></div>");

                // Adds the click function, which is just the closeTab function
                $j(".tabs .close").click(function () {
                    closeTab($j(this).parent());
                });
            }

            // Finds the last tab on the page and adds a new tab afterward.
            $j(".tabs>div").last().after("<div data-tcm=\"" + selectedItemId + suffix + "\" class=\" active\">" + selectedItemTitle + "<div class=\"close\"></div></div>")

            // Adds the close click function to the close button for this new tab.
            $j(".tabs>div").last().find(".close").click(function () {
                closeTab($j(this).parent());
            });

            // Finds the current active tab contents div.
            var activeBody = $j(".tab-body.active");

            // Removes the active class from this div.
            activeBody.removeClass("active");

            // If we find a tab contents div containing results we just activate it, rather than duplicate
            if ($j(".tab-body[data-tcm=" + selectedItemId + suffix + "]").length) {
                $j(".tab-body[data-tcm=" + selectedItemId + suffix + "]").addClass("active");
            }
            else {

                // If we don't, we create a new, empty tab results div with a data-tcm attribute to
                // match the tab we just created.
                activeBody.after("<div data-tcm=\"" + selectedItemId + suffix + "\" class=\"tab-body active\"></div>");
            }
        }
    }
})();