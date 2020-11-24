resource "azuread_application" "itan-ad-application-ui" {
  name = "itan-ui"
  owners = [var.subscription-owner-id]
  type = "webapp/api"
  identifier_uris = ["https://isthereanynewscodeblast.onmicrosoft.com/ui"]
  reply_urls = ["http://localhost:4200"]
  oauth2_allow_implicit_flow = false
  available_to_other_tenants = true

  required_resource_access {
    resource_app_id = azuread_application.itan-ad-application-api.application_id

    dynamic "resource_access" {
      iterator = item
      for_each = azuread_application.itan-ad-application-api.oauth2_permissions

      content {
        id = item.value["id"]
        type = "Scope"
      }
    }
  }
}

