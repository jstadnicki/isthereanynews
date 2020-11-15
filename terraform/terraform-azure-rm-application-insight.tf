resource "azurerm_application_insights" "itan-application-insights" {
  application_type = "web"
  location = var.location
  name = "itan-application-insights"
  resource_group_name = var.resource-group-name
}