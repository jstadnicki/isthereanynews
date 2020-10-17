resource "azurerm_app_service" "itan-app-service-function" {
  app_service_plan_id = azurerm_app_service_plan.itan-app-service-plan-function.id
  location = var.location
  name = "itan-app-service-function"
  resource_group_name = var.resource-group-name

  identity {
    type = "SystemAssigned"
  }
  
  depends_on = [azurerm_app_service_plan.itan-app-service-plan-function]
}