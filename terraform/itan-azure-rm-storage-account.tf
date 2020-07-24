resource "azurerm_storage_account" "itan_storage_account" {
  account_replication_type = "lrs"
  account_tier = "standard"
  location = var.location
  name = "itanstorageaccount"
  resource_group_name = var.resource-group-name
  
  depends_on = [azurerm_resource_group.itan_west_europe_resource_group]
}
