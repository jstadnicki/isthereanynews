resource "azurerm_key_vault" "itan-key-vault" {
  location = var.location
  name = "itan-key-vault"
  resource_group_name = var.resource-group-name
  sku_name = "standard"
  tenant_id = var.tenant-id
  
  access_policy {
    object_id = var.subscription-owner-id
    tenant_id = var.tenant-id
    
    secret_permissions = ["get","list","set","delete","purge"]
  }

  access_policy {
    object_id = azurerm_app_service.itan-app-service-api.identity[0].principal_id
    tenant_id = var.tenant-id

    secret_permissions = ["get","list"]
  }

  access_policy {
    object_id = azurerm_app_service.itan-app-service-function.identity[0].principal_id
    tenant_id = var.tenant-id

    secret_permissions = ["get","list"]
  }
  
  depends_on = [azurerm_resource_group.itan-west-europe-resource-group]
}
