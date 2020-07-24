resource "azurerm_app_service" "itan-app-webapi" {
  app_service_plan_id = azurerm_app_service_plan.itan-app-service-plan.id
  location = var.location
  name = "itan-app-webapi"
  resource_group_name = var.resource-group-name
}