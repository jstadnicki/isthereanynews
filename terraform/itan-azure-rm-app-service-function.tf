resource "azurerm_function_app" "itan-app-service-function" {
  app_service_plan_id = azurerm_app_service_plan.itan-app-service-plan-function.id
  location = var.location
  name = "itan-app-service-function"
  resource_group_name = var.resource-group-name

  identity {
    type = "SystemAssigned"
  }
  
  depends_on = [azurerm_app_service_plan.itan-app-service-plan-function]
  
  storage_account_name = azurerm_storage_account.itan_storage_account.name
  storage_account_access_key = azurerm_storage_account.itan_storage_account.primary_access_key

  version = "~3"

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.itan-application-insights.instrumentation_key
  }
}