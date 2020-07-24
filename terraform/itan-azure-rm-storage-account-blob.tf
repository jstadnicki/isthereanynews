resource "azurerm_storage_container" "itan-storage-container" {
  name = "itan-blob-container"
  storage_account_name = azurerm_storage_account.itan_storage_account.name
}