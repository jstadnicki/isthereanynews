resource "azurerm_storage_queue" "itan-queue-channel-to-download" {
  name = "channel-to-download"
  storage_account_name = azurerm_storage_account.itan_storage_account.name
}

resource "azurerm_storage_queue" "itan-queue-channel-update" {
  name = "channel-update"
  storage_account_name = azurerm_storage_account.itan_storage_account.name
}
