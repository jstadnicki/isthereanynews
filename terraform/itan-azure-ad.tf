//provider "azuread" {
//  version = "=1.0.0"
//  subscription_id = var.subscription-id
//  tenant_id = var.tenant-id
//}

terraform {
  required_providers {
    azuread = {
      source  = "hashicorp/azuread"
      version = "1.0.0"
    }
    azurerm = {
      source = "hashicorp/azurerm"
    }
    http = {
      source = "hashicorp/http"
    }
    null = {
      source = "hashicorp/null"
    }
    random = {
      source = "hashicorp/random"
    }
  }
}
