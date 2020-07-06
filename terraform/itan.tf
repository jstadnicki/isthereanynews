provider "azurerm" {
  version = "~>1.32.0"
  subscription_id = "25401b19-7bb7-436c-9fca-bbac0f4eaf95"
  tenant_id = "3408b585-a1ca-41d4-ae2f-ea3ea685223f"
}

variable "location" {
  default = "westeurope"
}

resource "azurerm_resource_group" "itan-west-europe-resource-group" {
  location = var.location
  name = "itan-west-europe-resource-group"
}

resource "azurerm_app_service_plan" "itan-app-service-plan" {
  location = var.location
  resource_group_name = azurerm_resource_group.itan-west-europe-resource-group.name
  name = "itan-app-service-plan"

  sku {
    size = "F1"
    tier = "Basic"
  }
}

resource "azurerm_app_service" "itan-app-webapi" {
  app_service_plan_id = azurerm_app_service_plan.itan-app-service-plan.id
  location = var.location
  name = "itan-app-webapi"
  resource_group_name = azurerm_resource_group.itan-west-europe-resource-group.name
}
