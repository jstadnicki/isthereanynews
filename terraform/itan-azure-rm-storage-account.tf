resource "azurerm_storage_account" "itan_storage_account" {
  account_replication_type = "lrs"
  account_tier = "standard"
  location = var.location
  name = "itanstorageaccount"
  resource_group_name = var.resource-group-name
  account_kind = "StorageV2"

  static_website {
    index_document = "index.html"
    error_404_document = "404.html"
  }

  depends_on = [azurerm_resource_group.itan-west-europe-resource-group]
}
