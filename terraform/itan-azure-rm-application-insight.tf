resource "azurerm_application_insights" "itan-application-insights" {
  application_type = "web"
  location = var.location
  name = "itan-application-insights"
  resource_group_name = var.resource-group-name

  depends_on = [azurerm_resource_group.itan-west-europe-resource-group]
}