resource "azurerm_app_service" "itan-app-service-api" {
  app_service_plan_id = azurerm_app_service_plan.itan-app-service-plan-api.id
  location = var.location
  name = "itan-app-service-webapi"
  resource_group_name = var.resource-group-name
  
  identity {
    type = "SystemAssigned"
  }
  
  depends_on = [azurerm_app_service_plan.itan-app-service-plan-api]

  app_settings = {
    APPINSIGHTS_INSTRUMENTATIONKEY = azurerm_application_insights.itan-application-insights.instrumentation_key
  }
}