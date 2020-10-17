resource "azurerm_app_service_plan" "itan-app-service-plan-function" {
  location = var.location
  name = "itan-app-service-plan-function"
  resource_group_name = var.resource-group-name
  kind = "functionapp"
  sku {
    capacity = 0
    size = "Y1"
    tier = "Dynamic"
  }
  depends_on = [azurerm_resource_group.itan-west-europe-resource-group]
}
