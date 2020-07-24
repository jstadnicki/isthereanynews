resource "azurerm_key_vault" "itan-key-vault" {
  location = var.location
  name = "itan-key-vault-2"
  resource_group_name = var.resource-group-name
  sku_name = "standard"
  tenant_id = var.tenant-id
  
  access_policy {
    object_id = var.subscription-owner-id
    tenant_id = var.tenant-id
    
    secret_permissions = ["get","list","set","delete","purge"]
  }
  
  depends_on = [azurerm_resource_group.itan_west_europe_resource_group]
}
