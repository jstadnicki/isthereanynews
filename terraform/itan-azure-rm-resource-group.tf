resource "azurerm_resource_group" "itan-west-europe-resource-group" {
  location = var.location
  name = var.resource-group-name
}