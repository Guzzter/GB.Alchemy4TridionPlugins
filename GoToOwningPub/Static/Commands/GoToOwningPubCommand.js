Alchemy.command("Go_To_Owning_Publication", "GoToOwningPub", {

	/**
	 * If an init function is created, this will be called from the command's constructor when a command instance
	 * is created.
	 */
	init: function () { },

	/**
	 * Whether or not the command is enabled for the user (will usually have extensions displayed but disabled).
	 * @returns {boolean}
	 */
	isEnabled: function (selection) {
		if (selection.getItems().length == 1) {
			var itemType = $models.getItemType(selection.getItem(0));
			if (itemType == $const.ItemType.COMPONENT ||
			itemType == $const.ItemType.COMPONENT_TEMPLATE ||
			itemType == $const.ItemType.SCHEMA ||
			itemType == $const.ItemType.TEMPLATE_BUILDING_BLOCK ||
			itemType == $const.ItemType.FOLDER ||
			itemType == $const.ItemType.STRUCTURE_GROUP ||
			itemType == $const.ItemType.PAGE ||
			itemType == $const.ItemType.PAGE_TEMPLATE) {
				return true;
			}
		}
		return false;
	},

	/**
	 * Whether or not the command is available to the user.
	 * @returns {boolean}
	 */
	isAvailable: function (selection) {
		return this.isEnabled(selection);
	},

	/**
	 * Executes your command. You can use _execute or execute as the property name.
	 */
	execute: function (selection) {
		var tcmid = selection.getItems()[0],
		it = $models.getItem(tcmid),
		self = this;

		this.loadItem(it, function () {
			self.goToParent(it.getXmlDocument());
		});
	},

	/**
	* Checks if current publication is the owner of the selected item. If not it navigates to the correct OrganizationItem. If it is the owner, the user is notified.
	*/
	goToParent: function (doc) {
		var pubId = $xml.selectNodes(doc, "//tcm:Publication/@xlink:href")[0].value,
		owningPubId = $xml.selectNodes(doc, "//tcm:OwningPublication/@xlink:href")[0].value;

		if (owningPubId != pubId) {
			//var it = $xml.selectNodes(doc, "//tcm:OrganizationalItem/@xlink:href")[0].value;
			var it = $xml.selectNodes(doc, "//tcm:Page/@ID")[0].value;

			var owningPubNr = owningPubId.slice(6, owningPubId.lastIndexOf("-"));
			var contextId = "tcm:" + owningPubNr + it.slice(it.indexOf("-"));

			$models.getNavigator().navigateToCmItem(contextId, false, window);
		} else if ($messages) {
			$messages.registerNotification('Current publication is the owner.');
		} else {
			alert('Current publication is the owner.');
		}
	},

	/**
	* Helper method for load a TCM item async.
	*/
	loadItem: function (item, cb) {
		if (item.isLoaded()) {
			cb();
			return;
		}
		$evt.addEventHandler(item, "load", function (event) {
			cb();
		});
		item.load();
	}
});