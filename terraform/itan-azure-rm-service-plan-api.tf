resource "azurerm_app_service_plan" "itan-app-service-plan" {
  location = var.location
  resource_group_name = var.resource-group-name
  name = "itan_app_service_plan"
  kind = "windows"

  sku {
    size = "F1"
    tier = "Free"
  }
  depends_on = [azurerm_resource_group.itan_west_europe_resource_group]
}
